using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Profile
{
    public class UserProfileModel
    {
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
        public int UserProfileId { get; set; }
    }
}