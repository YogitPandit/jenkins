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

using System.IO;
using System.Net.Http.Headers;
using Rest;
using RestSharp;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/CustomerLink")]
    public class CustomerLinkController : ApiController
    {
        //CustomerLinkAPIMethod context = new CustomerLinkAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        public static Logger logger = LogManager.GetCurrentClassLogger();


        //Add Customer To link table 
        [ResponseType(typeof(CustomerLink))]
        [Route("")]
        [AcceptVerbs("POST")]
        public CustomerLink addCustomerLink(CustomerLink CustomerLink)
        {
            logger.Info("start addCustomer: ");
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
                if (CustomerLink == null)
                {
                    throw new ArgumentNullException("item");
                }
                db.CustomerLinks.Attach(CustomerLink);
                db.Entry(CustomerLink).State = EntityState.Added;
                db.SaveChanges();
                return CustomerLink;
            }
            catch (Exception ex)
            {
                logger.Error("Error in addCustomer " + ex.Message);
                return null;
            }
        }
    }
}
