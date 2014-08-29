using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Utils;
using ParTech.ImageLibrary.Core.ViewModels.Manage;
using ParTech.ImageLibrary.Core.ViewModels.Seller;
using ParTech.ImageLibrary.Core.Workers;

namespace ParTech.ImageLibrary.Core.Repositories
{
    public interface IObjectRepository: IRepository
    {
        #region Brand

        Brand GetBrand(int? brandid);

        Brand GetBrandAndContext(int? brandid);

        IEnumerable<Brand> GetBrands();

        IEnumerable<Brand> GetBrandsAndContext();

        bool SaveBrand(Brand brand);

        #endregion

        #region Category

        Category GetCategory(int? categoryid);

        CategoryModel GetCategoryAndMapToCategoryModel(int? categoryid);

        IEnumerable<Category> GetCategories();

        IEnumerable<KeyValuePair<int, string>> GetCategoriesAndMapToSelectList();

        bool SaveCategory(CategoryModel category);

        #endregion

        #region Collection

        Collection GetCollection(int? collectionid);

        Collection GetCollectionAndContext(int? collectionid);

        IEnumerable<Collection> GetCollections();

        IEnumerable<Collection> GetCollectionsAndContext();

        bool SaveCollection(Collection collection);

        #endregion

        #region Country

        Country GetCountry(int? countryid);

        Country GetCountryAndContext(int? countryid);

        IEnumerable<Country> GetCountries();

        IEnumerable<Country> GetCountriesAndContext();

        bool SaveCountry(Country country);

        #endregion

        #region Gender

        Gender GetGender(int? genderid);

        GenderModel GetGenderAndMapToCategoryModel(int? genderid);

        IEnumerable<Gender> GetGenders();

        IEnumerable<KeyValuePair<int, string>> GetGendersAndMapToSelectList();

        bool SaveGender(GenderModel gender);

        #endregion

        #region Language

        Language GetLanguage(int? languageid);

        Language GetLanguageAndContext(int? languageid);

        IEnumerable<Language> GetLanguages();

        IEnumerable<Language> GetLanguagesAndContext();

        bool SaveLanguage(Language language);

        #endregion

        #region LoggedEvents

        LoggedEvent GetLoggedEvent(int loggedEventId);

        IEnumerable<LoggedEvent> GetLoggedEvents();

        LoggedEvent SaveLoggedEvent(LoggedEvent loggedEvent);

        #endregion

        #region Product

        Product GetProduct(int? productid);

        Product GetProductAndContext(int? productid);

        SellerProductModel GetProductAndMapToSellerProductModel(int? productid);

        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetProductsAndContext();

        IEnumerable<Product> GetProductsForUser(int userid);

        bool SaveProduct(SellerProductModel product, int userid);

        #endregion

        #region Salutation

        Salutation GetSalutation(int? salutationid);

        Salutation GetSalutationAndContext(int? salutationid);

        IEnumerable<Salutation> GetSalutations();

        IEnumerable<Salutation> GetSalutationsAndContext();

        bool SaveSalutation(Salutation salutation);

        #endregion

        #region Season

        Season GetSeason(int? seasonid);

        SeasonModel GetSeasonAndMapToCategoryModel(int? seasonid);

        IEnumerable<Season> GetSeasons();

        IEnumerable<KeyValuePair<int, string>> GetSeasonsAndMapToSelectList();

        bool SaveSeason(SeasonModel season);

        #endregion

    }

    public class ObjectRepository : IObjectRepository
    {
        public ILogger Logger { get; set; }

        private readonly ILuceneWorker _luceneWorker;

        public ObjectRepository(ILuceneWorker luceneWorker)
        {
            _luceneWorker = luceneWorker;
        }

        #region Brand

        public Brand GetBrand(int? brandid)
        {
            Brand brand = null;

            try
            {
                if (brandid != null)
                {
                    using (var db = new Entities())
                    {
                        brand = db.Brands.SingleOrDefault(i => i.BrandID == brandid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetBrand - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return brand;
        }

        public Brand GetBrandAndContext(int? brandid)
        {
            Brand brand = null;

            try
            {
                if (brandid != null)
                {
                    using (var db = new Entities())
                    {
                        brand = db.Brands.Where(i => i.BrandID == brandid)
                                         .Include("Products")
                                         .Include("Products.Images")
                                         .SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetBrandAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return brand;
        }

        public IEnumerable<Brand> GetBrands()
        {
            var brands = new List<Brand>();

            try
            {
                using (var db = new Entities())
                {
                    brands = db.Brands.OrderBy(i => i.Name)
                                      .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetBrands - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return brands;
        }

        public IEnumerable<Brand> GetBrandsAndContext()
        {
            var brands = new List<Brand>();

            try
            {
                using (var db = new Entities())
                {
                    brands = db.Brands.Include("Products")
                                      .OrderBy(i => i.Name)
                                      .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetBrandsAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return brands;
        }

        public bool SaveBrand(Brand brand)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (brand.BrandID > 0)
                    {
                        var tmpBrand = db.Brands.SingleOrDefault(i => i.BrandID == brand.BrandID);
                        if (tmpBrand != null)
                        {
                            tmpBrand.Description = brand.Description;
                            tmpBrand.Name = brand.Name;
                            tmpBrand.updated = DateTime.Now;

                            UpdateProductsInIndex(tmpBrand.Products);
                        }
                    }
                    else
                    {
                        brand.updated = DateTime.Now;
                        brand.created = DateTime.Now;

                        db.Brands.Add(brand);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveBrand - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region Category

        public Category GetCategory(int? categoryid)
        {
            Category category = null;

            try
            {
                if (categoryid != null)
                {
                    using (var db = new Entities())
                    {
                        category = db.Categories.SingleOrDefault(i => i.CategoryID == categoryid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCategory - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return category;
        }

        public CategoryModel GetCategoryAndMapToCategoryModel(int? categoryid)
        {
            CategoryModel categoryModel = null;

            try
            {
                if (categoryid != null)
                {
                    using (var db = new Entities())
                    {
                        var category = db.Categories.SingleOrDefault(i => i.CategoryID == categoryid);
                        if (category != null)
                        {
                            var stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(category.Name));
                            var serializer = new DataContractJsonSerializer(typeof(MultiLingualListModel));
                            var jsonResult = (MultiLingualListModel)serializer.ReadObject(stream);

                            categoryModel = new CategoryModel
                            {
                                CategoryID = category.CategoryID,
                                Name = jsonResult
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCategoryAndMapToCategoryModel - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return categoryModel;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>();

            try
            {
                using (var db = new Entities())
                {
                    categories = db.Categories.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCategories - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return categories;
        }

        public IEnumerable<KeyValuePair<int, string>> GetCategoriesAndMapToSelectList()
        {
            var categoriesList = new List<KeyValuePair<int, string>>();

            try
            {
                using (var db = new Entities())
                {
                    var categories = db.Categories.OrderBy(i => i.Name)
                                                  .ToList();
                    if (categories.Any())
                    {
                        categoriesList.AddRange(categories.Select(category => new KeyValuePair<int, string>(category.CategoryID, LanguageString.GetStringForCurrentLanguage(category.Name))));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCategoriesAndMapToSelectList - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return categoriesList;
        }

        public bool SaveCategory(CategoryModel categoryModel)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (categoryModel.CategoryID > 0)
                    {
                        var tmpCategory = db.Categories.SingleOrDefault(i => i.CategoryID == categoryModel.CategoryID);
                        if (tmpCategory != null)
                        {
                            tmpCategory.Name = SerializeMultiLingualListModelToJson(categoryModel.Name);
                            tmpCategory.updated = DateTime.Now;

                            UpdateProductsInIndex(tmpCategory.Products);
                        }
                    }
                    else
                    {
                        var category = new Category
                        {
                            Name = SerializeMultiLingualListModelToJson(categoryModel.Name),
                            updated = DateTime.Now,
                            created = DateTime.Now
                        };

                        db.Categories.Add(category);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveCategory - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region Collection

        public Collection GetCollection(int? collectionid)
        {
            Collection collection = null;

            try
            {
                if (collectionid != null)
                {
                    using (var db = new Entities())
                    {
                        collection = db.Collections.SingleOrDefault(i => i.CollectionID == collectionid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCollection - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return collection;
        }

        public Collection GetCollectionAndContext(int? collectionid)
        {
            Collection collection = null;

            try
            {
                if (collectionid != null)
                {
                    using (var db = new Entities())
                    {
                        collection = db.Collections.Where(i => i.CollectionID == collectionid)
                                                   .Include("Products")
                                                   .Include("Products.Images")
                                                   .SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCollectionAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return collection;
        }

        public IEnumerable<Collection> GetCollections()
        {
            var collections = new List<Collection>();

            try
            {
                using (var db = new Entities())
                {
                    collections = db.Collections.OrderBy(i => i.Name)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCollections - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return collections;
        }

        public IEnumerable<Collection> GetCollectionsAndContext()
        {
            var collections = new List<Collection>();

            try
            {
                using (var db = new Entities())
                {
                    collections = db.Collections.Include("Products")
                                                .OrderBy(i => i.Name)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCollectionsAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return collections;
        }
        
        public bool SaveCollection(Collection collection)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (collection.CollectionID > 0)
                    {
                        var tmpCollection = db.Collections.SingleOrDefault(i => i.CollectionID == collection.CollectionID);
                        if (tmpCollection != null)
                        {
                            tmpCollection.Name = collection.Name;
                            tmpCollection.updated = DateTime.Now;

                            UpdateProductsInIndex(tmpCollection.Products);
                        }
                    }
                    else
                    {
                        collection.updated = DateTime.Now;
                        collection.created = DateTime.Now;

                        db.Collections.Add(collection);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveCollection - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region Country

        public Country GetCountry(int? countryid)
        {
            Country country = null;

            try
            {
                if (countryid != null)
                {
                    using (var db = new Entities())
                    {
                        country = db.Countries.SingleOrDefault(i => i.CountryID == countryid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCountry - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return country;
        }

        public Country GetCountryAndContext(int? countryid)
        {
            Country country = null;

            try
            {
                if (countryid != null)
                {
                    using (var db = new Entities())
                    {
                        country = db.Countries.Where(i => i.CountryID == countryid)
                                              .Include("Profiles")
                                              .SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCountryAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return country;
        }

        public IEnumerable<Country> GetCountries()
        {
            var countrys = new List<Country>();

            try
            {
                using (var db = new Entities())
                {
                    countrys = db.Countries.OrderBy(i => i.Name)
                                           .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCountries - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return countrys;
        }

        public IEnumerable<Country> GetCountriesAndContext()
        {
            var countrys = new List<Country>();

            try
            {
                using (var db = new Entities())
                {
                    countrys = db.Countries.Include("Profiles")
                                          .OrderBy(i => i.Name)
                                          .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCountriesAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return countrys;
        }

        public bool SaveCountry(Country country)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (country.CountryID > 0)
                    {
                        var tmpCountry = db.Countries.SingleOrDefault(i => i.CountryID == country.CountryID);
                        if (tmpCountry != null)
                        {
                            tmpCountry.Name = country.Name;
                        }
                    }
                    else
                    {
                        db.Countries.Add(country);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveCountry - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region Gender

        public Gender GetGender(int? genderid)
        {
            Gender gender = null;

            try
            {
                if (genderid != null)
                {
                    using (var db = new Entities())
                    {
                        gender = db.Genders.SingleOrDefault(i => i.GenderID == genderid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetGender - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return gender;
        }

        public GenderModel GetGenderAndMapToCategoryModel(int? genderid)
        {
            GenderModel genderModel = null;

            try
            {
                if (genderid != null)
                {
                    using (var db = new Entities())
                    {
                        var gender = db.Genders.SingleOrDefault(i => i.GenderID == genderid);
                        if (gender != null)
                        {
                            var stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(gender.Name));
                            var serializer = new DataContractJsonSerializer(typeof(MultiLingualListModel));
                            var jsonResult = (MultiLingualListModel)serializer.ReadObject(stream);

                            genderModel = new GenderModel
                            {
                                GenderID = gender.GenderID,
                                Name = jsonResult
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetGenderAndMapToCategoryModel - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return genderModel;
        }

        public IEnumerable<Gender> GetGenders()
        {
            var genders = new List<Gender>();

            try
            {
                using (var db = new Entities())
                {
                    genders = db.Genders.OrderBy(j => j.Name)
                                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetGenders - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return genders;
        }

        public IEnumerable<KeyValuePair<int, string>> GetGendersAndMapToSelectList()
        {
            var gendersList = new List<KeyValuePair<int, string>>();

            try
            {
                using (var db = new Entities())
                {
                    var genders = db.Genders.OrderBy(i => i.Name)
                                            .ToList();
                    if (genders.Any())
                    {
                        gendersList.AddRange(genders.Select(gender => new KeyValuePair<int, string>(gender.GenderID, LanguageString.GetStringForCurrentLanguage(gender.Name))));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetGendersAndMapToSelectList - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return gendersList;
        }

        public bool SaveGender(GenderModel genderModel)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (genderModel.GenderID > 0)
                    {
                        var tmpGender = db.Genders.SingleOrDefault(i => i.GenderID == genderModel.GenderID);
                        if (tmpGender != null)
                        {
                            tmpGender.Name = SerializeMultiLingualListModelToJson(genderModel.Name);
                            tmpGender.updated = DateTime.Now;
                        }
                    }
                    else
                    {
                        var gender = new Gender
                        {
                            Name = SerializeMultiLingualListModelToJson(genderModel.Name),
                            updated = DateTime.Now,
                            created = DateTime.Now
                        };

                        db.Genders.Add(gender);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveGender - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region Language

        public Language GetLanguage(int? languageid)
        {
            Language language = null;

            try
            {
                if (languageid != null)
                {
                    using (var db = new Entities())
                    {
                        language = db.Languages.SingleOrDefault(i => i.LanguageID == languageid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetLanguage - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return language;
        }

        public Language GetLanguageAndContext(int? languageid)
        {
            Language language = null;

            try
            {
                if (languageid != null)
                {
                    using (var db = new Entities())
                    {
                        language = db.Languages.Where(i => i.LanguageID == languageid)
                                               .Include("Profiles")
                                               .SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetLanguageAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return language;
        }

        public IEnumerable<Language> GetLanguages()
        {
            var languages = new List<Language>();

            try
            {
                using (var db = new Entities())
                {
                    languages = db.Languages.OrderBy(i => i.Name)
                                            .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetLanguages - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return languages;
        }

        public IEnumerable<Language> GetLanguagesAndContext()
        {
            var languages = new List<Language>();

            try
            {
                using (var db = new Entities())
                {
                    languages = db.Languages.Include("Profiles")
                                            .OrderBy(i => i.Name)
                                            .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetLanguagesAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return languages;
        }

        public bool SaveLanguage(Language language)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (language.LanguageID > 0)
                    {
                        var tmpLanguage = db.Languages.SingleOrDefault(i => i.LanguageID == language.LanguageID);
                        if (tmpLanguage != null)
                        {
                            tmpLanguage.Name = language.Name;
                        }
                    }
                    else
                    {
                        db.Languages.Add(language);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveLanguage - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region LoggedEvents

        public LoggedEvent GetLoggedEvent(int loggedEventId)
        {
            LoggedEvent loggedEvent = null;

            try
            {
                using (var db = new Entities())
                {
                    if (loggedEventId > 0)
                    {
                        var tmpLoggedEvent = db.LoggedEvents.SingleOrDefault(i => i.LoggedEventID == loggedEventId);
                        if (tmpLoggedEvent != null)
                        {
                            loggedEvent = tmpLoggedEvent;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetLoggedEvent - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return loggedEvent;
        }

        public IEnumerable<LoggedEvent> GetLoggedEvents()
        {
            var loggedEvents = new List<LoggedEvent>();

            try
            {
                using (var db = new Entities())
                {
                    loggedEvents = db.LoggedEvents.OrderByDescending(i => i.DateStarted)
                                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetLoggedEvents - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return loggedEvents;
        }
        public LoggedEvent SaveLoggedEvent(LoggedEvent loggedEvent)
        {
            LoggedEvent savedEvent = null;

            try
            {
                using (var db = new Entities())
                {
                    if (loggedEvent.LoggedEventID > 0)
                    {
                        var tmpLoggedEvent = db.LoggedEvents.SingleOrDefault(i => i.LoggedEventID == loggedEvent.LoggedEventID);
                        if (tmpLoggedEvent != null)
                        {
                            tmpLoggedEvent.DateEnded = loggedEvent.DateEnded;
                            tmpLoggedEvent.DateStarted = loggedEvent.DateStarted;
                            tmpLoggedEvent.Name = loggedEvent.Name;
                            tmpLoggedEvent.Details = loggedEvent.Details;

                            savedEvent = tmpLoggedEvent;
                        }
                    }
                    else
                    {
                        db.LoggedEvents.Add(loggedEvent);

                        savedEvent = loggedEvent;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveLoggedEvent - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return savedEvent;
        }

        #endregion

        #region Product

        public Product GetProduct(int? productid)
        {
            Product product = null;

            try
            {
                if (productid != null)
                {
                    using (var db = new Entities())
                    {
                        product = db.Products.SingleOrDefault(i => i.ProductID == productid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProduct - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return product;
        }

        public Product GetProductAndContext(int? productid)
        {
            Product product = null;

            try
            {
                if (productid != null)
                {
                    using (var db = new Entities())
                    {
                        product = db.Products.Where(i => i.ProductID == productid)
                                             .Include("Brand")
                                             .Include("Category")
                                             .Include("Collection")
                                             .Include("Gender")
                                             .Include("Season")
                                             .Include("Images")
                                             .SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProductAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return product;
        }

        public SellerProductModel GetProductAndMapToSellerProductModel(int? productid)
        {
            var productModel = new SellerProductModel();

            var product = GetProduct(productid);
            if (product != null)
            {
                productModel.ProductId = product.ProductID;
                productModel.Name = product.Name;
                productModel.Edi = product.EDI;
                productModel.Sku = product.SKU;
                productModel.Year = product.Year;
                productModel.Material = product.Material;
                productModel.Size = product.Size;
                productModel.SeasonId = product.SeasonID;
                productModel.GenderId = product.GenderID;
                productModel.CategoryId = product.CategoryID;

                if (product.CollectionID != null)
                {
                    productModel.CollectionId = (int)product.CollectionID;
                }
                
                productModel.BrandId = product.BrandID;
            }

            return productModel;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>();

            try
            {
                using (var db = new Entities())
                {
                    products = db.Products.OrderBy(i => i.Name)
                                          .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProducts - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return products;
        }

        public IEnumerable<Product> GetProductsAndContext()
        {
            var products = new List<Product>();

            try
            {
                using (var db = new Entities())
                {
                    products = db.Products.OrderBy(i => i.Name)
                                          .Include("Brand")
                                          .Include("Category")
                                          .Include("Collection")
                                          .Include("Gender")
                                          .Include("Season")
                                          .Include("Images")
                                          .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProducts - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return products;
        }

        public IEnumerable<Product> GetProductsForUser(int userid)
        {
            var products = new List<Product>();

            try
            {
                using (var db = new Entities())
                {
                    products = db.Products.Where(i => i.UserID == userid)
                                          .Include("Images")
                                          .OrderBy(i => i.Name)
                                          .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProductsForUser - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return products;
        }

        public bool SaveProduct(SellerProductModel product, int userid)
        {
            var saveSucceeded = false;

            try
            {
                Product tmpProduct;

                using (var db = new Entities())
                {
                    if (product.ProductId > 0)
                    {
                        tmpProduct = db.Products.SingleOrDefault(i => i.ProductID == product.ProductId);
                        if (tmpProduct != null)
                        {
                            tmpProduct.Name = product.Name;
                            tmpProduct.EDI = product.Edi;
                            tmpProduct.SKU = product.Sku;
                            tmpProduct.Year = product.Year;
                            tmpProduct.Material = product.Material;
                            tmpProduct.Size = product.Size;
                            tmpProduct.BrandID = product.BrandId;
                            tmpProduct.CategoryID = product.CategoryId;
                            tmpProduct.CollectionID = product.CollectionId;
                            tmpProduct.GenderID = product.GenderId;
                            tmpProduct.SeasonID = product.SeasonId;
                            tmpProduct.updated = DateTime.Now;
                            tmpProduct.UserID = userid;
                        }
                    }
                    else
                    {
                        tmpProduct = new Product
                        {
                            Name = product.Name,
                            EDI = product.Edi,
                            SKU = product.Sku,
                            Year = product.Year,
                            Material = product.Material,
                            Size = product.Size,
                            BrandID = product.BrandId,
                            CategoryID = product.CategoryId,
                            CollectionID = product.CollectionId,
                            GenderID = product.GenderId,
                            SeasonID = product.SeasonId,
                            created = DateTime.Now,
                            updated = DateTime.Now,
                            UserID = userid
                        };

                        db.Products.Add(tmpProduct);
                    }

                    db.SaveChanges();
                }

                _luceneWorker.AddUpdateLuceneIndex(Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName, tmpProduct);

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveProduct - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region Salutation

        public Salutation GetSalutation(int? salutationid)
        {
            Salutation salutation = null;

            try
            {
                if (salutationid != null)
                {
                    using (var db = new Entities())
                    {
                        salutation = db.Salutations.SingleOrDefault(i => i.SalutationID == salutationid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSalutation - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return salutation;
        }

        public Salutation GetSalutationAndContext(int? salutationid)
        {
            Salutation salutation = null;

            try
            {
                if (salutationid != null)
                {
                    using (var db = new Entities())
                    {
                        salutation = db.Salutations.Where(i => i.SalutationID == salutationid)
                                                   .Include("Profiles")
                                                   .SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSalutationAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return salutation;
        }

        public IEnumerable<Salutation> GetSalutations()
        {
            var salutations = new List<Salutation>();

            try
            {
                using (var db = new Entities())
                {
                    salutations = db.Salutations.OrderBy(i => i.Name)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSalutations - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return salutations;
        }

        public IEnumerable<Salutation> GetSalutationsAndContext()
        {
            var salutations = new List<Salutation>();

            try
            {
                using (var db = new Entities())
                {
                    salutations = db.Salutations.Include("Profiles")
                                                .OrderBy(i => i.Name)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSalutationsAndContext - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return salutations;
        }

        public bool SaveSalutation(Salutation salutation)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (salutation.SalutationID > 0)
                    {
                        var tmpSalutation = db.Salutations.SingleOrDefault(i => i.SalutationID == salutation.SalutationID);
                        if (tmpSalutation != null)
                        {
                            tmpSalutation.Name = salutation.Name;
                        }
                    }
                    else
                    {
                        db.Salutations.Add(salutation);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveSalutation - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region Season

        public Season GetSeason(int? seasonid)
        {
            Season season = null;

            try
            {
                if (seasonid != null)
                {
                    using (var db = new Entities())
                    {
                        season = db.Seasons.SingleOrDefault(i => i.SeasonID == seasonid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSeason - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return season;
        }

        public SeasonModel GetSeasonAndMapToCategoryModel(int? seasonid)
        {
            SeasonModel seasonModel = null;

            try
            {
                if (seasonid != null)
                {
                    using (var db = new Entities())
                    {
                        var season = db.Seasons.SingleOrDefault(i => i.SeasonID == seasonid);
                        if (season != null)
                        {
                            var stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(season.Name));
                            var serializer = new DataContractJsonSerializer(typeof(MultiLingualListModel));
                            var jsonResult = (MultiLingualListModel)serializer.ReadObject(stream);

                            seasonModel = new SeasonModel
                            {
                                SeasonID = season.SeasonID,
                                Name = jsonResult
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSeasonAndMapToCategoryModel - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return seasonModel;
        }
        public IEnumerable<Season> GetSeasons()
        {
            var seasons = new List<Season>();

            try
            {
                using (var db = new Entities())
                {
                    seasons = db.Seasons.OrderBy(i => i.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSeasons - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return seasons;
        }

        public IEnumerable<KeyValuePair<int, string>> GetSeasonsAndMapToSelectList()
        {
            var seasonsList = new List<KeyValuePair<int, string>>();

            try
            {
                using (var db = new Entities())
                {
                    var seasons = db.Seasons.OrderBy(i => i.Name)
                                            .ToList();
                    if (seasons.Any())
                    {
                        seasonsList.AddRange(seasons.Select(season => new KeyValuePair<int, string>(season.SeasonID, LanguageString.GetStringForCurrentLanguage(season.Name))));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetSeasonsAndMapToSelectList - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return seasonsList;
        }

        public bool SaveSeason(SeasonModel seasonModel)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (seasonModel.SeasonID > 0)
                    {
                        var tmpSeason = db.Seasons.SingleOrDefault(i => i.SeasonID == seasonModel.SeasonID);
                        if (tmpSeason != null)
                        {
                            tmpSeason.Name = SerializeMultiLingualListModelToJson(seasonModel.Name);
                            tmpSeason.updated = DateTime.Now;

                            UpdateProductsInIndex(tmpSeason.Products);
                        }
                    }
                    else
                    {
                        var season = new Season
                        {
                            Name = SerializeMultiLingualListModelToJson(seasonModel.Name),
                            updated = DateTime.Now,
                            created = DateTime.Now
                        };

                        db.Seasons.Add(season);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveSeason - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        private static string SerializeMultiLingualListModelToJson(MultiLingualListModel model)
        {
            var serializer = new DataContractJsonSerializer(typeof (MultiLingualListModel));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, model);

            return Encoding.Default.GetString((stream.ToArray()));
        }

        private void UpdateProductsInIndex(IEnumerable<Product> products)
        {
            var entireProducts = products.Select(product => GetProductAndContext(product.ProductID))
                                         .Where(entireProduct => entireProduct != null)
                                         .ToList();

            if (entireProducts.Any())
            {
                _luceneWorker.AddUpdateLuceneIndex(Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName, entireProducts);
            }
        }

    }
}
