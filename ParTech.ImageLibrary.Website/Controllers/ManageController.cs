using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Manage;
using System.Threading;
using System.Web.Mvc;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class ManageController : Controller
    {
        private readonly IObjectRepository _objectRepository;

        public ManageController(IObjectRepository objectRepository)
        {
            _objectRepository = objectRepository;
        }

        //
        // GET: /Index/

        [Authorize(Roles = "Seller")]
        public ActionResult Index()
        {
            return View();
        }

        #region Brands and SaveBrand methods

        //
        // GET: /Brands/

        [Authorize(Roles = "Seller")]
        public ActionResult Brands(int? brandid)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.EditBrandFailure:
                        TempData["StatusMessage"] = "Something went wrong. The brand could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditBrandSuccess:
                        TempData["StatusMessage"] = "The brand was saved.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewBrandFailure:
                        TempData["StatusMessage"] = "Something went wrong. The brand could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewBrandSuccess:
                        TempData["StatusMessage"] = "The brand was added.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var mbm = new ManageBrandModel
            {
                ListBrands = _objectRepository.GetBrands()
            };

            if (brandid == null || brandid == 0)
            {
                ViewBag.FormHeader = "Add a new brand.";
                ViewBag.Legend = "New brand";
                ViewBag.ButtonText = "Add brand";
                
                mbm.BrandModel = new Brand();
            }
            else
            {
                ViewBag.FormHeader = "Edit a brand.";
                ViewBag.Legend = "Edit brand";
                ViewBag.ButtonText = "Save brand";

                mbm.BrandModel = _objectRepository.GetBrand(brandid);
            }

            return View(mbm);
        }

        //
        // POST: /Brands/SaveBrand

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveBrand(Brand brandModel)
        {
            var successMessage = MessageIdEnum.NewBrandSuccess;
            var failureMessage = MessageIdEnum.NewBrandFailure;

            if (brandModel.BrandID > 0)
            {
                successMessage = MessageIdEnum.EditBrandSuccess;
                failureMessage = MessageIdEnum.EditBrandFailure;
            }

            if (ModelState.IsValid)
            {
                if (_objectRepository.SaveBrand(brandModel))
                {
                    TempData["Message"] = successMessage;
                    return RedirectToAction("Brands", "Manage");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("Brands", "Manage");
        }

        #endregion

        #region Categories and SaveCategory methods

        //
        // GET: /Categories/

        [Authorize(Roles = "Seller")]
        public ActionResult Categories(int? categoryid)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.EditCategoryFailure:
                        TempData["StatusMessage"] = "Something went wrong. The category could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditCategorySuccess:
                        TempData["StatusMessage"] = "The category was saved.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewCategoryFailure:
                        TempData["StatusMessage"] = "Something went wrong. The category could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewCategorySuccess:
                        TempData["StatusMessage"] = "The category was added.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var mbm = new ManageCategoryModel
            {
                ListCategories = _objectRepository.GetCategories(Thread.CurrentThread.CurrentCulture.LCID)
            };

            if (categoryid == null || categoryid == 0)
            {
                ViewBag.FormHeader = "Add a new Category.";
                ViewBag.Legend = "New Category";
                ViewBag.ButtonText = "Add Category";

                mbm.CategoryModel = new Category();
            }
            else
            {
                ViewBag.FormHeader = "Edit a Category.";
                ViewBag.Legend = "Edit Category";
                ViewBag.ButtonText = "Save Category";

                mbm.CategoryModel = _objectRepository.GetCategory(categoryid);
            }

            return View(mbm);
        }

        //
        // POST: /Categories/SaveCategory

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveCategory(Category categoryModel)
        {
            var successMessage = MessageIdEnum.NewCategorySuccess;
            var failureMessage = MessageIdEnum.NewCategoryFailure;

            if (categoryModel.CategoryID > 0)
            {
                successMessage = MessageIdEnum.EditCategorySuccess;
                failureMessage = MessageIdEnum.EditCategoryFailure;
            }

            if (ModelState.IsValid)
            {
                if (_objectRepository.SaveCategory(categoryModel))
                {
                    TempData["Message"] = successMessage;
                    return RedirectToAction("Categories", "Manage");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("Categories", "Manage");
        }

        #endregion

        #region Collections and SaveCollection methods
        //
        // GET: /Collections/

        [Authorize(Roles = "Seller")]
        public ActionResult Collections(int? collectionid)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.EditCollectionFailure:
                        TempData["StatusMessage"] = "Something went wrong. The collection could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditCollectionSuccess:
                        TempData["StatusMessage"] = "The collection was saved.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewCollectionFailure:
                        TempData["StatusMessage"] = "Something went wrong. The collection could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewCollectionSuccess:
                        TempData["StatusMessage"] = "The collection was added.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var mbm = new ManageCollectionModel
            {
                ListCollections = _objectRepository.GetCollections()
            };

            if (collectionid == null || collectionid == 0)
            {
                ViewBag.FormHeader = "Add a new collection.";
                ViewBag.Legend = "New collection";
                ViewBag.ButtonText = "Add collection";

                mbm.CollectionModel = new Collection();
            }
            else
            {
                ViewBag.FormHeader = "Edit a collection.";
                ViewBag.Legend = "Edit collection";
                ViewBag.ButtonText = "Save collection";

                mbm.CollectionModel = _objectRepository.GetCollection(collectionid);
            }

            return View(mbm);
        }

        //
        // POST: /Collections/SaveCollection

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveCollection(Collection collectionModel)
        {
            var successMessage = MessageIdEnum.NewCollectionSuccess;
            var failureMessage = MessageIdEnum.NewCollectionFailure;

            if (collectionModel.CollectionID > 0)
            {
                successMessage = MessageIdEnum.EditCollectionSuccess;
                failureMessage = MessageIdEnum.EditCollectionFailure;
            }

            if (ModelState.IsValid)
            {
                if (_objectRepository.SaveCollection(collectionModel))
                {
                    TempData["Message"] = successMessage;
                    return RedirectToAction("Collections", "Manage");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("Collections", "Manage");
        }

        #endregion

        #region Genders, SaveGender methods

        //
        // GET: /Genders/

        [Authorize(Roles = "Seller")]
        public ActionResult Genders(int? genderid)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.EditGenderFailure:
                        TempData["StatusMessage"] = "Something went wrong. The gender could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditGenderSuccess:
                        TempData["StatusMessage"] = "The gender was saved.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewGenderFailure:
                        TempData["StatusMessage"] = "Something went wrong. The gender could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewGenderSuccess:
                        TempData["StatusMessage"] = "The gender was added.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var mbm = new ManageGenderModel
            {
                ListGenders = _objectRepository.GetGenders(Thread.CurrentThread.CurrentCulture.LCID)
            };

            if (genderid == null || genderid == 0)
            {
                ViewBag.FormHeader = "Add a new gender.";
                ViewBag.Legend = "New gender";
                ViewBag.ButtonText = "Add gender";

                mbm.GenderModel = new Gender();
            }
            else
            {
                ViewBag.FormHeader = "Edit a gender.";
                ViewBag.Legend = "Edit gender";
                ViewBag.ButtonText = "Save gender";

                mbm.GenderModel = _objectRepository.GetGender(genderid);
            }

            return View(mbm);
        }

        //
        // POST: /Genders/SaveGender

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveGender(Gender genderModel)
        {
            var successMessage = MessageIdEnum.NewGenderSuccess;
            var failureMessage = MessageIdEnum.NewGenderFailure;

            if (genderModel.GenderID > 0)
            {
                successMessage = MessageIdEnum.EditGenderSuccess;
                failureMessage = MessageIdEnum.EditGenderFailure;
            }

            if (ModelState.IsValid)
            {
                if (_objectRepository.SaveGender(genderModel))
                {
                    TempData["Message"] = successMessage;
                    return RedirectToAction("Genders", "Manage");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("Genders", "Manage");
        }

        #endregion
        
        #region Seasons and SaveSeason methods
        //
        // GET: /Seasons/

        [Authorize(Roles = "Seller")]
        public ActionResult Seasons(int? seasonid)
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    case MessageIdEnum.EditSeasonFailure:
                        TempData["StatusMessage"] = "Something went wrong. The season could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditSeasonSuccess:
                        TempData["StatusMessage"] = "The season was saved.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewSeasonFailure:
                        TempData["StatusMessage"] = "Something went wrong. The season could not be saved.";
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewSeasonSuccess:
                        TempData["StatusMessage"] = "The season was added.";
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var mbm = new ManageSeasonModel
            {
                ListSeasons = _objectRepository.GetSeasons()
            };

            if (seasonid == null || seasonid == 0)
            {
                ViewBag.FormHeader = "Add a new season.";
                ViewBag.Legend = "New season";
                ViewBag.ButtonText = "Add season";

                mbm.SeasonModel = new Season();
            }
            else
            {
                ViewBag.FormHeader = "Edit a season.";
                ViewBag.Legend = "Edit season";
                ViewBag.ButtonText = "Save season";

                mbm.SeasonModel = _objectRepository.GetSeason(seasonid);
            }

            return View(mbm);
        }

        //
        // POST: /Seasons/SaveSeason

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveSeason(Season seasonModel)
        {
            var successMessage = MessageIdEnum.NewSeasonSuccess;
            var failureMessage = MessageIdEnum.NewSeasonFailure;

            if (seasonModel.SeasonID > 0)
            {
                successMessage = MessageIdEnum.EditSeasonSuccess;
                failureMessage = MessageIdEnum.EditSeasonFailure;
            }

            if (ModelState.IsValid)
            {
                if (_objectRepository.SaveSeason(seasonModel))
                {
                    TempData["Message"] = successMessage;
                    return RedirectToAction("Seasons", "Manage");
                }
            }

            // If we got this far, something failed, display failure message
            TempData["Message"] = failureMessage;
            return RedirectToAction("Seasons", "Manage");
        }

        #endregion
    }
}
