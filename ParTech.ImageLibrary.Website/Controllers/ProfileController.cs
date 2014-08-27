using ParTech.ImageLibrary.Core.Enums;
using System.Web.Mvc;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Profile;
using ParTech.ImageLibrary.Core.Workers;
using Westwind.Globalization;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAccountsWorker _accountsWorker;

        private readonly IObjectRepository _objectRepository;

        private readonly IUserRepository _userRepository;

        public ProfileController(IAccountsWorker accountsWorker, IObjectRepository objectRepository, 
            IUserRepository userRepository)
        {
            _accountsWorker = accountsWorker;
            _objectRepository = objectRepository;
            _userRepository = userRepository;
        }

        //
        // GET: /Profile/EditProfile

        [Authorize(Roles = "Byer,Seller")]
        public ActionResult AdditionalAccounts(int? userProfileid)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.EditAccountFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditAccountFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditAccountSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditAccountSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewAccountFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewAccountFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewAccountSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewAccountSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var userProfile = _userRepository.GetUserProfileByName(User.Identity.Name);
            var aam = new AdditionalAccountsModel
            {
                ListUserProfiles = _userRepository.GetUserProfilesByProfileIdAndContext(userProfile.ProfileID, false)
            };

            if (userProfileid == null || userProfileid == 0)
            {
                ViewBag.FormHeader = DbRes.T("ProfileController.AddProfileForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ProfileController.AddProfileForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ProfileController.AddProfileForm.BtnSubmit", "Resources");

                aam.UserProfileModel = new UserProfileModel();
            }
            else
            {
                ViewBag.FormHeader = DbRes.T("ProfileController.EditProfileForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ProfileController.EditProfileForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ProfileController.EditProfileForm.BtnSubmit", "Resources");

                aam.UserProfileModel = _userRepository.GetUserProfileAndMapToUserProfileModel(userProfileid);
            }

            return View(aam);
        }

        //
        // GET: /Profile/EditProfile

        [Authorize(Roles = "Byer,Seller")]
        public ActionResult EditProfile()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.SaveProfileFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.SaveProfileFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.SaveProfileSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.SaveProfileSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var userProfile = _userRepository.GetUserProfileByName(User.Identity.Name);
            var editProfileModel = _userRepository.GetProfileAndMapItToEditProfileModel(userProfile.ProfileID);
            editProfileModel.CountryItems = _objectRepository.GetCountries();
            editProfileModel.LanguageItems = _objectRepository.GetLanguages();
            editProfileModel.SalutationItems = _objectRepository.GetSalutations();
            editProfileModel.MainAccount = userProfile.MainAccount;

            return View(editProfileModel);
        }

        //
        // POST: /Profile/SaveAccount

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Byer,Seller")]
        public ActionResult SaveAccount(UserProfileModel userProfileModel)
        {
            const MessageIdEnum successMessage = MessageIdEnum.NewAccountSuccess;
            const MessageIdEnum failureMessage = MessageIdEnum.NewAccountFailure;
            
            if (ModelState.IsValid)
            {
                var userProfileCurrentUser = _userRepository.GetUserProfileByName(User.Identity.Name);

                if (_accountsWorker.RegisterAdditionalAccount(userProfileModel, userProfileCurrentUser))
                {
                    var newUserProfile = _userRepository.GetUserProfileByName(userProfileModel.UserName);
                    if (newUserProfile != null 
                        && _accountsWorker.SendRegistrationConfirmationEmail(newUserProfile.Id))
                    {
                        TempData["Message"] = successMessage;
                        return RedirectToAction("AdditionalAccounts", "Profile");
                    }
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("AdditionalAccounts", "Profile");
        }

        //
        // POST: /Profile/SaveProfile

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Byer,Seller")]
        public ActionResult SaveProfile(EditProfileModel profileModel)
        {
            const MessageIdEnum successMessage = MessageIdEnum.SaveProfileSuccess;
            const MessageIdEnum failureMessage = MessageIdEnum.SaveProfileFailure;

            if (ModelState.IsValid)
            {
                if (_userRepository.SaveProfile(profileModel))
                {
                    TempData["Message"] = successMessage;
                    return RedirectToAction("EditProfile", "Profile");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("EditProfile", "Profile");
        }
    }
}
