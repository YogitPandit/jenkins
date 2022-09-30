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
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/CustomerPriceService")]
    public class CustomerServiceController : ApiController
    {
        CustomerAPIMethod context = new CustomerAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        CustomerLinkController CutomerLink = new CustomerLinkController();
        CustomerLink CustomerTable = new CustomerLink();
        public static Logger logger = LogManager.GetCurrentClassLogger();


        //get All Customer Services
        //[Authorize]
        [Route("GetCustomer")]
        public object Get()
        {
            logger.Info("start Get CustomerServices: ");
            List<CustomerServices> customerservice = new List<CustomerServices>();
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
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Customer: ");

                var customerservices = (from e in db.CustomerServicess
                                        where e.IsActive == true && e.Deleted == false && e.Company.Id == compid
                                        select new
                                        {
                                            e.ID,
                                            e.CustomerLink.CustomerLinkId,
                                            e.CustomerLink.Name,
                                            e.Provider.PeopleFirstName,
                                            e.Provider.PeopleID,
                                            e.PWarehouse.PWarehouseId,
                                            e.PWarehouse.Warehouse.WarehouseName,
                                            e.PItem.PItemId,
                                            e.PItem.ItemMasters.ItemName,
                                            e.Price,
                                            e.Quantity,
                                            e.StartDate,
                                            e.Shift.ShiftID,
                                            e.DeliveryTime
                                        }).OrderByDescending(x => x.ID).ToList();
                return customerservices;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }

        }

        //Get Service-BaseCategory
        [Route("GetSerices")]
        public IEnumerable<BaseCategory> Getbasecategory()
        {
            logger.Info("start bCategory: ");
            BaseCategory bcategory = new BaseCategory();
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
                var service = db.BaseCategoryDb.Where(c => c.Deleted == false).ToList();
                logger.Info("End  Customer: ");
                return service;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }

        //Get All Category GetSubCategory
        [Route("GetCategory")]
        public IEnumerable<Category> GetCategory()
        {
            logger.Info("start bCategory: ");
            Category category = new Category();
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
                var scategory = db.Categorys.Where(c => c.Deleted == false).ToList();
                logger.Info("End  Customer: ");
                return scategory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }

        //Get All Shift On Besis of BaseCategory 
        [Route("GetShift")]
        public IEnumerable<ShiftMaster> GetShift()
        {
            logger.Info("start Shift: ");
            ShiftMaster shiftList = new ShiftMaster();
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
                var shiftlst = db.ShiftMasters.Where(c => c.Deleted == false && c.IsActive == true).ToList();
                logger.Info("End  Shit List: ");
                return shiftlst;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Sub Category " + ex.Message);
                logger.Info("End  Sub Category: ");
                return null;
            }
        }

        //Get All Sub Sub Category 
        [Route("GetSubCategory")]
        public IEnumerable<SubCategory> GetSubCategory()
        {
            logger.Info("start bCategory: ");
            SubCategory subcategory = new SubCategory();
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
                var subcategorys = db.SubCategorys.Where(c => c.Deleted == false).ToList();
                logger.Info("End  Sub Category: ");
                return subcategorys;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Sub Category " + ex.Message);
                logger.Info("End  Sub Category: ");
                return null;
            }
        }

        //Get All Sub Category 
        [Route("GetSubSubCategory")]
        public IEnumerable<SubsubCategory> GetSubSubCategory()
        {
            logger.Info("start bCategory: ");
            SubsubCategory subsubcategory = new SubsubCategory();
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
                var subsubcategorys = db.SubsubCategorys.Where(c => c.Deleted == false).ToList();
                logger.Info("End  Sub Category: ");
                return subsubcategorys;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Sub Category " + ex.Message);
                logger.Info("End  Sub Category: ");
                return null;
            }
        }

        //Get Customer By Registered Mobile Number
        [Route("")]
        public Customer Get(string Mobile)
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
                customer = context.GetCustomerbyId(Mobile);
                logger.Info("End  Customer: ");
                return customer;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }

        //Get All Items from PItem Link By CompanyId
        [Route("GetItemById")]
        public IEnumerable<PItemLink> GetItem()
        {
            logger.Info("start City: ");
            PItemLink item = new PItemLink();
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
                var items = db.PItemLinks.Where(c => c.Deleted == false && c.Company.Id == CompanyId).Include("ItemMasters").ToList();
                logger.Info("End  item: ");
                return items;
            }
            catch (Exception ex)
            {
                logger.Error("Error in item " + ex.Message);
                logger.Info("End  item: ");
                return null;
            }
        }

        //Get All Customer from customerlink table by Company ID
        [Route("GetCustomerById")]
        public IEnumerable<CustomerLink> GetById()
        {
            logger.Info("start Customer Link: ");
            CustomerLink customer = new CustomerLink();
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
                var customers = db.CustomerLinks.Where(c => c.Deleted == false && c.Company.Id == CompanyId && c.ParentCustomer == null).ToList();
                logger.Info("End  Customer Link: ");
                return customers;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer Link " + ex.Message);
                logger.Info("End  Customer Link: ");
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
                var provider = db.Peoples.Where(c => c.Deleted == false && c.Active == true && c.Company.Id == CompanyId).ToList();
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

        //Get All WareHouse from Pwarehouse Link By CompanyId
        [Route("GetWareHouse")]
        public List<PWarehouseLink> GetWarehouse()
        {
            logger.Info("start City: ");
            PWarehouseLink Warehouse = new PWarehouseLink();
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
                var warehouse = db.PWarehouseLinks.Where(c => c.Deleted == false && c.Company.Id == CompanyId).Include("Warehouse").ToList();
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

        //Get All UnitMaster By CompanyId
        [Route("GetUnitMaster")]
        public IEnumerable<UnitMaster> GetUnitMaster()
        {
            logger.Info("start UnitMaster: ");
            UnitMaster Warehouse = new UnitMaster();
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
                var unitmaster = db.UnitMaters.Where(c => c.Deleted == false && c.CompanyId == CompanyId).ToList();
                logger.Info("End  UnitMaster: ");
                return unitmaster;
            }
            catch (Exception ex)
            {
                logger.Error("Error in UnitMaster" + ex.Message);
                logger.Info("End  UnitMaster: ");
                return null;
            }
        }


        //Add CustomerServices POST Method
        [ResponseType(typeof(CustomerServices))]
        [Route("AddCustomerServices")]
        [AcceptVerbs("POST")]
        public CustomerServices add(CustomerServices CustomersServices)
        {
            logger.Info("start addCustomerServices: ");
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
                if (CustomersServices == null)
                {
                    throw new ArgumentNullException("Customer Service");
                }
                int CompanyId = compid;
                //Server side validations 
                if (compid == 0 || userid == 0)
                {
                    CustomersServices.Message = "Please Check Access Token!";
                    return CustomersServices;
                }
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (CustomersServices.CustomerLinkId == 0 || CustomersServices.CustomerLinkId == null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    CustomersServices.Message = "CustomerId Is Required!";
                    return CustomersServices;
                }
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (CustomersServices.PWarehouseId == 0 || CustomersServices.PWarehouseId == null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    CustomersServices.Message = "PWarehouseId Is Required!";
                    return CustomersServices;
                }
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (CustomersServices.PItemId == 0 || CustomersServices.PItemId == null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    CustomersServices.Message = "PItemId Is Required!";
                    return CustomersServices;
                }
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (CustomersServices.ShiftId == 0 || CustomersServices.ShiftId == null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    CustomersServices.Message = "ShiftId Is Required!";
                    return CustomersServices;
                }
                if (CustomersServices.StartDate == null )
                {
                    CustomersServices.Message = "StartDate Is Required!";
                    return CustomersServices;
                }
                if (CustomersServices.Price == 0)
                {
                    CustomersServices.Message = "Price Is Greater than 0!";
                    return CustomersServices;
                }
                if (CustomersServices.Quantity == 0)
                {
                    CustomersServices.Message = "Quantity Is Greater then 0!";
                    return CustomersServices;
                }
                if (CustomersServices.DeliveryDays == "" || CustomersServices.DeliveryDays == null)
                {
                    CustomersServices.Message = "DeliveryDays Is Required!";
                    return CustomersServices;
                }
                if (CustomersServices.DeliveryTime == null)
                {
                    CustomersServices.Message = "DeliveryTime Is Required!";
                    return CustomersServices;
                }

                CustomersServices.CreatedBy = userid.ToString();
                CustomersServices.Status = "Active";
                db.AddCustomerServices(CustomersServices, CompanyId);
                CustomersServices.Company = null;
                CustomersServices.PBaseCategory = null;
                CustomersServices.PCategory = null;
                CustomersServices.PSubcategory = null;
                CustomersServices.CustomerLink = null;
                CustomersServices.PItem = null;
                CustomersServices.PWarehouse = null;
                CustomersServices.Provider = null;
                CustomersServices.Shift = null;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End Customer Services: ");
                return CustomersServices;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer services " + ex.Message);
                return null;
            }
        }


        //Update CustomerServices PUT Method
        [Authorize]
        [ResponseType(typeof(CustomerServices))]
        [Route("UpdateCustomerServices")]
        [AcceptVerbs("PUT")]
        public CustomerServices Put(CustomerServices customerservices)
        {
            logger.Info("start putCustomerservices: ");
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
                if (customerservices == null)
                {
                    throw new ArgumentNullException("Customer Service");
                }
                int CompanyId = compid;
                customerservices.UpdatedBy = userid.ToString();
                return db.PutCustomerServices(customerservices, CompanyId);
            }
            catch (Exception ex)
            {
                logger.Error("Error in put Customer Service " + ex.Message);
                return null;
            }
        }


        //Delete Data By Soft from Customer Additional Services
        [ResponseType(typeof(Customer))]
        [Route("")]
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
                var data = db.CustomerServicess.Where(c => c.ID == id).FirstOrDefault();
                if (data != null)
                {
                    data.Deleted = true;
                    db.SaveChanges();
                }
                logger.Info("End  delete Customer Service: ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in delete Customer Service " + ex.Message);
            }
        }


        //Customer services for android app
        //get All Customer Services 
        //[Authorize]
        [Route("GetCustomerServices")]
        public object GetServices()
        {
            logger.Info("start Get CustomerServices: ");
            List<CustomerServices> customerservice = new List<CustomerServices>();
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
                var customerservices = (from e in db.CustomerServicess
                                        join custlk in db.CustomerLinks on e.CustomerLink.CustomerLinkId equals custlk.CustomerLinkId
                                        join item in db.PItemLinks on e.PItem.PItemId equals item.PItemId
                                        join wh in db.PWarehouseLinks on e.PWarehouse.PWarehouseId equals wh.PWarehouseId
                                        //join leave in db.MonthLeaves on custlk.CustomerLinkId equals leave.CustomerLink.CustomerLinkId
                                        where e.Company.Id == CompanyId
                                        select new CustomerServicesDTO
                                        {
                                            ServiceId = e.ID,
                                            Price = e.Price,
                                            Quantity = e.Quantity,
                                            CustomerId = custlk.CustomerLinkId,
                                            DeliveryTime = e.DeliveryTime.Hour + ":" + e.DeliveryTime.Minute,
                                            ProductName = item.ItemMasters.ItemName,
                                            CustomeName = custlk.Name,
                                            WarehouseId = wh.PWarehouseId,
                                            WarehouseName = wh.Warehouse.WarehouseName                                           
                                            //LeaveFDate = leave.FromDate,
                                            // leaveTDate = leave.ToDate
                                        }).OrderByDescending(x => x.ServiceId).ToList();
                DateTime DayOfChallan = DateTime.Now.AddDays(1);
                DateTime startDate = DayOfChallan.Date.AddSeconds(-1);
                DateTime lastDayOfMonth = startDate.AddMonths(1).AddDays(-1);
                DateTime endDate = DayOfChallan.Date.AddDays(1);
                foreach (CustomerServicesDTO cs in customerservices)
                {
                    cs.LeaveRecords = new List<string>();       
                   List<MonthLeave> monthleavedata= db.MonthLeaves.Where(l => l.FromDate > startDate && l.ToDate < lastDayOfMonth && l.CustomerLink.CustomerLinkId == cs.CustomerId).ToList();
                    foreach(MonthLeave mon in monthleavedata)
                    {
                        var diffrence = mon.ToDate - mon.FromDate;
                        for(int days = 0; days < diffrence.Days; days++)
                        {
                            cs.LeaveRecords.Add(Convert.ToDateTime(mon.ToDate.AddDays(days)).ToString("dd"));
                        }
                    }
                   
                }

            return customerservices;
        }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }

}

        //Customer With services for android app
        //get All Customer With Services 
        //[Authorize]
        [Route("GetCustomerWithServices")]
        public object GetCustomerWithServices()
        {
            logger.Info("start Get CustomerServices: ");
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
                var customerservices = (from e in db.CustomerServicess
                                        join custlk in db.CustomerLinks on e.CustomerLink.CustomerLinkId equals custlk.CustomerLinkId
                                        join item in db.PItemLinks on e.Provider.PeopleID equals item.Provider.PeopleID
                                        join wh in db.PWarehouseLinks on e.PWarehouse.PWarehouseId equals wh.PWarehouseId
                                        where e.Company.Id == CompanyId
                                        select new
                                        {
                                            ServiceId = e.ID,
                                            Price = e.Price,
                                            Quantity = e.Quantity,
                                            CustomerId = custlk.CustomerLinkId,
                                            DeliveryTime = e.DeliveryTime.Hour + ":" + e.DeliveryTime.Minute,
                                            ProductName = item.ItemMasters.ItemName,
                                            CustomeName = custlk.Name,
                                            WarehouseId = wh.PWarehouseId,
                                            WarehouseName = wh.Warehouse.WarehouseName,
                                            Mobile = custlk.Mobile,
                                            Mobile1 = custlk.Mobile1,
                                            Mobile2 = custlk.Mobile2,
                                            Mobile3 = custlk.Mobile3,
                                            GpsLocation = custlk.GPSLocation,
                                            Landmark = custlk.LandMard,
                                            PhotoUrl = custlk.PhotoUrl
                                        }).OrderByDescending(x => x.ServiceId).ToList();
                return customerservices;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }

        }


        //** For Above DTO Class
        //Central Base Category DTO Class
        public class CentralCustomerLink
        {
            public int CustomerLinkId { get; set; }
            public string CustomerName { get; set; }
            public int Address { get; set; }
            public string Email { get; set; }
            public string Comment { get; set; }
            public ICollection<CentralCustomerServices> CentralCustomerServices { get; set; }
        }
        public class CentralCustomerServices
        {
            public int CustomerServiceId { get; set; }
            public string DeliveryDays { get; set; }
            public int DeliveryTime { get; set; }
            public string Quantity { get; set; }
            public string Price { get; set; }
            public ICollection<CentralBaseCatetegory> CentralBaseCatetegory { get; set; }
        }
        public class CentralShift
        {
            public int ShiftId { get; set; }
            public string ShiftName { get; set; }
            //public ICollection<CentralBaseCatetegory> CentralBaseCatetegory { get; set; }
        }
        public class CentralBaseCatetegory
        {
            public int BaseCategoryId { get; set; }
            public string BaseCategoryName { get; set; }
            public ICollection<CentralCategory> CentralCategory { get; set; }
        }

        public class CentralCategory
        {
            public int Categoryid { get; set; }
            public string CategoryName { get; set; }
            public ICollection<CentralSubCategory> CentralSubCategory { get; set; }
        }

        public class CentralSubCategory
        {
            public int SubCategoryId { get; set; }
            public string SubcategoryName { get; set; }
            public ICollection<CentralItm> CentralItem { get; set; }
        }

        public class CentralItm
        {
            public int ItemId { get; set; }
            public string itemname { get; set; }
            public string LogoUrl { get; set; }
        }

    }
}
