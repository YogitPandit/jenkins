using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJSAuthentication.Model
{
   public class User
    {
        [Key]
        public string userId { get; set; }
      
        public string firstName { get; set; }
       
        public string lastName { get; set; }
        public int age { get; set; }

        public DateTime? CreatedDate { get; set; }
      
        public String CreatedBy { get; set; }
        public DateTime? LastModifiedBy { get; set; }
        public int customerId { get; set; }
        
    }
}
