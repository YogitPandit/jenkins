using DailyNeed.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/MonthLeaves")]
    public class MonthLeaveController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        MonthLeaveAPIMethod mlapi = new MonthLeaveAPIMethod();

        //get All Month Leaves 
        //[Authorize]
        [Route("GetAllMonthLeaves")]
        public object GetAllMonthLeaves()
        {
            List<MonthLeave> leave = new List<MonthLeave>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                //Access claims
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
                int CompanyId = compid;
                var monthleave = (from e in db.MonthLeaves
                                  where e.IsActive == true && e.Deleted == false && e.Company.Id == CompanyId
                                  select new
                                  {
                                      e.Id,
                                      e.CustomerLink.CustomerLinkId,
                                      e.CustomerLink.Name,
                                      e.Provider.PeopleFirstName,
                                      e.Provider.PeopleID,
                                      e.Description,
                                      e.FromDate,
                                      e.ToDate
                                  }).OrderByDescending(x => x.Id).ToList();
                return monthleave;
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }
        }


        //Add MonthLeaves POST Method
        [ResponseType(typeof(CustomerServices))]
        [Route("AddMonthLeave")]
        [AcceptVerbs("POST")]
        public MonthLeave add(MonthLeave monthleave)
        {
            logger.Info("start MonthLeave: ");
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
                if (monthleave == null)
                {
                    throw new ArgumentNullException("MonthLeaves");
                }
                int CompanyId = compid;
                mlapi.MonthLeave(monthleave, userid, CompanyId);
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End MonthLeaves: ");
                return monthleave;
            }
            catch (Exception ex)
            {
                logger.Error("Error in MonthLeaves " + ex.Message);
                return null;
            }
        }


        //Delete MonthLeaves Method
        [ResponseType(typeof(MonthLeave))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public bool Remove(int id)
        {
            logger.Info("DELETE MonthLeaves: ");
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
                bool responser = mlapi.DeleteMonthLeave(id);
                return responser;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add MonthLeaves " + ex.Message);
                return false;
            }
        }


        //Update MonthLeaves Method
        [ResponseType(typeof(MonthLeave))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public MonthLeave Put(MonthLeave monthleaves)
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
                if (monthleaves == null)
                {
                    throw new ArgumentNullException("monthleaves");
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                mlapi.UpdateMonthLeave(monthleaves, userid, CompanyId);
                monthleaves.Message = "Successfully";
                return monthleaves;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add MonthLeaves " + ex.Message);
                monthleaves.Message = "Error";
                return monthleaves;
            }

        }

    }
}
