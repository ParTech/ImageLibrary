using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;

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

        IEnumerable<Category> GetCategories(int cultureId);

        bool SaveCategory(Category category);

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

        IEnumerable<Gender> GetGenders(int cultureId);

        bool SaveGender(Gender gender);

        #endregion

        #region Language

        Language GetLanguage(int? languageid);

        Language GetLanguageAndContext(int? languageid);

        IEnumerable<Language> GetLanguages();

        IEnumerable<Language> GetLanguagesAndContext();

        bool SaveLanguage(Language language);

        #endregion

        #region Product

        Product GetProduct(int? productid);

        Product GetProductAndContext(int? productid);

        IEnumerable<Product> GetProductsForUser(int userid);

        bool SaveProduct(Product product, int userid);

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

        IEnumerable<Season> GetSeasons();

        bool SaveSeason(Season season);

        #endregion
    }

    public class ObjectRepository : IObjectRepository
    {
        public ILogger Logger { get; set; }

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

        public IEnumerable<Category> GetCategories(int cultureId)
        {
            var categories = new List<Category>();

            try
            {
                using (var db = new Entities())
                {
                    categories = db.Categories.Where(i => i.LanguageID == cultureId)
                                              .OrderBy(j => j.Name)
                                              .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCategories - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return categories;
        }

        public bool SaveCategory(Category category)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (category.CategoryID > 0)
                    {
                        var tmpCategory = db.Categories.SingleOrDefault(i => i.CategoryID == category.CategoryID);
                        if (tmpCategory != null)
                        {
                            tmpCategory.Name = category.Name;
                            tmpCategory.LanguageID = Thread.CurrentThread.CurrentCulture.LCID;
                            tmpCategory.updated = DateTime.Now;
                        }
                    }
                    else
                    {
                        category.LanguageID = Thread.CurrentThread.CurrentCulture.LCID;
                        category.updated = DateTime.Now;
                        category.created = DateTime.Now;

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

        public IEnumerable<Gender> GetGenders(int cultureId)
        {
            var genders = new List<Gender>();

            try
            {
                using (var db = new Entities())
                {
                    genders = db.Genders.Where(i => i.LanguageID == cultureId)
                                        .OrderBy(j => j.Name)
                                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetGenders - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return genders;
        }

        public bool SaveGender(Gender gender)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (gender.GenderID > 0)
                    {
                        var tmpGender = db.Genders.SingleOrDefault(i => i.GenderID == gender.GenderID);
                        if (tmpGender != null)
                        {
                            tmpGender.Name = gender.Name;
                            tmpGender.LanguageID = Thread.CurrentThread.CurrentCulture.LCID;
                            tmpGender.updated = DateTime.Now;
                        }
                    }
                    else
                    {
                        gender.LanguageID = Thread.CurrentThread.CurrentCulture.LCID;
                        gender.updated = DateTime.Now;
                        gender.created = DateTime.Now;

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

        public bool SaveProduct(Product product, int userid)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (product.ProductID > 0)
                    {
                        var tmpProduct = db.Products.SingleOrDefault(i => i.ProductID == product.ProductID);
                        if (tmpProduct != null)
                        {
                            tmpProduct.Name = product.Name;
                            tmpProduct.EDI = product.EDI;
                            tmpProduct.SKU = product.SKU;
                            tmpProduct.Year = product.Year;
                            tmpProduct.Material = product.Material;
                            tmpProduct.Size = product.Size;
                            tmpProduct.BrandID = product.BrandID;
                            tmpProduct.CategoryID = product.CategoryID;
                            tmpProduct.CollectionID = product.CollectionID;
                            tmpProduct.GenderID = product.GenderID;
                            tmpProduct.SeasonID = product.SeasonID;

                            tmpProduct.updated = DateTime.Now;
                            tmpProduct.UserID = userid;
                        }
                    }
                    else
                    {
                        product.created = DateTime.Now;
                        product.updated = DateTime.Now;
                        product.UserID = userid;

                        db.Products.Add(product);
                    }

                    db.SaveChanges();
                }

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

        public bool SaveSeason(Season season)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (season.SeasonID > 0)
                    {
                        var tmpSeason = db.Seasons.SingleOrDefault(i => i.SeasonID == season.SeasonID);
                        if (tmpSeason != null)
                        {
                            tmpSeason.Name = season.Name;
                            tmpSeason.updated = DateTime.Now;
                        }
                    }
                    else
                    {
                        season.updated = DateTime.Now;
                        season.created = DateTime.Now;

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

    }
}
