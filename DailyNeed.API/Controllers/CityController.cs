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
    [RoutePrefix("api/City")]
    public class CityController : ApiController
    {
        CityAPIMethod context = new CityAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // [Authorize]
        [Route("")]
        public IEnumerable<City> Get()
        {
            logger.Info("start City: ");
            List<City> ass = new List<City>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
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

                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                if (Warehouse_id > 0)
                {
                    var wh = db.Warehouses.Where(x => x.WarehouseId == Warehouse_id).FirstOrDefault();
                    ass = db.Cities.Where(x => x.Cityid == wh.City.Cityid).ToList();
                }
                else
                {
                    ass = context.AllCitys().ToList();
                }

                logger.Info("End  City: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in City " + ex.Message);
                logger.Info("End  City: ");
                return null;
            }
        }

        //[Authorize]
        [Route("")]
        public IEnumerable<City> Get(string id)
        {
            logger.Info("start City: ");
            List<City> ass = new List<City>();
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
                int idd = Int32.Parse(id);
                ass = context.AllCity(idd).ToList();
                logger.Info("End  City: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in City " + ex.Message);
                logger.Info("End  City: ");
                return null;
            }
        }

        [ResponseType(typeof(City))]
        [Route("")]
        [AcceptVerbs("POST")]
        public City add(City item)
        {
            logger.Info("start add City: ");
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
                //item.CompanyId = compid;
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                context.AddCity(item);
                logger.Info("End add City: ");
                return item;
            }
            catch (Exception ex)
            {
                logger.Error("Error in add City " + ex.Message);
                logger.Info("End  addCity: ");
                return null;
            }
        }

        [ResponseType(typeof(City))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public City Put(City item)
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
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                return context.PutCity(item);
            }
            catch
            {
                return null;
            }
        }


        [ResponseType(typeof(City))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start deleteCityy: ");
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
                context.DeleteCity(id);
                logger.Info("End  delete City: ");
            }
            catch (Exception ex)
            {

                logger.Error("Error in deleteCity" + ex.Message);


            }
        }
    }
}



