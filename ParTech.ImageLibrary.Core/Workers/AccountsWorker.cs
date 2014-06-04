using System;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Account;
using Postal;
using WebMatrix.WebData;

namespace ParTech.ImageLibrary.Core.Workers
{
    public interface IAccountsWorker : IWorker
    {
        string GenerateRandomPassword(int length);

        bool RegisterUser(RegisterModel model);

        bool SendRecoverPasswordEmail(RecoverModel model, string resetPasswordActionUrl);

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

        public bool RegisterUser(RegisterModel model)
        {
            var registrationSucceeded = false;

            // Attempt to register the user
            try
            {
                var confirmationToken = WebSecurity.CreateUserAndAccount(
                    model.UserName,
                    model.Password,
                    new
                    {
                        model.Email,
                        AccountType = model.AccountType.GetHashCode(),
                        Active = 0
                    },
                    true);

                registrationSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(" RegisterUser - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return registrationSucceeded;
        }

        //public bool RegisterUserAndSendConfirmationEmail(RegisterModel model,
        //    string registerConfirmationActionUrl)
        //{
        //    var registrationSucceeded = false;

        //    // Attempt to register the user
        //    try
        //    {
        //        var confirmationToken = WebSecurity.CreateUserAndAccount(
        //            model.UserName,
        //            model.Password,
        //            new
        //            {
        //                model.Email,
        //                AccountType = model.AccountType.GetHashCode(),
        //                Active = 0
        //            },
        //            true);
        //        var confirmationLink = string.Format("<a href='http://{0}{1}/{2}'>{3}</a>",
        //                                             System.Web.HttpContext.Current.Request.Url.Host,
        //                                             registerConfirmationActionUrl,
        //                                             confirmationToken,
        //                                             "Confirm registration");

        //        var profile = _userRepository.GetUserProfileByName(model.UserName);
        //        if (profile != null)
        //        {
        //            if (_userRepository.SaveUserProfile(profile, model.AccountType.ToString()))
        //            {
        //                dynamic email = new Email("RegistrationConfirmation");
        //                email.To = model.Email;
        //                email.UserName = model.UserName;
        //                email.ConfirmationLink = confirmationLink;
        //                email.Send();

        //                registrationSucceeded = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.ErrorFormat(" RegisterUserAndSendConfirmationEmail - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
        //    }

        //    return registrationSucceeded;
        //}

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
