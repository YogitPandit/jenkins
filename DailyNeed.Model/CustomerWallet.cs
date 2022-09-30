using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class CustomerWallet
    {
        [Key]
        public int Id { get; set; }
        public float AvailableBalance { get; set; }
        public int ServiceId { get; set; }
        public float WalletLimit { get; set; }
        public virtual People Provider { get; set; }
        public virtual CustomerLink CustomerLink { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public string BalanceType { get; set; }
        [NotMapped]
        public string Message { get; set; }
    }
}