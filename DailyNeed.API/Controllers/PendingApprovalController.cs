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
    [RoutePrefix("api/PendingApproval")]
    public class PendingApprovalController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // List of Companies waiting for approval
        [Route("")]
        public IEnumerable<PeopleCompany> Get()
        {
            logger.Info("Get Peoples: ");
            int compid = 0, userid = 0;
            List<PeopleCompany> person = new List<PeopleCompany>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
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
                logger.Info("End Get Company: ");
                // get recoreds from people and company tables
                person = (from e in db.Peoples
                          where e.Active == false && e.Permissions == "HQ Master login"
                          join i in db.Companies on e.Company.Id equals i.Id
                          select new PeopleCompany
                          {
                              CompanyName = i.CompanyName,
                              CompanyPhone = i.CompanyPhone,
                              Address = i.Address,
                              Email = e.Email,
                              userId = e.PeopleID
                          }).OrderByDescending(x => x.Email).ToList();

                return person;

            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }
        }

        // Companies approval by super admin
        [Route("ApprovePendingRequest")]
        [AcceptVerbs("PUT")]
        public PeopleCompany Put(PeopleCompany approval)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid1 = 0;
                // Access claims
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "compid")
                    {
                        compid = int.Parse(claim.Value);
                    }
                    if (claim.Type == "userid")
                    {
                        userid1 = int.Parse(claim.Value);
                    }
                }
                
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid1);

                People People = db.Peoples.Where(x => x.PeopleID == approval.userId && x.Active == false).FirstOrDefault();
                
                if (approval != null)
                {
                    try
                    {
                        People.Active = true;
                        db.Peoples.Attach(People);
                        db.Entry(People).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception ) { }

                    return approval;
                }
                else
                {
                    return approval;
                }

            }

            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);

                return null;
            }
        }

    }

    public class PeopleCompany
    {
        public int userId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CompanyPhone { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
    }
}
