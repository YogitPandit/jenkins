using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AngularJSAuthentication.Model
{
    public class Category
    {
        [Key]
        public int Categoryid { get; set; }
       // public int CompanyId { get; set; }
        //public int Warehouseid { get; set; }
        public string CategoryName { get; set; }
        public string Discription { get; set; }     
        public DateTime CreatedDate { get; set; }                
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public string LogoUrl { get; set; }
       // public List<SubCategory> subcat { get; set; }
       // public List<SubsubCategory> subsubcat { get; set; }
        public bool Deleted { get; set; }
      //  public virtual ICollection<SubCategory> SubCategory { get; set; }
    }
}
