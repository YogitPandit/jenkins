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
    [RoutePrefix("api/NewItemRequest")]
    public class NewItemRequestController : ApiController
    {
        ItemMasterNAPIMethod context = new ItemMasterNAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

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
                var itemslist = (from e in db.NewItemRequests
                                 where e.Deleted == false && e.Active == false || e.Active == true && e.Status == "Pending" || e.Status == "Approved"
                                 select new
                                 {
                                     e.NewItemRequestId,
                                     e.ItemName,
                                     e.Category,
                                     e.SubCategory,
                                     e.CostPrice,
                                     e.SellingPrice,
                                     e.description,
                                     e.Status
                                     //e.SubCategory,
                                     //e.SubCategory.SubcategoryName,
                                     //e.Category.Categoryid,
                                     //e.Category.CategoryName,
                                     ////e.Warehouse.WarehouseName,
                                     //e.Warehouse.WarehouseId,
                                     //e.BasePrice,
                                     //e.Quantity,
                                     //e.City.Cityid,
                                     //e.City.CityName,
                                 }).OrderByDescending(x => x.NewItemRequestId).ToList();
                return itemslist;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Item " + ex.Message);
                logger.Info("End  Item: ");
                return null;
            }

        }

        [ResponseType(typeof(NewItemRequest))]
        [Route("")]
        [AcceptVerbs("POST")]
        public NewItemRequest addNewItem(NewItemRequest items)
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
                // NewItemRequest requestItem = db.NewItemRequests.Where(x => x.NewItemRequestId == items.NewItemRequestId).FirstOrDefault();
                Company comp = db.Companies.Where(x => x.Id == compid && x.Deleted == false).FirstOrDefault();
                items.Company = comp;
                People provider = db.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
                items.Provider = provider;
                PWarehouseLink pwh = db.PWarehouseLinks.Where(w => w.PWarehouseId == items.PWarehouseId && w.Deleted == false).FirstOrDefault();
                if (items != null)
                {
                    items.Status = "Pending";
                    items.Active = false;
                    items.CreatedDate = indianTime;
                    items.PWarehouse = pwh;
                    db.NewItemRequests.Add(items);
                    int id = db.SaveChanges();
                    items.Message = "Successfully";
                    //context.AddReqNewItem(items , CompanyId, userid);
                    return items;
                }
                else
                {
                    items.Message = "Error";
                    return items;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Item Master " + ex.Message);
                return null;
            }
        }

        [ResponseType(typeof(NewItemRequest))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public NewItemRequest updateItem(NewItemRequest items)
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
                Company comp = db.Companies.Where(x => x.Id == compid && x.Deleted == false).FirstOrDefault();
                items.Company = comp;
                People provider = db.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
                items.Provider = provider;
                if (items != null)
                {
                    NewItemRequest requestItem = db.NewItemRequests.Where(x => x.NewItemRequestId == items.NewItemRequestId).FirstOrDefault();

                    requestItem.ItemName = items.ItemName;
                    requestItem.Category = items.Category;
                    requestItem.SubCategory = items.SubCategory;
                    requestItem.description = items.description;
                    requestItem.SellingPrice = items.SellingPrice;
                    requestItem.CostPrice = items.CostPrice;
                    requestItem.Status = "Pending";
                    requestItem.Active = false;
                    requestItem.UpdatedDate = indianTime;
                    db.Entry(requestItem).State = EntityState.Modified;
                    db.SaveChanges();
                    requestItem.Message = "Successfully";
                    //context.AddReqNewItem(items , CompanyId, userid);
                    return requestItem;
                }
                else
                {
                    items.Message = "Error";
                    return items;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Item Master " + ex.Message);
                items.Message = "Error";
                return null;
            }
        }

        // Delete Item By id / Delete Method
        [ResponseType(typeof(NewItemRequest))]
        [Route("")]
        [AcceptVerbs("DELETE")]
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
                NewItemRequest itemlist = db.NewItemRequests.Where(x => x.NewItemRequestId == id && x.Deleted == false).FirstOrDefault();
                itemlist.Deleted = true;
                itemlist.Active = false;
                db.NewItemRequests.Attach(itemlist);
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
