using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class ManageSeasonModel
    {
        public IEnumerable<Language> Languages { get; set; }

        public IEnumerable<Season> ListSeasons { get; set; }

        public SeasonModel SeasonModel { get; set; }
    }
}