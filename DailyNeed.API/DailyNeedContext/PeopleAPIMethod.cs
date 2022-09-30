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
    public class PeopleAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //get all people by company
        public IEnumerable<People> AllPeoples(int compid)
        {
            List<People> person = new List<People>();
            person = db.Peoples.Where(e => e.Company.Id == compid && e.Deleted == false && e.Active == true).Include("Warehouse").Include("State").Include("City").ToList();
            return person.AsEnumerable();
        }


        //Signup from registration page
        public People AddPeoplebyAdmin(People people, int CompanyId)
        {
            List<People> peoples = db.Peoples.Where(c => c.Email.Trim().Equals(people.Email.Trim()) && c.Deleted == false && c.Company.Id == CompanyId).ToList();
            State state = db.States.Where(x => x.Stateid == people.StateId && x.Deleted == false).Select(x => x).FirstOrDefault();
            City city = db.Cities.Where(x => x.Cityid == people.CityId && x.Deleted == false).Select(x => x).FirstOrDefault();
            //PWarehouseLink wh = db.PWarehouseLinks.Where(w => w.PWarehouseId == people.PWarehouseId).FirstOrDefault();
            Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            People objPeople = new People();
            if (peoples.Count == 0)
            {
                people.DisplayName = people.PeopleFirstName + " " + people.PeopleLastName;
                people.PeopleFirstName = people.PeopleFirstName;
                people.PeopleLastName = people.PeopleLastName;
                people.State = state;
                people.City = city;
                people.CreatedBy = "Admin";
                people.CreatedDate = indianTime;
                people.Email = people.Email;
                people.Password = people.Password;
                people.Mobile = people.Mobile;
                people.Permissions = people.Permissions;
                //people.WarehouseId = wh;
                people.Company = comp;
                people.Active = true;
                people.Deleted = false;
                db.Peoples.Add(people);
                int id = db.SaveChanges();
                people.Message = "Successfully";
                return people;
            }
            else
            {
                people.Message = "Error";
                return null;
            }
        }

        //update by admin // method
        public People PutPeoplebyAdmin(People objCust, int CompanyId)
        {
            People proj = db.Peoples.Where(x => x.PeopleID == objCust.PeopleID && x.Deleted == false && x.Company.Id == CompanyId).FirstOrDefault();
            State state = db.States.Where(x => x.Stateid == objCust.StateId && x.Deleted == false).Select(x => x).FirstOrDefault();
            City city = db.Cities.Where(x => x.Cityid == objCust.CityId && x.Deleted == false).Select(x => x).FirstOrDefault();
            //PWarehouseLink wh = db.PWarehouseLinks.Where(w => w.PWarehouseId == objCust.PWarehouseId).FirstOrDefault();
            Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            if (proj != null)
            {
                proj.DisplayName = proj.PeopleFirstName + " " + proj.PeopleLastName;
                proj.PeopleFirstName = objCust.PeopleFirstName;
                proj.PeopleLastName = objCust.PeopleLastName;
                proj.State = state;
                proj.City = city;
                proj.UpdatedDate = indianTime;
                proj.Email = objCust.Email;
                proj.Password = objCust.Password;
                proj.Mobile = objCust.Mobile;
                proj.Permissions = objCust.Permissions;
                proj.UpdateBy = objCust.UpdateBy;
                proj.EmailConfirmed = objCust.EmailConfirmed;
                //proj.PWarehouse = wh;
                proj.Company = comp;
                proj.Active = true;
                proj.Deleted = false;
                db.Peoples.Attach(proj);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
                objCust.Message = "Successfully";
                return objCust;
            }
            else
            {
                objCust.Message = "Error";
                return objCust;
            }
        }

        //Delete method for people by id
        public bool DeletePeople(int id)
        {
            try
            {
                People proj = db.Peoples.Where(x => x.PeopleID == id && x.Deleted == false && x.Active == true /*&& x.CompanyId == CompanyId*/).FirstOrDefault();
                proj.Deleted = true;
                proj.Active = false;
                db.Peoples.Attach(proj);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}