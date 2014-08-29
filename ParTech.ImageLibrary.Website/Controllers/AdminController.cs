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
using ParTech.ImageLibrary.Core.Utils;
using ParTech.ImageLibrary.Core.Workers;
using Westwind.Globalization;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class AdminController : Controller
    {
        #region Properties

        private const string IndexRebuildTaskId = "0";

        private const string GenerateInvoicesTaskId = "1";
        
        public ILogger Logger { get; set; }

        private readonly IAccountsWorker _accountsWorker;

        private readonly IImageRepository _imageRepository;

        private readonly ILuceneWorker _luceneWorker;

        private readonly IObjectRepository _objectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IOrderWorker _orderWorker;

        private readonly IUserRepository _userRepository;

// ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static IDictionary<Guid, string> _tasks = new Dictionary<Guid, string>();

        #endregion

        public AdminController(IAccountsWorker accountsWorker, IImageRepository imageRepository, 
            ILuceneWorker luceneWorker, IObjectRepository objectRepository, IOrderRepository orderRepository, 
            IOrderWorker orderWorker, IUserRepository userRepository)
        {
            _accountsWorker = accountsWorker;
            _imageRepository = imageRepository;
            _luceneWorker = luceneWorker;
            _objectRepository = objectRepository;
            _orderRepository = orderRepository;
            _orderWorker = orderWorker;
            _userRepository = userRepository;
        }

        #region Invoice related action methods

        //
        // POST: /Admin/StartGenerateInvoices

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult StartGenerateInvoices()
        {
            var taskId = Guid.NewGuid();
            _tasks.Add(taskId, GenerateInvoicesTaskId);

            Task.Factory.StartNew(() => TaskGenerateInvoices(taskId));

            return Json(taskId);
        }

        //
        // POST: /Admin/ProgressGenerateInvoices

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ProgressGenerateInvoices(Guid id)
        {
            return Json(_tasks.Keys.Contains(id) ? _tasks[id] : "100");
        }

        #endregion

        #region Lucene Index action methods

        //
        // POST: /Admin/StartIndexRebuild

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult StartIndexRebuild()
        {
            var currentLanguage = Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName;
            var taskId = Guid.NewGuid();
            _tasks.Add(taskId, IndexRebuildTaskId);

            _luceneWorker.ClearLuceneIndex();

            Task.Factory.StartNew(() => TaskIndexRebuild(currentLanguage, taskId));

            return Json(taskId);
        }

        //
        // POST: /Admin/ProgressIndexRebuild

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ProgressIndexRebuild(Guid id)
        {
            return Json(_tasks.Keys.Contains(id) ? _tasks[id] : "100");
        }
        
        #endregion

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

        #region Misc action methods

        //
        // GET: /Admin/Processes

        [Authorize(Roles = "Admin")]
        public ActionResult Processes()
        {
            var loggedEvents = _objectRepository.GetLoggedEvents();

            return View(loggedEvents);
        }

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

        #endregion

        private void TaskGenerateInvoices(Guid taskId)
        {
            var loggedEvent = LoggingEvents.LogStartEvent(_objectRepository, "Task Generate Invoices", string.Empty);

            var invoicesCreated = 0;
            var orderLinesForMonth = _orderRepository.GetOrderLinesForByerInvoices(DateTime.Now.AddMonths(-1)).ToList();
            var byerIdsWithOrderLines = orderLinesForMonth.GroupBy(ol => ol.BuyerID).ToList();
            for (var i = 0; i < byerIdsWithOrderLines.Count; i++)
            {
                // update task progress
                var x = Decimal.Divide(i, orderLinesForMonth.Count);
                _tasks[taskId] = (x * 100).ToString("f0", CultureInfo.InvariantCulture);

                var newInvoice = _orderWorker.GenerateInvoiceForByer(byerIdsWithOrderLines[i].Key, byerIdsWithOrderLines[i].ToList());
                if (newInvoice != null)
                {
                    invoicesCreated = invoicesCreated++;
                }

                // simulate long running operation
                //Thread.Sleep(2000);
            }

            LoggingEvents.LogEndEvent(_objectRepository, 
                                      loggedEvent.LoggedEventID,
                                      string.Format("{0} invoices created; {1} errors",
                                                    invoicesCreated,
                                                    (byerIdsWithOrderLines.Count - invoicesCreated)));

            _tasks.Remove(taskId);
        }

        private void TaskIndexRebuild(string language, Guid taskId)
        {
            var loggedEvent = LoggingEvents.LogStartEvent(_objectRepository, "Task Index Rebuild", string.Empty);

            var allProducts = _objectRepository.GetProductsAndContext().ToList();
            var productsInStep = new List<Product>();
            for (var i = 0; i < allProducts.Count; i++)
            {
                // update task progress
                var x = Decimal.Divide(i, allProducts.Count);
                _tasks[taskId] = (x * 100).ToString("f0", CultureInfo.InvariantCulture);

                var step = i % 10;
                if (step == 0)
                {
                    _luceneWorker.AddUpdateLuceneIndex(language, productsInStep);

                    productsInStep = new List<Product>();
                }
                else
                {
                    productsInStep.Add(allProducts[i]);
                }

                // simulate long running operation
                //Thread.Sleep(2000);
            }

            if (productsInStep.Any())
            {
                _luceneWorker.AddUpdateLuceneIndex(language, productsInStep);
            }

            LoggingEvents.LogEndEvent(_objectRepository,
                                      loggedEvent.LoggedEventID,
                                      string.Format("{0} products indexed",
                                                    allProducts.Count));

            _tasks.Remove(taskId);
        }

    }
}
