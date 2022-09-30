using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class Zone
    {
        [Key]
        public int Zoneid { get; set; }
        public string ZoneName { get; set; }
        public string ZoneAddress { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        public virtual Company Company { get; set; }
        [NotMapped]
        public int id { get; set; }
        public virtual People Provider { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        [NotMapped]
        public int WarehouseId { get; set; }
        public virtual Depot Depot { get; set; }
        [NotMapped]
        public int Depotid { get; set; }
        public string DepotName { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool Success { get; set; }
    }
}
