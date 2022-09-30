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
    [RoutePrefix("api/MonthInvoiceBill")]
    public class InvoiceBillController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext context = new DailyNeedContext();
        InvoiceBillAPIMethod db = new InvoiceBillAPIMethod();

        //get List of Open status Invoice By companyId // Unpaid list 
        //[Authorize]
        [Route("GetAllPaidBillList")]
        public object getAllPaidBillList()
        {
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
                var amount = context.CustomerInvoices.Where(a => /*a.Company.Id == compid && */a.Status == "Open").Include("Customerlk").ToList();
                return amount;
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }
        }



        //Add Invoice Bill// POST Method
        [ResponseType(typeof(InvoiceBill))]
        [Route("AddInvoiceBill")]
        [AcceptVerbs("POST")]
        public InvoiceBill add(InvoiceBill bill)
        {
            logger.Info("start Bill: ");
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
                if (bill == null)
                {
                    throw new ArgumentNullException("Bill");
                }

                Company comp = context.Companies.Where(x => x.Id == compid && x.Deleted == false).FirstOrDefault();
                bill.Company = comp;
                People provider = context.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
                bill.Provider = provider;
                return db.addInvoiceBill(bill);
            }
            catch (Exception ex)
            {
                logger.Error("Error in Bill " + ex.Message);
                return null;
            }
        }

        //get Available Amount of Customer
        //[Authorize]
        [Route("GetWalletAmtByCId")]
        public object GetAmount(int CustlkId)
        {
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
                var amount = context.CustomerWallets.Where(x => x.CustomerLink.CustomerLinkId == CustlkId && x.Provider.PeopleID == userid).FirstOrDefault();
                return amount;
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting Peoples " + ex.Message);
                return null;
            }
        }

        
    }
}
