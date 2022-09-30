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
using System.Runtime.Caching;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/signup")]
    public class CustomerRegistrationController : ApiController
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        DailyNeedContext context = new DailyNeedContext();
        //private DailyNeedContext db = new DailyNeedContext();

        [Route("")]
        public IList<CustomerDetails> Get()
        {
            try
            {
                logger.Info("get customer from registration");
                var customers = (from a in context.CustomerRegistrations
                                 join e in context.Warehouses on a.WarehouseId equals e.WarehouseId
                                 select new CustomerDetails
                                 {
                                     CustomerName = a.CustomerName,
                                     Email = a.Email,
                                     Mobile = a.Mobile,
                                     Country = a.Country,
                                     State = a.State,
                                     City = a.City,
                                     CreatedDate = a.CreatedDate,
                                     WarehouseName = e.WarehouseName
                                 }).ToList();
                return customers;
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting customer" + ex.Message);
                return null;
            }
        }

        [Route("")]
        public Customer Get(string mob)
        {
            try
            {
                Customer customers = context.getcustomers(mob);
                if (customers == null)
                {
                    return null;
                }
                else
                {
                    return customers;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting customer by mobile" + ex.Message);
                return null;
            }

        }

      
        [Route("")]
        [AcceptVerbs("PUT")]
        public Customer Put(Customer cust)
        {
            try
            {
                return context.CustomerUpdate(cust);
            }
            catch (Exception )
            {
                return null;
            }
        }

        [ResponseType(typeof(CustomerRegistration))]
        [Route("")]
        [AcceptVerbs("POST")]
        public Customer Post(Customer customer)
        {
            customeritems ibjtosend = new customeritems();

            Customer newcustomer = new Customer();
            Customer cust = context.Customers.Where(x => x.Deleted == false).Where(x => x.Mobile == customer.Mobile && x.Password == customer.Password).SingleOrDefault();
            if (cust == null)
            {
                return newcustomer;
            }
            else
            {
                if(customer.fcmId != null && customer.fcmId.Trim() != "" && customer.fcmId.Trim().ToUpper() != "NULL")
                {
                    cust.fcmId = customer.fcmId;
                    context.Customers.Attach(cust);
                    context.Entry(cust).State = EntityState.Modified;
                    context.SaveChanges();

                    try
                    {
                        var wallet = context.GetWalletbyCustomerid(cust.CustomerId);
                        var reward = context.GetRewardbyCustomerid(cust.CustomerId);
                    }
                    catch (Exception )
                    {
                        
                    }

                }
                return cust;
            }
        }
        [ResponseType(typeof(Customer))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public Customer Put(Customer cust, string rs)
        {

            try
            {
                //var context = new DailyNeedContext(new DailyNeedContext());
                return context.Resetpassword(cust);
            }
            catch (Exception )
            {

                return null;
            }
        }

        [ResponseType(typeof(CustomerRegistration))]
        [Route("")]
        [AcceptVerbs("POST")]
        public Customer Post(string type, Customer customer)
        {
            Customer newcustomer = new Customer();
            try
            {
                if (type == "ids")
                {
                    Customer cust = context.Customers.Where(x => x.Deleted == false).Where(x => x.Mobile == customer.Mobile).SingleOrDefault();
                    if (cust == null)
                    {
                        newcustomer = context.CustomerRegistration(customer);
                    }
                    else
                    {
                        if (customer.Mobile == cust.Mobile && customer.Password == cust.Password)
                        {
                            return cust;
                        }
                        else if (customer.Mobile == cust.Mobile && customer.Password != cust.Password)
                        {
                            newcustomer.Mobile = customer.Mobile;
                            newcustomer.Password = null;
                            return newcustomer;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return newcustomer;
        }       
    }

    public class customeritems
    {
        public People  ps { get; set; }
        public Customer cs { get; set; }
        public IEnumerable<Basecats> Basecats { get; set; }
        public List<Categories> Categories { get; set; }
        public IEnumerable<SubCategories> SubCategories { get; set; }
    }
    public class Basecats
    {
        public int BaseCategoryId { get; set; }
       
        public string BaseCategoryName { get; set; }

        public string LogoUrl { get; set; }

    }
    public class Categories
    {
        public int BaseCategoryId { get; set; }
        public int Categoryid { get; set; }
        public string CategoryName { get; set; }
        public string LogoUrl { get; set; }
    }
    public class SubCategories
    {
        public int SubCategoryId { get; set; }
        public int Categoryid { get; set; }
        public string SubcategoryName { get; set; }
    }

}