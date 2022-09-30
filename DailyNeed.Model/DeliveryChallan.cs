using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
   public class DeliveryChallan
    {
        [Key]
        public int ChallanId { get; set; }
        public string ChallanNumber { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
        public float FinalQty { get; set; }
        public string ItemName { get; set; }
        public string TransactionType { get; set; }
        public virtual Company Company { get; set; }
        public virtual People Provider { get; set; }
        public virtual CustomerLink CustomerLink { get; set; }
        public virtual CustomerServices CustomerService { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public string flag { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public int PeopleID { get; set; }
        [NotMapped]
        public int CustomerLinkId { get; set; }
        [NotMapped]
        public int ID { get; set; }
    }
}
