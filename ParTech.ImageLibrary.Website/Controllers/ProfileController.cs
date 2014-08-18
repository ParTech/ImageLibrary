using ParTech.ImageLibrary.Core.Enums;
using System.Web.Mvc;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Profile;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IObjectRepository _objectRepository;

        private readonly IUserRepository _userRepository;

        public ProfileController(IObjectRepository objectRepository, IUserRepository userRepository)
        {
            _objectRepository = objectRepository;
            _userRepository = userRepository;
        }

        //
        // GET: /Profile/

        [Authorize(Roles = "Byer,Seller")]
        public ActionResult EditProfile()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.SaveProfileFailure:
                        TempData["StatusMessage"] = "Something went wrong. The profile could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.SaveProfileSuccess:
                        TempData["StatusMessage"] = "The profile was saved.";
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

            return View(editProfileModel);
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
