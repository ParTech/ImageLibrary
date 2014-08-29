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
        [Display]
        public string CompanyName { get; set; }

        [Required]
        [Display]
        public int LanguageId { get; set; }

        [Required]
        [Display]
        public int SalutationId { get; set; }

        [Required]
        [Display]
        public string FirstName { get; set; }

        [Required]
        [Display]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display]
        public string Email { get; set; }

        [Display]
        public string Telephone { get; set; }

        [Required]
        [Display]
        public string Address { get; set; }

        [Required]
        [Display]
        public string PostalCode { get; set; }

        [Required]
        [Display]
        public string City { get; set; }

        [Required]
        [Display]
        public int CountryId { get; set; }

        [Required]
        [Display]
        public int SubscriptionTypeId { get; set; }
       
        public bool MainAccount { get; set; }

        public IEnumerable<Country> CountryItems { get; set; }

        public IEnumerable<Language> LanguageItems { get; set; }

        public IEnumerable<Salutation> SalutationItems { get; set; }

        public IEnumerable<SubscriptionType> SubscriptionTypeItems { get; set; }
    }
}