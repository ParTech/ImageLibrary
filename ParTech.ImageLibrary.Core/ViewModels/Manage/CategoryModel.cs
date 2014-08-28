using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }
        
        public MultiLingualListModel Name { get; set; }

        public CategoryModel()
        {
        }

        public CategoryModel(IEnumerable<Language> languages)
        {
            Name = new MultiLingualListModel(languages);
        }
    }
}
