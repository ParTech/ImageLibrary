﻿using System;
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

        [Authorize(Roles = "Admin")]
        public ActionResult ActivateUser(int userid)
        {
            if (_userRepository.UpdateActiveFlagUserProfile(userid, true))
            {
                TempData["Message"] = MessageIdEnum.ActivateUserSuccess;
                return RedirectToAction("ShowInactiveUsers", "Admin");
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.ActivateUserFailure;
            return RedirectToAction("ShowInactiveUsers", "Admin");
        }

        //
        // GET: /Admin/DeactivateUser

        [Authorize(Roles = "Admin")]
        public ActionResult DeactivateUser(int userid)
        {
            if (_userRepository.UpdateActiveFlagUserProfile(userid, false))
            {
                TempData["Message"] = MessageIdEnum.DeactivateUserSuccess;
                return RedirectToAction("ShowInactiveUsers", "Admin");
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.DeactivateUserFailure;
            return RedirectToAction("ShowActiveUsers", "Admin");
        }

        //
        // GET: /Admin/SendConfirmationEmail

        [Authorize(Roles = "Admin")]
        public ActionResult SendConfirmationEmail(int userid)
        {
            if (_accountsWorker.SendRegistrationConfirmationEmail(userid))
            {
                TempData["Message"] = MessageIdEnum.RegistrationConfirmationEmailSuccess;
                return RedirectToAction("ShowInactiveUsers", "Admin");
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.RegistrationConfirmationEmailFailure;
            return RedirectToAction("ShowInactiveUsers", "Admin");
        }

        //
        // GET: /Admin/ShowActiveUsers

        [Authorize(Roles = "Admin")]
        public ActionResult ShowActiveUsers()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.DeactivateUserFailure:
                        TempData["StatusMessage"] = "The user could not be deactivated.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.DeactivateUserSuccess:
                        TempData["StatusMessage"] = "The user has been deactivated.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var model = _userRepository.GetUserProfilesAndContext(true);

            return View(model);
        }

        //
        // GET: /Admin/ShowInactiveUsers

        [Authorize(Roles = "Admin")]
        public ActionResult ShowInactiveUsers()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.ActivateUserFailure:
                        TempData["StatusMessage"] = "The user could not be activated.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.ActivateUserSuccess:
                        TempData["StatusMessage"] = "The user has been activated.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.RegistrationConfirmationEmailFailure:
                        TempData["StatusMessage"] = "The confirmation email could not be sent.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.RegistrationConfirmationEmailSuccess:
                        TempData["StatusMessage"] = "The confirmation email has been sent.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var model = _userRepository.GetUserProfilesAndContext(false);

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