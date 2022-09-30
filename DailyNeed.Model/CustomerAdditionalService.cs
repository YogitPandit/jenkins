using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyNeed.Model;

namespace DailyNeed.Model
{
  public class CustomerAdditionalService
    {
        [Key]
        public int custAddServID { get; set; }         
        public virtual Company Company { get; set; }
        public virtual CustomerLink CustomerLink { get; set; }
        [NotMapped]
        public int CustomerLinkId { get; set; }
        public virtual People Provider { get; set; }
        public virtual CustomerServices CustomerService { get; set; }
        [NotMapped]
        public int CustomerServiceId { get; set; }
        public virtual PWarehouseLink PWarehouse { get; set; }
        [NotMapped]
        public int PWarehouseId { get; set; }
        public float Quantity { get; set; }
        public DateTime Date { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsLess { get; set; }
        [NotMapped]
        public string Message { get; set; }
    }
}
