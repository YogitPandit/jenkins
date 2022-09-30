using AngularJSAuthentication.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/Peoples")]
    public class PeoplesController : ApiController
    {
        iAuthContext context = new AuthContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

       // [Authorize]
        [Route("")]
        public IEnumerable<People> Get()
        //public People Get()
        {
            logger.Info("Get Peoples: ");
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
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
            }

            //return context.AllPeoples(compid);
            logger.Info("End Get Company: ");
            //return context.GetPeoplebyCompanyId(compid);
            List<People> person = context.AllPeoples(compid).ToList();
            return person;
        }

        [Route("")]
        public IEnumerable<People> Get(string id)
        {
            logger.Info("start single User: ");
            List<People> ass = new List<People>();
            try
            {

                logger.Info("in user");
                int idd = Int32.Parse(id);
                ass = context.singleuser(idd).ToList();
                logger.Info("End  single People: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in single User " + ex.Message);
                logger.Info("End  single User: ");
                return null;
            }
        }


        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("POST")]
        public People add(People item)
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

                item.CompanyID = compid;
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                context.AddPeoplebyAdmin(item);
                logger.Info("End  Add Peoples: ");
                return item;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);
              
                return null;
            }
        }

        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public People Put(People item)
        {
            try
            {
                return context.PutPeoplebyAdmin(item);
            }
            catch
            {
                return null;
            }
        }


        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
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
                context.DeletePeople(id);
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);

            }
        }
        }
}



