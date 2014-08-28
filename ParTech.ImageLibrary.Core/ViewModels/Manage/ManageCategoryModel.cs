using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class ManageCategoryModel
    {
        public IEnumerable<Language> Languages { get; set; }

        public IEnumerable<Category> ListCategories { get; set; }

        public CategoryModel CategoryModel { get; set; }
    }
}