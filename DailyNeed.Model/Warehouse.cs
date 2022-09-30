using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public virtual Company Company { get; set; }
        [NotMapped]
        public int CompanyId { get; set; }
        public virtual State State { get; set; }
        [NotMapped]
        public int StateId { get; set; }
        public virtual City City { get; set; }
        [NotMapped]
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }                
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public string Message { get; set; }

    }
}
