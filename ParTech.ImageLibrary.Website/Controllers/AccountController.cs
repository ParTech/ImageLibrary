using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Account;
using ParTech.ImageLibrary.Core.Workers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ParTech.ImageLibrary.Website.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountsWorker _accountsWorker;

        private readonly IUserRepository _userRepository;

        public AccountController(IAccountsWorker accountsWorker,
            IUserRepository userRepository)
        {
            _accountsWorker = accountsWorker;
            _userRepository = userRepository;
        }

        #region Login and Logoff methods
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!_userRepository.CheckIfUserProfileIsActive(model.UserName))
            {
                ModelState.AddModelError("", "Your account is not active.");
                return View(model);
            }

            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Register and RegisterConfirmation methods

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (_accountsWorker.RegisterUser(model))
                {
                    TempData["Message"] = MessageIdEnum.RegisterStepTwo;
                    return RedirectToAction("Message", "Messages");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.RegisterFailure;
            return RedirectToAction("Message", "Messages");
        }

        //
        // GET: /Account/RegisterConfirmation

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string id)
        {
            if (WebSecurity.ConfirmAccount(id))
            {
                TempData["Message"] = MessageIdEnum.RegisterConfirmationSuccess;
                return RedirectToAction("Message", "Messages");
            }

            TempData["Message"] = MessageIdEnum.RegisterConfirmationFailure;
            return RedirectToAction("Message", "Messages");
        }

        #endregion

        #region Recover and ResetPassword methods

        //
        // GET: /Account/Recover

        [AllowAnonymous]
        public ActionResult Recover()
        {
            return View();
        }

        //
        // POST: /Account/Recover

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Recover(RecoverModel model)
        {
            if (ModelState.IsValid)
            {
                if (_accountsWorker.SendRecoverPasswordEmail(model, 
                    Url.Action("ResetPassword", "Account", new { un = "{0}", rt = "{1}" })))
                {
                    TempData["Message"] = MessageIdEnum.RecoverSuccess;
                    return RedirectToAction("Message", "Messages");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = MessageIdEnum.RecoverFailure;
            return RedirectToAction("Message", "Messages");
        }

        //
        // GET: /Account/ResetPassword

        [AllowAnonymous]
        public ActionResult ResetPassword(string un, string rt)
        {
            TempData["Message"] = _accountsWorker.SendResetPasswordEmail(un, rt);
            return RedirectToAction("Message", "Messages");
        }

        #endregion

        #region Manage, ChangePassword and SaveUserProfileData methods

        //
        // GET: /Account/Manage

        public ActionResult Manage()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.ChangePasswordSuccess:
                        TempData["StatusMessage"] = "Your password has been changed.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.ChangeUserProfileDataFailure:
                        TempData["StatusMessage"] = "Something went wrong. The changes were not saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.ChangeUserProfileDataSuccess:
                        TempData["StatusMessage"] = "The changes were saved.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.SetPasswordSuccess:
                        TempData["StatusMessage"] = "Your password has been set.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.RemoveLoginSuccess:
                        TempData["StatusMessage"] = "The external login was removed.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }
            
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");

            var model =  new LocalAccountModel
            {
                PasswordModel = new LocalPasswordModel(),
                UserModel = _userRepository.GetUserProfileAndMapToLocalUserModel()
            };

            return View(model);
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        TempData["Message"] = MessageIdEnum.ChangePasswordSuccess;
                        return RedirectToAction("Manage");
                    }
                    
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                var state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);

                        TempData["Message"] = MessageIdEnum.SetPasswordSuccess;
                        return RedirectToAction("Manage");
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Manage");
        }

        //
        // POST: /Account/SaveUserProfileData

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUserProfileData(LocalUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.SaveUserProfile(model))
                {
                    TempData["Message"] = MessageIdEnum.ChangeUserProfileDataSuccess;
                    return RedirectToAction("Manage");
                }
            }

            TempData["Message"] = MessageIdEnum.ChangeUserProfileDataFailure;
            return RedirectToAction("Manage");
        }

        #endregion

        #region ExternalLogin, ExternalLoginCallback, ExternalLoginConfirmation, ExternalLoginFailure, ExternalLoginsList and RemoveExternalLogins methods

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }

            // User is new, ask for their desired membership name
            var loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider;
            string providerUserId;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                if (_userRepository.AddUserProfile(model.UserName))
                {
                    OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                    OAuthWebSecurity.Login(provider, providerUserId, false);

                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            var accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            var externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                var clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #endregion

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Index", "Home");
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion
    }
}
