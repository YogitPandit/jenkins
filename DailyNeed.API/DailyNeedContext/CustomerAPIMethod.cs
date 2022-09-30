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
using System.Globalization;

namespace DailyNeed.API.Controllers
{
    public class CustomerAPIMethod
    {
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        CultureInfo ci = new CultureInfo("en-US");
        //Get Customer By Mobile Number
        public Customer GetCustomerbyId(string Mobile)
        {
            Customer customer = db.Customers.Where(c => c.Mobile == Mobile).SingleOrDefault();
            if (customer != null)
            {
                return customer;
            }
            else
            {
                customer = new Customer();
            }
            return customer;
        }

        public Customer AddCustomer(Customer customer)
        {
            //Geting DNCODE(Required) for saving Data Uniqu ID
            customer.DNCode = this.dncode();
            if (customer.Password == null)
            {
                customer.Password = "123456";
            }
            //Geting Customer By UI enter Mobile Number from Customer Main table
            List<Customer> customers = db.Customers.Where(c => c.Mobile.Trim().Equals(customer.Mobile.Trim()) || c.Skcode == customer.Skcode).ToList();
            Customer objCustomer = new Customer();
            PWarehouseLink wh = db.PWarehouseLinks.Where(x => x.PWarehouseId == customer.PWarehouseId && x.Deleted == false).FirstOrDefault();
            if (customers.Count == 0)
            {
                customer.Address = customer.Address;
                customer.CreatedDate = indianTime;
                customer.UpdatedDate = indianTime;
                customer.BillingName = customer.BillingName;
                customer.LandMard = customer.LandMard;
                customer.Mobile1 = customer.Mobile1;
                customer.Mobile2 = customer.Mobile2;
                customer.Mobile3 = customer.Mobile3;
                customer.PhotoUrl = customer.PhotoUrl;
                customer.GPSLocation = customer.GPSLocation;
                customer.PWarehouse = wh;
                customer.Active = true;
                customer.Deleted = false;
                customer.Password = customer.Password;
                customer.Name = customer.Name;
                customer.EmailId = customer.EmailId;
                db.Customers.Attach(customer);
                db.Entry(customer).State = EntityState.Added;
                int id = db.SaveChanges();
                return customer;
            }
            else
            {
                objCustomer.Message = "Already";
                return objCustomer;
            }
        }

        public Customer AddCustomerBulk(Customer customer)
        {
            //Geting DNCODE(Required) for saving Data Uniqu ID
            customer.DNCode = this.dncode();
            //customer.DNCode = this.dncode();
            if (customer.Password == null)
            {
                customer.Password = "123456";
            }
            //Geting Customer By UI enter Mobile Number from Customer Main table
            List<Customer> customers = db.Customers.Where(c => c.Mobile.Trim().Equals(customer.Mobile.Trim()) || c.DNCode == customer.DNCode).ToList();
            Warehouse wh = db.Warehouses.Where(x => x.WarehouseId == customer.WarehouseId && x.Deleted == false).FirstOrDefault();
            Customer custs = new Customer();
            if (customers.Count == 0)
            {
                custs.Address = customer.Address;
                custs.CreatedDate = indianTime;
                custs.UpdatedDate = indianTime;
                custs.Mobile = customer.Mobile;
                custs.BillingName = customer.BillingName;
                custs.LandMard = customer.LandMard;
                custs.Mobile1 = customer.Mobile1;
                custs.Mobile2 = customer.Mobile2;
                custs.Mobile3 = customer.Mobile3;
                custs.PhotoUrl = customer.PhotoUrl;
                custs.GPSLocation = customer.GPSLocation;
                custs.DNCode = customer.DNCode;
                custs.Warehouse = wh;
                custs.Active = true;
                custs.Deleted = false;
                custs.Password = customer.Password;
                custs.Name = customer.Name;
                custs.EmailId = customer.EmailId;
                db.Customers.Add(custs);
                int id = db.SaveChanges();
                return custs;
            }
            else
            {
                custs.Message = "Already";
                return custs;
            }
        }

        // add customer services in bulk customer
        public Customer AddCustomerServicesBulk(Customer customerService, int CompanyId)
        {
            if (customerService != null)
            {
                //Cheaking Dropdown Value with Db 
                Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                People provider = db.Peoples.Where(x => x.Company.Id == CompanyId && x.Deleted == false).FirstOrDefault();

                Warehouse wh = db.Warehouses.Where(x => x.WarehouseId == customerService.WarehouseId && x.Company.Id == CompanyId).FirstOrDefault();
                PItemLink pItemlk = db.PItemLinks.Where(x => x.PItemId == customerService.PItemId && x.Deleted == false).Include("Subcategory").FirstOrDefault();
                CustomerLink customerlk = db.CustomerLinks.Where(x => x.CustomerLinkId == customerService.CustomerId && x.Deleted == false).FirstOrDefault();
                PSubCategoryLink pSubCatelk = db.PSubCategoryLinks.Where(x => x.SubCategoryes.SubCategoryId == pItemlk.Subcategory.SubCategoryId && x.Deleted == false).Include("Category").FirstOrDefault();
                PCategoryLink pCatelk = db.PCategoryLinks.Where(x => x.Categoryes.Categoryid == pSubCatelk.Category.Categoryid && x.Deleted == false).Include("Basecategory").FirstOrDefault();
                PBaseCategoryLink pBaselk = db.PBaseCategoryLinks.Where(x => x.BaseCategorys.BaseCategoryId == pCatelk.Basecategory.BaseCategoryId && x.Deleted == false).FirstOrDefault();
                ShiftMaster shft = null;
                //create customer object
                CustomerServices cservices = new CustomerServices();
                //check shift name then get shift
                shft = db.ShiftMasters.Where(x => x.ShiftName == customerService.ShiftName && x.Deleted == false && x.IsActive == true).FirstOrDefault();
                
                try
                {
                    //saving Data to customerServices table
                    cservices.Warehouse = wh;
                    cservices.Provider = provider;
                    cservices.Shift = shft;
                    cservices.PCategory = pCatelk;
                    cservices.CustomerLink = customerlk;
                    cservices.PItem = pItemlk;
                    cservices.Company = comp;
                    cservices.PSubcategory = pSubCatelk;
                    cservices.PBaseCategory = pBaselk;
                    if (shft.ShiftID != 0)
                    {
                        cservices.Shift = shft;
                        cservices.ServiceName = shft.ShiftName;
                    }
                    else
                    {
                        cservices.ServiceName = "One";
                    }
                    cservices.CreatedBy = provider.PeopleID.ToString();
                    cservices.Status = "Active";
                    DateTime dt = DateTime.ParseExact(customerService.StartDate, "dd-MM-yyyy", ci);
                    cservices.StartDate = dt;
                    cservices.Price = customerService.Price;
                    cservices.BillingType = "Monthly"; 
                    cservices.Quantity = customerService.Quantity;
                    cservices.CreatedDate = indianTime;
                    cservices.Deleted = false;
                    cservices.IsActive = true;
                    cservices.DeliveryDays = customerService.DeliveryDays;
                    cservices.DeliveryTime = customerService.DeliveryTime;
                    cservices.LastBillingDate = indianTime;
                    db.CustomerServicess.Add(cservices);
                    //this.Entry(customerService).State = EntityState.Added;
                    cservices.Message = "Sucessfully";
                    int id = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    customerService.Message = "Error";
                }
            }
            return customerService;
        }
        public string skcode()
        {
            var customer = db.Customers.OrderByDescending(e => e.Skcode).FirstOrDefault();
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
                        List<Customer> check = db.Customers.Where(s => s.Skcode.Trim().ToLower() == skcode.Trim().ToLower()).ToList();
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

        public string dncode()
        {
            var customer = db.Customers.OrderByDescending(e => e.DNCode).FirstOrDefault();
            var dncode = "";
            if (customer != null)
            {
                int i = 1;
                bool flag = false;
                while (flag == false)
                {
                    if (customer.DNCode != null || customer.DNCode != "")
                    {
                        var skList = customer.DNCode.Split('N');
                        var skint = Convert.ToInt32(skList[1]) + i;
                        dncode = (skList[0] + "N" + Convert.ToString(skint)).Trim();
                        List<Customer> check = db.Customers.Where(s => s.DNCode.Trim().ToLower() == dncode.Trim().ToLower()).ToList();
                        if (check.Count == 0)
                        {
                            flag = true;
                            return dncode;
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
                dncode = "DN1000";
            }

            return dncode;
        }

        //Update Customer Put method
        public CustomerLink PutCustomerLink(CustomerLink customerl)
        {
            CustomerLink cust = db.CustomerLinks.Where(x => x.CustomerLinkId == customerl.CustomerLinkId).FirstOrDefault();
            PWarehouseLink wh = db.PWarehouseLinks.Where(x => x.PWarehouseId == customerl.PWarehouseId && x.Deleted == false).FirstOrDefault();

            if (cust != null)
            {
                cust.UpdatedDate = indianTime;
                cust.Address = customerl.Address;
                cust.Name = customerl.Name;
                cust.EmailId = customerl.EmailId;
                cust.BillingName = customerl.BillingName;
                cust.LandMard = customerl.LandMard;
                cust.Mobile1 = customerl.Mobile1;
                cust.Mobile2 = customerl.Mobile2;
                cust.Mobile3 = customerl.Mobile3;
                cust.PhotoUrl = customerl.PhotoUrl;
                cust.GPSLocation = customerl.GPSLocation;
                cust.PWarehouse = wh;
                cust.Active = true;
                cust.Deleted = false;
                cust.Message = "Sucessfully";
                db.CustomerLinks.Attach(cust);
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();
                return cust;
            }
            else
            {
                cust.Message = "Error";
                return cust;
            }
        }

        //Delete Customer
        public bool DeleteCustomer(int id)
        {
            try
            {
                CustomerLink cust = db.CustomerLinks.Where(x => x.CustomerLinkId == id && x.Deleted == false).FirstOrDefault();
                cust.Deleted = true;
                cust.Active = false;
                db.CustomerLinks.Attach(cust);
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}