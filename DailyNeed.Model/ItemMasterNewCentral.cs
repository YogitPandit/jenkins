using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
   public class ItemMasterNewCentral
    {
        [Key]
        public int ItemCentralId { get; set; }
        public virtual Company Company { get; set; }
        public virtual City City { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemType { get; set; }
        public string Barcode { get; set; }
        public double BasePrice { get; set; }
        public double Quantity { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
