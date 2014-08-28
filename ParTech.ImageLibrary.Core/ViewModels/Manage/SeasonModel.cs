using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class SeasonModel
    {
        public int SeasonID { get; set; }
        
        public MultiLingualListModel Name { get; set; }

        public SeasonModel()
        {
        }

        public SeasonModel(IEnumerable<Language> languages)
        {
            Name = new MultiLingualListModel(languages);
        }
    }
}
