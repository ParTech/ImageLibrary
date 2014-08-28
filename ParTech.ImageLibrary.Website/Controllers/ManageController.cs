using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Manage;
using System.Threading;
using System.Web.Mvc;
using Westwind.Globalization;

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
                        TempData["StatusMessage"] = DbRes.T("Messages.EditBrandFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditBrandSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditBrandSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewBrandFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditAccountFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewBrandSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewBrandSuccess", "Resources");
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
                ViewBag.FormHeader = DbRes.T("ManageController.AddBrandForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.AddBrandForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.AddBrandForm.BtnSubmit", "Resources");
                
                mbm.BrandModel = new Brand();
            }
            else
            {
                ViewBag.FormHeader = DbRes.T("ManageController.EditBrandForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.EditBrandForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.EditBrandForm.BtnSubmit", "Resources");

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
                        TempData["StatusMessage"] = DbRes.T("Messages.EditCategoryFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditCategorySuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditCategorySuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewCategoryFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewCategoryFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewCategorySuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewCategorySuccess", "Resources");
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
                Languages = _objectRepository.GetLanguages(),
                ListCategories = _objectRepository.GetCategories()
            };

            if (categoryid == null || categoryid == 0)
            {
                ViewBag.FormHeader = DbRes.T("ManageController.AddCategoryForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.AddCategoryForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.AddCategoryForm.BtnSubmit", "Resources");

                mbm.CategoryModel = new CategoryModel(mbm.Languages);
            }
            else
            {
                ViewBag.FormHeader = DbRes.T("ManageController.EditCategoryForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.EditCategoryForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.EditCategoryForm.BtnSubmit", "Resources");

                mbm.CategoryModel = _objectRepository.GetCategoryAndMapToCategoryModel(categoryid);
            }

            return View(mbm);
        }

        //
        // POST: /Categories/SaveCategory

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveCategory(CategoryModel categoryModel)
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
                        TempData["StatusMessage"] = DbRes.T("Messages.EditCollectionFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditCollectionSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditCollectionSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewCollectionFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewCollectionFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewCollectionSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewCollectionSuccess", "Resources");
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
                ViewBag.FormHeader = DbRes.T("ManageController.AddCollectionForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.AddCollectionForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.AddCollectionForm.BtnSubmit", "Resources");

                mbm.CollectionModel = new Collection();
            }
            else
            {
                ViewBag.FormHeader = DbRes.T("ManageController.EditCollectionForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.EditCollectionForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.EditCollectionForm.BtnSubmit", "Resources");

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
                        TempData["StatusMessage"] = DbRes.T("Messages.EditGenderFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditGenderSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditGenderSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewGenderFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewGenderFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewGenderSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewGenderSuccess", "Resources");
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
                Languages = _objectRepository.GetLanguages(),
                ListGenders = _objectRepository.GetGenders()
            };

            if (genderid == null || genderid == 0)
            {
                ViewBag.FormHeader = DbRes.T("ManageController.AddGenderForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.AddGenderForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.AddGenderForm.BtnSubmit", "Resources");

                mbm.GenderModel = new GenderModel(mbm.Languages);
            }
            else
            {
                ViewBag.FormHeader = DbRes.T("ManageController.EditGenderForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.EditGenderForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.EditGenderForm.BtnSubmit", "Resources");

                mbm.GenderModel = _objectRepository.GetGenderAndMapToCategoryModel(genderid);
            }

            return View(mbm);
        }

        //
        // POST: /Genders/SaveGender

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveGender(GenderModel genderModel)
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
                        TempData["StatusMessage"] = DbRes.T("Messages.EditSeasonFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.EditSeasonSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.EditSeasonSuccess", "Resources");
                        TempData["StatusMessageClass"] = "message-success";
                        break;
                    case MessageIdEnum.NewSeasonFailure:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewSeasonFailure", "Resources");
                        TempData["StatusMessageClass"] = "message-error";
                        break;
                    case MessageIdEnum.NewSeasonSuccess:
                        TempData["StatusMessage"] = DbRes.T("Messages.NewSeasonSuccess", "Resources");
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
                Languages = _objectRepository.GetLanguages(),
                ListSeasons = _objectRepository.GetSeasons()
            };

            if (seasonid == null || seasonid == 0)
            {
                ViewBag.FormHeader = DbRes.T("ManageController.AddSeasonForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.AddSeasonForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.AddSeasonForm.BtnSubmit", "Resources");

                mbm.SeasonModel = new SeasonModel(mbm.Languages);
            }
            else
            {
                ViewBag.FormHeader = DbRes.T("ManageController.EditSeasonForm.Header", "Resources");
                ViewBag.Legend = DbRes.T("ManageController.EditSeasonForm.Legend", "Resources");
                ViewBag.ButtonText = DbRes.T("ManageController.EditSeasonForm.BtnSubmit", "Resources");

                mbm.SeasonModel = _objectRepository.GetSeasonAndMapToCategoryModel(seasonid);
            }

            return View(mbm);
        }

        //
        // POST: /Seasons/SaveSeason

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seller")]
        public ActionResult SaveSeason(SeasonModel seasonModel)
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
