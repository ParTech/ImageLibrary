using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class GenderModel
    {
        public int GenderID { get; set; }
        
        public MultiLingualListModel Name { get; set; }

        public GenderModel()
        {
        }

        public GenderModel(IEnumerable<Language> languages)
        {
            Name = new MultiLingualListModel(languages);
        }
    }
}
