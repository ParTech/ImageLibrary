using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Account type")]
        public AccountTypeEnum AccountType { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "The maximum length of the email address field is 255 characters.")]
        [EmailAddress(ErrorMessage = "The email address is not valid!")]
        [Display(Name = "Email address")]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

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

        public IEnumerable<Country> CountryItems { get; set; }

        public IEnumerable<Language> LanguageItems { get; set; }

        public IEnumerable<Salutation> SalutationItems { get; set; }
    }
}