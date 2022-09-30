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
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public virtual Company Company { get; set; }
        [NotMapped]
        public int CompanyId { get; set; }
        public string Skcode { get; set; }
        public virtual PWarehouseLink PWarehouse { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        [NotMapped]
        public int PWarehouseId { get; set; }
        public string DNCode { get; set; }
        // whatsapp number
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string BillingName { get; set; }
        public string GPSLocation { get; set; }
        public string PhotoUrl { get; set; }
        public string LandMard { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public DateTime? DOB { get; set; }
        public virtual Customer ParentCustomer { get; set; }
        [NotMapped]
        public int DepoId { get; set; }
        public virtual Depot Depo { get; set; }
        [NotMapped]
        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
        [NotMapped]
        public int categoryId { get; set; }
        public virtual Category Category { get; set; }
        [NotMapped]
        public int subCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        [NotMapped]
        public int ItemId { get; set; }
        public virtual ItemMasterNew Item { get; set; }
        [NotMapped]
        public string Message { get; set; }
        
        public string Comment { get; set; }

        //add service fields
        [NotMapped]
        public int ShiftId { get; set; }
        [NotMapped]
        public string ShiftName { get; set; }
        [NotMapped]
        public string StartDate { get; set; }
        [NotMapped]
        public string DeliveryDays { get; set; }
        [NotMapped]
        public DateTime DeliveryTime { get; set; }
        [NotMapped]
        public int PItemId { get; set; }
        [NotMapped]
        public int CustomerLinkId { get; set; }
        [NotMapped]
        public float Price { get; set; }
        [NotMapped]
        public float Quantity { get; set; }
        [NotMapped]
        public int WarehouseId { get; set; }


    }

    
  
}