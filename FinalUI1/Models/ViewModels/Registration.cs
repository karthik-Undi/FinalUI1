using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FinalUI1.Models.ViewModels
{
    public class Registration
    {
        [Required]
        public string LoginType { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Name can't be shorter than 5 characters")]
        public string Name { get; set; }
        [Required]
        [Range(101,200,ErrorMessage ="Only House no 101 to 200 Exist")]
        [Remote("IsHouseFree", "Login", ErrorMessage = "House already registered. Check the number again")]

        public int HouseNo { get; set; }
        [Required]
        public string ResidenceType { get; set; }

        [Required]
        public string Role { get; set; }
        [Required]

        [RegularExpression("^[0-9]{10}",
        ErrorMessage = "Mobile number should only have 10 integers")]
        public string MobileNo { get; set; }
        [Required]

        [DataType(DataType.EmailAddress)]
        [Remote("DoesEmailExist", "Login",ErrorMessage = "EmailId already exists in database.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote("DoesEmailExistinEmployees", "Login", ErrorMessage = "EmailId already exists in database.")]
        public string Email_Emp { get; set; }


        [Required]

        [MembershipPassword(
                MinRequiredPasswordLength = 8,
                ErrorMessage = "Your password needs to be at least 8 characters long",
                MinRequiredNonAlphanumericCharacters = 1,
                MinNonAlphanumericCharactersError = "At least one of the characters needs to be non-alphanumeric")]
        public string Password { get; set; }
        [Required]

        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage ="Passwords Donot Match")]
        public string CmpPassword { get; set; }

    }
}