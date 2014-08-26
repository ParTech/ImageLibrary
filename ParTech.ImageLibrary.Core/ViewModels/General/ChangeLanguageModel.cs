using System.Collections.Generic;
using System.Linq;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.Workers;

namespace ParTech.ImageLibrary.Core.ViewModels.General
{
    public class ChangeLanguageModel
    {
        private readonly IObjectRepository _objectRepository = new ObjectRepository(new LuceneWorker());

        public List<Language> Languages { get; set; }
        
        public ChangeLanguageModel()
        {
            Languages = GetLanguages();
        }

        public List<Language> GetLanguages()
        {
            return _objectRepository.GetLanguages().ToList();
        }
    }
}
