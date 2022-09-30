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
    public class CompanyAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Get Company BY Id - GET Method
        public Company GetCompanybyCompanyId(int id)
        {
            Company p = db.Companies.Where(c => c.Id == id).SingleOrDefault();
            if (p != null)
            {
            }
            else
            {
                p = new Company();
            }
            return p;
        }

        //Update Company - PUT Method
        public Company PutCompany(Company company)
        {
            Company proj = db.Companies.Where(x => x.Id == company.Id).FirstOrDefault();
            if (proj != null)
            {
                proj.UpdatedDate = indianTime;
                proj.AlertDay = company.AlertDay;
                proj.AlertTime = company.AlertTime;
                proj.FreezeDay = company.FreezeDay;
                proj.TFSUrl = company.TFSUrl;
                proj.TFSUserId = company.TFSUserId;
                proj.TFSPassword = company.TFSPassword;
                proj.LogoUrl = company.LogoUrl;
                proj.Address = company.Address;
                proj.CompanyName = company.CompanyName;
                proj.contactinfo = company.contactinfo;
                proj.currency = company.currency;
                proj.dateformat = company.dateformat;
                proj.fiscalyear = company.fiscalyear;
                proj.startweek = company.startweek;
                proj.timezone = company.timezone;
                proj.Webaddress = company.Webaddress;
                db.Companies.Attach(proj);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
                return proj;
            }
            else
            {
                return null;
            }
        }
    }
}