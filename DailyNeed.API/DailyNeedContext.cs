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

namespace DailyNeed.API
{
    public class DailyNeedContext : IdentityDbContext<IdentityUser>, iDailyNeedContext
    {
        //nlogger
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        ShiftMaster shft;
        public DailyNeedContext() : base("DailyNeedContext")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        public static DailyNeedContext Create()
        {
            return new DailyNeedContext();
        }
        #region For Create DbSet
    
        public DbSet<CheckCurrency> CheckCurrencyDB { get; set; }   
   
        public DbSet<appVersion> appVersionDb { get; set; }
     
        public DbSet<BaseCategory> BaseCategoryDb { get; set; }
     
        public DbSet<Category> Categorys { get; set; }
        public DbSet<SubCategory> SubCategorys { get; set; }
        public DbSet<SubsubCategory> SubsubCategorys { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<CustomerWallet> CustomerWallets { get; set; }
        public DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public DbSet<People> Peoples { get; set; }
        public DbSet<Role> UserRole { get; set; }
        public DbSet<Company> Companies { get; set; }
      
        public DbSet<Customer> Customers { get; set; }
       
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserTraking> UserTrakings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CustomerRegistration> CustomerRegistrations { get; set; }
   
        public DbSet<CategoryImage> CategoryImageDB { get; set; }
      
        public DbSet<UserAccessPermission> UserAccessPermissionDB { get; set; }
      
        public DbSet<CustomerLink> CustomerLinks { get; set; }
        public DbSet<UnitMaster> UnitMaters { get; set; }
        public DbSet<CustomerServices> CustomerServicess { get; set; }
        public DbSet<CustomerAdditionalService> CustomerAdditionalServicess { get; set; }
        public DbSet<MonthLeave> MonthLeaves { get; set; }
        public DbSet<MonthlyStatement> MonthlyStatements { get; set; }
     
        public DbSet<PWarehouseLink> PWarehouseLinks { get; set; }
        public DbSet<PBaseCategoryLink> PBaseCategoryLinks { get; set; }
        public DbSet<PCategoryLink> PCategoryLinks { get; set; }
        public DbSet<PSubCategoryLink> PSubCategoryLinks { get; set; }
        public DbSet<PItemLink> PItemLinks { get; set; }
        public DbSet<ShiftMaster> ShiftMasters { get; set; }
        public DbSet<ItemMasterNew> ItemMasterNews { get; set; }
        public DbSet<ItemMasterNewCentral> ItemMasterNewCentrals { get; set; }
        public DbSet<DeliveryChallan> DeliveryChallans { get; set; }
        public DbSet<InvoiceBill> InvoiceBills { get; set; }
        public DbSet<NewItemRequest> NewItemRequests { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<FarmRegistration> FarmRegistrations { get; set; }
        public DbSet<ShedRegistration> ShedRegistrations { get; set; }

        public DbSet<GroupRegistration> GroupRegistrations { get; set; }

        public DbSet<CattleRegistration> CattleRegistrations { get; set; }

        public DbSet<MilkYield> MilkYields { get; set; }




        public DbSet<CattleType> CattleTypes { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }

        public DbSet<SpeciesType> SpeciesTypes { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<Insimination> Insiminations { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

      









        #endregion
        //======Start of function =================================================================================================================================//



        public UserAccessPermission getRoleDetail(string RoleName)
        {
            UserAccessPermission uap = new UserAccessPermission();
            uap = UserAccessPermissionDB.Where(x => x.RoleName == RoleName).SingleOrDefault();
            string id = "0";
            if (uap != null)
            {
                id = uap.RoleId;
            }
            return uap;
        }

        public Customer Resetpassword(Customer customer)
        {
            Customer cust = Customers.Where(x => x.Mobile == customer.Mobile && x.Deleted == false).FirstOrDefault();
            cust.Password = customer.Password;
            Customers.Attach(cust);
            this.Entry(cust).State = EntityState.Modified;
            this.SaveChanges();
            return cust;
        }
       
        public List<Customer> AddBulkcustomer(List<Customer> CustCollection)
        {
            logger.Info("start addbulk customer");
            try
            {
                foreach (var o in CustCollection)
                {
                    List<Customer> cust = Customers.Where(c => c.Skcode.Equals(o.Skcode) || c.Mobile == o.Mobile).ToList();
                    Customer objitemMaster = new Customer();
                    if (cust.Count == 0)
                    {
                        o.CreatedDate = indianTime;
                        o.UpdatedDate = indianTime;
                        o.CreatedBy = "admin";

                        //var clstr = Clusters.Where(x => x.ClusterId == o.ClusterId).SingleOrDefault();
                        //if (clstr != null)
                        //{
                        //    o.ClusterId = clstr.ClusterId;
                        //    o.ClusterName = clstr.ClusterName;
                        //}
                        //else
                        //{
                        //    Cluster fclstr = Clusters.FirstOrDefault();
                        //    o.ClusterId = fclstr.ClusterId;
                        //    o.ClusterName = fclstr.ClusterName;
                        //}
                        //if (o.Day == null)
                        //{
                        //    o.Day = "";
                        //}
                        Customers.Add(o);
                        int id = this.SaveChanges();

                    }
                    else
                    {
                        List<Customer> cust1 = Customers.Where(c => c.Skcode.ToLower().Equals(o.Skcode.ToLower()) && c.Mobile.Trim() == o.Mobile.Trim()).ToList();
                        if (cust1.Count() == 1)
                        {
                            logger.Info("Skcode already exists");
                            Customer editcust = cust1[0];
                            editcust.EmailId = o.EmailId;
                            editcust.Name = o.Name;
                            editcust.Mobile = o.Mobile;
                            editcust.Address = o.Address;
                            editcust.UpdatedDate = indianTime;
                            Customers.Attach(editcust);
                            this.Entry(editcust).State = EntityState.Modified;
                            this.SaveChanges();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("error in adding customer collection" + ex.Message);
            }
            return null;
        }
        public List<People> AddBulkpeople(List<People> CustCollection)
        {
            logger.Info("start addbulk customer");
            try
            {
                foreach (var o in CustCollection)
                {
                    List<People> cust = Peoples.Where(c => c.Mobile.Equals(o.Mobile) && c.Deleted == false).ToList();

                    People objitemMaster = new People();
                    if (cust.Count == 0)
                    {
                        o.CreatedDate = indianTime;
                        o.UpdatedDate = indianTime;
                        Peoples.Add(o);
                        int id = this.SaveChanges();

                    }
                    else
                    {
                        logger.Info("Mobile number already exists");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info("error in adding Sales Executive collection");
            }
            return null;
        }       


        private string smstemplate(string nm, string invoice, string text)
        {
            string bodytext = text;
            bodytext = bodytext.Replace("%CustomerName%", nm);
            bodytext = bodytext.Replace("%OrderId%", invoice);
            return bodytext;
        }

        


        public IEnumerable<SubsubCategory> sAllCategory(int compid)
        {
            if (SubsubCategorys.AsEnumerable().Count() > 0)
            {
                return SubsubCategorys.Where(p => p.Deleted == false).AsEnumerable();
            }
            else
            {
                List<SubsubCategory> category = new List<SubsubCategory>();
                return category.AsEnumerable();
            }
        }
        

        public int AddCategoryImage(CategoryImage item)
        {
            try
            {
                CategoryImage ci = new CategoryImage();
                ci.CreatedDate = indianTime;
                ci.UpdatedDate = indianTime;
                ci.IsActive = true;
                ci.Deleted = false;
                ci.CategoryImg = item.CategoryImg;
                ci.CategoryId = item.CategoryId;
                CategoryImageDB.Add(ci);
                this.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public int PutCategoryImage(CategoryImage item)
        {
            try
            {
                CategoryImage categoryimg = CategoryImageDB.Where(x => x.CategoryImageId == item.CategoryImageId && x.Deleted == false).FirstOrDefault();
                categoryimg.CategoryImageId = item.CategoryImageId;
                categoryimg.CategoryImg = item.CategoryImg;
                categoryimg.CreatedDate = item.CreatedDate;
                categoryimg.UpdatedDate = indianTime;
                categoryimg.Deleted = false;
                categoryimg.IsActive = item.IsActive;
                categoryimg.CategoryId = item.CategoryId;
                CategoryImageDB.Attach(categoryimg);
                this.Entry(categoryimg).State = EntityState.Modified;
                this.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public List<CategoryImageData> AllCategoryImages()
        {
            try
            {
                List<CategoryImageData> ci = new List<CategoryImageData>();
                ci = (from cia in CategoryImageDB
                      join c in Categorys on cia.CategoryId equals c.Categoryid
                      where cia.Deleted == false && cia.IsActive == true
                      select new CategoryImageData
                      {
                          CategoryImageId = cia.CategoryImageId,
                          CategoryName = c.CategoryName,
                          CategoryImg = cia.CategoryImg,
                          IsActive = cia.IsActive,
                          CategoryId = cia.CategoryId,
                          CreatedDate = cia.CreatedDate

                      }).ToList();
                return ci;
            }
            catch (Exception)
            {
                return null;
            }

        }

        //Get Zone Method
        public IEnumerable<Zone> AllZones(int compid)
        {
            if (Zones.AsEnumerable().Count() > 0)
            {
                // && p.Company.Id== compid
                return Zones.Where(p => p.Deleted == false && p.Company.Id == compid).Include("Warehouse").Include("Depot").AsEnumerable();
            }
            else
            {
                List<Zone> zone = new List<Zone>();
                return zone.AsEnumerable();
            }
        }

        public IEnumerable<Zone> AllZoness(int zne, int CompanyId)
        {
            if (Zones.AsEnumerable().Count() > 0)
            {
                return Zones.Where(p => p.Depotid == zne && p.Company.Id == CompanyId && p.Deleted == false).AsEnumerable();
            }
            else
            {
                List<Zone> subcategory = new List<Zone>();
                return Zones.AsEnumerable();
            }
        }

        //Add Zone Method
        public Zone AddZone(Zone zone, int compid, int userid)
        {

            Depot depot = Depots.Where(d => d.Depotid == zone.Depotid && d.Deleted == false).FirstOrDefault();
            Warehouse wh = Warehouses.Where(w => w.WarehouseId == zone.WarehouseId && w.Deleted == false).FirstOrDefault();
            Company comp = Companies.Where(c => c.Id == compid && c.Deleted == false).FirstOrDefault();
            People peop = Peoples.Where(p => p.PeopleID == userid && p.Deleted == false).FirstOrDefault();
            Zone objSubcat = new Zone();
            if (depot != null)
            {
                zone.Depot = depot;
                zone.DepotName = depot.DepotName;
                zone.Company = comp;
                zone.Warehouse = wh;
                zone.Provider = peop;
                zone.CreatedBy = Convert.ToString(userid);
                zone.CreatedDate = indianTime;
                //zone.UpdatedDate = objSubcat.UpdatedDate;
                zone.IsActive = true;
                zone.Deleted = false;
                zone.Message = "Sucessfully";
                zone.Success = true;
                var data = Zones.Add(zone);
                int id = this.SaveChanges();
                return data;
            }
            else
            {
                return objSubcat;
            }
        }

        //Update Zone Method
        public Zone PutZone(Zone objZone, int CompanyId, int UserId)
        {
            Zone zone = Zones.Where(z => z.Zoneid == objZone.Zoneid && z.Deleted == false).FirstOrDefault();
            Depot depot = Depots.Where(d => d.Depotid == objZone.Depotid).FirstOrDefault();
            Warehouse wh = Warehouses.Where(w => w.WarehouseId == objZone.WarehouseId && w.Deleted == false).FirstOrDefault();
            Company comp = Companies.Where(c => c.Id == CompanyId && c.Deleted == false).FirstOrDefault();
            People peop = Peoples.Where(p => p.PeopleID == UserId && p.Deleted == false).FirstOrDefault();
            if (zone != null)
            {
                if (depot != null)
                {
                    zone.Depotid = depot.Depotid;
                    zone.DepotName = depot.DepotName;
                }
                else
                {
                    zone.Depotid = objZone.Depotid;
                }
                zone.Company = comp;
                zone.Warehouse = wh;
                zone.Provider = peop;
                zone.UpdatedDate = indianTime;
                zone.Zoneid = objZone.Zoneid;
                zone.ZoneName = objZone.ZoneName;
                zone.ZoneAddress = objZone.ZoneAddress;
                zone.Description = objZone.Description;
                zone.IsActive = true;
                zone.Deleted = false;
                zone.UpdateBy = Convert.ToString(UserId);
                zone.Message = "Successfully";
                zone.Success = true;
                Zones.Attach(zone);
                this.Entry(zone).State = EntityState.Modified;
                this.SaveChanges();
                return zone;
            }
            else
            {
                zone.Message = "Error";
                zone.Success = false;
                return zone;
            }
        }

        //Delete Zone Mehtod
        public bool DeleteZone(int id, int CompanyId)
        {
            try
            {
                Zone zone = Zones.Where(z => z.Zoneid == id && z.Deleted == false).FirstOrDefault();
                zone.Deleted = true;
                zone.IsActive = false;
                zone.Message = "Deleted Successfully";
                zone.Success = true;
                Zones.Attach(zone);
                this.Entry(zone).State = EntityState.Modified;
                this.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<SubCategory> AllSubCategory(int compid)
        {
            if (SubCategorys.AsEnumerable().Count() > 0)
            {
                return SubCategorys.Where(p => p.Deleted == false).AsEnumerable();
            }
            else
            {
                List<SubCategory> subcategory = new List<SubCategory>();
                return subcategory.AsEnumerable();
            }
        }

        public IEnumerable<SubCategory> AllSubCategoryy(int subcat, int CompanyId)
        {
            if (SubCategorys.AsEnumerable().Count() > 0)
            {
                return SubCategorys.Where(p => p.Categoryid == subcat && p.Deleted == false).AsEnumerable();
            }
            else
            {
                List<SubCategory> subcategory = new List<SubCategory>();
                return subcategory.AsEnumerable();
            }
        }

        //Add SubCategory Method
        public SubCategory AddSubCategory(SubCategory subCategory)
        {
            Category category = Categorys.Where(c => c.Categoryid == subCategory.Categoryid && c.Deleted == false).FirstOrDefault();
            SubCategory objSubcat = new SubCategory();
            if (category != null)
            {
                subCategory.CreatedBy = subCategory.CreatedBy;
                subCategory.CreatedDate = indianTime;
                subCategory.UpdatedDate = indianTime;
                subCategory.CategoryName = category.CategoryName;
                subCategory.IsActive = true;
                subCategory.Deleted = false;
                subCategory.Message = "Sucessfully";
                SubCategorys.Add(subCategory);
                int id = this.SaveChanges();

                
                return subCategory;
            }
            else
            {
                return objSubcat;
            }
        }

        //Update SubCategory Method
        public SubCategory PutSubCategory(SubCategory objSubCategory, int userid)
        {
            SubCategory subCategory = SubCategorys.Where(x => x.SubCategoryId == objSubCategory.SubCategoryId && x.Deleted == false).FirstOrDefault();
            Category cat = Categorys.Where(x => x.Categoryid == objSubCategory.Categoryid).FirstOrDefault();
            if (subCategory != null)
            {
                if (cat != null)
                {
                    subCategory.Categoryid = cat.Categoryid;
                    subCategory.CategoryName = cat.CategoryName;
                }
                else
                {
                    subCategory.Categoryid = objSubCategory.Categoryid;
                }
                subCategory.UpdatedDate = indianTime;
                subCategory.SubCategoryId = objSubCategory.SubCategoryId;
                subCategory.SubcategoryName = objSubCategory.SubcategoryName;
                subCategory.Discription = objSubCategory.Discription;
                subCategory.LogoUrl = objSubCategory.LogoUrl;
                subCategory.Code = objSubCategory.Code;
                subCategory.IsActive = objSubCategory.IsActive;
                subCategory.Deleted = objSubCategory.Deleted;
                subCategory.UpdateBy = userid.ToString();
                objSubCategory.Message = "Sucessfully";
                SubCategorys.Attach(subCategory);
                this.Entry(subCategory).State = EntityState.Modified;
                this.SaveChanges();

                
                return objSubCategory;
            }
            else
            {
                return objSubCategory;
            }
        }

        //Delete SubCategory Mehtod
        public bool DeleteSubCategory(int id, int CompanyId)
        {
            try
            {
                SubCategory subCategory = SubCategorys.Where(x => x.SubCategoryId == id && x.Deleted == false).FirstOrDefault();
                subCategory.Deleted = true;
                subCategory.IsActive = false;
                SubCategorys.Attach(subCategory);
                this.Entry(subCategory).State = EntityState.Modified;
                this.SaveChanges();

                
                return true;
            }
            catch
            {
                return false;
            }
        }


        public IEnumerable<SubsubCategory> AllSubsubCat(int compid)
        {
            if (SubsubCategorys.AsEnumerable().Count() > 0)
            {
                return SubsubCategorys.Where(p => p.Deleted == false).AsEnumerable();
            }
            else
            {
                List<SubsubCategory> subsubcat = new List<SubsubCategory>();
                return subsubcat.AsEnumerable();
            }
        }
        public dynamic GenerateSubSubCode(int compid)
        {
            string SubSubCode1 = string.Empty;

            var SubSubCode = 101;
            if (SubSubCode != 0)
            {
                int i = 1;
                bool flag = false;
                while (flag == false)
                {
                    var skint = SubSubCode + i;
                    SubSubCode1 = skint.ToString();
                    List<SubsubCategory> check = SubsubCategorys.Where(s => s.Code.Trim().ToLower() == SubSubCode1.Trim().ToLower()).ToList();
                    if (check.Count == 0)
                    {
                        flag = true;
                        return SubSubCode1.ToString();
                    }
                    else
                    {
                        i = i + 1;
                    }
                }
            }
            return SubSubCode1;
        }      
        public SubsubCategory AddQuesAnsxl(SubsubCategory quesans)
        {
            List<SubsubCategory> quesanss = SubsubCategorys.Where(c => c.Categoryid.Equals(quesans.Categoryid) && c.Deleted == false).ToList();
            SubsubCategory objQuesAns = new SubsubCategory();
            if (quesanss.Count == 0)
            {
                quesans.CreatedBy = quesans.CreatedBy;
                quesans.CreatedDate = indianTime;
                quesans.UpdatedDate = indianTime;
                SubsubCategorys.Add(quesans);
                int id = this.SaveChanges();
                return quesans;
            }
            else
            {
                return objQuesAns;
            }
        }
       
        public IEnumerable<CustomerRegistration> Allcustomers()
        {
            if (CustomerRegistrations.AsEnumerable().Count() > 0)
            {
                return CustomerRegistrations.AsEnumerable();
            }
            else
            {
                List<Category> category = new List<Category>();
                return CustomerRegistrations.AsEnumerable();
            }
        }
        public Customer getcustomers(string mobile)
        {
            try
            {
                Customer customers = Customers.Where(c => c.Mobile.Trim().Equals(mobile.Trim()) && c.Deleted == false).SingleOrDefault();
                if (customers == null)
                {
                    return null;
                }
                else
                {
                    return customers;
                }
            }
            catch
            {
                return null;
            }
        }
        public Customer getAllcustomers(string Mobile, string Password)
        {
            try
            {
                Customer customers = Customers.Where(c => c.Mobile.Trim().Equals(Mobile.Trim()) && c.Deleted == false).Where(d => d.Password.Trim().Equals(Password.Trim())).SingleOrDefault();
                if (customers == null)
                {
                    return null;
                }
                else
                {
                    return customers;
                }
            }
            catch
            {
                return null;
            }
        }
      
 
        //public Customer CustomerUpdate(Customer Cust)
        //{
        //    Customer customer = Customers.Where(c => c.Mobile.Trim().Equals(Cust.Mobile.Trim()) && c.Deleted == false).SingleOrDefault();
        //    try
        //    {
        //        if (customer != null)
        //        {
        //            customer.Emailid = Cust.Emaild;
        //            customer.Name = Cust.Name;
        //            customer.ShopName = Cust.ShopName;
        //            customer.RefNo = Cust.RefNo;
        //            customer.Password = Cust.Password;
        //            customer.DOB = Cust.DOB;
        //            customer.UploadRegistration = Cust.UploadRegistration;
        //            customer.ResidenceAddressProof = Cust.ResidenceAddressProof;
        //            Customers.Attach(customer);
        //            this.Entry(customer).State = EntityState.Modified;
        //            this.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //    }
        //    return customer;
        //}

        public People CheckPeople(string mob, string password)
        {
            People check = new People();
            check = Peoples.Where(p => p.Mobile == mob && p.Password == password && p.Deleted == false && p.Active == true).SingleOrDefault();
            if (check != null)
            {
                return check;
            }
            else
            {
                return null;
            }
        }
        public CustomerRegistration PutCustomerRegistration(CustomerRegistration customer)
        {
            CustomerRegistration cust = CustomerRegistrations.Where(x => x.Mobile == customer.Mobile).FirstOrDefault();
            if (cust != null)
            {
                cust.City = customer.City;
                cust.Country = customer.Country;
                cust.State = customer.State;
                cust.GeoLocation = customer.GeoLocation;
                cust.ZipCode = customer.ZipCode;
                cust.Address = customer.Address;
                //cust.Warehouseid = 6;
                CustomerRegistrations.Attach(cust);
                this.Entry(cust).State = EntityState.Modified;
                this.SaveChanges();
                return cust;
            }
            else
            {
                return null;
            }
        }


   
        public IEnumerable<Customer> AllCustomerbyCompanyId(int cmpid)
        {
            return Customers.Where(c => c.Deleted == false).AsEnumerable();
        }
        public IEnumerable<Customer> AllCustomers(int compid)
        {
            { return Customers.Where(x => x.Deleted == false).AsEnumerable(); }

        }

        public IEnumerable<CustomerLink> AllCustomersLink(int compid)
        {
            { return CustomerLinks.Where(x => x.Provider.PeopleID == compid).AsEnumerable(); }

        }
        
        public CustomerServices AddCustomerServices(CustomerServices customerService , int CompanyId)
         {
            if (customerService != null)
            {
                //Cheaking Dropdown Value with Db 
                Company comp = Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                CustomerServices objCustomerService = new CustomerServices();
                PWarehouseLink pwh = PWarehouseLinks.Where(x => x.PWarehouseId == customerService.PWarehouseId  && x.Company.Id == CompanyId).FirstOrDefault();
                PItemLink pItemlk = PItemLinks.Where(x => x.PItemId == customerService.PItemId && x.Deleted == false).Include("Subcategory").FirstOrDefault();
                CustomerLink customerlk = CustomerLinks.Where(x => x.CustomerLinkId == customerService.CustomerLinkId && x.Deleted == false).FirstOrDefault();
                //UnitMaster unit = UnitMaters.Where(x => x.UnitID == customerService.Unit.UnitID && x.Deleted == false).FirstOrDefault();
                People provider = Peoples.Where(x => x.Company.Id == CompanyId /*customerService.ProviderId*/ && x.Deleted == false).FirstOrDefault();
                PSubCategoryLink pSubCatelk = PSubCategoryLinks.Where(x => x.SubCategoryes.SubCategoryId == pItemlk.Subcategory.SubCategoryId && x.Deleted == false).Include("Category").FirstOrDefault();
                PCategoryLink pCatelk = PCategoryLinks.Where(x => x.Categoryes.Categoryid == pSubCatelk.Category.Categoryid && x.Deleted == false).Include("Basecategory").FirstOrDefault();
                PBaseCategoryLink pBaselk = PBaseCategoryLinks.Where(x => x.BaseCategorys.BaseCategoryId == pCatelk.Basecategory.BaseCategoryId && x.Deleted == false).FirstOrDefault();
                if (customerService.ShiftId != 0)
                {
                    shft = ShiftMasters.Where(x => x.ShiftID == customerService.ShiftId && x.Deleted == false && x.IsActive == true).FirstOrDefault();
                }
                try
                {
                    //saving Data to customerServices table
                    customerService.Provider = provider;
                    customerService.PWarehouse = pwh;
                    customerService.PCategory = pCatelk;
                    customerService.CustomerLink = customerlk;
                    customerService.PItem = pItemlk;
                    customerService.Company = comp;
                    customerService.PSubcategory = pSubCatelk;
                    customerService.PBaseCategory = pBaselk;
                    if (customerService.ShiftId != 0)
                    {
                        customerService.Shift = shft;
                        customerService.ServiceName = /*customer.Skcode + "-" + service.BaseCategoryName + "-" +*/ shft.ShiftName;
                    }
                    else
                    {
                        customerService.ServiceName =/* customer.Skcode + "-" + service.BaseCategoryName + "-" +*/ "One";
                    }
                    customerService.StartDate = customerService.StartDate;
                    customerService.Price = customerService.Price;
                    customerService.BillingType = "Monthly"; /*customerService.BillingType;*/
                    customerService.Quantity = customerService.Quantity;
                    customerService.CreatedDate = indianTime;
                    customerService.Deleted = false;
                    customerService.IsActive = true;
                    customerService.DeliveryDays = customerService.DeliveryDays;
                    customerService.DeliveryTime = customerService.DeliveryTime;
                    customerService.LastBillingDate = indianTime;
                    this.CustomerServicess.Add(customerService);
                    //this.Entry(customerService).State = EntityState.Added;
                    customerService.Message = "Sucessfully";
                    int id = this.SaveChanges();
                }
                catch (Exception)
                {
                    customerService.Message = "Error";
                    return customerService;
                }
            }
            return customerService;
        }

        //Get CustomerService For Update
        public CustomerServices PutCustomerServices(CustomerServices customerservicesls, int CompanyId)
        {
            var customerService = CustomerServicess.Where(x => x.ID == customerservicesls.ID).FirstOrDefault();
            if (customerService != null)
            {
                Company comp = Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                CustomerServices objCustomerService = new CustomerServices();
                PWarehouseLink pwh = PWarehouseLinks.Where(x => x.PWarehouseId == customerservicesls.PWarehouseId && x.Deleted == false).FirstOrDefault();
                PItemLink pItemlk = PItemLinks.Where(x => x.PItemId == customerservicesls.PItemId && x.Deleted == false).Include("Subcategory").FirstOrDefault();
                CustomerLink customerlk = CustomerLinks.Where(x => x.CustomerLinkId == customerservicesls.CustomerLinkId && x.Deleted == false).FirstOrDefault();
                People provider = Peoples.Where(x => x.Company.Id == CompanyId /*customerService.ProviderId*/ && x.Deleted == false).FirstOrDefault();
                PSubCategoryLink pSubCatelk = PSubCategoryLinks.Where(x => x.SubCategoryes.SubCategoryId == pItemlk.Subcategory.SubCategoryId && x.Deleted == false).Include("Category").FirstOrDefault();
                PCategoryLink pCatelk = PCategoryLinks.Where(x => x.Categoryes.Categoryid == pSubCatelk.Category.Categoryid && x.Deleted == false).Include("Basecategory").FirstOrDefault();
                PBaseCategoryLink pBaselk = PBaseCategoryLinks.Where(x => x.BaseCategorys.BaseCategoryId == pCatelk.Basecategory.BaseCategoryId && x.Deleted == false).FirstOrDefault();
                if (customerservicesls.ShiftId != 0)
                {
                    shft = ShiftMasters.Where(x => x.ShiftID == customerservicesls.ShiftId && x.Deleted == false && x.IsActive == true).FirstOrDefault();
                }
                try
                {
                    //saving Data to customerServices table
                    customerService.Provider = provider;
                    customerService.PWarehouse = pwh;
                    customerService.PCategory = pCatelk;
                    customerService.CustomerLink = customerlk;
                    customerService.PItem = pItemlk;
                    customerService.Company = comp;
                    customerService.PSubcategory = pSubCatelk;
                    customerService.PBaseCategory = pBaselk;
                    if (customerservicesls.ShiftId != 0)
                    {
                        customerService.Shift = shft;
                        customerService.ServiceName = /*customer.Skcode + "-" + service.BaseCategoryName + "-" +*/ shft.ShiftName;
                    }
                    else
                    {
                        customerService.ServiceName =/* customer.Skcode + "-" + service.BaseCategoryName + "-" +*/ "One";
                    }
                    customerService.StartDate = customerservicesls.StartDate;
                    customerService.Price = customerservicesls.Price;
                    customerService.BillingType = "Monthly"; /*customerService.BillingType;*/
                    customerService.Quantity = customerservicesls.Quantity;
                    customerService.CreatedDate = indianTime;
                    customerService.Deleted = false;
                    customerService.IsActive = true;
                    customerService.DeliveryDays = customerservicesls.DeliveryDays;
                    customerService.DeliveryTime = customerservicesls.DeliveryTime;
                    customerService.LastBillingDate = indianTime;
                    this.Entry(customerService).State = EntityState.Modified;
                    customerService.Message = "Sucessfully";
                    this.SaveChanges();
                }
                catch (Exception)
                {
                    customerService.Message = "Error";
                    return customerService;
                }
            }
            return customerService;
        }

        ////Get Customer Additional Service For Update
        //public CustomerAdditionalService PutCustomeraddServices(CustomerAdditionalService customeraddservices)
        //{
        //    CustomerAdditionalService cust = CustomerAdditionalServicess.Where(x => x.custAddServID == customeraddservices.custAddServID).FirstOrDefault();
        //    try
        //    {
        //        if (cust != null)
        //        {
        //            CustomerAdditionalService objCustomeraddService = new CustomerAdditionalService();
        //            Warehouse wh = Warehouses.Where(x => x.WarehouseId == customeraddservices.WareHouse.WarehouseId && x.Deleted == false).SingleOrDefault();
        //            CustomerServices custService = CustomerServicess.Where(x => x.ID == customeraddservices.Service.ID && x.Deleted == false).SingleOrDefault();
        //            Customer customer = Customers.Where(x => x.CustomerId == customeraddservices.Customer.CustomerId && x.Deleted == false).SingleOrDefault();
        //            //UnitMaster unitm = UnitMaters.Where(x => x.UnitID== customeraddservices.Unit && x.Deleted == false).SingleOrDefault();
        //            People provider = Peoples.Where(x => x.PeopleID == customeraddservices.Provider.PeopleID && x.Deleted == false).SingleOrDefault();

        //            cust.CompanyId = customeraddservices.CompanyId;
        //            cust.Service = custService;
        //            cust.Customer = customer;
        //            cust.Provider = provider;
        //            cust.WareHouse = wh;
        //            cust.UpdateDate = indianTime;
        //            cust.UpdatedBy = customeraddservices.UpdatedBy;
        //            cust.Quantity = customeraddservices.Quantity;
        //            cust.IsLess = customeraddservices.IsLess;
        //            if (cust.IsLess == true)
        //            {
        //                cust.Quantity = -cust.Quantity;
        //            }
        //            cust.Deleted = false;
        //            cust.IsActive = true;
        //            this.Entry(cust).State = EntityState.Modified;
        //            this.SaveChanges();
        //        }
        //        else
        //        {
        //            cust.Message = "Error";
        //            return null;
        //        }

        //    }
        //    catch (Exception )
        //    {
        //        return null;
        //    }
        //    cust.Message = "Sucessfully";
        //    return cust;
        //}

        ////Add Customer Additional Service To CustomerAdditionalServices Main table
        //public CustomerAdditionalService CustomerAddServices(CustomerAdditionalService customeradditionalService, int usreid)
        //{
        //    //Cheaking Dropdown Value with Db 
        //    CustomerAdditionalService objCustomeraddService = new CustomerAdditionalService();
        //    Warehouse wh = Warehouses.Where(x => x.WarehouseId == customeradditionalService.WareHouse.WarehouseId && x.Deleted == false).SingleOrDefault();
        //    CustomerServices custSrevice = CustomerServicess.Where(x => x.ID == customeradditionalService.Service.ID && x.Deleted == false).SingleOrDefault();
        //    Customer customer = Customers.Where(x => x.CustomerId == customeradditionalService.Customer.CustomerId && x.Deleted == false).SingleOrDefault();
        //    People provider = Peoples.Where(x => x.PeopleID == customeradditionalService.Provider.PeopleID && x.Deleted == false).SingleOrDefault();
        //    try
        //    {
        //        //saving Data to customerServices table
        //        customeradditionalService.CompanyId = customeradditionalService.CompanyId;
        //        customeradditionalService.Service = custSrevice;
        //        customeradditionalService.Customer = customer;
        //        customeradditionalService.Provider = provider;
        //        customeradditionalService.WareHouse = wh;
        //        customeradditionalService.CreateDate = indianTime;
        //        customeradditionalService.UpdateDate = indianTime;
        //        customeradditionalService.CreatedBy = usreid.ToString();
        //        customeradditionalService.Deleted = false;
        //        customeradditionalService.IsActive = true;
        //        customeradditionalService.IsLess = customeradditionalService.IsLess;
        //        if(customeradditionalService.IsLess==true)
        //        {
        //            customeradditionalService.Quantity =- customeradditionalService.Quantity;
        //        }
        //        CustomerAdditionalServicess.Add(customeradditionalService);
        //        int id = this.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        customeradditionalService.Message = "Error";
        //        return customeradditionalService;
        //    }
        //    customeradditionalService.Message = "Sucessfully";
        //    return customeradditionalService;
        //}

        //Add MonthLeave 
       
        public IEnumerable<Role> AllRoles(int compid)
        {
            if (UserRole.AsEnumerable().Count() > 0)
            {
                return UserRole.Where(p => p.CompanyId == compid).AsEnumerable();
            }
            else
            {
                List<Role> role = new List<Role>();
                return role.AsEnumerable();
            }
        }
        public Role AddRole(Role role)
        {
            List<Role> roles = UserRole.Where(c => c.RoleTitle.Trim().Equals(role.RoleTitle.Trim())).ToList();
            Role objrole = new Role();
            if (roles.Count == 0)
            {
                role.CreatedBy = role.CreatedBy;
                role.CreatedDate = indianTime;
                role.UpdatedDate = indianTime;
                UserRole.Add(role);
                int id = this.SaveChanges();
                return role;
            }
            else
            {
                return objrole;
            }
        }
        public Role PutRoles(Role objrole)
        {

            Role roles = UserRole.Where(x => x.RoleId == objrole.RoleId).FirstOrDefault();
            if (roles != null)
            {
                roles.UpdatedDate = indianTime;
                roles.RoleTitle = objrole.RoleTitle;
                roles.CreatedBy = objrole.CreatedBy;
                roles.CreatedDate = objrole.CreatedDate;
                roles.UpdateBy = objrole.UpdateBy;

                UserRole.Attach(roles);
                this.Entry(roles).State = EntityState.Modified;
                this.SaveChanges();
                return objrole;
            }
            else
            {
                return objrole;
            }
        }
        public bool DeleteRole(int id)
        {
            try
            {
                Role DL = new Role();
                DL.RoleId = id;
                Entry(DL).State = EntityState.Deleted;

                SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        public People AddPeople(People people)
        {
            List<People> peoples = Peoples.Where(c => c.Email.Trim().Equals(people.Email.Trim()) && c.Deleted == false && c.Company.Id == people.Company.Id).ToList();

            People objPeople = new People();
            if (peoples.Count == 0)
            {
                people.DisplayName = people.PeopleFirstName + " " + people.PeopleLastName;

                people.CreatedBy = people.CreatedBy;
                people.CreatedDate = indianTime;
                people.UpdatedDate = indianTime;
                Peoples.Add(people);
                int id = this.SaveChanges();
                return people;
            }
            else
            {
                return objPeople;
            }
        }
        public People PutPeople(People objCust)
        {
            People proj = Peoples.Where(x => x.PeopleID == objCust.PeopleID && x.Deleted == false && x.CompanyId == objCust.CompanyId).FirstOrDefault();
            if (proj != null)
            {
                proj.UpdatedDate = indianTime;
                proj.PeopleFirstName = objCust.PeopleFirstName;
                proj.PeopleLastName = objCust.PeopleLastName;
                proj.Email = objCust.Email;
                proj.Mobile = objCust.Mobile;
                //proj.Department = objCust.Department;
                //proj.BillableRate = objCust.BillableRate;
                //proj.CostRate = objCust.CostRate;
                //proj.ImageUrl = objCust.ImageUrl;
                //proj.Skcode = objCust.Skcode;
                proj.Permissions = objCust.Permissions;
                proj.DisplayName = objCust.PeopleFirstName + " " + objCust.PeopleLastName;
                proj.CreatedBy = objCust.CreatedBy;
                proj.CreatedDate = objCust.CreatedDate;
                proj.UpdateBy = objCust.UpdateBy;
                //proj.PWarehouseId = objCust.PWarehouseId;
                Peoples.Attach(proj);
                this.Entry(proj).State = EntityState.Modified;
                this.SaveChanges();
                return objCust;
            }
            else
            {
                return objCust;
            }
        }
       
      
        public IEnumerable<Company> AllCompanies
        {
            get { return Companies.AsEnumerable(); }
        }

        public object Mapping { get; internal set; }
        public object MilkYield { get; internal set; }

        public Company AddCompany(Company company)
        {
            //List<Company> cmp = Companies.Where(c => c.Name.Trim().Equals(company.Name.Trim())).ToList();
            //if (cmp.Count == 0)
            //{
                company.CreatedBy = "System";
                company.CreatedDate = indianTime;
                company.UpdatedDate = indianTime;
                Companies.Add(company);
                int id = this.SaveChanges();
                return company;
            //}
            //else
            //{
            //    return cmp[0];
            //}
        }
        
        public bool DeleteCompany(int id)
        {
            throw new NotImplementedException();
        }
        public bool CompanyExists(string companyName)
        {
            List<Company> cmp = Companies.Where(c => c.Name.Trim().Equals(companyName.Trim())).ToList();
            Company objCompany = new Company();
            if (cmp.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    
     
      
   
       
      
        public People getPersonIdfromEmail(string email)
        {
            People ps = new People();
            ps = Peoples.Where(x => x.Deleted == false).Where(p => p.Email.Trim().Equals(email.Trim())).Include("Company").Include("Warehouse").FirstOrDefault();
            int id = 0;
            if (ps != null)
            {
                id = ps.PeopleID;
            }
            return ps;
        }

        public People getPersonIdfrommobile(string mobile)
        {
            People ps = new People();
            ps = Peoples.Where(x => x.Deleted == false).Where(p => p.Mobile.Trim().Equals(mobile.Trim())).Include("Company").Include("Warehouse").FirstOrDefault();
            if (ps.Email == null)
            {
                ps.Email = "";
            }
            int id = 0;
            if (ps != null)
            {
                id = ps.PeopleID;
            }
            return ps;
        }

        public List<People> GetPeoplebyCompanyId(int id)
        {
            List<People> p = this.Peoples.Where(x => x.Deleted == false && x.CompanyId == id).ToList();
            if (p != null)
            {
            }
            else
            {
                p = new List<People>();
            }
            return p;
        }
        public People GetPeoplebyId(int compid, string mobile)
        {
            People p = Peoples.Where(x => x.Deleted == false && x.Company.Id == compid && x.Mobile == mobile).FirstOrDefault();
            if (p != null)
            {
            }
            else
            {
                p = new People();
            }
            return p;
        }
       
      
        public BaseCategory AddBaseCategory(BaseCategory basecat)
        {
            List<BaseCategory> bcat = BaseCategoryDb.Where(c => c.Deleted == false && c.BaseCategoryName.Trim().Equals(basecat.BaseCategoryName.Trim())).ToList();
            BaseCategory objcat = new BaseCategory();
            if (bcat.Count == 0)
            {
                basecat.CreatedDate = indianTime;
                basecat.UpdatedDate = indianTime;
                basecat.IsActive = true;
                basecat.Deleted = false;
                BaseCategoryDb.Add(basecat);
                this.SaveChanges();
                basecat.LogoUrl = "http://137.59.52.130/../../images/basecatimages/" + basecat.BaseCategoryId + ".jpg";
                BaseCategoryDb.Attach(basecat);
                Entry(basecat).State = EntityState.Modified;
                int id = this.SaveChanges();
                return basecat;
            }
            else
            {
                return objcat;
            }
        }
        
  
        public string skcode()
        {
            var customer = Customers.OrderByDescending(e => e.Skcode).FirstOrDefault();
            var skcode = "";
            if (customer != null)
            {
                int i = 1;
                bool flag = false;
                while (flag == false)
                {
                    if (customer.Skcode != null || customer.Skcode != "")
                    {
                        var skList = customer.Skcode.Split('N');
                        var skint = Convert.ToInt32(skList[1]) + i;
                        skcode = (skList[0] + "N" + Convert.ToString(skint)).Trim();
                        List<Customer> check = Customers.Where(s => s.Skcode.Trim().ToLower() == skcode.Trim().ToLower()).ToList();
                        if (check.Count == 0)
                        {
                            flag = true;
                            return skcode;
                        }
                        else
                        {
                            i = i + 1;
                        }
                    }
                }
            }
            else
            {
                skcode = "DN1000";
            }

            return skcode;
        }

        public List<SubsubCategory> subsubcategorybyWarehouse(int id, int compid)
        {
            List<SubsubCategory> cat = SubsubCategorys.Where(w => w.Deleted == false && w.IsActive == true).ToList();
            return cat;
        }

        //Add Wallet 
        public CustomerWallet AddWallet(CustomerWallet custWallet, float prvAmt)
        {
            var cust = CustomerWallets.Where(x => x.CustomerLink.CustomerLinkId == custWallet.CustomerLink.CustomerLinkId).FirstOrDefault();
            try
            {
                if (cust != null)
                {
                    cust.AvailableBalance = prvAmt;
                    this.Entry(cust).State = EntityState.Modified;
                    this.SaveChanges();
                }
                else
                {
                    this.CustomerWallets.Add(custWallet);
                    this.SaveChanges();
                }

            }
            catch (Exception)
            {
                return null;
            }
            //cust.Message = "Sucessfully";
            return cust;
        }
    }
}
