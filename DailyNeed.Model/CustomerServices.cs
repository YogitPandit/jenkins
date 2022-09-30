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
    public class CustomerServices
    {
        [Key]
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public virtual Company Company { get; set; }
        public virtual PBaseCategoryLink PBaseCategory { get; set; }
        public virtual PCategoryLink PCategory { get; set; }
        public virtual PSubCategoryLink PSubcategory { get; set; }
        public virtual CustomerLink CustomerLink { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PItemLink PItem { get; set; }
        public virtual PWarehouseLink PWarehouse { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual People Provider { get; set; }
        public virtual ShiftMaster Shift { get; set; }
        public float Price { get; set; }
        public float Quantity { get; set; }
        public virtual UnitMaster Unit { get; set; }
        public DateTime? LastBillingDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public string BillingType { get; set; }
        public string DeliveryDays { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public string Message { get; set; }
        public string Comment { get; set; }

        [NotMapped]
        public int CustomerLinkId { get; set; }
        [NotMapped]
        public int PItemId { get; set; }
        [NotMapped]
        public int PWarehouseId { get; set; }
        [NotMapped]
        public int ProviderId { get; set; }
        [NotMapped]
        public int ShiftId { get; set; }
    }

    public class CustomerServicesDTO
    {
        public int ServiceId { get; set; }
        public float Price { get; set; }
        public float Quantity { get; set; }
        public int CustomerId { get; set; }
        public string DeliveryTime { get; set; }
        public string CustomeName { get; set; }
        public string ProductName { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public List<string> LeaveRecords { get; set; }
    }
}
