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
    [RoutePrefix("api/CreateCompany")]
    public class CreateCompanyController : ApiController
    {
        PeopleAPIMethod context = new PeopleAPIMethod();
        DailyNeedContext db = new DailyNeedContext();

        private static Logger logger = LogManager.GetCurrentClassLogger();
        [Route("")]
        public IEnumerable<PeopleDTO> Get()
        {
            logger.Info("Get Peoples: ");
            int compid = 0, userid = 0;
            int Warehouse_id = 0;
            List<PeopleDTO> person = new List<PeopleDTO>();
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

                //person = db.Peoples.Where(e => e.CompanyId == compid && e.Deleted == false).ToList();

                person = (from e in db.Peoples
                          where e.Deleted == false && e.Permissions == "HQ Master login"
                          join i in db.Companies on e.CompanyId equals i.Id
                          select new PeopleDTO
                          {

                              CompanyId = i.Id,
                              CompanyName = i.CompanyName,
                              CompanyPhone = i.CompanyPhone,
                              EmployeesCount = i.EmployeesCount,
                              Address = i.Address,
                              CompanyZip = i.CompanyZip,
                              Email = e.Email,
                              DisplayName = e.DisplayName,
                              Mobile = e.Mobile,
                              Password = e.Password,
                              Permissions = e.Permissions,
                              Active = e.Active,
                              userId = e.PeopleID,
                              //CreatedDate = e.CreatedDate
                          }).OrderByDescending(x => x.Email).ToList();

                return person;

            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }


        }

        [Route("")]
        [AcceptVerbs("PUT")]
        public PeopleDTO Put(PeopleDTO item)
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

                People People = db.Peoples.Where(x => x.PeopleID == item.userId && x.Deleted == false).FirstOrDefault();

                Company comp = db.Companies.Where(x => x.Id == item.CompanyId && x.Deleted == false).Select(x => x).SingleOrDefault();
                if (item != null)
                {

                    People.Email = item.Email;
                    People.Active = item.Active;
                    db.Peoples.Attach(People);
                    db.Entry(People).State = EntityState.Modified;
                    db.SaveChanges();
                    try
                    {
                        comp.CompanyName = item.CompanyName;
                        comp.Address = item.Address;
                        comp.CompanyZip = item.CompanyZip;
                        comp.CompanyPhone = item.CompanyPhone;
                        comp.EmployeesCount = item.EmployeesCount;
                        db.Companies.Attach(comp);
                        db.Entry(comp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception) { }

                    return item;
                }
                else
                {
                    return item;
                }

            }

            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);

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
                int CompanyId = compid;
                context.DeletePeople(id);
            }
            catch (Exception ex)
            {
                logger.Error("Error in Add Peoples " + ex.Message);

            }
        }
    }
    public class PeopleDTO
    {
        public int userId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Permissions { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int EmployeesCount { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyPhone { get; set; }
        public string Address { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}



