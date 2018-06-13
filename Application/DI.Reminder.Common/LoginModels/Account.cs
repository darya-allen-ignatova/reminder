﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DI.Reminder.Common.LoginModels
{
    public class Account
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Field must be filled")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Field must be filled")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Field must be filled")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords aren't similar")]  
        public string PasswordConfirm { get; set; }
        public List<Role> Roles { get; set; }
    }
}
