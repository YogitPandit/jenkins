using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
    public class NewItemRequest
    {
        [Key]
        public int NewItemRequestId { get; set; }
        public virtual Company Company { get; set; }
        public virtual People Provider { get; set; }
        public virtual PWarehouseLink PWarehouse { get; set; }
        [NotMapped]
        public int PWarehouseId { get; set; }
        public string ItemName { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }
        public string description { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public string Message { get; set; }
    }
}
