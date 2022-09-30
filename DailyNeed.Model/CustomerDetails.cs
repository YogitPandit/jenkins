using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
   public class CustomerDetails
    {
      
        public string Mobile { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
  
        public DateTime CreatedDate { get; set; }
      
     
        public string WarehouseName { get; set; }

    }
}
