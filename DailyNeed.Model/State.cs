using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNeed.Model
{
    public class State
    {
        [Key]
        public int Stateid { get; set; }
        public string StateName { get; set; }
        public string AliasName { get; set; }  
        public bool active { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }                
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public string Message { get; set; }

    }
}
