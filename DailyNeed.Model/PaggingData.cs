using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNeed.Model
{
    public class PaggingData
    {
        public int total_count { get; set; }
        public dynamic ordermaster { get; set; }
    }
    public class PaggingDatas
    {
        public int total_count { get; set; }
        public dynamic notificationmaster { get; set; }
    }
    public class PaggingDatastock
    {
        public int total_count { get; set; }
        public dynamic damagest { get; set; }
    }
    public class PaggingDataitem
    {
        public int total_count { get; set; }
        public dynamic otmaster { get; set; }
    }


    public class PaggingData_st
    {
        public int total_count { get; set; }
        public dynamic ordermaster { get; set; }

    }

    public class PaggingData_wt
    {
        public int total_count { get; set; }
        public dynamic ordermaster { get; set; }

    }

    public class PaggingData_AgentAmount
    {
        public int total_count { get; set; }
        public dynamic agentamount { get; set; }

    }
    public class PaggingData_ctin
    {
        public int total_count { get; set; }
        public dynamic historyamount { get; set; }

    }
    public class PaggingData_ctout
    {
        public int total_count1 { get; set; }
        public dynamic historyamountout { get; set; }

    }

}
