using System.Collections.Generic;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.ViewModels.Profile
{
    public class AdditionalAccountsModel
    {
        public IEnumerable<UserProfile> ListUserProfiles { get; set; }
        public UserProfileModel UserProfileModel { get; set; }
    }
}
