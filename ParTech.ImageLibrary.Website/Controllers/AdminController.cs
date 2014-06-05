using System.Web.Mvc;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.Workers;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class AdminController : Controller
    {
        public ILogger Logger { get; set; }

        private readonly IAccountsWorker _accountsWorker;

        private readonly IImageRepository _imageRepository;

        private readonly IObjectRepository _objectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserRepository _userRepository;

        public AdminController(IAccountsWorker accountsWorker, IImageRepository imageRepository, 
            IObjectRepository objectRepository, IOrderRepository orderRepository, 
            IUserRepository userRepository)
        {
            _accountsWorker = accountsWorker;
            _imageRepository = imageRepository;
            _objectRepository = objectRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

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
    }
}
