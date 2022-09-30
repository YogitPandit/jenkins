using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NLog;
using DailyNeed.API.Controllers;
using System.Runtime.Caching;
using DailyNeed.Model;
using GenricEcommers.Models;
using DailyNeed.Model.NotMapped;
using System.Net;
using System.Text;
using System.IO;

namespace DailyNeed.API.Controllers
{
    public class MonthLeaveAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        public MonthLeave MonthLeave(MonthLeave leave, int usreid, int CompanyId)
        {
            //Cheaking Dropdown Value with Db 
            MonthLeave mleave = new MonthLeave();
            CustomerLink customerlk = db.CustomerLinks.Where(x => x.CustomerLinkId == leave.CustomerLinkId).FirstOrDefault();
            People provider = db.Peoples.Where(x => x.PeopleID == usreid && x.Deleted == false).FirstOrDefault();
            Company comp = db.Companies.Where(x => x.Id == CompanyId && x.Deleted == false).FirstOrDefault();
            try
            {
                //saving Data to MonthLeave table
                mleave.CustomerLink = customerlk;
                mleave.Provider = provider;
                mleave.Company = comp;
                mleave.FromDate = leave.FromDate;
                mleave.ToDate = leave.ToDate;
                mleave.CreatedBy = usreid.ToString();
                //use description variable for add comment 
                mleave.Description = leave.Description;
                mleave.CreatedDate = indianTime;
                mleave.Deleted = false;
                mleave.IsActive = true;
                db.MonthLeaves.Add(mleave);
                int id1 = db.SaveChanges();
            }
            catch (Exception)
            {
                leave.Message = "Error";
                return leave;
            }
            leave.Message = "Successfully";
            return leave;
        }

        //Delete MonthLeave
        public bool DeleteMonthLeave(int id)
        {
            try
            {
                MonthLeave mlobj = db.MonthLeaves.Where(x => x.Id == id && x.Deleted == false && x.IsActive == true).FirstOrDefault();
                mlobj.Deleted = true;
                mlobj.IsActive = false;
                db.MonthLeaves.Attach(mlobj);
                db.Entry(mlobj).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Put MonthLeave
        public MonthLeave UpdateMonthLeave(MonthLeave leave, int userid, int CompanyId)
        {
            MonthLeave mleave = db.MonthLeaves.Where(x => x.Id == leave.Id).FirstOrDefault();
            CustomerLink customerlk = db.CustomerLinks.Where(x => x.CustomerLinkId == leave.CustomerLinkId && x.Deleted == false).FirstOrDefault();
            People provider = db.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
            Company comp = db.Companies.Where(x => x.Id == CompanyId && x.Deleted == false).FirstOrDefault();
            if (mleave != null)
            {
                mleave.FromDate = leave.FromDate;
                mleave.ToDate = leave.ToDate;
                mleave.UpdatedBy = userid.ToString();
                mleave.UpdatedDate = indianTime;
                mleave.CustomerLink = customerlk;
                mleave.Provider = provider;
                mleave.Company = comp;
                //use description variable for add comment 
                mleave.Description = leave.Description;
                mleave.IsActive = true;
                mleave.Deleted = false;
                db.Entry(mleave).State = EntityState.Modified;
                db.SaveChanges();
                return mleave;
            }
            else
            {
                return mleave;
            }
        }



    }
}