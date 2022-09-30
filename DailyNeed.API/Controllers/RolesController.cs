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
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //[Authorize]
        [Route("")]
        public IEnumerable<Role> Get()
        {
            logger.Info("start Role: ");
            List<Role> ass = new List<Role>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 1, userid=0;
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
                ass = context.AllRoles(compid).ToList() ;
                logger.Info("End  roles: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Role " + ex.Message);
                logger.Info("End  Role: ");
                return null;
            }
        }


        [ResponseType(typeof(Role))]
        [Route("")]
        [AcceptVerbs("POST")]
        public Role add(Role item)
        {
            logger.Info("start addRole: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid=0;
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
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                context.AddRole(item);
                logger.Info("End  AddRole: ");
                return item;
            }
            catch (Exception ex)
            {
                logger.Error("Error in AddRole " + ex.Message);
                logger.Info("End  AddRole: ");
                return null;
            }
        }

        [ResponseType(typeof(Role))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public Role Put(Role item)
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
                return context.PutRoles(item);
            }
            catch
            {
                return null;
            }
        }


        [ResponseType(typeof(Role))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(string id)
        {
            logger.Info("start deleteRole: ");
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
                context.DeleteRole(Int32.Parse(id));
                logger.Info("End  delete Role: ");
            }
            catch (Exception ex)
            {

                logger.Error("Error in deleteRole " + ex.Message);
                
             
            }
        }    
    }
}



