using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class AnimalType
    {
        [Key]
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
        public string AnimalAlias { get; set; }

        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public bool Flag { get; set; }

    }
}
