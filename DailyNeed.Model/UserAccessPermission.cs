using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
   public class UserAccessPermission
    {
        [Key]
        public int id { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Admin { get; set; }
        public bool Delivery { get; set; }
        public bool TaxMaster { get; set; }
        public bool Customer { get; set; }
        public bool Supplier { get; set; }
        public bool Warehouse { get; set; }
        public bool CurrentStock { get; set; }
        public bool OrderMaster { get; set; }
        public bool DamageStock { get; set; }
        public bool ItemMaster { get; set; }
        public bool Reports { get; set; }
        public bool StatisticalRep { get; set; }
        public bool Offer { get; set; }
        public bool Sales { get; set; }
        public bool AppPromotion { get; set; }
        public bool ItemCategory { get; set; }
        public bool CRM { get; set; }
        public bool Request { get; set; }
        public bool UnitEconomics { get; set; }
        public bool PromoPoint { get; set; }
        public bool News { get; set; }
    }
}
