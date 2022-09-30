using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/LogOut")]
    public class LogOutController : ApiController
    {
        [HttpPost]
        public void logoutData()
        {
        //    DailyNeedContext dc = new DailyNeedContext();
          
        //    var data = dc.UserTrakings.Where(o => o.Id ==12 && o.PeopleId=="156").SingleOrDefault();
        //    data.LogOutTime = DateTime.Now;
        //    data.Remark = data.Remark + "=> logout page ,";
        //    dc.UserTrakings.Add(data);
        //    dc.SaveChanges();
        }

    }
}
