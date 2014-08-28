using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class ManageGenderModel
    {
        public IEnumerable<Language> Languages { get; set; }

        public IEnumerable<Gender> ListGenders { get; set; }

        public GenderModel GenderModel { get; set; }
    }
}