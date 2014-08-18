using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Profile
{
    public class EditProfileModel
    {
        [Required]
        public int ProfileId { get; set; }

        [Required]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Language")]
        public int LanguageId { get; set; }

        [Required]
        [Display(Name = "Salutation")]
        public int SalutationId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "The email address is not valid!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Telephone { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
       
        public bool MainAccount { get; set; }

        public IEnumerable<Country> CountryItems { get; set; }

        public IEnumerable<Language> LanguageItems { get; set; }

        public IEnumerable<Salutation> SalutationItems { get; set; }
    }
}