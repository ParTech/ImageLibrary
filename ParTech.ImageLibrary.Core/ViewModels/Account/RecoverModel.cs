using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class RecoverModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "The email address is not valid!")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
}