using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.Workers;
using Westwind.Globalization;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class AdminController : Controller
    {
        public ILogger Logger { get; set; }

        private readonly IAccountsWorker _accountsWorker;

        private readonly IImageRepository _imageRepository;

        private readonly ILuceneWorker _luceneWorker;

        private readonly IObjectRepository _objectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserRepository _userRepository;

        private static IDictionary<Guid, string> tasks = new Dictionary<Guid, string>();

        public AdminController(IAccountsWorker accountsWorker, IImageRepository imageRepository, 
            ILuceneWorker luceneWorker, IObjectRepository objectRepository, IOrderRepository orderRepository, 
            IUserRepository userRepository)
        {
            _accountsWorker = accountsWorker;
            _imageRepository = imageRepository;
            _luceneWorker = luceneWorker;
            _objectRepository = objectRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        #region User related action methods 

        //
        // GET: /Admin/ActivateUser

        [Authorize(Roles = "Admin,Byer,Seller")]
        public ActionResult ActivateUser(int userid, string returnTo)
        {
            if (_userRepository.UpdateActiveFlagUserProfile(userid, true))
            {
                TempData["Message"] = MessageIdEnum.ActivateUserSuccess;

                if (returnTo == "AdditionalAccounts")
                {
                    return RedirectToAction("AdditionalAccounts", "Profile");
                }
                
                return RedirectToAction("ShowUsers", "Admin");
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.ActivateUserFailure;

            if (returnTo == "AdditionalAccounts")
            {
                return RedirectToAction("AdditionalAccounts", "Profile");
            }

            return RedirectToAction("ShowUsers", "Admin");
        }

        //
        // GET: /Admin/DeactivateUser

        [Authorize(Roles = "Admin,Byer,Seller")]
        public ActionResult DeactivateUser(int userid, string returnTo)
        {
            if (_userRepository.UpdateActiveFlagUserProfile(userid, false))
            {
                TempData["Message"] = MessageIdEnum.DeactivateUserSuccess;
                
                if (returnTo == "AdditionalAccounts")
                {
                    return RedirectToAction("AdditionalAccounts", "Profile");
                }
                
                return RedirectToAction("ShowUsers", "Admin");
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.DeactivateUserFailure;
            
            if (returnTo == "AdditionalAccounts")
            {
                return RedirectToAction("AdditionalAccounts", "Profile");
            } 
            
            return RedirectToAction("ShowUsers", "Admin");
        }

        //
        // GET: /Admin/SendConfirmationEmail

        [Authorize(Roles = "Admin,Byer,Seller")]
        public ActionResult SendConfirmationEmail(int userid, string returnTo)
        {
            if (_accountsWorker.SendRegistrationConfirmationEmail(userid))
            {
                TempData["Message"] = MessageIdEnum.RegistrationConfirmationEmailSuccess;
                
                if (returnTo == "AdditionalAccounts")
                {
                    return RedirectToAction("AdditionalAccounts", "Profile");
                } 
                
                return RedirectToAction("ShowUsers", "Admin");
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.RegistrationConfirmationEmailFailure;
            
            if (returnTo == "AdditionalAccounts")
            {
                return RedirectToAction("AdditionalAccounts", "Profile");
            } 
            
            return RedirectToAction("ShowUsers", "Admin");
        }

        //
        // GET: /Admin/ShowUsers

        [Authorize(Roles = "Admin")]
        public ActionResult ShowUsers()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.ActivateUserFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.ActivateUserFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.ActivateUserSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.ActivateUserSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.DeactivateUserFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.DeactivateUserFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.DeactivateUserSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.DeactivateUserSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.RegistrationConfirmationEmailFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.RegistrationConfirmationEmailFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.RegistrationConfirmationEmailSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.RegistrationConfirmationEmailSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var model = _userRepository.GetUserProfilesAndContext();

            return View(model);
        }

        #endregion

        #region Lucene Index action methods

        //
        // GET: /Admin/Index

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Admin/StartIndexRebuild

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult StartIndexRebuild()
        {
            var currentLanguage = Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName;
            var taskId = Guid.NewGuid();
            tasks.Add(taskId, "0");

            _luceneWorker.ClearLuceneIndex();

            Task.Factory.StartNew(() => TaskUpdateAllProducts(currentLanguage, taskId));

            return Json(taskId);
        }

        //
        // POST: /Admin/ProgressIndexRebuild

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ProgressIndexRebuild(Guid id)
        {
            return Json(tasks.Keys.Contains(id) ? tasks[id] : "100");
        }

        private void TaskUpdateAllProducts(string language, Guid taskId)
        {
            var allProducts = _objectRepository.GetProductsAndContext().ToList();
            var productsInStep = new List<Product>();
            for (var i = 0; i <= allProducts.Count; i++)
            {
                // update task progress
                var x = Decimal.Divide(i, allProducts.Count);
                tasks[taskId] = (x*100).ToString("f0", CultureInfo.InvariantCulture);

                int step = i%10;
                if (step == 0)
                {
                    _luceneWorker.AddUpdateLuceneIndex(language, productsInStep);

                    productsInStep = new List<Product>();
                }
                else
                {
                    productsInStep.Add(allProducts[i - 1]);
                }

                // simulate long running operation
                Thread.Sleep(2000);
            }

            if (productsInStep.Any())
            {
                _luceneWorker.AddUpdateLuceneIndex(language, productsInStep);
            }

            tasks.Remove(taskId);
        }
        
        #endregion

        //
        // GET: /Byer/ShowProduct

        [Authorize(Roles = "Admin")]
        public ActionResult ShowProduct(int productid = 0)
        {
            var productModel = _objectRepository.GetProductAndContext(productid);

            return View(productModel);
        }

        //
        // GET: /Byer/ShowThumbnail

        [Authorize(Roles = "Byer")]
        public ActionResult ShowThumbnail(int imageid = 0)
        {
            var imageModel = _imageRepository.GetImage(imageid);
            if (imageModel != null)
            {
                return new FileStreamResult(new FileStream(imageModel.Thumbnailpath, FileMode.Open), imageModel.ImageFormat);
            }

            return null;
        }

    }
}
