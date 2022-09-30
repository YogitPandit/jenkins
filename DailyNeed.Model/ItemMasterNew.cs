using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
   public class ItemMasterNew
    {
        [Key]
        public int ItemId { get; set; }
        public virtual Company Company { get; set; }
        [NotMapped]
        public int CompanyId { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        [NotMapped]
        public int WarehouseId { get; set; }
        public virtual City City { get; set; }
        [NotMapped]
        public int CityId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemType { get; set; }                              
        public string Barcode { get; set; }
        public double Discount { get; set; }
        public double BasePrice { get; set; }
        public double Quantity { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string LogoUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual BaseCategory BaseCategory { get; set; }
        [NotMapped]
        public int BaseCategoryId { get; set; }
        public virtual Category Category { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        [NotMapped]
        public int SubCategoryId { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public int id { get; set; }

    }
}
