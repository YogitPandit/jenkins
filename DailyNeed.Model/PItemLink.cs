using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class PItemLink
    {
        [Key]
        public int PItemId { get; set; }
        public virtual People Provider { get; set; }
        public virtual Company Company { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual BaseCategory Basecategory { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory Subcategory { get; set; }
        public virtual ItemMasterNew ItemMasters { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}