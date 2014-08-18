using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.General;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Workers;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class GeneralController : Controller
    {
        private readonly IImageRepository _imageRepository;

        private readonly ILuceneWorker _luceneWorker;

        public GeneralController(IImageRepository imageRepository, ILuceneWorker luceneWorker)
        {
            _imageRepository = imageRepository;
            _luceneWorker = luceneWorker;
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
                        TempData["StatusMessage"] = "Something went wrong. The search could not be executed.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.SearchSuccess:
                        TempData["StatusMessage"] = string.Format("Found {0} product(s) on '{1}'.",
                            searchModel.FoundProducts.Count, searchModel.SearchString);
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
