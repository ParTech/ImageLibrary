
using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class LocalUserModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "The email address is not valid!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
