using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class RegisterModel
    {
        [Required]
        [Display]
        public AccountTypeEnum AccountType { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        [Display]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display]
        public string Password { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 6)]
        [Display]
        public string UserName { get; set; }

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

        public IEnumerable<Country> CountryItems { get; set; }

        public IEnumerable<Language> LanguageItems { get; set; }

        public IEnumerable<Salutation> SalutationItems { get; set; }

        public IEnumerable<SubscriptionType> SubscriptionTypeItems { get; set; }
    }
}