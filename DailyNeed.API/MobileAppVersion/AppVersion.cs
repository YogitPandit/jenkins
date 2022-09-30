using DailyNeed.Model;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using NLog;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/appVersion")]
    public class AppVersionController : ApiController
    {
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        [HttpGet]
        [Route("")]
        [AcceptVerbs("GET")]
        public HttpResponseMessage Get()
        {
            try
            {                
                var item = db.appVersionDb.AsEnumerable();
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage add(appVersion item)
        {
            logger.Info("start add Feedback: ");
            try
            {                
                item.createdDate = indianTime;
                db.appVersionDb.Add(item);
                db.SaveChanges();
                logger.Info("End add feedBack: ");
                return Request.CreateResponse(HttpStatusCode.OK, "requesting brand added suscessfully");
            }
            catch (Exception ex)
            {
                logger.Error("Error in add feedBack " + ex.Message);
                logger.Info("End  addCity: ");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error:"+ ex.Message);
            }
        }

        [HttpPost]
        [Route("fcm")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage post(Customer cust)
        {
            logger.Info("start add Feedback: ");
            try
            {
                Customer customer = db.Customers.Where(x=>x.CustomerId == cust.CustomerId).SingleOrDefault();
                if (customer != null) {
                    //customer.fcmId = cust.fcmId;
                    db.Customers.Attach(customer);
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    logger.Info("End add feedBack: ");
                    appVersion app = db.appVersionDb.OrderByDescending(e => e.id).FirstOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, app);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, "request not add  ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in add feedBack " + ex.Message);
                logger.Info("End  addCity: ");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error:" + ex.Message);
            }
        }
    }
    public class appVersion
    {
        [Key]
        public int id { get; set; }
        public double App_version { get; set; }
        public bool isCompulsory { get; set; }
        public DateTime createdDate { get; set; }
    }
}