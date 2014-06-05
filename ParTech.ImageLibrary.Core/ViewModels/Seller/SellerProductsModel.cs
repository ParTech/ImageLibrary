using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Seller
{
    public class SellerProductsModel
    {
        public IEnumerable<Product> ListProducts { get; set; }

        public SellerProductModel SellerProductModel { get; set; }
    }
}