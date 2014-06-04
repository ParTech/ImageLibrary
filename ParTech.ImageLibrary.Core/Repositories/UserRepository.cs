using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.ViewModels.Account;
using ParTech.ImageLibrary.Core.ViewModels.Profile;

namespace ParTech.ImageLibrary.Core.Repositories
{
    public interface IUserRepository : IRepository
    {
        #region Profile

        Profile GetProfile(int? profileId);

        EditProfileModel GetProfileAndMapItToEditProfileModel(int? profileId);

        bool SaveProfile(Profile profile);

        #endregion
        
        #region UserProfile

        bool AddUserProfile(string userName);

        bool CheckIfUserProfileIsActive(string userName);

        LocalUserModel GetUserProfileAndMapToLocalUserModel();

        UserProfile GetUserProfileById(int userId);

        UserProfile GetUserProfileByName(string userName);

        UserProfile GetUserProfileByNameAndEmail(string userName, string emailAddress);

        IEnumerable<UserProfile> GetUserProfiles(bool active);

        bool SaveUserProfile(LocalUserModel profile);

        bool SaveUserProfile(UserProfile profile, string roleName);

        #endregion

        #region WebpagesMembership

        webpages_Membership GetMembership(int userId);

        bool VerifyPasswordVerificationToken(int userId, string passwordVerificationToken);

        #endregion
    }

    public class UserRepository : IUserRepository
    {
        public ILogger Logger { get; set; }

        #region Profile

        public Profile GetProfile(int? profileId)
        {
            Profile outProfile = null;

            if (profileId != null)
            {
                try
                {
                    using (var db = new Entities())
                    {
                        var tmpProfile = db.Profiles.Single(u => u.ProfileID == profileId);
                        if (tmpProfile != null)
                        {
                            outProfile = tmpProfile;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("GetProfile - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
                }
            }

            return outProfile;
        }

        public EditProfileModel GetProfileAndMapItToEditProfileModel(int? profileId)
        {
            var profile = GetProfile(profileId);
            if (profile == null)
            {
                return new EditProfileModel();
            }

            var editProfileModel = new EditProfileModel
            {
                CompanyName = profile.CompanyName,
                LanguageId = profile.LanguageID,
                SalutationId = profile.SalutationID,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.Email,
                Telephone = profile.Telephone,
                Address = profile.Address,
                PostalCode = profile.PostalCode,
                City = profile.City,
                CountryId = profile.CountryID
            };

            return editProfileModel;
        }

        public bool SaveProfile(Profile profile)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    var tmpProfile = db.Profiles.Single(u => u.ProfileID == profile.ProfileID);
                    if (tmpProfile != null)
                    {
                        tmpProfile.CompanyName = profile.CompanyName;

                        tmpProfile.SalutationID = profile.SalutationID;
                        tmpProfile.FirstName = profile.FirstName;
                        tmpProfile.LastName = profile.LastName;
                        tmpProfile.Email = profile.Email;
                        tmpProfile.Telephone = profile.Telephone;

                        tmpProfile.Address = profile.Address;
                        tmpProfile.PostalCode = profile.PostalCode;
                        tmpProfile.City = profile.City;
                        tmpProfile.CountryID = profile.CountryID;

                        db.SaveChanges();
                    }
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveProfile - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region UserProfile

        public bool AddUserProfile(string userName)
        {
            var userProfileCreated = false;

            try
            {
                // Insert a new user into the database
                using (var db = new Entities())
                {
                    var user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = userName });
                        db.SaveChanges();

                        userProfileCreated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("AddUserProfile - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return userProfileCreated;
        }

        public bool CheckIfUserProfileIsActive(string userName)
        {
            var userProfile = GetUserProfileByName(userName);
            if (userProfile != null)
            {
                return userProfile.Active;
            }

            return false;
        }

        public LocalUserModel GetUserProfileAndMapToLocalUserModel()
        {
            var userModel = new LocalUserModel();
            
            var userProfile = GetUserProfileByName(HttpContext.Current.User.Identity.Name);
            if (userProfile != null)
            {
                userModel.Email = userProfile.Email;
                userModel.Id = userProfile.Id;
            }

            return userModel;
        }

        public UserProfile GetUserProfileById(int userId)
        {
            UserProfile outProfile = null;

            try
            {
                using (var db = new Entities())
                {
                    var tmpProfile = db.UserProfiles.Single(u => u.Id == userId);
                    if (tmpProfile != null)
                    {
                        outProfile = tmpProfile;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProfileById - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return outProfile;
        }

        public UserProfile GetUserProfileByName(string userName)
        {
            UserProfile outProfile = null;

            try
            {
                using (var db = new Entities())
                {
                    var tmpProfile = db.UserProfiles.Where(u => u.UserName.ToLower() == userName.ToLower())
                                                    .Include("webpages_Roles")
                                                    .Single();
                    if (tmpProfile != null)
                    {
                        outProfile = tmpProfile;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProfileByName - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return outProfile;
        }

        public UserProfile GetUserProfileByNameAndEmail(string userName, string emailAddress)
        {
            UserProfile outProfile = null;

            try
            {
                using (var db = new Entities())
                {
                    var tmpProfile = db.UserProfiles.SingleOrDefault(u => u.UserName.ToLower() == userName.ToLower()
                                                                          && u.Email == emailAddress);
                    if (tmpProfile != null)
                    {
                        outProfile = tmpProfile;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetProfileByNameAndEmail - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return outProfile;
        }

        public IEnumerable<UserProfile> GetUserProfiles(bool active)
        {
            List<UserProfile> userProfiles = null;

            try
            {
                using (var db = new Entities())
                {
                    userProfiles = db.UserProfiles.Where(u => u.Active == active)
                                                  .OrderBy(u => u.UserName)
                                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetUserProfiles - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return userProfiles;
        }

        public bool SaveUserProfile(LocalUserModel profile)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    var tmpProfile = db.UserProfiles.Single(u => u.Id == profile.Id);
                    if (tmpProfile != null)
                    {
                        tmpProfile.Email = profile.Email;

                        db.SaveChanges();
                    }
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveProfile - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        public bool SaveUserProfile(UserProfile profile, string roleName)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    var tmpProfile = db.UserProfiles.Where(u => u.Id == profile.Id)
                                                    .Include("webpages_Roles")
                                                    .Single();
                    if (tmpProfile != null)
                    {
                        tmpProfile.AccountType = profile.AccountType;
                        tmpProfile.Email = profile.Email;
                        tmpProfile.ProfileID = profile.ProfileID;

                        if (!string.IsNullOrEmpty(roleName))
                        {
                            if (tmpProfile.webpages_Roles.All(i => i.RoleName != roleName))
                            {
                                var role = db.webpages_Roles.Single(u => u.RoleName.ToLower() == roleName.ToLower());
                                if (role != null)
                                {
                                    tmpProfile.webpages_Roles.Add(role);
                                }
                            }
                        }

                        db.SaveChanges();
                    }
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveProfile - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region WebpagesMembership

        public webpages_Membership GetMembership(int userId)
        {
            webpages_Membership membership = null;

            try
            {
                using (var db = new Entities())
                {
                    membership = db.webpages_Membership.Single(i => i.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetMembership - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return membership;
        }

        public bool VerifyPasswordVerificationToken(int userId, string passwordVerificationToken)
        {
            var tokenVerified = false;

            try
            {
                using (var db = new Entities())
                {
                    tokenVerified = db.webpages_Membership.Any(i => i.PasswordVerificationToken == passwordVerificationToken
                                                                    && DateTime.Now < i.PasswordVerificationTokenExpirationDate
                                                                    && i.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("VerifyPasswordVerificationToken - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return tokenVerified;
        }

        #endregion
    }
}
