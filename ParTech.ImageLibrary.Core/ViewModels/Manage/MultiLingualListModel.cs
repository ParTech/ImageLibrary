using System.Collections.Generic;
using System.Linq;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class MultiLingualListModel
    {
        public MultiLingualListItemModel[] Values { get; set; }

        public MultiLingualListModel()
        {
        }

        public MultiLingualListModel(IEnumerable<Language> languages)
        {
            Values = languages.Select(language => new MultiLingualListItemModel { Code = language.IsoCode, Value = string.Empty }).ToArray();
        }
    }
}
