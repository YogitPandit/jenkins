using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace DailyNeed.Model
{
  public class InvoiceBill
    {
        [Key]
        public int BillId { get; set; }
        public float PaidAmount { get; set; }
        public float RemainingAmount { get; set; }
        public string PaymentType { get; set; }
        public DateTime BillCreatedDate { get; set; }
        public virtual Company Company { get; set; }
        public virtual People Provider { get; set; }
        public virtual CustomerLink CustomerLink { get; set; }
        [NotMapped]
        public int CustomerLinkId { get; set; }
        public virtual CustomerInvoice CustomerInvoice { get; set; }
        [NotMapped]
        public int CustomerInvoiceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string Comment { get; set; }
        public string AmountType { get; set; }
        [NotMapped]
        public string Message { get; set; }
    }
}
