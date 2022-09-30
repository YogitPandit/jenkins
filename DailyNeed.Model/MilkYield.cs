using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DailyNeed.Model
{
    public class MilkYield
    {
        [Key]
        public int MilkYieldId { get; set; }
        public int TagNumber { get; set; }
        public virtual ShedRegistration Shed { get; set; }        public virtual GroupRegistration Group { get; set; }

        public virtual FarmRegistration Farm { get; set; }

        public virtual CattleRegistration Cattle { get; set; }
        public decimal Quantity { get; set; }
        public decimal Fat { get; set; }
        public decimal Snf { get; set; }
        public string Shift { get; set; }
        public DateTime Date { get; set; }
        public virtual People Peoples { get; set; }
        [NotMapped]
        public List<MilkYield> milkingAnimal { get; set; }

        


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
        public int Fid { get; set; }


        [NotMapped]        public int GroupID { get; set; }


        [NotMapped]
        public int ShedId { get; set; }


        [NotMapped]
        public int FarmId { get; set; }

        [NotMapped]

        public int CattleId { get; set; }

    }
}
