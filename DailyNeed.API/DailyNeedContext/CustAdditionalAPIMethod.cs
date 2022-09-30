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
    public class CustAdditionalAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Add Customer Additional Service To CustomerAdditionalServices Main table
        public CustomerAdditionalService CustomerAddServices(CustomerAdditionalService customeradditionalService)
        {
            //Cheaking Dropdown Value with Db 
            CustomerAdditionalService objCustomeraddService = new CustomerAdditionalService();
            PWarehouseLink wh = db.PWarehouseLinks.Where(x => x.PWarehouseId == customeradditionalService.PWarehouseId && x.Deleted == false).FirstOrDefault();
            CustomerServices custSrevice = db.CustomerServicess.Where(x => x.ID == customeradditionalService.CustomerServiceId && x.Deleted == false).FirstOrDefault();
            CustomerLink customerlk =db.CustomerLinks.Where(x => x.CustomerLinkId == customeradditionalService.CustomerLinkId && x.Deleted == false).FirstOrDefault();
            try
            {
                //saving Data to customerServices table
                customeradditionalService.CustomerService = custSrevice;
                customeradditionalService.CustomerLink = customerlk;
                customeradditionalService.PWarehouse = wh;
                customeradditionalService.CreateDate = indianTime;
                customeradditionalService.UpdateDate = indianTime;
                customeradditionalService.CreatedBy = customeradditionalService.Provider.PeopleID.ToString();
                customeradditionalService.Deleted = false;
                customeradditionalService.IsActive = true;
                customeradditionalService.IsLess = customeradditionalService.IsLess;
                if (customeradditionalService.IsLess == true)
                {
                    customeradditionalService.Quantity = -customeradditionalService.Quantity;
                }
                db.CustomerAdditionalServicess.Add(customeradditionalService);
                int id = db.SaveChanges();
            }
            catch (Exception)
            {
                customeradditionalService.Message = "Error";
                return customeradditionalService;
            }
            customeradditionalService.Message = "Successfully";
            return customeradditionalService;
        }

        //Get Customer Additional Service For Update
        public CustomerAdditionalService PutCustomeraddServices(CustomerAdditionalService customeraddservices)
        {
            CustomerAdditionalService cust = db.CustomerAdditionalServicess.Where(x => x.custAddServID == customeraddservices.custAddServID).FirstOrDefault();
            try
            {
                CustomerAdditionalService objCustomeraddService = new CustomerAdditionalService();
                PWarehouseLink wh = db.PWarehouseLinks.Where(x => x.PWarehouseId == customeraddservices.PWarehouseId && x.Deleted == false).FirstOrDefault();
                CustomerServices custSrevice = db.CustomerServicess.Where(x => x.ID == customeraddservices.CustomerServiceId && x.Deleted == false).FirstOrDefault();
                CustomerLink customerlk = db.CustomerLinks.Where(x => x.CustomerLinkId == customeraddservices.CustomerLinkId && x.Deleted == false).FirstOrDefault();

                if (cust != null)
                {
                    cust.CustomerService = custSrevice;
                    cust.CustomerLink = customerlk;
                    cust.PWarehouse = wh;
                    cust.UpdateDate = indianTime;
                    cust.Quantity = customeraddservices.Quantity;
                    cust.IsLess = customeraddservices.IsLess;
                    if (cust.IsLess == true)
                    {
                        cust.Quantity = -cust.Quantity;
                    }
                    cust.Deleted = false;
                    cust.IsActive = true;
                    db.Entry(cust).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    cust.Message = "Error";
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
            cust.Message = "Successfully";
            return cust;
        }
    }
}