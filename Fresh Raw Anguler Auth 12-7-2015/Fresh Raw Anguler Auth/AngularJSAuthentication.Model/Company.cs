using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJSAuthentication.Model
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int    EmployeesCount { get; set;  }
        public string CompanyName { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyPhone { get; set; }
        public string Webaddress { get; set; }
        //[Required]
        //[MaxLength(100)]
      //  public string emailDomain { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }

        public string TFSUrl { get; set; }
        public string TFSUserId { get; set; }
        public string TFSPassword { get; set; }
        [DefaultValue("false")]
        public bool EnableTFS { get; set; }

        public int FreezeDay { get; set; }
        public string AlertDay { get; set; }
        public int AlertTime { get; set; }
        public string startweek { get; set; }
        public string currency { get; set; }
        public string timezone { get; set; }
        public string dateformat { get; set; }
        public string contactinfo { get; set; }
        
        public string fiscalyear { get; set; }

        public string LogoUrl { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String CreatedBy { get; set; }
        public String LastModifiedBy { get; set; }
        public bool Deleted { get; set; }

      public string O365Url { get; set; }
        public string O365User { get; set; }
        public string O365Password { get; set; }
        [DefaultValue("false")]
        public bool EnableO365 { get; set; }

    }
}
