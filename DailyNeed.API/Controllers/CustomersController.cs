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
using System.Data;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/Customers")]
    public class CustomersController : ApiController
    {
        CustomerServiceController custPrice = new CustomerServiceController();
        CustomerAPIMethod context = new CustomerAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        CustomerLinkController CutomerLink = new CustomerLinkController();
        CustomerLink CustomerTable = new CustomerLink();
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        int cid;
        /// <summary>
        /// Create rendom otp
        /// </summary>
        /// <param name="iOTPLength"></param>
        /// <param name="saAllowedCharacters"></param>
        /// <returns></returns>
        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        /// <summary>
        /// OTP Genration code 
        /// </summary>
        /// <returns></returns>
        [Route("Genotp")]
        public OTP Getotp(string MobileNumber)
        {
            logger.Info("start Gen OTP: ");
            try
            {
                string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);
                string OtpMessage = " is Your Dailyneed Varification Code. :)";
                string CountryCode = "91";
                string Sender = "SHOPKR";
                string authkey = "100498AhbWDYbtJT56af33e3";
                int route = 4;

                string path = "http://bulksms.newrise.in/api/sendhttp.php?authkey=" + authkey + "&mobiles=" + MobileNumber + "&message=" + sRandomOTP + " :" + OtpMessage + " &sender=" + Sender + "&route=" + route + "&country=" + CountryCode;

                //string path ="http://bulksms.newrise.in/api/sendhttp.php?authkey=100498AhbWDYbtJT56af33e3&mobiles=9770838685&message= DN OTP is : " + sRandomOTP + " &sender=SHOPKR&route=4&country=91";

                var webRequest = (HttpWebRequest)WebRequest.Create(path);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:28.0) Gecko/20100101 Firefox/28.0";
                webRequest.ContentLength = 0; // added per comment 
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                webRequest.Accept = "*/*";
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK) Console.WriteLine("{0}", webResponse.Headers);
                logger.Info("OTP Genrated: " + sRandomOTP);
                OTP a = new OTP()
                {
                    OtpNo = sRandomOTP
                };
                return a;
            }
            catch (Exception)
            {
                logger.Error("Error in OTP Genration.");
                return null;
            }
        }

        //Get all Parent customers by company id and provider id
        //[Authorize]
        [Route("")]
        public object Get()
        {
            logger.Info("start Get Customer: ");
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
                //getting all from customer link table
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Customer link: ");
                var customer = db.CustomerLinks.Where(x => x.Company.Id == CompanyId && x.ParentCustomer == null && x.Deleted == false).Include("PWarehouse").Include("Customer").ToList();
                return customer;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer Link " + ex.Message);
                logger.Info("End  Customer Link: ");
                return null;
            }
        }

        //Get customers by company id and Cust Id
        //[Authorize]
        [Route("GetCustomerByID")]
        public object GetCustomerById(int custid)
        {
            logger.Info("start Get Customer: ");
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
                //getting all customer with link table
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Customer: ");
                CustomerLinkDto customer = (from e in db.CustomerLinks
                                                //join pw in db.PWarehouseLinks on e.PWarehouseId equals pw.PWarehouseId
                                            where e.CustomerLinkId == custid && e.Active == true && e.Deleted == false && e.Company.Id == CompanyId
                                            select new CustomerLinkDto
                                            {
                                                CustomerLinkId = e.CustomerLinkId,
                                                CustomerName = e.Name,
                                                Address = e.Address,
                                                MobileNumber = e.Mobile,
                                                EmailId = e.EmailId,
                                                PWarehouseId = e.PWarehouse.PWarehouseId
                                            }).FirstOrDefault();
                return customer;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }


        //Get customers Services List by company id and Cust Id
        //[Authorize]
        [Route("GetCustomerServiceByID")]
        public object GetCustomerServiceById(int custid)
        {
            logger.Info("start Get Customer: ");
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
                //getting all customer with link table
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Customer: ");
                var customer = (from e in db.CustomerServicess
                                join shift in db.ShiftMasters on e.Shift.ShiftID equals shift.ShiftID
                                join Pitem in db.ItemMasterNews on e.PItem.PItemId equals Pitem.ItemId
                                where e.CustomerLink.CustomerLinkId == custid && e.IsActive == true && e.Deleted == false && e.Company.Id == CompanyId
                                select new CustomerServDto
                                {
                                    ServiceId = e.ID,
                                    PItemId = e.PItem.PItemId,
                                    ItemName = Pitem.ItemName,
                                    ShiftId = e.Shift.ShiftID,
                                    ShiftName = shift.ShiftName,
                                    Quantity = e.Quantity,
                                    Days = e.DeliveryDays,
                                    Time = e.DeliveryTime.Hour + ":" + e.DeliveryTime.Minute,
                                    Comment = e.Comment,
                                    PWarehouseID = e.PWarehouse.PWarehouseId
                                }).ToList();
                return customer;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }

        //Get Customer By Mobile No.
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

        [Route("InActive")]
        public List<Customer> GetInActive()
        {
            logger.Info("start customer: ");
            DailyNeedContext db = new DailyNeedContext();
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
                List<Customer> customer = db.Customers.Where(x => x.Active == false).ToList();
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

        [Route("Forgrt")]
        public HttpResponseMessage GetForgrt(string Mobile)
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
                if (customer != null)
                {
                    //   new Sms().sendOtp(customer.Mobile, "Hi " + customer.ShopName + " \n\t You Recently requested a forget password on DailyNeed. Your account Password is '" + customer.Password + "'\n If you didn't request then ingore this message\n\t\t Thanks\n\t\t Dailyneed.com");
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        //Search Warehouse in Warehouse table by warehouse name for bulk customer
        [HttpGet]
        [Route("serach")]
        public bool serachWarehouse(string key, int companyId)
        {
            try
            {
                var warehouse = db.Warehouses.Where(c => (c.WarehouseName == key) && c.Company.Id == companyId && c.Deleted == false).ToList();
                if (warehouse.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Search Depot in Depot table by Depot name for bulk customer
        [HttpGet]
        [Route("serach")]
        public bool serachDepot(string key, int companyId)
        {
            try
            {
                var depot = db.Depots.Where(c => (c.DepotName == key) && c.Company.Id == companyId && c.Deleted == false).ToList();
                if (depot.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //Search Zone in zone table by zone name for bulk customer
        [HttpGet]
        [Route("serach")]
        public bool serachZone(string key, int companyId)
        {
            try
            {
                var zone = db.Zones.Where(c => (c.ZoneName == key) && c.Company.Id == companyId && c.Deleted == false).ToList();
                if (zone.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Search Category in Category table by category name for bulk customer
        [HttpGet]
        [Route("serach")]
        public bool serachCategory(string key, int companyId)
        {
            try
            {
                var cate = db.Categorys.Where(c => (c.CategoryName == key) && c.Company.Id == companyId && c.Deleted == false).ToList();
                if (cate.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Search SubCategory in SubCategory table by subcategory name for bulk customer
        [HttpGet]
        [Route("serach")]
        public bool serachSubCategory(string key, int companyId)
        {
            try
            {
                var subcate = db.SubCategorys.Where(c => (c.SubcategoryName == key) && c.Company.Id == companyId && c.Deleted == false).ToList();
                if (subcate.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Search Item in Item table by Item name for bulk customer
        [HttpGet]
        [Route("serach")]
        public bool serachItem(string key, int companyId)
        {
            try
            {
                var item = db.ItemMasterNews.Where(c => (c.ItemName == key) && c.Company.Id == companyId && c.Deleted == false).ToList();
                if (item.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //Search Cutomer in Customer By Mobile Number
        [HttpGet]
        [Route("serach")]
        public bool serach(string key)
        {
            DailyNeedContext db = new DailyNeedContext();
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
                var customer = db.Customers.Where(c => (c.DNCode.Contains(key) || c.Mobile.Contains(key)) && c.Deleted == false).ToList();
                if (customer.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //Search Cutomer in CustomerLink By Mobile Number and CompnyID
        [HttpGet]
        [Route("serachLink")]
        public bool serachLink(string Mobileno, int CompnyId)
        {
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
                var customer = db.CustomerLinks.Where(c => (c.Customer.Mobile.Contains(Mobileno)
                               && (c.Provider.PeopleID.ToString().Contains(CompnyId.ToString())))).ToList();

                if (customer.Count > 0)
                {
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, customer);
                    return false;
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.OK, customer);
                    return true;
                }
                //return null;
            }
            catch (Exception ex)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                return false;
            }
        }

        [Route("export")]
        [HttpGet]
        public dynamic export()
        {
            logger.Info("start City: ");
            dynamic customer = null;
            DailyNeedContext db = new DailyNeedContext();
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

                customer = (from i in db.Customers
                                //join j in db.Peoples on i.ExecutiveId equals j.PeopleID into ps
                                //from j in ps.DefaultIfEmpty()
                                // join k in db.Clusters on i.ClusterId equals k.ClusterId into ps1
                                //  from k in ps1.DefaultIfEmpty()
                            select new
                            {
                                RetailerId = i.CustomerId,
                                RetailersCode = i.Skcode,
                                //ShopName = i.ShopName,
                                RetailerName = i.Name,
                                Mobile = i.Mobile,
                                Address = i.Address,
                                //Warehouse = i.WarehouseName,
                                Emailid = i.EmailId,
                                Active = i.Active,
                                Deleted = i.Deleted
                            }).OrderBy(x => x.RetailerId).ToList();

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


        //Add Customer Method
        [ResponseType(typeof(Customer))]
        [Route("AddCustomer")]
        [AcceptVerbs("POST")]
        public Customer add(Customer Customers)
        {
            logger.Info("start addCustomer: ");
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
                if (Customers == null)
                {
                    throw new ArgumentNullException("item");
                }
                int CompanyId = compid;
                //Server side validations 
                if (compid == 0 || userid == 0)
                {
                    Customers.Message = "Please Check Access Token!";
                    return Customers;
                }
                if (Customers.Name == "" || Customers.Name == null)
                {
                    Customers.Message = "Customer Name Is Requaired!";
                    return Customers;
                }
                if (Customers.Mobile == "" || Customers.Mobile == null)
                {
                    Customers.Message = "Customer Mobile Number Is Requaired!";
                    return Customers;
                }
                if (Customers.Address == "" || Customers.Address == null)
                {
                    Customers.Message = "Customer Address Is Requaired!";
                    return Customers;
                }
                if (Customers.EmailId == "" || Customers.EmailId == null)
                {
                    Customers.Message = "Customer EmailId Is Requaired!";
                    return Customers;
                }
                if (Customers.PWarehouseId == 0)
                {
                    Customers.Message = "Customer Warehouse Is Requaired!";
                    return Customers;
                }
                PWarehouseLink wh = db.PWarehouseLinks.Where(x => x.PWarehouseId == Customers.PWarehouseId).FirstOrDefault();
                //Search For Customer Mobile Number
                var CMobile = serach(Customers.Mobile);

                //If Customer Fond in Customer Main table then
                if (CMobile == true)
                {
                    //Search For Customer Link table
                    var LinkCustomer = serachLink(Customers.Mobile, CompanyId);

                    //If User Not Found in link table with same provider then
                    if (LinkCustomer == true)
                    {
                        Company Comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                        Customer customers = db.Customers.Where(c => (c.DNCode.Contains(Customers.Mobile) || c.Mobile.Contains(Customers.Mobile)) && c.Deleted == false).SingleOrDefault();
                        People Provider = db.Peoples.Where(x => x.PeopleID == userid).FirstOrDefault();
                        //customers.DNCode = context.dncode();
                        if (customers.Password == null)
                        {
                            Customers.Password = "123456";

                        }
                        CustomerTable.Customer = customers;
                        CustomerTable.DNCode = customers.DNCode;
                        CustomerTable.Provider = Provider;
                        CustomerTable.Company = Comp;
                        CustomerTable.Mobile = Customers.Mobile;
                        CustomerTable.BillingName = Customers.BillingName;
                        CustomerTable.LandMard = Customers.LandMard;
                        CustomerTable.Mobile1 = Customers.Mobile1;
                        CustomerTable.Mobile2 = Customers.Mobile2;
                        CustomerTable.Mobile3 = Customers.Mobile3;
                        CustomerTable.PhotoUrl = Customers.PhotoUrl;
                        CustomerTable.GPSLocation = Customers.GPSLocation;
                        CustomerTable.Address = Customers.Address;
                        CustomerTable.Name = Customers.Name;
                        CustomerTable.EmailId = Customers.EmailId;
                        CustomerTable.Password = "123456";
                        CustomerTable.PWarehouse = wh;
                        CustomerTable.Comment = Customers.Comment;
                        CustomerTable.Active = true;
                        CustomerTable.Deleted = false;
                        CustomerTable.CreatedDate = indianTime;
                        CutomerLink.addCustomerLink(CustomerTable);
                        Customers.Message = "Sucessfully";
                        return Customers;
                    }

                    //if provider and customer are same or found in both 
                    if (LinkCustomer == false)
                    {
                        Customers.Message = "Customer already Exist!";
                        return Customers;
                    }
                }

                //If Customer Not found In Main Table
                else
                {
                    //If Provider and Cutomer is new then
                    Company Comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                    Customers.Company = Comp;
                    //Add Customer In Customer Center Table
                    context.AddCustomer(Customers);
                    //Finding Customer From Central Customer Table
                    Customer customers = db.Customers.Where(c => (c.DNCode.Contains(Customers.Mobile) || c.Mobile.Contains(Customers.Mobile)) && c.Deleted == false).FirstOrDefault();
                    //Finding Provider From People Table
                    People Provider = db.Peoples.Where(x => x.PeopleID == userid).FirstOrDefault();
                    CustomerTable.Customer = customers;
                    CustomerTable.DNCode = customers.DNCode;
                    CustomerTable.Provider = Provider;
                    CustomerTable.Company = Comp;
                    CustomerTable.BillingName = Customers.BillingName;
                    CustomerTable.LandMard = Customers.LandMard;
                    CustomerTable.Mobile1 = Customers.Mobile1;
                    CustomerTable.Mobile2 = Customers.Mobile2;
                    CustomerTable.Mobile3 = Customers.Mobile3;
                    CustomerTable.PhotoUrl = Customers.PhotoUrl;
                    CustomerTable.GPSLocation = Customers.GPSLocation;
                    CustomerTable.Mobile = Customers.Mobile;
                    CustomerTable.Address = Customers.Address;
                    CustomerTable.Name = Customers.Name;
                    CustomerTable.EmailId = Customers.EmailId;
                    CustomerTable.Password = Customers.Password;
                    CustomerTable.CreatedDate = indianTime;
                    CustomerTable.PWarehouse = wh;
                    CustomerTable.Comment = Customers.Comment;
                    CustomerTable.Active = true;
                    CustomerTable.Deleted = false;
                    CutomerLink.addCustomerLink(CustomerTable);
                    logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                    logger.Info("End add Customer link: ");
                    Customers.CustomerId = CustomerTable.CustomerLinkId;
                    Customers.CompanyId = Customers.Company.Id;
                    Customers.Company = null;
                    Customers.Message = "Sucessfully";
                    return Customers;
                }
                return Customers;
            }
            catch (Exception ex)
            {
                logger.Error("Error in addCustomer " + ex.Message);
                Customers.Message = "Error";
                return Customers;
            }
        }


        //GET Customer Activetd Services
        [Route("GetCustomerService")]
        public object getcustservice(int custid)
        {
            logger.Info("start addCustomer: ");
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
            var custsr = db.CustomerServicess.Where(x => x.Company.Id == CompanyId && x.CustomerLink.CustomerLinkId == custid && x.IsActive == true && x.Deleted == false).Select(x => new CustomerWithServicesReturn { CustomerLink = x.CustomerLink.CustomerLinkId, ActiveItemId = x.PItem.PItemId }).ToList();
            return custsr;
        }

        //CustomerWithServices class 
        public class CustomerWithServicesReturn
        {
            public int CustomerLink { get; set; }
            public int ActiveItemId { get; set; }
        }


        //CustomerWithServices class
        public class CustomerWithServices
        {
            public Customer Customerobj { get; set; }
            public CustomerServices CustomerServicesobj { get; set; }
        }

        //add customer With Services
        [ResponseType(typeof(Customer))]
        [Route("")]
        [AcceptVerbs("POST")]
        public CustomerWithServices add(CustomerWithServices Customers)
        {
            logger.Info("start addCustomer: ");
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
                if (identity == null)
                {
                    compid = Convert.ToInt32(Customers.Customerobj.CompanyId);
                    userid = Convert.ToInt32(Customers.Customerobj.CreatedBy);
                }
                if (Customers == null)
                {
                    throw new ArgumentNullException("item");
                }
                int CompanyId = compid;
                PWarehouseLink wh = db.PWarehouseLinks.Where(x => x.PWarehouseId == Customers.Customerobj.PWarehouseId).FirstOrDefault();
                //Search For Customer Mobile Number
                var CMobile = serach(Customers.Customerobj.Mobile);

                //If Customer Fond in Customer Main table then
                if (CMobile == true)
                {
                    //Search For Customer Link table
                    var LinkCustomer = serachLink(Customers.Customerobj.Mobile, CompanyId);

                    //If User Not Found in link table with same provider then
                    if (LinkCustomer == true)
                    {
                        Company Comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                        Customer customers = db.Customers.Where(c => (c.Skcode.Contains(Customers.Customerobj.Mobile) || c.Mobile.Contains(Customers.Customerobj.Mobile)) && c.Deleted == false).SingleOrDefault();
                        People Provider = db.Peoples.Where(x => x.PeopleID == userid).FirstOrDefault();
                        customers.Skcode = context.skcode();
                        if (customers.Password == null)
                        {
                            Customers.Customerobj.Password = "123456";
                        }
                        CustomerTable.Customer = customers;
                        CustomerTable.Customer.DNCode = customers.Skcode;
                        CustomerTable.Provider = Provider;
                        CustomerTable.Company = Comp;
                        CustomerTable.Mobile = Customers.Customerobj.Mobile;
                        CustomerTable.Address = Customers.Customerobj.Address;
                        CustomerTable.Name = Customers.Customerobj.Name;
                        CustomerTable.EmailId = Customers.Customerobj.EmailId;
                        CustomerTable.Password = Customers.Customerobj.Password;
                        CustomerTable.PWarehouse = wh;
                        CustomerTable.Active = true;
                        CustomerTable.Deleted = false;
                        CustomerTable.CreatedDate = indianTime;
                        CutomerLink.addCustomerLink(CustomerTable);
                        Customers.Customerobj.Message = "Sucessfully";
                        return Customers;
                    }

                    //if provider and customer are same or found in both 
                    if (LinkCustomer == false)
                    {
                        Customers.Customerobj.Message = "Found";
                        return Customers;
                    }
                }

                //If Customer Not found In Main Table
                else
                {
                    //If Provider and Cutomer is new then
                    Company Comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                    Customers.Customerobj.Company = Comp;
                    context.AddCustomer(Customers.Customerobj);
                    Customer customers = db.Customers.Where(c => (c.Skcode.Contains(Customers.Customerobj.Mobile) || c.Mobile.Contains(Customers.Customerobj.Mobile)) && c.Deleted == false).FirstOrDefault();
                    People Provider = db.Peoples.Where(x => x.PeopleID == userid).FirstOrDefault();
                    CustomerTable.Customer = customers;
                    CustomerTable.DNCode = customers.Skcode;
                    CustomerTable.Provider = Provider;
                    CustomerTable.Company = Comp;
                    CustomerTable.Mobile = Customers.Customerobj.Mobile;
                    CustomerTable.Address = Customers.Customerobj.Address;
                    CustomerTable.Name = Customers.Customerobj.Name;
                    CustomerTable.EmailId = Customers.Customerobj.EmailId;
                    CustomerTable.Password = Customers.Customerobj.Password;
                    CustomerTable.CreatedDate = indianTime;
                    CustomerTable.PWarehouse = wh;
                    CustomerTable.Active = true;
                    CustomerTable.Deleted = false;
                    CustomerLink Cust = CutomerLink.addCustomerLink(CustomerTable);
                    logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                    logger.Info("End add Customer link: ");
                    Customers.Customerobj.Message = "Sucessfully";
                    Customers.CustomerServicesobj.CustomerLinkId = Cust.CustomerLinkId;

                    //return Customers.Customerobj;
                }
                //return Customers.Customerobj;
            }
            catch (Exception ex)
            {
                logger.Error("Error in addCustomer " + ex.Message);
                return null;
            }
            if (Customers.CustomerServicesobj != null)
            {
                Customers.CustomerServicesobj.Customer = Customers.Customerobj;
                custPrice.add(Customers.CustomerServicesobj);
            }
            return Customers;
        }

        //Get child customers by company id and Parent customer id from cusotmer link table
        //[Authorize]
        [Route("GetChildCustomer")]
        public List<CustomerLink> GetChildCustomer(int parentid)
        {
            logger.Info("start Get Customer Link: ");
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
                //getting all child customers from customer table
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End  Child Customer: ");
                var childCustomer = db.CustomerLinks.Where(x => x.Company.Id == CompanyId && x.ParentCustomer.CustomerLinkId == parentid).ToList();
                return childCustomer;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Child Customer Link " + ex.Message);
                logger.Info("End Child Customer Link: ");
                return null;
            }
        }

        //Add Child Customer in cusotmer link table on the bases of parent 
        [ResponseType(typeof(CustomerLink))]
        [Route("ChildCustomers")]
        [AcceptVerbs("POST")]
        public CustomerLink addChildCustomers(CustomerLink Customers)
        {
            logger.Info("start addCustomer: ");
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
                if (Customers == null)
                {
                    throw new ArgumentNullException("Child Customer");
                }
                // logic for add child customer information with parent id 
                int CompanyId = compid;
                Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                var childcust = db.CustomerLinks.Where(x => x.Company.Id == CompanyId && x.CustomerLinkId == Customers.ParentCustomer.CustomerLinkId).FirstOrDefault();
                var cust = db.Customers.Where(x => x.CustomerId == Customers.CustomerId).FirstOrDefault();
                People provider = db.Peoples.Where(x => x.PeopleID == CompanyId).FirstOrDefault();
                PWarehouseLink wh = db.PWarehouseLinks.Where(x => x.PWarehouseId == Customers.PWarehouseId && x.Deleted == false).FirstOrDefault();
                if (childcust != null)
                {
                    Customers.ParentCustomer = childcust;
                    Customers.CreatedDate = indianTime;
                    Customers.Mobile = Customers.Mobile;
                    Customers.Address = Customers.Address;
                    Customers.Name = Customers.Name;
                    Customers.EmailId = Customers.EmailId;
                    Customers.Password = childcust.Password;
                    Customers.DNCode = childcust.DNCode;
                    Customers.Provider = provider;
                    Customers.Company = comp;
                    Customers.Customer = cust;
                    Customers.PWarehouse = wh;
                    Customers.Active = true;
                    Customers.Deleted = false;
                    Customers.Message = "Sucessfully";
                    db.CustomerLinks.Add(Customers);
                    db.SaveChanges();
                }
                return Customers;
            }
            catch (Exception ex)
            {
                logger.Error("Error in addCustomer " + ex.Message);
                return null;
            }
        }

        //Update Customer Link data into Cusotmerlink table
        //[Authorize]
        [ResponseType(typeof(CustomerLink))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public CustomerLink Put(CustomerLink linkitem)
        {
            logger.Info("start putCustomer Link: ");
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
                if (linkitem == null)
                {
                    throw new ArgumentNullException("itemLink");
                }
                linkitem.CompanyId = compid;
                return context.PutCustomerLink(linkitem);
            }
            catch (Exception ex)
            {
                logger.Error("Error in put Customer Link " + ex.Message);
                return null;
            }
        }

        //get All PWarehouse by company in Customer 
        //[Authorize]
        [Route("GetPWarehouse")]
        public object GetPWarehouse()
        {
            logger.Info("start Get GetPWarehouse: ");
            List<PWarehouseLink> customerservice = new List<PWarehouseLink>();
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
                logger.Info("End  Pwarehouse: ");

                var Warehouse = db.PWarehouseLinks.Where(x => x.Company.Id == CompanyId && x.Deleted == false && x.IsActive == true).Include("Warehouse").ToList();
                return Warehouse;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }

        }

        //Delete Customer in customer link
        [ResponseType(typeof(CustomerLink))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public bool Remove(int id)
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
                logger.Info("End  delete Customer: ");
                return context.DeleteCustomer(id);
            }
            catch (Exception ex)
            {
                logger.Error("Error in delete Customer " + ex.Message);
                return false;
            }
        }

        [Route("Mobile")]
        [HttpGet]
        public HttpResponseMessage CheckMobile(string Mobile)
        {
            DailyNeedContext db = new DailyNeedContext();
            try
            {
                logger.Info("Get Peoples: ");
                int compid = 0, userid = 0;
                int Warehouse_id = 0;
                string email = "";
                var identity = User.Identity as ClaimsIdentity;
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

                var RDMobile = db.Customers.Where(x => x.Mobile == Mobile).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, RDMobile);



            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, false);
            }



        }

        [Route("Email")]
        [HttpGet]
        public HttpResponseMessage CheckEmail(string Email)
        {
            DailyNeedContext db = new DailyNeedContext();
            try
            {
                logger.Info("Get Peoples: ");
                int compid = 0, userid = 0;
                int Warehouse_id = 0;
                string email = "";
                var identity = User.Identity as ClaimsIdentity;
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

                var RDMobile = db.Customers.Where(x => x.EmailId == Email).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, RDMobile);



            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, false);
            }
        }

        // add bulk customer 
        public List<Customer> AddBulkcustomer(List<Customer> CustCollection, int companyId)
        {
            logger.Info("start addbulk customer");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (CustCollection == null)
                {
                    throw new ArgumentNullException("item");
                }
                int CompanyId = companyId;

                foreach (var Customersrd in CustCollection)
                {
                    Warehouse wh = db.Warehouses.Where(x => x.WarehouseId == Customersrd.WarehouseId).FirstOrDefault();
                    //Search For Customer Mobile Number
                    var CMobile = serach(Customersrd.Mobile);

                    //If Customer Fond in Customer Main table then
                    if (CMobile == true)
                    {
                        //Search For Customer Link table
                        var LinkCustomer = serachLink(Customersrd.Mobile, CompanyId);

                        //If User Not Found in link table with same provider then
                        if (LinkCustomer == true)
                        {
                            Company Comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                            Customer customers = db.Customers.Where(c => (c.Skcode.Contains(Customersrd.Mobile) || c.Mobile.Contains(Customersrd.Mobile)) && c.Deleted == false).FirstOrDefault();
                            People Provider = db.Peoples.Where(x => x.Company.Id == companyId).FirstOrDefault();
                            //customers.DNCode = context.dncode();
                            if (customers.Password == null)
                            {
                                Customersrd.Password = "123456";
                            }
                            CustomerTable.Customer = customers;
                            CustomerTable.DNCode = customers.DNCode;
                            CustomerTable.Provider = Provider;
                            CustomerTable.Company = Comp;
                            CustomerTable.Mobile = Customersrd.Mobile;
                            CustomerTable.Address = Customersrd.Address;
                            CustomerTable.Name = Customersrd.Name;
                            CustomerTable.EmailId = Customersrd.EmailId;
                            CustomerTable.Password = Customersrd.Password;
                            CustomerTable.Warehouse = wh;
                            CustomerTable.Comment = Customersrd.Comment;
                            CustomerTable.Active = true;
                            CustomerTable.Deleted = false;
                            CustomerTable.CreatedDate = indianTime;
                            CutomerLink.addCustomerLink(CustomerTable);
                            Customersrd.CustomerId = CustomerTable.CustomerLinkId;
                            Customersrd.CompanyId = Customersrd.Company.Id;
                            Customersrd.Message = "Successfully";
                            //add customer with customer services call bulk services method
                            context.AddCustomerServicesBulk(Customersrd, companyId);
                        }
                        //If the customer is exist in customer link table also then create service of that customer
                        else
                        {
                            CustomerLink customerlk = db.CustomerLinks.Where(x => x.Mobile == Customersrd.Mobile && x.Company.Id == companyId).FirstOrDefault();
                            Customersrd.CustomerId = customerlk.CustomerLinkId;
                            CustomerServices custservice = db.CustomerServicess.Where(x => x.ServiceName == Customersrd.Item.ItemName && x.CustomerLink.CustomerId == customerlk.CustomerLinkId).FirstOrDefault();
                            if (custservice != null)
                            {
                                context.AddCustomerServicesBulk(Customersrd, companyId);
                            }
                        }
                    }
                    //If Customer Not found In Main Table
                    else
                    {
                        //If Provider and Cutomer is new then
                        Company Comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                        Customersrd.Company = Comp;
                        //Add Customer In Customer Center Table
                        context.AddCustomerBulk(Customersrd);
                        //Finding Customer From Central Customer Table
                        Customer customer = db.Customers.Where(c => (c.DNCode.Contains(Customersrd.Mobile) || c.Mobile.Contains(Customersrd.Mobile)) && c.Deleted == false).FirstOrDefault();
                        //Finding Provider From People Table
                        People Provider = db.Peoples.Where(x => x.Company.Id == companyId).FirstOrDefault();
                        CustomerTable.Customer = customer;
                        CustomerTable.DNCode = customer.DNCode;
                        CustomerTable.Provider = Provider;
                        CustomerTable.Company = Comp;
                        CustomerTable.Mobile = Customersrd.Mobile;
                        CustomerTable.Address = Customersrd.Address;
                        CustomerTable.Name = Customersrd.Name;
                        CustomerTable.EmailId = Customersrd.EmailId;
                        CustomerTable.Password = Customersrd.Password;
                        CustomerTable.CreatedDate = indianTime;
                        CustomerTable.Warehouse = wh;
                        CustomerTable.Comment = Customersrd.Comment;
                        CustomerTable.Active = true;
                        CustomerTable.Deleted = false;
                        CutomerLink.addCustomerLink(CustomerTable);
                        logger.Info("End add Customer link: ");
                        Customersrd.CustomerId = CustomerTable.CustomerLinkId;
                        //Customersrd.CompanyId = Customersrd.Company.Id;
                        Customersrd.Message = "Successfully";
                        //add customer with customer services call bulk services method
                        context.AddCustomerServicesBulk(Customersrd, companyId);
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Info("error in adding customer collection" + ex.Message);
            }
            return null;
        }

        //Get Providers by name search textbox 
        //[Authorize]
        [HttpGet]
        [Route("getProviders")]
        public object getProvider(string pname)
        {
            logger.Info("start Get providers: ");
            try
            {
                //getting provider 
                logger.Info("User ID : {0} , Company Id : {1}");
                logger.Info("End  Peoples: ");
                var provider = db.Peoples.Where(x => x.PeopleFirstName != null && x.Deleted == false && x.PeopleFirstName.StartsWith(pname)).Select(x => x.PeopleFirstName).ToList();
                return provider;
            }
            catch (Exception ex)
            {
                logger.Error("Error in getProvider " + ex.Message);
                logger.Info("End  provider: ");
                return null;
            }
        }

        public class OTP
        {
            public string OtpNo { get; set; }
        }
    }
}