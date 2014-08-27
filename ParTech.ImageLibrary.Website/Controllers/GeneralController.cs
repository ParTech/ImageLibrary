using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.General;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Workers;
using Westwind.Globalization;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class GeneralController : Controller
    {
        private readonly IImageRepository _imageRepository;

        private readonly ILuceneWorker _luceneWorker;

        private readonly IObjectRepository _objectRepository;

        public GeneralController(IImageRepository imageRepository, ILuceneWorker luceneWorker,
            IObjectRepository objectRepository)
        {
            _imageRepository = imageRepository;
            _luceneWorker = luceneWorker;
            _objectRepository = objectRepository;
        }
        
        //
        // GET: /General/ChangeLanguage

        public ActionResult ChangeLanguage(string lang, string returnUrl)
        {
            var langCookie = new HttpCookie("locale", lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);

            returnUrl = HttpUtility.UrlDecode(returnUrl);
            if (!string.IsNullOrEmpty(returnUrl) 
                && !returnUrl.StartsWith(string.Concat("/", CultureInfo.CurrentUICulture.Name.ToLower()))
                && !returnUrl.StartsWith(string.Concat("/", lang)))
            {
                returnUrl = string.Concat("/", lang, returnUrl);
            }

            return Redirect(returnUrl);
        }

        //
        // POST: /General/SaveProfile

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Byer,Seller")]
        public ActionResult Search(SearchModel searchModel)
        {
            const MessageIdEnum successMessage = MessageIdEnum.SearchSuccess;
            const MessageIdEnum failureMessage = MessageIdEnum.SearchFailure;
            searchModel.FoundProducts = new List<Product>();

            if (ModelState.IsValid)
            {
                var tmpFoundProducts = _luceneWorker.SearchDefault(searchModel.SearchString, searchModel.FieldName).ToList();
                if (tmpFoundProducts != null)
                {
                    foreach (var tmpFoundProduct in tmpFoundProducts)
                    {
                        tmpFoundProduct.Images =
                            _imageRepository.GetImagesForProduct(tmpFoundProduct.ProductID).ToList();
                    }
                    
                    TempData["Message"] = successMessage;
                    searchModel.FoundProducts = tmpFoundProducts;
                }
            }

            if (searchModel.FoundProducts == null)
            {
                // If we got this far, something failed, display failure message
                TempData["Message"] = failureMessage;
            }

            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.SearchFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.SearchFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.SearchSuccess:
                        TempData["StatusMessage"] = string.Format(DbRes.T("Messages.SearchSuccess", "Resources"),
                                                                  searchModel.FoundProducts.Count, 
                                                                  searchModel.SearchString);
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            return View(searchModel);
        }
    }
}
