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
    public class CityAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Get All City
        public List<City> AllCitys()
        {
            //if (db.Cities.AsEnumerable().Count() > 0)
            //{
            //    return db.Cities.Where(p => p.Deleted == false).AsEnumerable();
            //}
            //else
            //{
            //List<City> city = new List<City>();
            //return city.AsEnumerable();
            //}
            var city = db.Cities.Where(x=>x.Deleted == false).Include("State").ToList();
            return city;
        }

        //Get City By State ID
        public IEnumerable<City> AllCity(int sid)
        {
            if (db.SubsubCategorys.AsEnumerable().Count() > 0)
            {
                return db.Cities.Where(p => p.State.Stateid == sid && p.Deleted == false).AsEnumerable();
            }
            else
            {
                List<City> city = new List<City>();
                return city.AsEnumerable();
            }
        }

        //Add City-POST Method
        public City AddCity(City city)
        {
            State states = db.States.Where(c => c.Stateid == city.State.Stateid && c.Deleted == false).FirstOrDefault();
            List<City> citys = db.Cities.Where(c => c.CityName.Trim().Equals(city.CityName.Trim()) && c.Deleted == false).ToList();
            City objCity = new City();
            try
            {
                if (citys.Count == 0)
                {
                    city.CreatedBy = city.CreatedBy;
                    city.CreatedDate = indianTime;
                    city.UpdatedDate = indianTime;
                    city.State = states;
                    city.Message = "Successfully";
                    db.Cities.Add(city);
                    int id = db.SaveChanges();
                    return city;
                }
                else
                {
                    return city;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in addCity " + ex.Message);
                logger.Info("End  addCity: ");
                city.Message = "Error";
                return city;
            }

        }

        //Update City - PUT Method
        public City PutCity(City objcity)
        {

            City citys = db.Cities.Where(x => x.Cityid == objcity.Cityid && x.Deleted == false).FirstOrDefault();
            if (citys != null)
            {
                citys.UpdatedDate = indianTime;
                citys.CityName = objcity.CityName;
                citys.aliasName = objcity.aliasName;
                citys.Code = objcity.Code;
                citys.CreatedBy = objcity.CreatedBy;
                citys.CreatedDate = objcity.CreatedDate;
                citys.UpdateBy = objcity.UpdateBy;
                objcity.Message = "Successfully";
                db.Cities.Attach(citys);
                db.Entry(citys).State = EntityState.Modified;
                db.SaveChanges();
                return objcity;
            }
            else
            {
                objcity.Message = "Error";
                return objcity;
            }
        }

        //Delete City - Delete Method(Soft)
        public bool DeleteCity(int id)
        {
            try
            {
                City citys = db.Cities.Where(x => x.Cityid == id).FirstOrDefault();
                citys.Deleted = true;
                db.Cities.Attach(citys);
                db.Entry(citys).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}