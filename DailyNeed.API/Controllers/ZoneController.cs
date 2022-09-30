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
    [RoutePrefix("api/Zone")]
    public class ZoneController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();

        //Get All Zones
        //[Authorize]
        [Route("")]
        public IEnumerable<Zone> Get()
        {
            logger.Info("start Zone: ");
            List<Zone> zones = new List<Zone>();
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
                if(compid != 0)
                { 
                zones = context.AllZones(compid).ToList();
                logger.Info("End Zone: ");                
                return zones;
                }
                else
                {
                    Zone _zone = new Zone();
                    _zone.Message = "Company does not Exists!";
                    _zone.Success = false;
                    zones.Add(_zone);
                    return zones;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in Zone " + ex.Message);
                logger.Info("End Zone: ");
                return null;
            }
        }

        //Get Zone by depotid
        [Route("GetZoneById")]
        public IEnumerable<Zone> Get(int id)
        {
            logger.Info("start Zones: ");
            List<Zone> zn = new List<Zone>();
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
                int CompanyId = compid;
                zn = context.AllZoness(id,compid).ToList();
                logger.Info("End  Zones: ");

                return zn;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Zones " + ex.Message);
                logger.Info("End  Zones: ");
                return null;
            }
        }

        [Route("GetWarehouseById")]
        public IEnumerable<Depot> GetWarehouse(int warid)
        {
            logger.Info("start GetWarehouse: ");
            List<Depot> _depotLst = new List<Depot>();
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
                if(compid != 0) {                
                var depot = db.Depots.Where(c => c.Warehouse.WarehouseId == warid && c.Company.Id == compid && c.Deleted == false).ToList();
                logger.Info("End  GetWarehouse: ");
                
                    return depot;
                }
                else
                {
                    Depot _depot = new Depot();
                    _depot.Message = "Company does not Exists!";
                    _depot.Success = false;
                    _depotLst.Add(_depot);
                    return _depotLst;
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error in GetWarehouse " + ex.Message);
                logger.Info("End  GetWarehouse: ");
                return null;
            }
        }

   
        //Add Zones POST Method
        [ResponseType(typeof(Zone))]
        [Route("")]
        [AcceptVerbs("POST")]
        public Zone add(Zone zone)
        {
            logger.Info("start add Zone: ");
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
         
                if (zone == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                if (compid != 0 && userid != 0) { 
                    context.AddZone(zone, compid, userid);
                logger.Info("End  add Zone: ");
                return zone;
                }
                else
                {
                    Zone _zone = new Zone();
                    _zone.Message = "Company or Provider does not Exists!";
                    _zone.Success = false;
                    return _zone;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in add Zone " + ex.Message);
                logger.Info("End  add Zone: ");
                zone.Message = "Failed";
                zone.Success = false;
                return zone;
            }
        }

        //Update Zone PUT method
        [ResponseType(typeof(Zone))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public Zone Put(Zone zone)
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
                if (compid != 0 && userid != 0)
                {
                    var data = context.PutZone(zone, compid, userid);
                    return data;
                }
                else
                {
                    Zone _zone = new Zone();
                    _zone.Message = "Company or Provider does not Exists!";
                    _zone.Success = false;
                    return _zone;
                }
            }
            catch(Exception ex)
            {
                logger.Error("Error in updateZone " + ex.Message);
                logger.Info("End  updateZone: ");
                zone.Message = "Failed";
                zone.Success = false;
                return zone;
            }
        }

        //Delete record by zone
        [ResponseType(typeof(Zone))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public Zone Remove(int id)
        {
            Zone zone = null;
            logger.Info("start delete Zone: ");
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
                zone = db.Zones.Where(z => z.Zoneid == id && z.Deleted == false).FirstOrDefault();
                if (zone != null) {                                
                context.DeleteZone(id, compid);
                logger.Info("End  delete  Zone: ");
                zone.Message = "Zone deleted Successfully";
                zone.Success = true;
                return zone;
                }
                else
                {
                    Zone _zone = new Zone();
                    _zone.Message = "Zone is not deleted";
                    _zone.Success = false;
                    return _zone;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in delete Zone " + ex.Message);
                zone.Message = "Delete Failed";
                zone.Success = false;
                return zone;
            }
        }
    }
}



