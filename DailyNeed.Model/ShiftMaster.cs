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
  public class ShiftMaster
    {
        [Key]
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public virtual BaseCategory Bcategory { get; set; }
        public virtual People People { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public string Message { get; set; }
    }
}
