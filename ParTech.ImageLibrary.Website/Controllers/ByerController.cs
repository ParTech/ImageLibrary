using System.Linq;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Byer;
using System;
using System.IO;
using System.Web.Mvc;
using ParTech.ImageLibrary.Core.Workers;
using Westwind.Globalization;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class ByerController : Controller
    {
        public ILogger Logger { get; set; }

        private readonly IImageRepository _imageRepository;

        private readonly IObjectRepository _objectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IShoppingCartWorker _shoppingCartWorker;

        private readonly IUserRepository _userRepository;

        public ByerController(IImageRepository imageRepository, IObjectRepository objectRepository, 
            IOrderRepository orderRepository, IShoppingCartWorker shoppingCartWorker,
            IUserRepository userRepository)
        {
            _imageRepository = imageRepository;
            _objectRepository = objectRepository;
            _orderRepository = orderRepository;
            _shoppingCartWorker = shoppingCartWorker;
            _userRepository = userRepository;
        }
        
        #region Brands, Collections and Images methods 

        //
        // GET: /Byer/Brands

        [Authorize(Roles = "Byer")]
        public ActionResult Brands()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var bbm = new ByerBrandsModel
            {
                ListBrands = _objectRepository.GetBrandsAndContext()
            };

            return View(bbm);
        }

        //
        // GET: /Byer/Collections

        [Authorize(Roles = "Byer")]
        public ActionResult Collections()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var bcm = new ByerCollectionsModel
            {
                ListCollections = _objectRepository.GetCollectionsAndContext()
            };

            return View(bcm);
        }
        //
        // GET: /Byer/Images

        [Authorize(Roles = "Byer")]
        public ActionResult Images()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.CreateDownloadCartFailed:
                        TempData["StatusMessage"] = DbRes.T("Messages.CreateDownloadCartFailed", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.CreateDownloadCartSuccess:
                        TempData["StatusMessage"] = string.Format(DbRes.T("Messages.CreateDownloadCartSuccess", "Resources"), 
                                                                  "<a href='/byer/downloadzipfile'>", 
                                                                  "</a>");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.DeleteCartFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.DeleteCartFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.DeleteCartSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.DeleteCartSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.DeleteCartItemFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.DeleteCartItemFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.DeleteCartItemSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.DeleteCartItemSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.DownloadZipFileFailed:
                        TempData["StatusMessage"] = DbRes.T("Messages.DownloadZipFileFailed", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var sim = new ByerImagesModel
            {
                ListImages = _imageRepository.GetImages()
            };

            return View(sim);
        }

        #endregion

        #region DownloadHistory

        //
        // GET: /Byer/DownloadHistory

        [Authorize(Roles = "Byer")]
        public ActionResult DownloadHistory()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var userProfile = _userRepository.GetUserProfileByName(User.Identity.Name);
            var downloads = _orderRepository.GetOrderLinesForByer(userProfile.ProfileID).ToList();

            return View(downloads);
        }

        #endregion

        #region ShowBrand, ShowCollection, ShowProduct, ShowSmallThumbnail and ShowThumbnail methods

        //
        // GET: /Byer/ShowBrand

        [Authorize(Roles = "Byer")]
        public ActionResult ShowBrand(int brandid = 0)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.AddImagesToCartFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.AddImagesToCartFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.AddImagesToCartSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.AddImagesToCartSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }
            
            var brandModel = _objectRepository.GetBrandAndContext(brandid);

            return View(brandModel);
        }

        //
        // GET: /Byer/ShowCollection

        [Authorize(Roles = "Byer")]
        public ActionResult ShowCollection(int collectionid = 0)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.AddImagesToCartFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.AddImagesToCartFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.AddImagesToCartSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.AddImagesToCartSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var collectionModel = _objectRepository.GetCollectionAndContext(collectionid);

            return View(collectionModel);
        }

        //
        // GET: /Byer/ShowProduct

        [Authorize(Roles = "Byer")]
        public ActionResult ShowProduct(int productid = 0)
        {
            var productModel = _objectRepository.GetProductAndContext(productid);

            return View(productModel);
        }

        //
        // GET: /Byer/ShowSmallThumbnail

        [Authorize(Roles = "Byer")]
        public ActionResult ShowSmallThumbnail(int imageid = 0)
        {
            var imageModel = _imageRepository.GetImage(imageid);
            if (imageModel != null)
            {
                return new FileStreamResult(new FileStream(imageModel.SmallThumbnailpath, FileMode.Open), imageModel.ImageFormat);
            }

            return null;
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

        #region ShoppingCart methods

        //
        // GET: /Byer/AddBrandImagesToShoppingCart

        [Authorize(Roles = "Byer")]
        public ActionResult AddBrandImagesToShoppingCart(int brandid = 0)
        {
            TempData["Message"] = _shoppingCartWorker.AddBrandImagesToShoppingCart(brandid);
            return RedirectToAction("ShowBrand", "Byer", new { brandid });
        }

        //
        // GET: /Byer/AddCollectionImagesToShoppingCart

        [Authorize(Roles = "Byer")]
        public ActionResult AddCollectionImagesToShoppingCart(int collectionid = 0)
        {
            TempData["Message"] = _shoppingCartWorker.AddCollectionImagesToShoppingCart(collectionid);
            return RedirectToAction("ShowCollection", "Byer", new { collectionid });
        }
        //
        // GET: /Byer/AddToShoppingCart

        [Authorize(Roles = "Byer")]
        public ActionResult AddToShoppingCart(int imageid = 0)
        {
            var cart = _shoppingCartWorker.AddImageToCart(imageid);

            return Json(cart, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Byer/CreateDownloadForImagesInCart

        [Authorize(Roles = "Byer")]
        public ActionResult CreateDownloadForImagesInCart()
        {
            TempData["Message"] = _shoppingCartWorker.CreateDowload();
            return RedirectToAction("Images", "Byer");
        }

        //
        // GET: /Byer/DeleteCart

        [Authorize(Roles = "Byer")]
        public ActionResult DeleteCart()
        {
            TempData["Message"] = _shoppingCartWorker.DeleteCart();
            return RedirectToAction("Images", "Byer");
        }

        //
        // GET: /Byer/DeleteCartItem

        [Authorize(Roles = "Byer")]
        public ActionResult DeleteCartItem(int imageid = 0)
        {
            TempData["Message"] = _shoppingCartWorker.DeleteCartItem(imageid);
            return RedirectToAction("Images", "Byer");
        }

        //
        // GET: /Byer/DownloadZipFile

        [Authorize(Roles = "Byer")]
        public ActionResult DownloadZipFile()
        {
            if (Session != null && Session["ZipFileStream"] != null)
            {
                var zipFileStream = (MemoryStream)Session["ZipFileStream"];

                var fileStreamResult = new FileStreamResult(zipFileStream, "application/zip")
                {
                    FileDownloadName = "image-library-download-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".zip"
                };

                Session["ZipFileStream"] = null;
                Session.Remove("ZipFileStream");

                return fileStreamResult;
            }

            TempData["Message"] = MessageIdEnum.DownloadZipFileFailed;
            return RedirectToAction("Images", "Byer");
        }

        //
        // GET: /Byer/GetShoppingCart

        [Authorize(Roles = "Byer")]
        public ActionResult GetShoppingCart()
        {
            var cart = _shoppingCartWorker.GetCart();

            return Json(cart, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
