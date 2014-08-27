using ParTech.ImageLibrary.Core.Enums;
using System.Web.Mvc;
using Westwind.Globalization;

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
                    ViewBag.Title = DbRes.T("Messages.RecoverFailure.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.RecoverFailure.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.RecoverFailure", "Resources");
                    break;
                case MessageIdEnum.RecoverSuccess:
                    ViewBag.Title = DbRes.T("Messages.RecoverSuccess.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.RecoverSuccess.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.RecoverSuccess", "Resources");
                    break;
                case MessageIdEnum.RegisterConfirmationFailure:
                    ViewBag.Title = DbRes.T("Messages.RegisterConfirmationFailure.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.RegisterConfirmationFailure.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.RegisterConfirmationFailure", "Resources");
                    break;
                case MessageIdEnum.RegisterConfirmationSuccess:
                    ViewBag.Title = DbRes.T("Messages.RegisterConfirmationSuccess.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.RegisterConfirmationSuccess.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.RegisterConfirmationSuccess", "Resources");
                    break;
                case MessageIdEnum.RegisterFailure:
                    ViewBag.Title = DbRes.T("Messages.RegisterFailure.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.RegisterFailure.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.RegisterFailure", "Resources");
                    break;
                case MessageIdEnum.RegisterStepTwo:
                    ViewBag.Title = DbRes.T("Messages.RegisterStepTwo.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.RegisterStepTwo.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.RegisterStepTwo", "Resources");
                    break;
                case MessageIdEnum.ResetPasswordFailure:
                    ViewBag.Title = DbRes.T("Messages.ResetPasswordFailure.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.ResetPasswordFailure.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.ResetPasswordFailure", "Resources");
                    break;
                case MessageIdEnum.ResetPasswordNoMatch:
                    ViewBag.Title = DbRes.T("Messages.ResetPasswordNoMatch.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.ResetPasswordNoMatch.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.ResetPasswordNoMatch", "Resources");
                    break;
                case MessageIdEnum.ResetPasswordNotReset:
                    ViewBag.Title = DbRes.T("Messages.ResetPasswordNotReset.Title", "Resources");
                    ViewBag.Subtitle = DbRes.T("Messages.ResetPasswordNotReset.Subtitle", "Resources");
                    ViewBag.MessageText = DbRes.T("Messages.ResetPasswordNotReset", "Resources");
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
