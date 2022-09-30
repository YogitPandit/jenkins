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
    public class ItemMasterNAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Add AddItemMasterNew 
        public ItemMasterNew AddItemMasterNew(ItemMasterNew item, int CompanyId)
        {
            Company comp = db.Companies.Where(x => x.Id == CompanyId && x.Deleted == false).FirstOrDefault();
            Warehouse wh = db.Warehouses.Where(x => x.WarehouseId == item.WarehouseId && x.Deleted == false).FirstOrDefault();
            City city = db.Cities.Where(X => X.Cityid == item.CityId && X.Deleted == false).FirstOrDefault();
            BaseCategory bCategory = db.BaseCategoryDb.Where(x => x.BaseCategoryId == item.BaseCategoryId && x.Deleted == false).FirstOrDefault();
            Category category = db.Categorys.Where(x => x.Categoryid == item.CategoryId && x.Deleted == false).FirstOrDefault();
            SubCategory subCategory = db.SubCategorys.Where(x => x.SubCategoryId == item.SubCategoryId && x.Deleted == false).FirstOrDefault();
            if (item != null)
            {
                item.CreatedDate = indianTime;
                item.BasePrice = item.BasePrice;
                item.Quantity = item.Quantity;
                item.ItemName = item.ItemName;
                item.Warehouse = wh;
                item.BaseCategory = bCategory;
                item.Category = category;
                item.SubCategory = subCategory;
                item.ItemCode = item.ItemCode;
                item.Company = comp;
                item.City = city;
                item.Active = true;
                item.Deleted = false;
                db.ItemMasterNews.Add(item);
                int id = db.SaveChanges();
                item.Message = "Successfully";
                return item;
            }
            else
            {
                item.Message = "Error";
                return item;
            }
        }

        //Get Items by Company
        public IEnumerable<ItemMasterNew> AllItemMasterNew(int compid)
        {
            if (db.ItemMasterNews.AsEnumerable().Count() > 0)
            {
                return db.ItemMasterNews.Where(p => p.Deleted == false && p.Company.Id == compid).Include("Warehouse").Include("City").ToList();
            }
            else
            {
                List<ItemMasterNew> ItemList = new List<ItemMasterNew>();
                return db.ItemMasterNews.AsEnumerable();
            }
        }

        // Put MonthLeave
        public ItemMasterNew PutItemMasterNew(ItemMasterNew item, int CompanyId)
        {
            Company comp = db.Companies.Where(x => x.Id == CompanyId && x.Deleted == false).FirstOrDefault();
            Warehouse wh = db.Warehouses.Where(x => x.WarehouseId == item.WarehouseId && x.Deleted == false).FirstOrDefault();
            City city = db.Cities.Where(X => X.Cityid == item.CityId && X.Deleted == false).FirstOrDefault();
            BaseCategory bCategory = db.BaseCategoryDb.Where(x => x.BaseCategoryId == item.BaseCategoryId && x.Deleted == false).FirstOrDefault();
            Category category = db.Categorys.Where(x => x.Categoryid == item.CategoryId && x.Deleted == false).FirstOrDefault();
            SubCategory subCategory = db.SubCategorys.Where(x => x.SubCategoryId == item.SubCategoryId && x.Deleted == false).FirstOrDefault();
            if (item != null)
            {
                item.UpdatedDate = indianTime;
                item.ItemId = item.ItemId;
                item.BasePrice = item.BasePrice;
                item.Quantity = item.Quantity;
                item.ItemName = item.ItemName;
                item.Warehouse = wh;
                item.BaseCategory = bCategory;
                item.Category = category;
                item.SubCategory = subCategory;
                //item.ItemCode = item.ItemCode;
                item.Company = comp;
                item.City = city;
                item.Active = true;
                item.Deleted = false;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                item.Message = "Successfully";
                return item;
            }
            else
            {
                item.Message = "Error";
                return item;
            }
        }
    }
}