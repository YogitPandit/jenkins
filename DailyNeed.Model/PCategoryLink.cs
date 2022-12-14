using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class PCategoryLink
    {
        [Key]
        public int PCategoryId { get; set; }
        public virtual People Provider { get; set; }
        public virtual Company Company { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual BaseCategory Basecategory { get; set; }
        public virtual Category Categoryes { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public ICollection<PSubCategoryLink> AllPsubCategory { get; set; }
    }
}