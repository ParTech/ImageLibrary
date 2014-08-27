using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class ManageBrandModel
    {
        public IEnumerable<Brand> ListBrands { get; set; }

        public Brand BrandModel { get; set; }
    }
}