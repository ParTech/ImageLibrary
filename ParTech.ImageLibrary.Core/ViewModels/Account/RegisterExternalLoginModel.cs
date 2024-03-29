﻿using System.ComponentModel.DataAnnotations;

namespace ParTech.ImageLibrary.Core.ViewModels.Account
{
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }
}