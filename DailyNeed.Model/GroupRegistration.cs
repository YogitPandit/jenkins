using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class GroupRegistration
    {

        [Key]
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public virtual FarmRegistration Farm { get; set; }
        public virtual ShedRegistration Shed { get; set; }
        public string Remark { get; set; }



        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string Message { get; set; }
        public bool Flag { get; set; }


        [NotMapped]
        public int FarmId { get; set; }
        [NotMapped]
        public int ShedId { get; set; }



    }
}
