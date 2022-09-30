using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class CattleRegistration
    {
        [Key]
        public int CattleRegId { get; set; }
        public string CattleType { get; set; }
        public string AnimalName { get; set; }
        public int TagNumber { get; set; }
        public string UploadImage { get; set; }
        public int BodyWeight { get; set; }
        public string Breed { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public string Agency { get; set; }
        public DateTime? Purchasedate { get; set; }
        public DateTime? Expirydate { get; set; }
        public string Mother { get; set; }
        public string Father { get; set; }
        public virtual Gender Gender { get; set; }
        public DateTime? Calvingdate { get; set; }
        public int Cost { get; set; }
        public string Market { get; set; }
        public string Source { get; set; }
        public string Vendor { get; set; }
        public int Bodysize { get; set; }
        public string Pregnancymonth { get; set; }
        public DateTime? Lastinseminationdate { get; set; }
        public string Lactation { get; set; }
        public int Milkoutput { get; set; }
        public virtual ShedRegistration Shed { get; set; }
        public virtual GroupRegistration Group { get; set; }
        public virtual FarmRegistration Farm { get; set; }
        public virtual Breed BreedId { get; set; }
        public virtual AnimalType AnimalType { get; set; }
       
        public virtual SpeciesType SpeciesType { get; set; }
        public bool IsAnimalPregnant { get; set; }
        public virtual CattleType CattleID { get; set; }

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
        public int Catid { get; set; }

        [NotMapped]

        public int GroupID { get; set; }
        [NotMapped]

        public int ShedId { get; set; }


        [NotMapped]
        public int FarmId { get; set; }

    }
}
