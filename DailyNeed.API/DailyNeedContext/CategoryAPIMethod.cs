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
    public class CategoryAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Get All Category BY Company ID
        public List<Category> AllCategory(int compid)
        {
            //if (db.Categorys.AsEnumerable().Count() > 0)
            //{
            //    return db.Categorys.Where(p => p.Deleted == false).ToList();
            //}
            //else
            //{
                var catList = db.Categorys.Include("BaseCategory").ToList();
                return catList;
            //}
        }

        //Add Category 
        public Category AddCategory(Category category, int CompanyId)
        {
            Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
            List<Category> cat = db.Categorys.Where(c => c.Deleted == false && c.CategoryName.Trim().Equals(category.CategoryName.Trim())).ToList();
            BaseCategory baseCate = db.BaseCategoryDb.Where(x => x.BaseCategoryId == category.BaseCategoryId).FirstOrDefault();
            Category objcat = new Category();
            if (cat.Count == 0)
            {
                category.CreatedBy = objcat.CreatedBy;
                category.CreatedDate = indianTime;
                category.UpdatedDate = indianTime;
                category.Company = comp;
                category.BaseCategory = baseCate;
                category.IsActive = true;
                category.Deleted = false;
                db.Categorys.Add(category);
                db.SaveChanges();
                category.Message = "Successfully";
                category.LogoUrl = category.LogoUrl;
                // category.LogoUrl = "http://137.59.52.130/../../images/catimages/" + category.BaseCategoryId + ".jpg";
                db.Categorys.Attach(category);
                db.Entry(category).State = EntityState.Modified;
                int id = db.SaveChanges();
                return category;
            }
            else
            {
                category.Message = "Error";
                return category;
            }
        }

        //PUT- Update Category
        public Category PutCategory(Category objcat)
        {
            Category category = db.Categorys.Where(x => x.Categoryid == objcat.Categoryid && x.Deleted == false).FirstOrDefault();
            BaseCategory BC =db.BaseCategoryDb.Where(x => x.BaseCategoryId == objcat.BaseCategoryId && x.Deleted == false).SingleOrDefault();
            if (category != null)
            {
                try
                {
                    category.BaseCategory = BC;
                    category.UpdatedDate = indianTime;
                    category.CategoryName = objcat.CategoryName;
                    category.CategoryHindiName = objcat.CategoryHindiName;
                    category.Discription = objcat.Discription;
                    category.LogoUrl = objcat.LogoUrl;
                    category.Code = objcat.Code;
                    category.LogoUrl = objcat.LogoUrl;
                    category.IsActive = objcat.IsActive;
                    category.Deleted = objcat.Deleted;
                    category.Message = "Successfully";
                    db.Categorys.Attach(category);
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return category;
                }
                catch (Exception ex)
                {
                    logger.Error("Error in addCategory " + ex.Message);
                    logger.Info("End  addCategory: ");
                    category.Message = "Error";
                    return category;
                }
            }
            else
            {
                return objcat;
            }
        }
    }
}