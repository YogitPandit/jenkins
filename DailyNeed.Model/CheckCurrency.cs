using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenricEcommers.Models
{
    public class CheckCurrency
    {
        [Key]
        public int CheckCurrencyId { get; set; }
        public int DBoyCId { get; set; }
        public int DeliveryIssuanceId { get; set; }
        public int PeopleId { get; set; }
        public string Peoplename { get; set; }
        public string Mobile { get; set; }
        public string status { get; set; }
        public double checkTotalAmount { get; set; }
        public string checknumber { get; set; }
        public string checkamount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public bool check { get; set; }
        //[NotMapped]
        //public bool settlecheck { get; set; }
        [NotMapped]
        public bool DboycheckRecived { get; set; }

        [NotMapped]
        public int OrderId { get; set; }

    }
}
