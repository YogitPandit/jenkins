using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DailyNeed.API.Models
{
    public class UserModel
    {
        [Display(Name = "DepartmentId")]
        public string DepartmentId { get; set; }

        [Required]
        public string MobileNumber { get; set; }


        [Display(Name = "User name")]
        [DefaultValue("")]
        public string UserName { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }



        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }


        [Display(Name = "Company Zip")]
        public string CompanyZip { get; set; }
        public string Address { get; set; }


        [Display(Name = "Company Phone")]
        public string CompanyPhone { get; set; }


        [Display(Name = "Company Phone")]
        public int Employees { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string PeopleFirstName { get; set; }
        public string PeopleLastName { get; set; }
        public int PWarehouseId { get; set; }
        public int Stateid { get; set; }
        public int Cityid { get; set; }
        public string Mobile { get; set; }
        public string Department { get; set; }
        public string Skcode { get; set; }
        public string Permissions { get; set; }
        public bool Active { get; set; }
        public string SUPPLIERCODES { get; set; }
        public string Salesexecutivetype { get; set; }
        public string AgentCode { get; set; }
        public string GSTIN { get; set; }

        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool Success { get; set; }
    }


}