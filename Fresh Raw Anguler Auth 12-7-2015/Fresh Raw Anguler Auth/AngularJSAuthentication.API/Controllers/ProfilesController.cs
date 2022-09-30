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
    [RoutePrefix("api/Profiles")]
    public class ProfilesController : ApiController
    {
        iAuthContext context = new AuthContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //[Authorize]
        [Route("")]
        //public IEnumerable<People> Get()
        public People Get()
        {
            logger.Info("Get Company: ");
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
                logger.Error("Error in getting Company " + ex.Message);
            }

            //return context.AllPeoples(compid);
            logger.Info("End Get Company: ");
            return context.GetPeoplebyId(userid);
            //return context.AllPeoples(compid);
        }


        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("POST")]
        public People add(People item)
        {
            var identity = User.Identity as ClaimsIdentity;
            int compid = 0;
            // Access claims
            foreach (Claim claim in identity.Claims)
            {
                if (claim.Type == "compid")
                {
                    compid = int.Parse(claim.Value);
                }
            }

            item.CompanyID = compid;
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            context.AddPeople(item);

            return item;
        }

        [ResponseType(typeof(People))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public People Put(People item)
        {
            try
            {
                return context.PutPeople(item);
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
            context.DeletePeople(id);
        }
    }
}



