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
    public class DepotAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Get All Depot BY Company ID
        public List<Depot> AllDepot(int compid)
        {
            //if (db.Categorys.AsEnumerable().Count() > 0)
            //{
            //    return db.Categorys.Where(p => p.Deleted == false).ToList();
            //}
            //else
            //{
                var depotList = db.Depots.Where(d => d.Deleted == false && d.IsActive == true && d.Company.Id==compid).Include("Warehouse").Include("Company").ToList();
                return depotList;
            //}
        }

        //Add Depot 
        public Depot AddDepot(Depot depot, int CompanyId, int UserId)
        {
            //Finding Depot by Name to token company
            List<Depot> dpt = db.Depots.Where(c => c.Deleted == false && c.DepotName.Trim().Equals(depot.DepotName.Trim()) && c.Company.Id==CompanyId).ToList();
            //Getting Company By token 
            Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            //Getting Provider By token 
            People peop = db.Peoples.Where(p => p.PeopleID == UserId && p.Deleted == false).FirstOrDefault();
            //Getting Warehouse By token 
            Warehouse wh = db.Warehouses.Where(w => w.WarehouseId == depot.WarehouseId && w.Deleted == false).FirstOrDefault();

            Depot objcat = new Depot();
            if (dpt.Count == 0)
            {
                depot.CreatedBy = Convert.ToString(UserId);
                depot.CreatedDate = indianTime;
                depot.Company = comp;
                depot.Warehouse = wh;
                depot.Provider = peop;
                depot.IsActive = true;
                depot.Deleted = false;
                db.Depots.Add(depot);
                db.SaveChanges();
                depot.Message = "Successfully";
                depot.Success = true;
                db.Depots.Attach(depot);
                db.Entry(depot).State = EntityState.Modified;
                int id = db.SaveChanges();
                return depot;
            }
            else
            {
                depot.Message = "Already Exists!";
                depot.Success = false;
                return depot;
            }
        }

        //PUT- Update Depot
        public Depot PutDepot(Depot objcat, int CompanyId, int UserId)
        {
            Depot depot = db.Depots.Where(x => x.Depotid == objcat.Depotid && x.Deleted == false).FirstOrDefault();
            Warehouse wh = db.Warehouses.Where(w => w.WarehouseId == objcat.WarehouseId && w.Deleted == false).SingleOrDefault();
            Company comp = db.Companies.Where(c => c.Id == CompanyId && c.Deleted == false).FirstOrDefault();
            People peop = db.Peoples.Where(p => p.PeopleID == UserId && p.Deleted == false).FirstOrDefault();
            if (depot != null)
            {
                try
                {
                    depot.Company = comp;
                    depot.Warehouse = wh;
                    depot.Provider = peop;
                    depot.UpdatedDate = indianTime;
                    depot.DepotName = objcat.DepotName;
                    depot.DepotAddress = objcat.DepotAddress;
                    depot.Description = objcat.Description;
                    depot.WarehouseId = objcat.WarehouseId;
                    depot.UpdateBy = Convert.ToString(UserId);
                    depot.IsActive = true;
                    depot.Deleted = false;
                    depot.Message = "Successfully";
                    depot.Success = true;
                    db.Depots.Attach(depot);
                    db.Entry(depot).State = EntityState.Modified;
                    db.SaveChanges();
                    return depot;
                }
                catch (Exception ex)
                {
                    logger.Error("Error in updateDepot " + ex.Message);
                    logger.Info("End  updateDepot: ");
                    depot.Message = "Error";
                    depot.Success = false;
                    return depot;
                }
            }
            else
            {
                objcat.Message = "Error";
                objcat.Success = false;
                return objcat;
            }
        }
    }
}