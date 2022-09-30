using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class CategoryImage
    {
        [Key]
        public int CategoryImageId { get; set; }
        //public int CompanyId { get; set; }
        //public int Warehouseid { get; set; }

        public int CategoryId { get; set; }
        public string CategoryImg { get; set; }
        public DateTime CreatedDate { get; set; }                
        public DateTime UpdatedDate { get; set; }

        public string Code { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }

     
        
    }
}
