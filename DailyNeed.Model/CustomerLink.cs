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
  public class CustomerLink
    {
        [Key]
        public int CustomerLinkId { get; set; }
        [NotMapped]
        public int ProviderId { get; set; }
        public virtual People Provider { get; set; }
        [NotMapped]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [NotMapped]
        public int PWarehouseId { get; set; }
        public virtual PWarehouseLink PWarehouse { get; set; }
        [NotMapped]
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        [NotMapped]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string DNCode { get; set; }
        public string Mobile { get; set; }
        public string BillingName { get; set; }
        public string GPSLocation { get; set; }
        public string PhotoUrl { get; set; }
        public string LandMard { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string Comment { get; set; }
        public virtual CustomerLink ParentCustomer { get; set; }
        [NotMapped]
        public string Message { get; set; }
    }

    public class CustomerLinkDto
    {
        public int CustomerLinkId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public int PWarehouseId { get; set; }
        public string MobileNumber { get; set; }
    }
    public class CustomerServDto
    {
        public int ServiceId { get; set; }
        public int PItemId { get; set; }
        public string ItemName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public float Quantity { get; set; }
        public string Days { get; set; }
        public string Time { get; set; }
        public string Comment { get; set; }
        public int PWarehouseID { get; set; }
    }
}
