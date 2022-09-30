
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyNeed.Model
{
    //public enum ApplicationTypes
    //{
    //    JavaScript = 0,
    //    NativeConfidential = 1
    //};
    
    public class setting
    {
        [Key] 
        public string Id { get; set; }
        //[Required]
        public string Webaddress { get; set; }
        //[Required]
        //[MaxLength(100)]
        public string companyName { get; set; }
        public string  Address { get; set; }
        public bool Active { get; set; }
   
        public string startweek { get; set; }
        public string currency { get; set; }
        public string timezone { get; set; }
        public string dateformat { get; set; }
        public string contactinfo { get; set; }
        public DateTime updated { get; set; }
        public string fiscalyear { get; set; }
    }
}