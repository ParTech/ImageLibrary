using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display()]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display()]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display()]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}