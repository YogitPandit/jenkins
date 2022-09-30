using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class SubsubCategory
    {
        [Key]
        public int SubsubCategoryid { get; set; }
        public string SubsubcategoryName { get; set; }
        public int BaseCategoryId { get; set; }
        //public int CompanyId { get; set; }
        public int Categoryid { get; set; }
        //public int Warehouseid { get; set; }

        public int? SortOrder { get; set; }
        public bool? IsPramotional { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubcategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public double? CommisionPercent { get; set; }

        public bool? IsExclusive
        {
            get; set;
        }
        [NotMapped]
        public string Message { get; set; }


    }
}
