using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
  public class CustomerRegistration
    {
        [Key]
       
        public int CustomerId { get; set;}
        public int WarehouseId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string GeoLocation { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Deleted { get; set; }

    }
}
