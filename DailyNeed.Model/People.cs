using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace DailyNeed.Model
{
    public class People
    {
        public People()
        {
            if (this.DisplayName == null)
            {
                this.DisplayName = PeopleFirstName + " " + PeopleLastName;
            }
        }
        [Key]
        public int PeopleID { get; set; }
        [NotMapped]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [NotMapped]
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        //public virtual PWarehouseLink PWarehouse { get; set; }
        [DefaultValue("")]
        public string PeopleFirstName { get; set; }
        [DefaultValue("")]
        public string PeopleLastName { get; set; }
        public string Email { get; set; }
        [DefaultValue("")]
        public string DisplayName { get; set; }
        [NotMapped]
        public int StateId { get; set; }
        public virtual State State { get; set; }
        [NotMapped]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public string Mobile { get; set; }
        public bool IsVerify { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public string Permissions { get; set; }
        public bool Deleted { get; set; }
        [DefaultValue("false")]
        public bool Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string GSTIN { get; set; }
        public string UpdateBy { get; set; }
        public string UserLoginType { get; set; }
        public bool EmailConfirmed { get; set; }
        [NotMapped]
        public string Message { get; set; }
        

    }
}
