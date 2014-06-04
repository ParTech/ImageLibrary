using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Seller
{
    public class SellerProductModel
    {
        public IEnumerable<Product> ListProducts { get; set; }

        public Product ProductModel { get; set; }

        public IEnumerable<Brand> BrandItems { get; set; }

        public IEnumerable<Category> CategoryItems { get; set; }

        public IEnumerable<Collection> CollectionItems { get; set; }

        public IEnumerable<Gender> GenderItems { get; set; }

        public IEnumerable<Season> SeasonItems { get; set; }
    }
}