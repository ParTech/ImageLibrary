using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class RecoverModel
    {
        [Required]
        [Display()]
        public string UserName { get; set; }

        [Required]
        [EmailAddress()]
        [Display()]
        public string Email { get; set; }
    }
}