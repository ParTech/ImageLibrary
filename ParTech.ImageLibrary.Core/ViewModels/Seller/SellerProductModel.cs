using System;
using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Seller
{
    public class SellerProductModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Edi { get; set; }

        public string Sku { get; set; }

        public int Year { get; set; }

        public string Material { get; set; }

        public string Size { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int UserId { get; set; }

        public int SeasonId { get; set; }

        public int GenderId { get; set; }

        public int CategoryId { get; set; }

        public int CollectionId { get; set; }

        public int BrandId { get; set; }

        public IEnumerable<Brand> BrandItems { get; set; }

        public IEnumerable<Category> CategoryItems { get; set; }

        public IEnumerable<Collection> CollectionItems { get; set; }

        public IEnumerable<Gender> GenderItems { get; set; }

        public IEnumerable<Season> SeasonItems { get; set; }
    }
}
