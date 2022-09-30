using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/UserAccessPermission")]
    public class UserAccessPermissionController : ApiController
    {
        DailyNeedContext dc = new DailyNeedContext();
        [Route("")]
        [HttpGet]
        public List<UserAccessPermission>  Get()
        {
            var data = dc.UserAccessPermissionDB.ToList();

            return data;
        }
        [Route("")]
        [HttpGet]
        public object Get(string rollid)
        {
            var data = dc.UserAccessPermissionDB.Where(z => z.RoleId == rollid).SingleOrDefault();
            
            return data;
        }
        [Route("")]
        [HttpPost]
        public HttpResponseMessage CreateLead(UserAccessPermission name)
        {
            var us = dc.UserAccessPermissionDB.Where(t => t.RoleId == name.RoleId).SingleOrDefault();
            us.Admin = name.Admin;
            us.Delivery = name.Delivery;
            us.TaxMaster = name.TaxMaster;
            us.Customer = name.Customer;
            us.Supplier = name.Supplier;
            us.Warehouse = name.Warehouse;
            us.CurrentStock = name.CurrentStock;
            us.OrderMaster = name.OrderMaster;
            us.DamageStock = name.DamageStock;
            us.ItemMaster = name.ItemMaster;
            us.Reports = name.Reports;
            us.StatisticalRep = name.StatisticalRep;
            us.Offer = name.Offer;
            us.Sales = name.Sales;
            us.AppPromotion = name.AppPromotion;
            us.ItemCategory = name.ItemCategory;
            us.CRM = name.CRM;
            us.Request = name.Request;
            us.UnitEconomics = name.UnitEconomics;
            us.PromoPoint = name.PromoPoint;
            us.News = name.News;            
            dc.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }
    }   
}
