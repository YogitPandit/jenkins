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
    public class WarehouseAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
       // City city; State St;


        public Warehouse getwarehousebyid(int id, int CompanyId)
        {
            return db.Warehouses.Where(p => p.WarehouseId == id && p.Deleted == false && p.CompanyId == CompanyId).FirstOrDefault();
        }
        public IEnumerable<Warehouse> AllWarehouseWid(int compid, int Warehouse_id)
        {
            if (db.Warehouses.AsEnumerable().Count() > 0)
            {
                return db.Warehouses.Where(p => p.CompanyId == compid && p.Deleted == false && p.WarehouseId == Warehouse_id).AsEnumerable();
            }
            else
            {
                List<Warehouse> warehouse = new List<Warehouse>();
                return warehouse.AsEnumerable();
            }
        }
        public IEnumerable<Warehouse> AllWarehouse(int compid)
        {
            if (db.Warehouses.AsEnumerable().Count() > 0)
            {
                return db.Warehouses.Where(p => p.Company.Id == compid && p.Deleted == false).Include("State").Include("City").ToList();
            }
            else
            {
                List<Warehouse> warehouse = new List<Warehouse>();
                return warehouse.AsEnumerable();
            }
        }

        //Add Warehouse Method
        public Warehouse AddWarehouse(Warehouse objwarehouse, int compid)
        {
            try
            {
                //Cheaking All Data Which is related to Parent Table
                List<Warehouse> warehouse = db.Warehouses.Where(x => x.WarehouseId == objwarehouse.WarehouseId && x.Deleted == false).ToList();
                City cit = db.Cities.Where(x => x.Cityid == objwarehouse.CityId && x.Deleted == false).Select(x => x).FirstOrDefault();
                State st = db.States.Where(x => x.Stateid == objwarehouse.StateId && x.Deleted == false).Select(x => x).FirstOrDefault();
                Company com = db.Companies.Where(x => x.Id == compid && x.Deleted == false).Select(x => x).FirstOrDefault();
                People provider = db.Peoples.Where(x => x.Company.Id == compid).FirstOrDefault();
                Warehouse wh = new Warehouse();
                if (warehouse.Count == 0)
                {
                    objwarehouse.CreatedBy = objwarehouse.CreatedBy;
                    objwarehouse.CreatedDate = indianTime;
                    objwarehouse.UpdatedDate = indianTime;
                    objwarehouse.City = cit;
                    objwarehouse.State = st;
                    objwarehouse.Company = com;
                    objwarehouse.Active = true;
                    objwarehouse.Deleted = false;
                    db.Warehouses.Add(objwarehouse);
                    int id = db.SaveChanges();
                    if (id != 0)
                    {
                        objwarehouse.Message = "Successfully";
                    }
                    else
                    {
                        objwarehouse.Message = "Error";
                    }
                    Warehouse whl = db.Warehouses.Where(x => x.WarehouseId == objwarehouse.WarehouseId && x.Deleted == false).FirstOrDefault();
                    PWarehouseLink Pwh = new PWarehouseLink();
                    Pwh.Company = com;
                    Pwh.Provider = provider;
                    Pwh.IsActive = true;
                    Pwh.Deleted = false;
                    Pwh.Warehouse = whl;
                    Pwh.CreatedBy = objwarehouse.CreatedBy;
                    db.PWarehouseLinks.Add(Pwh);
                    db.SaveChanges();
                    objwarehouse.Company = null;
                }
                else
                {
                    return objwarehouse;
                }
                return objwarehouse;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }
        }

        public Warehouse PutWarehouse(Warehouse objwarehouse, int compid)
        {
            Warehouse warehouse = db.Warehouses.Where(x => x.WarehouseId == objwarehouse.WarehouseId && x.Deleted == false).FirstOrDefault();
            City cit = db.Cities.Where(x => x.Cityid == objwarehouse.CityId && x.Deleted == false).Select(x => x).FirstOrDefault();
            State st = db.States.Where(x => x.Stateid == objwarehouse.StateId && x.Deleted == false).Select(x => x).FirstOrDefault();
            Company com = db.Companies.Where(x => x.Id == compid && x.Deleted == false).Select(x => x).FirstOrDefault();
            People provider = db.Peoples.Where(x => x.Company.Id == compid).FirstOrDefault();
            if (warehouse != null)
            {
                warehouse.UpdatedDate = indianTime;
                warehouse.State = st;
                warehouse.City = cit;
                warehouse.WarehouseName = objwarehouse.WarehouseName;
                warehouse.CreatedBy = objwarehouse.CreatedBy;
                warehouse.UpdateBy = objwarehouse.UpdateBy;
                warehouse.Address = objwarehouse.Address;
                warehouse.Company = com;
                warehouse.Active = objwarehouse.Active;
                warehouse.Message = "Successfully";
                db.Warehouses.Attach(warehouse);
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                return warehouse;
            }
            else
            {
                warehouse.Message = "Error";
                return warehouse;
            }
        }
        public bool DeleteWarehouse(int id, int CompanyId)
        {
            try
            {
                Warehouse warehouse = db.Warehouses.Where(x => x.WarehouseId == id).Where(x => x.Deleted == false && x.Company.Id == CompanyId).FirstOrDefault();
                warehouse.Deleted = true;
                db.Warehouses.Attach(warehouse);
                db.Entry(warehouse).State = EntityState.Modified;
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