using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class ManageCategoryModel
    {
        public IEnumerable<Category> ListCategories { get; set; }

        public Category CategoryModel { get; set; }
    }
}