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


namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/Warehouse")]
    public class WarehouseController : ApiController
    {
        WarehouseAPIMethod context = new WarehouseAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Get all warehouse
        [Authorize]
        [Route("")]
        public IEnumerable<Warehouse> Get()
        {
            logger.Info("start Warehouse: ");
            List<Warehouse> ass = new List<Warehouse>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                int Warehouse_id=0 ;

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
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid, Warehouse_id);

                if (Warehouse_id > 0)
                {
                    logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                    ass = context.AllWarehouseWid(compid, Warehouse_id).ToList();
                   
                    return ass;

                }
                else {
                    logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                    ass = context.AllWarehouse(compid).ToList();
                    logger.Info("End  Warehouse: ");
                    return ass;
                }
              
            }
            catch (Exception ex)
            {
                logger.Error("Error in Warehouse " + ex.Message);
                logger.Info("End  Warehouse: ");
                return null;
            }
        }
               
        // Add Warehouse //Method for post Warehouse
        [ResponseType(typeof(Warehouse))]
        [Route("")]
        [AcceptVerbs("POST")]
        public Warehouse add(Warehouse warehouel)
        {
            logger.Info("start addWarehouse: ");
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
                warehouel.CompanyId = compid;
                if (warehouel == null)
                {
                    throw new ArgumentNullException("item");
                }
                //Company com = db.Companies.Where(x => x.Deleted == false && x.Id == compid).FirstOrDefault();
                //item.Company.CompanyName = com.CompanyName;
                if(compid == 0 || userid == 0)
                {
                    warehouel.Message = "Please Check Access Token!";
                    return warehouel;
                }
                if (warehouel.WarehouseName=="" || warehouel.WarehouseName == null)
                {
                    warehouel.Message = "WareHouse Name Is Requaired!";
                    return warehouel;
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                warehouel.CreatedBy = userid.ToString();
                context.AddWarehouse(warehouel, compid);
                logger.Info("End  Warehouse: ");
                return warehouel;
            }
            catch (Exception ex)
            {
                logger.Error("Error in addQuesAns " + ex.Message);
                logger.Info("End  addWarehouse: ");
                if(warehouel == null)
                {
                    warehouel.Message = "Error";
                }
                return warehouel;
            }
        }

        //Update Warehouse // Method for put Warehouse
        [ResponseType(typeof(Warehouse))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public Warehouse Put(Warehouse item)
        {
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
                item.CompanyId = compid;

                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);

                //Company com = db.Companies.Where(x => x.Deleted == false && x.Id == compid).FirstOrDefault();
               return context.PutWarehouse(item,compid);
            }
            catch
            {
                return null;
            }
        }

        //Delete Warehouse by Warehouse id
        [ResponseType(typeof(Warehouse))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start Warehouse: ");
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
                int CompanyId = compid;
                context.DeleteWarehouse(id, CompanyId);
                logger.Info("End  delete Warehouse: ");
            }
            catch (Exception ex)
            {

                logger.Error("Error in deleteWarehouse " + ex.Message);


            }
        }

    }
}



