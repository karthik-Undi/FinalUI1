using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FinalUI1.Models.ViewModels
{
    public class LoginAll
    {
        [Required(ErrorMessage ="Email can't be empty")]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login Type can't be empty")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Password can't be empty")]
        public string Password { get; set; }
    }
}