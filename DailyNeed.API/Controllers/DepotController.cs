using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using System.Security.Claims;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/Depot")]
    public class DepotController : ApiController
    {
        DepotAPIMethod context = new DepotAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Get All Depot
        //[Authorize]
        [Route("")]
        public IEnumerable<Depot> Get()
        {
            logger.Info("start Depot: ");
            List<Depot> depot = new List<Depot>();
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
                if (compid != 0)
                {
                    depot = context.AllDepot(compid).ToList();
                    logger.Info("End  Depot: ");
                    return depot;

                }
                else
                {
                    Depot _depot = new Depot();
                    _depot.Message = "Company does not Exists!";
                    _depot.Success = false;
                    depot.Add(_depot);
                    return depot;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in Depot " + ex.Message);
                logger.Info("End  Depot: ");
                return null;
            }
        }


        //Get All Depot By Warehouse ID
        [Route("GetByWarehouseId")]
        public IEnumerable<Depot> Get(int warId)
        {
            logger.Info("start Depot: ");
            List<Depot> _depotLst = new List<Depot>();
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
                if(compid!=0)
                {
                    _depotLst = db.Depots.Where(d => d.Deleted == false && d.Warehouse.WarehouseId == warId && d.Company.Id==compid).ToList();
                    logger.Info("End  Depot: ");
                    return _depotLst;
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
                logger.Error("Error in Depot " + ex.Message);
                logger.Info("End  Depot: ");
                return null;
            }
        }


        // Add Depot POST MEthod
        [ResponseType(typeof(Depot))]
        [Route("")]
        [AcceptVerbs("POST")]
        public Depot add(Depot depot)
        {
            logger.Info("start addDepot: ");
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
                if (depot == null)
                {
                    throw new ArgumentNullException("ctegory");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);

                if(compid!=0 && userid!=0)
                {
                    context.AddDepot(depot, compid, userid);
                    logger.Info("End  addDepot: ");
                    return depot;
                }
                else
                {
                    Depot _depot = new Depot();
                    _depot.Message = "Company or Provider does not Exists!";
                    _depot.Success = false;
                    return _depot;
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error in addDepot " + ex.Message);
                logger.Info("End  addDepot: ");
                return null;
            }
        }


        // Update Depot PUT MEthod
        [ResponseType(typeof(Depot))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public Depot Put(Depot item)
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
                if (compid != 0 && userid != 0) { 
                    return context.PutDepot(item, compid, userid);
                }
                else
                {
                    Depot _depot = new Depot();
                    _depot.Message = "Company or Provider does not Exists!";
                    _depot.Success = false;
                    return _depot;
                }
            }
            catch(Exception ex)
            {
                logger.Error("Error in updateDepot " + ex.Message);
                logger.Info("End  updateDepot: ");
                return null;
            }
        }

        // Delete Depot / Delete MEthod
        [ResponseType(typeof(Depot))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public Depot Remove(int id)
        {
            Depot depot = null;
            logger.Info("start del Depot: ");
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

                depot = db.Depots.Where(x => x.Depotid == id && x.Deleted == false && x.Company.Id == compid).FirstOrDefault();
                if (depot != null)
                {
                    depot.Deleted = true;
                    depot.IsActive = false;
                    depot.Message = "Successfully";
                    depot.Success = true;
                    db.Depots.Attach(depot);
                    db.Entry(depot).State = EntityState.Modified;
                    db.SaveChanges();
                    logger.Info("End  delete Depot: ");
                    return depot;
                }
                else
                {
                    Depot _depot = new Depot();
                    _depot.Message = "Depot Not Exists!";
                    _depot.Success = false;
                    return _depot;
                }
            }
            catch (Exception ex)
            {
                depot.Message = "Failed";
                depot.Success = false;
                logger.Error("Error in del Depot " + ex.Message);
                return depot;
            }
        }
    }
}



