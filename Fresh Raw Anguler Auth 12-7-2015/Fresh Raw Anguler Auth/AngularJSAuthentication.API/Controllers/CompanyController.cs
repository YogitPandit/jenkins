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
    [RoutePrefix("api/Companys")]
    public class CompanyController : ApiController
    {
        iAuthContext context = new AuthContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

       // [Authorize]
        [Route("")]
        public Company Get()
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
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End Get Company: ");
               

                return context.GetCompanybyCompanyId(compid);
            }
              
             catch (Exception ex)
             {
                 logger.Error("Error in getting Company " + ex.Message);
                return null;
             }

}



        [ResponseType(typeof(Company))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public Company Put(Company item)
        {
            try
            {
                //var context = new AuthContext(new AuthContext());
                return context.PutCompany(item);
            }
            catch
            {
                return null;
            }
        }



    }


}



