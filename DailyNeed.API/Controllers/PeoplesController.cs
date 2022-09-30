using DailyNeed.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/Peoples")]
    public class PeoplesController : ApiController
    {
        PeopleAPIMethod context = new PeopleAPIMethod();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        //get all people
        //[Authorize(Roles="Admin")]
        [Route("")]
        public IEnumerable<People> Get()
        {
            logger.Info("Get Peoples: ");
            int compid = 0, userid = 0;
            int Warehouse_id = 0;
            string email = "";
            try
            {
                var identity = User.Identity as ClaimsIdentity;

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
                    if (claim.Type == "email")
                    {
                        email = claim.Value;
                    }
                }
                logger.Info("End Get Company: ");
                List<People> person = context.AllPeoples(compid).ToList();
                return person;
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }


        }

        [Route("user")]
        public People Get(int PeopleId)
        {
            DailyNeedContext db = new DailyNeedContext();
            int compid = 0, userid = 0;
            try
            {
                var identity = User.Identity as ClaimsIdentity;

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
                People person = db.Peoples.Where(u => u.PeopleID == PeopleId && u.Company.Id == compid).SingleOrDefault();
                return person;

            }

            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }

        }


        //Add people 
        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("POST")]
        public People add(People people)
        {
            logger.Info("Add Peoples: ");
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
                if (people == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                var pp = context.AddPeoplebyAdmin(people, compid);
                if (pp == null)
                {
                    return null;
                }
                logger.Info("End  Add Peoples: ");
                return people;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);

                return null;
            }
        }

        //Update method 
        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public People Put(People item)
        {
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
                int Companyid = compid;
                // item.Company.Id = compid;
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                return context.PutPeoplebyAdmin(item, Companyid);
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);

                return null;
            }
        }

        //Delete People by people id
        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public bool Remove(int id)
        {
            logger.Info("DELETE Peoples: ");
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
                People proj = db.Peoples.Where(x => x.PeopleID == id && x.Deleted == false && x.Active == true && x.Company.Id == compid).FirstOrDefault();
                bool responser = context.DeletePeople(id);
                return responser;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);
                return false;
            }
        }

        [ResponseType(typeof(People))]
        [Route("retailer")]
        [AcceptVerbs("POST")]
        public People postfromaapp(string mob, string password)
        {
            DailyNeedContext context = new DailyNeedContext();
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
                //  People check = context.CheckPeople(mob, password,CompanyId);
                People check = context.CheckPeople(mob, password);

                return check;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);
                return null;

            }

        }


        [Route("")]
        public People Get(string mob, string password)
        {
            logger.Info("Get Peoples: ");

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
                DailyNeedContext context = new DailyNeedContext();
                int CompanyId = compid;
                //    People check = context.CheckPeople(mob, password,CompanyId);
                People check = context.CheckPeople(mob, password);
                return check;
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }

        }

        [Route("Email")]
        [HttpGet]
        public HttpResponseMessage CheckEmail(string Email)
        {
            DailyNeedContext db = new DailyNeedContext();
            try
            {
                logger.Info("Get Peoples: ");
                int compid = 0, userid = 0;
                int Warehouse_id = 0;
                string email = "";
                var identity = User.Identity as ClaimsIdentity;
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
                    if (claim.Type == "email")
                    {
                        email = claim.Value;
                    }
                }
                logger.Info("End Get Company: ");
                if (Warehouse_id > 0)
                {
                    var RDEmail = db.Peoples.Where(x => x.Email == Email && x.Warehouse.WarehouseId == Warehouse_id).FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, RDEmail);
                }
                else
                {
                    var RDEmail = db.Peoples.Where(x => x.Email == Email).FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, RDEmail);

                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, false);
            }
        }

        [Route("Mobile")]
        [HttpGet]
        public HttpResponseMessage CheckMobile(string Mobile)
        {
            DailyNeedContext db = new DailyNeedContext();
            try
            {
                logger.Info("Get Peoples: ");
                int compid = 0, userid = 0;
                int Warehouse_id = 0;
                string email = "";
                var identity = User.Identity as ClaimsIdentity;
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
                    if (claim.Type == "email")
                    {
                        email = claim.Value;
                    }
                }
                logger.Info("End Get Company: ");
                if (Warehouse_id > 0)
                {
                    var RDMobile = db.Peoples.Where(x => x.Mobile == Mobile && x.Warehouse.WarehouseId == Warehouse_id).FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, RDMobile);
                }
                else
                {
                    var RDMobile = db.Peoples.Where(x => x.Mobile == Mobile).FirstOrDefault();

                    return Request.CreateResponse(HttpStatusCode.OK, RDMobile);

                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, false);
            }
        }


    }
}
