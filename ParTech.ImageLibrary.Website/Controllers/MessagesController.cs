using ParTech.ImageLibrary.Core.Enums;
using System.Web.Mvc;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class MessagesController : Controller
    {
        //
        // GET: /Messages/Message

        [AllowAnonymous]
        public ActionResult Message()
        {
            switch ((MessageIdEnum)TempData["Message"])
            {
                case MessageIdEnum.RecoverFailure:
                    ViewBag.Title = "Recovery Failure";
                    ViewBag.Subtitle = "Recovery Failure";
                    ViewBag.MessageText = "There was an error sending the recovery email. Return to the 'Recover password' page and try again.";
                    break;
                case MessageIdEnum.RecoverSuccess:
                    ViewBag.Title = "Recovery Success";
                    ViewBag.Subtitle = "Recovery Success";
                    ViewBag.MessageText = "We sent the new password to your email addres. You can logon to the system by clicking on the logon link.";
                    break;
                case MessageIdEnum.RegisterConfirmationFailure:
                    ViewBag.Title = "Register Confirmation Failure";
                    ViewBag.Subtitle = "Registration Error";
                    ViewBag.MessageText = "There was an error confirming your email. Please try again.";
                    break;
                case MessageIdEnum.RegisterConfirmationSuccess:
                    ViewBag.Title = "Register Confirmation Success";
                    ViewBag.Subtitle = "Registration Completed";
                    ViewBag.MessageText = "You have completed the registration process. You can now logon to the system by clicking on the logon link.";
                    break;
                case MessageIdEnum.RegisterFailure:
                    ViewBag.Title = "Register Failure";
                    ViewBag.Subtitle = "Registration Not Completed";
                    ViewBag.MessageText = "There was an error creating the account. Return to the 'Register' page and try again.";
                    break;
                case MessageIdEnum.RegisterStepTwo:
                    ViewBag.Title = "Registration instructions";
                    ViewBag.Subtitle = "Registration instructions";
                    ViewBag.MessageText = "To complete the registration process look for an email in your inbox that provides further instructions.";
                    break;
                case MessageIdEnum.ResetPasswordFailure:
                    ViewBag.Title = "Reset Password Failure";
                    ViewBag.Subtitle = "The specified user name does not exist.";
                    ViewBag.MessageText = "Return to the 'Recover password' page and try again.";
                    break;
                case MessageIdEnum.ResetPasswordNoMatch:
                    ViewBag.Title = "Reset Password Failure";
                    ViewBag.Subtitle = "The user name and/or verification token do not match.";
                    ViewBag.MessageText = "Return to the 'Recover password' page and try again.";
                    break;
                case MessageIdEnum.ResetPasswordNotReset:
                    ViewBag.Title = "Reset Password Failure";
                    ViewBag.Subtitle = "The password could not be reset.";
                    ViewBag.MessageText = "Return to the 'Recover password' page and try again.";
                    break;
                default:
                    ViewBag.Title = string.Empty;
                    ViewBag.Subtitle = string.Empty;
                    ViewBag.MessageText = string.Empty;
                    break;
            }

            return View();
        }
    }
}
