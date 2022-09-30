using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/itemMasterNew")]
    public class ItemMasterNewController : ApiController
    {
        ItemMasterNAPIMethod context = new ItemMasterNAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        public static Logger logger = LogManager.GetCurrentClassLogger();


        //get All Item by company 
        //[Authorize]
        [Route("")]
        public object Get()
        {
            logger.Info("start Get Item: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "compid")
                    {
                        compid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "userid")
                    {
                        userid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "Warehouseid")
                    {
                        Warehouse_id = int.Parse(claim.Value);
                    }
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Item: ");
                var itemslist = (from e in db.ItemMasterNews where e.Deleted == false && e.Active == true
                                 select new
                                 {
                                     e.ItemId,
                                     e.ItemName,
                                     e.BaseCategory.BaseCategoryId,
                                     e.BaseCategory.BaseCategoryName,
                                     e.SubCategory.SubCategoryId,
                                     e.SubCategory.SubcategoryName,
                                     e.Category.Categoryid,
                                     e.Category.CategoryName,
                                     e.Warehouse.WarehouseName,
                                     e.Warehouse.WarehouseId,
                                     e.BasePrice,
                                     e.Quantity,
                                     e.City.Cityid,
                                     e.City.CityName
                                 }).OrderByDescending(x => x.ItemId).ToList();
                return itemslist;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Item " + ex.Message);
                logger.Info("End  Item: ");
                return null;
            }

        }

        //get All Item by company 
        //[Authorize]
        [Route("ItemByProvider")]
        public object GetProviderItem()
        {
            logger.Info("start Get Item: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "compid")
                    {
                        compid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "userid")
                    {
                        userid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "Warehouseid")
                    {
                        Warehouse_id = int.Parse(claim.Value);
                    }
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Item: ");

                var itemslist = (from c in db.PItemLinks
                                 join e in db.ItemMasterNews on c.Company.Id equals e.Company.Id
                                 where e.Deleted == false && e.Active == true
                                 select new
                                 {
                                     e.ItemId,
                                     e.ItemName,
                                     e.BasePrice,
                                     e.Quantity,
                                 }).OrderByDescending(x => x.ItemId).ToList();
                return itemslist;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Item " + ex.Message);
                logger.Info("End  Item: ");
                return null;
            }

        }


        //Add Item // Post Method
        [ResponseType(typeof(ItemMasterNew))]
        [Route("addNewItem")]
        [AcceptVerbs("POST")]
        public ItemMasterNew addNewItem(ItemMasterNew items)
        {
            logger.Info("start addItem Master: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                int Warehouse_id = 0;
                // Access claims
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "compid")
                    {
                        compid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "userid")
                    {
                        userid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "Warehouseid")
                    {
                        Warehouse_id = int.Parse(claim.Value);
                    }
                }
                int CompanyId = compid;
                if (items == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  addItem Master: ");
                context.AddItemMasterNew(items , CompanyId);
                return items;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Item Master " + ex.Message);
                return null;
            }
        }

        //Get All BaseCategory in dropdown for add item master 
        //[Authorize]
        [Route("BaseCategoryItem")]
        [AcceptVerbs("GET")]
        public IEnumerable<BaseCategory> BaseCategoryGet()
        {
            logger.Info("start Category: ");
            List<BaseCategory> blist = new List<BaseCategory>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                // Access claims
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "compid")
                    {
                        compid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "userid")
                    {
                        userid = int.Parse(claim.Value);
                    }
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                blist = db.BaseCategoryDb.ToList();
                logger.Info("End  Category: ");
                return blist;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }

        //Update Item PUT Method
        //[Authorize]
        [ResponseType(typeof(ItemMasterNew))]
        [Route("UpdateItem")]
        [AcceptVerbs("PUT")]
        public ItemMasterNew Put(ItemMasterNew itemMaster)
        {
            logger.Info("start putCustomerservices: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "compid")
                    {
                        compid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "userid")
                    {
                        userid = int.Parse(claim.Value);
                    }
                }
                if (itemMaster == null)
                {
                    throw new ArgumentNullException("Customer Service");
                }
                int CompanyId = compid;
                return context.PutItemMasterNew(itemMaster , CompanyId);
            }
            catch (Exception ex)
            {
                logger.Error("Error in put Customer Service " + ex.Message);
                return null;
            }
        }

        // Delete Item By id / Delete Method
        [ResponseType(typeof(ItemMasterNew))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start del Items: ");
            try
            {

                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "compid")
                    {
                        compid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "userid")
                    {
                        userid = int.Parse(claim.Value);
                    }
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                ItemMasterNew itemlist = db.ItemMasterNews.Where(x => x.ItemId == id && x.Deleted == false).FirstOrDefault();
                itemlist.Deleted = true;
                itemlist.Active = false;
                db.ItemMasterNews.Attach(itemlist);
                db.Entry(itemlist).State = EntityState.Modified;
                db.SaveChanges();
                logger.Info("End  delete Item: ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in del Item " + ex.Message);
            }
        }

    }
}
