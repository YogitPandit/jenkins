using DailyNeed.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/Profiles")]
    public class ProfilesController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Authorize]
        [Route("")]
        //public IEnumerable<People> Get()
        public People Get()
        {
            logger.Info("Get Company: ");
            int compid = 0, userid = 0;
            int Warehouse_id = 0;
            string email="";
            string Email = "";
            string Mobile = "";
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
                    if (claim.Type == "Email")
                    {
                        Email = claim.Value;
                    }
                    if (claim.Type == "mobile")
                    {
                        Mobile = claim.Value;
                    }
                }
           
                return context.GetPeoplebyId(compid, Mobile);
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Company " + ex.Message);
                return null;
            }
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

            item.CompanyId = compid;
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
                var identity = User.Identity as ClaimsIdentity;
                logger.Info("Get Company: ");
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

                item.CompanyId = compid;
                return context.PutPeople(item);
            }
            catch
            {
                return null;
            }
        }

    }
}



