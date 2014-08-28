using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Seller
{
    public class SellerProductModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Display]
        public string Name { get; set; }

        [Required]
        [Display]
        public string Edi { get; set; }

        [Required]
        [Display]
        public string Sku { get; set; }

        [Required]
        [Display]
        public int Year { get; set; }

        [Display]
        public string Material { get; set; }

        [Display]
        public string Size { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Display]
        public int SeasonId { get; set; }

        [Required]
        [Display]
        public int GenderId { get; set; }

        [Required]
        [Display]
        public int CategoryId { get; set; }

        [Display]
        public int CollectionId { get; set; }

        [Required]
        [Display]
        public int BrandId { get; set; }

        public IEnumerable<Brand> BrandItems { get; set; }

        public IEnumerable<KeyValuePair<int, string>> CategoryItems { get; set; }

        public IEnumerable<Collection> CollectionItems { get; set; }

        public IEnumerable<KeyValuePair<int, string>> GenderItems { get; set; }

        public IEnumerable<KeyValuePair<int, string>> SeasonItems { get; set; }
    }
}
