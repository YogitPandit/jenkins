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
    [RoutePrefix("api/States")]
    public class StateController : ApiController
    {
        StateAPIMethod context = new StateAPIMethod();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Get All State method
        //[Authorize]
        [Route("")]
        public IEnumerable<State> Get()
        {

            logger.Info("start State: ");
            List<State> ass = new List<State>();
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

                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                ass = context.Allstates().ToList() ;
                logger.Info("End  Stste: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in state " + ex.Message);
                logger.Info("End  state: ");
                return null;
            }
        }

        //Add State method
        [ResponseType(typeof(State))]
        [Route("")]
        [AcceptVerbs("POST")]
        public State add(State item)
        {
            logger.Info("start addState: ");
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
                context.AddState(item);
                logger.Info("End  AddState: ");
                return item;
            }
            catch (Exception ex)
            {
                logger.Error("Error in AddState " + ex.Message);
                logger.Info("End  AddState: ");
                return null;
            }
        }

        //Update State method
        [ResponseType(typeof(State))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public State Put(State item)
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
                return context.PutState(item);
            }
            catch
            {
                return null;
            }
        }

        //Delete State 
        [ResponseType(typeof(State))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start deleteState: ");
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
                context.DeleteState(id);
                logger.Info("End  delete State: ");
            }
            catch (Exception ex)
            {

                logger.Error("Error in deleteSTate " + ex.Message);
                
             
            }
        }    
    }
}



