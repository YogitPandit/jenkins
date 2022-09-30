using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
    public class CustomerWalletHistory
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int WarehouseId { get; set; }
        public int CustomerId { get; set; }
        public int? OrderId { get; set; }
        public int? PeopleId { get; set; }
        public double? TotalWalletAmount { get; set; }
        public double? NewAddedWAmount { get; set; }
        public double? NewOutWAmount { get; set; }
        public double? rewardPoint { get; set; }
        public double? EarningPoint { get; set; }
        public double? UsedPoint { get; set; }
        public double? MilestonePoint { get; set; }
        public double? TotalrewardPoint { get; set; }
        public double? TotalEarningPoint { get; set; }
        public double? TotalUsedPoint { get; set; }
        public double? TotalMilestonePoint { get; set; }
        public string ShopName { get; set; }
        public string Through { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public string Skcode { get; set; }

    }      
}
