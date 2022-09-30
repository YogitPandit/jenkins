using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AngularJSAuthentication.Model
{
    public class People
    {
        public People()
        {
            if (this.DisplayName == null)
            {
                this.DisplayName = PeopleFirstName + " "+ MiddleName + "" + PeopleLastName;
            }
        }
        [Key]
        public int PeopleID { get; set; }
        public int CompanyID { get; set; }
        [DefaultValue("")]
        public string PeopleFirstName { get; set; }

        [DefaultValue("")]
        public string MiddleName { get; set; }
        [DefaultValue("")]
        public string PeopleLastName { get; set; }



       // public string FullName { get; set; }
        public string Email { get; set; }
        [DefaultValue("")]
        public string DisplayName { get; set; }
        public string Country { get; set; }
       
        public int? Stateid { get; set; }
        public string state { get; set; }
        public int? Cityid { get; set; }
        public string city { get; set; }

        public string Mobile { get; set; }//new fields 07/09/2015
        public int? RoleId { get; set; }//new fields 07/09/2015
        //  public string RoleTitle { get; set; }

        //end
        public string Department { get; set; }
        public double BillableRate { get; set; }
        public string CostRate { get; set; }
        public string Permissions { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public bool Deleted { get; set; }
        [DefaultValue("false")]
        public bool EmailConfirmed { get; set; }
        [DefaultValue("true")]
        public bool Approved { get; set; }
        [DefaultValue("true")]
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        //  public string Password { get; set; }
    }
}
