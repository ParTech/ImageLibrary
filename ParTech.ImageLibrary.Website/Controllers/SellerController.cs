using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Seller;
using System;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class SellerController : Controller
    {
        private readonly IImageRepository _imageRepository;

        private readonly IObjectRepository _objectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserRepository _userRepository;

        public SellerController(IImageRepository imageRepository, IObjectRepository objectRepository, 
            IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _imageRepository = imageRepository;
            _objectRepository = objectRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        #region DownloadHistory

        //
        // GET: /Seller/DownloadHistory

        [Authorize(Roles = "Seller")]
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
            var downloads = _orderRepository.GetOrderLinesForSeller(userProfile.ProfileID).ToList();

            return View(downloads);
        }

        #endregion

        #region Images, ShowThumbnail and UploadImage methods

        //
        // GET: /Seller/Images

        [Authorize(Roles = "Seller")]
        public ActionResult Images()
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
            
            var sim = new SellerImagesModel
            {
                ListImages = _imageRepository.GetImagesForUser(WebSecurity.GetUserId(User.Identity.Name)),
                ListProducts = _objectRepository.GetProductsForUser(WebSecurity.GetUserId(User.Identity.Name))
            };

            return View(sim);
        }
        
        //
        // GET: /Seller/ShowThumbnail

        [Authorize(Roles = "Seller")]
        public ActionResult ShowThumbnail(int imageid = 0)
        {
            var imageModel = _imageRepository.GetImage(imageid);
            if (imageModel != null)
            {
                return new FileStreamResult(new FileStream(imageModel.Thumbnailpath,FileMode.Open), imageModel.ImageFormat);
            }

            return null;
        }

        //
        // POST: /Seller/UploadImages

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public ActionResult UploadImage(int? product, HttpPostedFileBase upl)
        {
            var status = "error";
            var additionalinfo = string.Empty;
            var startPath = ConfigurationManager.AppSettings.Get("ParTech.StartPathImages");

            int tmpInt;
            var newWidthAsString = ConfigurationManager.AppSettings.Get("ParTech.ThumbnailWidth");
            var newWidth = 0;
            if (int.TryParse(newWidthAsString, out tmpInt))
            {
                newWidth = tmpInt;
            }

            var newHeightAsString = ConfigurationManager.AppSettings.Get("ParTech.ThumbnailHeight");
            var newHeight = 0;
            if (int.TryParse(newHeightAsString, out tmpInt))
            {
                newHeight = tmpInt;
            }

            newWidthAsString = ConfigurationManager.AppSettings.Get("ParTech.SmallThumbnailWidth");
            var newSmallWidth = 0;
            if (int.TryParse(newWidthAsString, out tmpInt))
            {
                newSmallWidth = tmpInt;
            }

            newHeightAsString = ConfigurationManager.AppSettings.Get("ParTech.SmallThumbnailHeight");
            var newSmallHeight = 0;
            if (int.TryParse(newHeightAsString, out tmpInt))
            {
                newSmallHeight = tmpInt;
            }

            if (string.IsNullOrEmpty(startPath) || newWidth == 0 || newHeight == 0 || newSmallWidth == 0 || newSmallHeight == 0)
            {
                additionalinfo = "There are some missing settings.";
            }
            else
            {
                if (product != null && upl != null && upl.ContentLength > 0)
                {
                    //try
                    //{
                        Product productModel = _objectRepository.GetProduct(product);
                        if (productModel != null)
                        {
                            var image = new System.Drawing.Bitmap(upl.InputStream);

                            var thumbnail = new System.Drawing.Bitmap(newWidth, newHeight);
                            using (var gr = System.Drawing.Graphics.FromImage(thumbnail))
                            {
                                gr.SmoothingMode = SmoothingMode.HighQuality;
                                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                gr.DrawImage(image, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));
                            }

                            var smallThumbnail = new System.Drawing.Bitmap(newSmallWidth, newSmallHeight);
                            using (var gr = System.Drawing.Graphics.FromImage(smallThumbnail))
                            {
                                gr.SmoothingMode = SmoothingMode.HighQuality;
                                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                gr.DrawImage(image, new System.Drawing.Rectangle(0, 0, newSmallWidth, newSmallHeight));
                            }

                            var rootDirectory = startPath + DateTime.Now.ToString("yyyyMM");
                            var uploadDateTimeDirectory = rootDirectory + "/" + DateTime.Now.ToString("yyyyMMdd-HHmmss-fff");
                            var filePath = uploadDateTimeDirectory + "/" + Path.GetFileName(upl.FileName);
                            filePath = Server.MapPath(filePath);

                            var thumbnailPath = uploadDateTimeDirectory + "/thumbnail/" + Path.GetFileName(upl.FileName);
                            thumbnailPath = Server.MapPath(thumbnailPath);

                            var smallThumbnailPath = uploadDateTimeDirectory + "/thumbnail/small-" + Path.GetFileName(upl.FileName);
                            smallThumbnailPath = Server.MapPath(smallThumbnailPath);

                            var newImage = new Image
                            {
                                Filepath = filePath,
                                Resolution = image.Height + "x" + image.Width + "px",
                                Thumbnailpath = thumbnailPath,
                                ProductID = productModel.ProductID,
                                ImageFormat =
                                    "image/" + _imageRepository.GetImageTypeEnum(image.RawFormat).ToString().ToLower(),
                                SmallThumbnailpath = smallThumbnailPath
                            };

                            if (_imageRepository.SaveImage(newImage))
                            {
                                if (!Directory.Exists(Server.MapPath(rootDirectory)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(rootDirectory));
                                }

                                if (!Directory.Exists(Server.MapPath(uploadDateTimeDirectory)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(uploadDateTimeDirectory));
                                }

                                if (!Directory.Exists(Server.MapPath(uploadDateTimeDirectory + "/thumbnail")))
                                {
                                    Directory.CreateDirectory(Server.MapPath(uploadDateTimeDirectory + "/thumbnail"));
                                }

                                thumbnail.Save(thumbnailPath);

                                smallThumbnail.Save(smallThumbnailPath);

                                image.Save(filePath);

                                status = "success";
                            }
                            else
                            {
                                additionalinfo = "The meta data of the image could not be saved.";
                            }

                            thumbnail.Dispose();
                            smallThumbnail.Dispose();
                            image.Dispose();
                        }
                        else
                        {
                            additionalinfo = "Unknown product.";
                        }
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                }
                else
                {
                    additionalinfo = "Product and/or upl not specified.";
                }
            }

            return Json(new { status, additionalinfo });
        }

        #endregion

        #region Products, SaveProduct and ShowProduct methods

        //
        // GET: /Seller/Products

        [Authorize(Roles = "Seller")]
        public ActionResult Products(int? productid)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.NewProductFailure:
                        TempData["StatusMessage"] = "Something went wrong. The product could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewProductSuccess:
                        TempData["StatusMessage"] = "The product was added.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var spm = new SellerProductsModel
            {
                ListProducts = _objectRepository.GetProductsForUser(WebSecurity.GetUserId(User.Identity.Name))
            };

            if (productid == null || productid == 0)
            {
                ViewBag.FormHeader = "Add a new product.";
                ViewBag.Legend = "New product";
                ViewBag.ButtonText = "Add product";

                spm.SellerProductModel = new SellerProductModel();
            }
            else
            {
                ViewBag.FormHeader = "Edit a product.";
                ViewBag.Legend = "Edit product";
                ViewBag.ButtonText = "Save product";

                spm.SellerProductModel = _objectRepository.GetProductAndMapToSellerProductModel(productid);
            }

            spm.SellerProductModel.BrandItems = _objectRepository.GetBrands();
            spm.SellerProductModel.CategoryItems = _objectRepository.GetCategories(Thread.CurrentThread.CurrentCulture.LCID);
            spm.SellerProductModel.CollectionItems = _objectRepository.GetCollections();
            spm.SellerProductModel.GenderItems = _objectRepository.GetGenders(Thread.CurrentThread.CurrentCulture.LCID);
            spm.SellerProductModel.SeasonItems = _objectRepository.GetSeasons();

            return View(spm);
        }

        //
        // POST: /Seller/SaveProduct

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveProduct(SellerProductModel productModel)
        {
            var successMessage = MessageIdEnum.NewProductSuccess;
            var failureMessage = MessageIdEnum.NewProductFailure;

            if (productModel.ProductId > 0)
            {
                successMessage = MessageIdEnum.EditProductSuccess;
                failureMessage = MessageIdEnum.EditProductFailure;
            }

            if (ModelState.IsValid)
            {
                if (_objectRepository.SaveProduct(productModel, WebSecurity.GetUserId(User.Identity.Name)))
                {
                    TempData["Message"] = successMessage;
                    return RedirectToAction("Products", "Seller");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("Products", "Seller");
        }

        //
        // GET: /Seller/ShowProduct

        [Authorize(Roles = "Seller")]
        public ActionResult ShowProduct(int productid = 0)
        {
            var productModel = _objectRepository.GetProductAndContext(productid);

            return View(productModel);
        }

        #endregion
    }
}
