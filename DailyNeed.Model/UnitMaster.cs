using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyNeed.Model;

namespace DailyNeed.Model
{
  public class UnitMaster
    {
        [Key]
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public string UAlias { get; set; }
        public string UFormul { get; set; }
        public string UDiscription { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public int Warehouseid { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        
    }
}
