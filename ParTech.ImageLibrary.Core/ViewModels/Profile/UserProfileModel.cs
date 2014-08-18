using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Profile
{
    public class UserProfileModel
    {
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
        public int UserProfileId { get; set; }
    }
}