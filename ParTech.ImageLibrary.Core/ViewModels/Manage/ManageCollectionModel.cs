using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Manage
{
    public class ManageCollectionModel
    {
        public IEnumerable<Collection> ListCollections { get; set; }
        public Collection CollectionModel { get; set; }
    }
}