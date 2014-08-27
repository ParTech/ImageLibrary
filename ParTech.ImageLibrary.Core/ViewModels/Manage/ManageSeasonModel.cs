using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class ManageSeasonModel
    {
        public IEnumerable<Season> ListSeasons { get; set; }

        public Season SeasonModel { get; set; }
    }
}