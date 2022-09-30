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

using System.IO;
using System.Net.Http.Headers;
using Rest;
using RestSharp;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/CustomerAdditional")]
    public class CustomerAdditionalController : ApiController
    {
        DailyNeedContext db = new DailyNeedContext();
        CustAdditionalAPIMethod CustAddAPI = new CustAdditionalAPIMethod();

        public static Logger logger = LogManager.GetCurrentClassLogger();


        //Add CustomerAddServices POST Method
        [ResponseType(typeof(CustomerServices))]
        [Route("CustomerAdditionalService")]
        [AcceptVerbs("POST")]
        public CustomerAdditionalService add(CustomerAdditionalService CustomersAdditionalServices)
        {
            logger.Info("start CustomerAdditionalServices: ");
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
                if (CustomersAdditionalServices == null)
                {
                    throw new ArgumentNullException("CustomerAdditionalService");
                }
                Company comp = db.Companies.Where(x => x.Id == compid && x.Deleted == false).FirstOrDefault();
                CustomersAdditionalServices.Company = comp;
                People provider = db.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
                CustomersAdditionalServices.Provider = provider;
                CustAddAPI.CustomerAddServices(CustomersAdditionalServices);
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  CustomerAdditionalService: ");
                return CustomersAdditionalServices;
            }
            catch (Exception ex)
            {
                logger.Error("Error in CustomerAdditionalService " + ex.Message);
                return null;
            }
        }

        //Get All Provider By CompanyId
        [Route("GetProvider")]
        public IEnumerable<People> GetProvider()
        {
            logger.Info("start City: ");
            People Provider = new People();
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
                int CompanyId = compid;
                var provider = db.Peoples.Where(c => c.Deleted == false && c.Active == true && c.CompanyId == CompanyId).ToList();
                logger.Info("End  Provider: ");
                return provider;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Provider " + ex.Message);
                logger.Info("End  Provider: ");
                return null;
            }
        }

        //Get All Customer Service By Customer ID
        [Route("GetCustServicesById")]
        public IEnumerable<CustomerServices> GetcustService(int custid)
        {
            logger.Info("start CustServices: ");
            CustomerServices cService = new CustomerServices();
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
                int CompanyId = compid;
                //geting variable data
                var CustServ = db.CustomerServicess.Where(c => c.Deleted == false && c.IsActive==true && c.CustomerLink.CustomerLinkId==custid).ToList();
                logger.Info("End  CustServices: ");
                return CustServ;
            }
            catch (Exception ex)
            {
                logger.Error("Error in CustServices " + ex.Message);
                logger.Info("End  CustServices: ");
                return null;
            }
        }

        //Get All WareHouse By CompantId
        [Route("GetWareHouse")]
        public IEnumerable<Warehouse> GetWarehouse()
        {
            logger.Info("start City: ");
            Warehouse Warehouse = new Warehouse();
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
                int CompanyId = compid;
                var warehouse = db.Warehouses.Where(c => c.Deleted == false).ToList();
                logger.Info("End  warehouse: ");
                return warehouse;
            }
            catch (Exception ex)
            {
                logger.Error("Error in warehouse " + ex.Message);
                logger.Info("End  warehouse: ");
                return null;
            }
        }

        //Get All Customer by Company ID
        [Route("GetBycomnyId")]
        public IEnumerable<Customer> GetById()
        {
            logger.Info("start City: ");
            Customer customer = new Customer();
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
                int CompanyId = compid;
                //geting variable data
                var customers = db.Customers.Where(c => c.Deleted == false && c.Company.Id == CompanyId).ToList();
                logger.Info("End  Customer: ");
                return customers;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }

        //get All Customer 
        [Authorize]
        [Route("GetCustomer")]
        public object Get()
        {
            logger.Info("start Get CustomerServices: ");
            List<CustomerAdditionalService> customeradditionalservice = new List<CustomerAdditionalService>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
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
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Customer: ");

                var customeraddservices = (from e in db.CustomerAdditionalServicess
                                        where e.IsActive == true && e.Deleted == false
                                        select new
                                        {
                                            e.custAddServID,
                                            e.CustomerService.ID,
                                            e.CustomerService.ServiceName,
                                            e.CustomerLink.CustomerLinkId,
                                            e.Provider.PeopleFirstName,
                                            e.Provider.PeopleID,
                                            e.PWarehouse.Warehouse.WarehouseName,
                                            e.PWarehouse.PWarehouseId,
                                            e.Quantity,
                                            e.Date,
                                            e.IsLess,
                                        }).OrderByDescending(x => x.ID).ToList();
                return customeraddservices;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }

        }

        //Delete Customer Additional Service ? Soft Delete
        [ResponseType(typeof(CustomerAdditionalService))]
        [Route("Delete")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start delete Customer: ");
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
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                //context.DeleteCustomer(id);
                var data = db.CustomerAdditionalServicess.Where(c => c.custAddServID == id).FirstOrDefault();
                if (data != null)
                {
                    data.Deleted = true;
                    db.SaveChanges();
                }
                logger.Info("End  delete Customer: ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in delete Customer " + ex.Message);
            }
        }

        //Update Customer Additional Services PUT Method
        //[Authorize]
        [ResponseType(typeof(CustomerAdditionalService))]
        [Route("UpdateCustomerAddServices")]
        [AcceptVerbs("PUT")]
        public CustomerAdditionalService Put(CustomerAdditionalService customeraddservices)
        {
            logger.Info("start putCustomer add services: ");
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
                if (customeraddservices == null)
                {
                    throw new ArgumentNullException("Customer Add Services");
                }
                Company comp = db.Companies.Where(x => x.Id == compid && x.Deleted == false).FirstOrDefault();
                customeraddservices.Company = comp;
                People provider = db.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
                customeraddservices.Provider = provider;
                customeraddservices.UpdatedBy = userid.ToString();
                return CustAddAPI.PutCustomeraddServices(customeraddservices);
            }
            catch (Exception ex)
            {
                logger.Error("Error in put Customer " + ex.Message);
                return null;
            }
        }
    }
}
