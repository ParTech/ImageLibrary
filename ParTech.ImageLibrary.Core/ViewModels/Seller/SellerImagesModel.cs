using System.Collections.Generic;
using System.Web;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Seller
{
    public class SellerImagesModel
    {
        public IEnumerable<Image> ListImages { get; set; }

        public IEnumerable<Product> ListProducts { get; set; }
        
        public Product Product { get; set; }
        
        public HttpPostedFileBase Upl { get; set; }
    }
}