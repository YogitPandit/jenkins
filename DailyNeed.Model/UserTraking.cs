using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
   public class UserTraking
    {
        [Key]
        public int Id { get; set; }

        public string PeopleId { get; set; }

        public string Type { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public string Action { get; set; }
        public string Action_t { get; set; }
        public string Remark { get; set; }

    }
}
