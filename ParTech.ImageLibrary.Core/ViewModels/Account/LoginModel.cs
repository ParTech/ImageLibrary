using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class LoginModel
    {
        [Required]
        [Display()]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display()]
        public string Password { get; set; }

        [Display()]
        public bool RememberMe { get; set; }
    }
}