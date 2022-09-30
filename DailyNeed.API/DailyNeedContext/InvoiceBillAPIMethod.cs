using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NLog;
using DailyNeed.API.Controllers;
using System.Runtime.Caching;
using DailyNeed.Model;
using GenricEcommers.Models;
using DailyNeed.Model.NotMapped;
using System.Net;
using System.Text;
using System.IO;

namespace DailyNeed.API.Controllers
{
    public class InvoiceBillAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Add Invoice Bill Method
        public InvoiceBill addInvoiceBill(InvoiceBill objbill)
        {
            try
            {
                //Cheaking All Data Which is related to Parent Table
                CustomerLink customerlk = db.CustomerLinks.Where(x => x.CustomerLinkId == objbill.CustomerLinkId && x.Deleted == false).FirstOrDefault();
                CustomerInvoice customerinvoice = db.CustomerInvoices.Where(x => x.Id == objbill.CustomerInvoiceId && x.Deleted == false).FirstOrDefault();
                if (objbill != null)
                {
                    objbill.CreatedBy = objbill.Provider.PeopleID.ToString();
                    objbill.CreatedDate = indianTime;
                    objbill.UpdatedDate = indianTime;
                    objbill.PaidAmount = objbill.PaidAmount;
                    objbill.PaymentType = objbill.PaymentType;
                    objbill.BillCreatedDate = indianTime;
                    objbill.RemainingAmount = objbill.PaidAmount - customerinvoice.TotalPrice;
                    if(objbill.RemainingAmount > 0)
                    {
                        objbill.AmountType = "Cr.";
                    }
                    else
                    {
                        objbill.AmountType = "Dr.";
                        objbill.RemainingAmount = -(objbill.RemainingAmount);
                    }
                    objbill.CustomerLink = customerlk;
                    objbill.CustomerInvoice = customerinvoice;
                    objbill.Active = true;
                    objbill.Deleted = false;
                    db.InvoiceBills.Add(objbill);
                    int id = db.SaveChanges();

                    //update wallet amount when customer bill paid 
                    var walletReflact = db.CustomerWallets.Where(x => x.CustomerLink.CustomerLinkId == objbill.CustomerLink.CustomerLinkId).FirstOrDefault();
                    walletReflact.AvailableBalance = objbill.PaidAmount - walletReflact.AvailableBalance;
                    if (walletReflact.AvailableBalance < 0)
                    {
                        walletReflact.AvailableBalance = -(walletReflact.AvailableBalance);
                    }
                    db.Entry(walletReflact).State = EntityState.Modified;
                    db.SaveChanges();

                    //update Invoice status when customer bill paid
                    customerinvoice.Status = "Close";
                    db.Entry(customerinvoice).State = EntityState.Modified;
                    db.SaveChanges();
                    if (id != 0)
                    {
                        objbill.Message = "Successfully";
                    }
                    else
                    {
                        objbill.Message = "Error";
                    }
                }
                else
                {
                    return objbill;
                }
                return objbill;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }
        }
        
    }
}