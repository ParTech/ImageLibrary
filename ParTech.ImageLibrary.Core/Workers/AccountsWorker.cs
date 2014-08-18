using System;
using System.Globalization;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Account;
using ParTech.ImageLibrary.Core.ViewModels.Profile;
using Postal;
using WebMatrix.WebData;

namespace ParTech.ImageLibrary.Core.Workers
{
    public interface IAccountsWorker : IWorker
    {
        bool ConfirmAndActivateUserProfile(string confirmationToken);

        string GenerateRandomPassword(int length);

        bool RegisterAdditionalAccount(UserProfileModel model, UserProfile userProfile);

        bool RegisterUser(RegisterModel model);

        bool SendRecoverPasswordEmail(RecoverModel model, string resetPasswordActionUrl);

        bool SendRegistrationConfirmationEmail(int userid);

        MessageIdEnum SendResetPasswordEmail(string username, string passwordVerificationToken);
    }

    public class AccountsWorker : IAccountsWorker
    {
        private readonly IUserRepository _userRepository;

        public ILogger Logger { get; set; }

        public AccountsWorker(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ConfirmAndActivateUserProfile(string confirmationToken)
        {
            var confirmationSucceeded = false;

            try
            {
                // first confirm the user
                if (WebSecurity.ConfirmAccount(confirmationToken))
                {
                    // then activate the user
                    var webpagesMembership = _userRepository.GetMembershipByConfirmationToken(confirmationToken);
                    if (webpagesMembership != null &&
                        _userRepository.UpdateActiveFlagUserProfile(webpagesMembership.UserId, true))
                    {
                        confirmationSucceeded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" ConfirmAndActivateUserProfile - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return confirmationSucceeded;
        }
        
        public string GenerateRandomPassword(int length)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";

            var chars = new char[length];
            var rd = new Random();
            for (var i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public bool RegisterAdditionalAccount(UserProfileModel model, UserProfile userProfile)
        {
            var registrationSucceeded = false;

            try
            {
                AccountTypeEnum accountType;

                if (Enum.TryParse(userProfile.AccountType.ToString(CultureInfo.InvariantCulture), out accountType));
                {
                    // register the new (inactive) user 
                    WebSecurity.CreateUserAndAccount(
                        model.UserName,
                        model.Password,
                        new
                        {
                            Email = model.UserEmail,
                            AccountType = accountType.GetHashCode(),
                            ProfileID = userProfile.ProfileID,
                            Active = 0,
                            MainAccount = 0
                        },
                        true);

                    // and add the selected role to the new user
                    var profile = _userRepository.GetUserProfileByName(model.UserName);
                    if (profile != null &&
                        _userRepository.SaveUserProfile(profile, accountType.ToString()))
                    {
                        registrationSucceeded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" RegisterAdditionalAccount - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return registrationSucceeded;
        }
        public bool RegisterUser(RegisterModel model)
        {
            var registrationSucceeded = false;

            try
            {
                // first save the profile information of the user
                var newProfileId = _userRepository.AddProfile(model);
                if (newProfileId > 0)
                {
                    // then register the new (inactive) user 
                    WebSecurity.CreateUserAndAccount(
                        model.UserName,
                        model.Password,
                        new
                        {
                            Email = model.UserEmail,
                            AccountType = model.AccountType.GetHashCode(),
                            ProfileID = newProfileId,
                            Active = 0,
                            MainAccount = 1
                        },
                        true);
                    
                    // and finally add the selected role to the new user
                    var profile = _userRepository.GetUserProfileByName(model.UserName);
                    if (profile != null &&
                        _userRepository.SaveUserProfile(profile, model.AccountType.ToString()))
                    {
                        registrationSucceeded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" RegisterUser - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return registrationSucceeded;
        }

        public bool SendRecoverPasswordEmail(RecoverModel model, string resetPasswordActionUrl)
        {
            var emailSent = false;
            
            try
            {
                var profile = _userRepository.GetUserProfileByNameAndEmail(model.UserName, model.Email);
                if (profile != null)
                {
                    // generate a reset token en specify that this token is valid for 24 hours (default value)
                    var resetToken = WebSecurity.GeneratePasswordResetToken(profile.UserName);

                    if (!string.IsNullOrEmpty(resetPasswordActionUrl))
                    {
                        resetPasswordActionUrl = System.Web.HttpUtility.UrlDecode(resetPasswordActionUrl);
                        // ReSharper disable once AssignNullToNotNullAttribute
                        resetPasswordActionUrl = string.Format(resetPasswordActionUrl, profile.UserName, resetToken);
                    }
                    
                    var resetLink = string.Format("<a href='http://{0}{1}'>{2}</a>",
                                                  System.Web.HttpContext.Current.Request.Url.Host,
                                                  resetPasswordActionUrl,
                                                  "Confirm reset password");

                    dynamic email = new Email("RecoverPassword");
                    email.To = profile.Email;
                    email.UserName = profile.UserName;
                    email.ResetLink = resetLink;
                    email.Send();

                    emailSent = true;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" SendRecoverPasswordEmail - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return emailSent;
        }

        public bool SendRegistrationConfirmationEmail(int userid)
        {
            var emailSent = false;

            try
            {
                var userProfile = _userRepository.GetUserProfileAndContextById(userid);
                if (userProfile != null)
                {
                    var confirmationLink = string.Format("<a href='http://{0}/Account/RegisterConfirmation/{1}'>{2}</a>",
                                                            System.Web.HttpContext.Current.Request.Url.Host,
                                                            userProfile.webpages_Membership.ConfirmationToken,
                                                            "Confirm registration");

                    dynamic email = new Email("RegistrationConfirmation");
                    email.To = userProfile.Email;
                    email.UserName = userProfile.UserName;
                    email.ConfirmationLink = confirmationLink;
                    email.Send();

                    emailSent = true;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" ActivateUserAndSendRegistrationConfirmationEmail - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return emailSent;
        }

        public MessageIdEnum SendResetPasswordEmail(string username, string passwordVerificationToken)
        {
            var returnMessageId = MessageIdEnum.ResetPasswordFailure;
            
            try
            {
                var profile = _userRepository.GetUserProfileByName(username);
                if (profile == null)
                {
                    returnMessageId = MessageIdEnum.ResetPasswordUnknownUser;
                }
                else
                {
                    if (_userRepository.VerifyPasswordVerificationToken(profile.Id, passwordVerificationToken))
                    {
                        var newPassword = GenerateRandomPassword(6);
                        if (WebSecurity.ResetPassword(passwordVerificationToken, newPassword))
                        {
                            dynamic email = new Email("ResetPassword");
                            email.To = profile.Email;
                            email.UserName = profile.UserName;
                            email.NewPassword = newPassword;
                            email.Send();

                            returnMessageId = MessageIdEnum.RecoverSuccess;
                        }
                        else
                        {
                            returnMessageId = MessageIdEnum.ResetPasswordNotReset;
                        }
                    }
                    else
                    {
                        returnMessageId = MessageIdEnum.ResetPasswordNoMatch;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" SendResetPasswordEmail - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return returnMessageId;
        }
    }
}
