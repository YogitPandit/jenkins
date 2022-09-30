using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class UserModel
    {

       
        [Display(Name = "DepartmentId")]
        public string DepartmentId { get; set; }


        [Required]
        [Display(Name = "User name")]
        [DefaultValue("")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "MiddleName")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "MobileNumber")]
        public string MobileNumber { get; set; }

        [Required]
        [Display(Name = "Company Phone")]
        public int Employees { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
    }

   
}