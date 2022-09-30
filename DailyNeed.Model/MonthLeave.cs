using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class MonthLeave
    {
        [Key]
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public virtual Company Company { get; set; }
        public virtual CustomerLink CustomerLink { get; set; }
        public virtual People Provider { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public int CustomerLinkId { get; set; }
    }
}