using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CsharpBlackBelt.Models
{
    public class NewUser
    {
        [Required]
        [MinLength(3, ErrorMessage = "First Name Must be 3 characters or longer!")]
        [MaxLength(50, ErrorMessage = "First Name Must be 50 characters or shorter!")]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Last Name Must be 3 characters or longer!")]
        [MaxLength(50, ErrorMessage = "Last Name Must be 50 characters or shorter!")]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password Must be 8 characters or longer!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[#$@!-_%&*?])[A-Za-z\d#$@!%-_&*?]{8,20}$",
        ErrorMessage = "Password must contain at least 1 Upper and 1 Lower and 1 special chracter.")]
        [Display(Name = "Password:")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [CompareAttribute("Password", ErrorMessage = "Passwords Must Match!")]
        [Display(Name = "Confirm your password:")]
        public string PasswordConfirm { get; set; }


        //Coveret this form data to the obj that need to add to dababase

        public User GetNewuser()
        {
            User newUSer = new User();
            newUSer.FirstName = FirstName;
            newUSer.LastName = LastName;
            newUSer.Email = Email;
            newUSer.Password = Password;
            return newUSer;
        }
    }
}