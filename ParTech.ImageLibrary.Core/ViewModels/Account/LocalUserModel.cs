
using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class LocalUserModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display]
        public string Email { get; set; }
    }
}
