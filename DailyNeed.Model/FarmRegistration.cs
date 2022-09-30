using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class FarmRegistration
    {
        [Key]
        public int FarmID { get; set; }
        public string FarmName { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string PrimaryContactNumber { get; set; }
        public virtual Company Company { get; set; }
        public string EmailId { get; set; }
        public string Remark { get; set; }
        public bool IsDefault { get; set; }


        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool Flag { get; set; }

    }
}
