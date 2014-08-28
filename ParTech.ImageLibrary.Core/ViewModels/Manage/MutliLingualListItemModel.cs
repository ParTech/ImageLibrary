using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class MultiLingualListItemModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
