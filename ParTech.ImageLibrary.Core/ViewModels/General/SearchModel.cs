using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.General
{
    public class SearchModel
    {
        [Required]
        [Display]
        public string FieldName { get; set; }

        [Required]
        [Display]
        public string SearchString { get; set; }

        public List<Product> FoundProducts { get; set; }

        public SearchModel()
        {
            FieldName = ConfigurationManager.AppSettings.Get("Lucene.DefaultIndex");
        }

        public string GetUserMainRole()
        {
            if (HttpContext.Current.User.IsInRole("Byer"))
            {
                return "Byer";
            }

            if (HttpContext.Current.User.IsInRole("Seller"))
            {
                return "Seller";
            }

            return "Admin";
        }
    }
}
