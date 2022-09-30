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
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/BaseCategory")]
    public class BaseCategoryController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Get All Base Category By Claim Compid and wharehouseid
        //[Authorize]
        [Route("")]
        public IEnumerable<BaseCategory> Get()
        {
            logger.Info("start Category: ");
            List<BaseCategory> ass = new List<BaseCategory>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, warehouseid = 0;
                // Access claims
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
                        warehouseid = int.Parse(claim.Value);
                    }
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                ass = db.BaseCategoryDb.Where(x => x.Deleted == false).ToList();
                logger.Info("End  BaseCategory: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in BaseCategory " + ex.Message);
                logger.Info("End  BaseCategory: ");
                return null;
            }
        }

        //Export all basecategory table data in excel
        [Route("export")]
        [HttpGet]
        public dynamic export()
        {
            logger.Info("start BaseCategory: ");
            dynamic BaseCategory = null;
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

                BaseCategory = db.BaseCategoryDb.ToList();

                logger.Info("End  BaseCategory: ");
                return BaseCategory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in BaseCategory " + ex.Message);
                logger.Info("End  BaseCategory: ");
                return null;
            }
        }

        //Add Base category//POST Method
        [ResponseType(typeof(BaseCategory))]
        [Route("")]
        [AcceptVerbs("POST")]
        public BaseCategory add(BaseCategory bCategory)
        {
            logger.Info("start addBaseCategory: ");
            try
            {
                //Cheaking Claim Authentication
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, warehouseid = 0;
                {
                    // Access claims
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
                            warehouseid = int.Parse(claim.Value);
                        }
                    }

                    //If bCategory Object Null
                    if (bCategory == null)
                    {
                        throw new ArgumentNullException("Base Category");
                    }
                    if(bCategory.Code == "" || bCategory.Code == null)
                    {
                        bCategory.Code = code();
                    }
                    //Passing bCategory Object 
                    logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                    bCategory.CreatedBy = userid.ToString();                 
                    bCategory.CreatedDate = indianTime;
                    bCategory.IsActive = false;
                    bCategory.Deleted = false;
                    bCategory.Message = "Successfully";
                    //Saving object to Add Method in db
                    db.BaseCategoryDb.Add(bCategory);
                    int id = db.SaveChanges();
                    //DailyNeed.API.Helper.refreshCategory();
                    logger.Info("End  addBaseCategory: ");
                    return bCategory;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in addBaseCategory " + ex.Message);
                logger.Info("End  addBaseCategory: ");
                bCategory.Message = "Error";
                return bCategory;
            }
        }


        //Update Base Category / PUT Method
        [ResponseType(typeof(BaseCategory))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public BaseCategory Put(BaseCategory basecategory)
        {
            BaseCategory bcategory = null;
            try
            {
                //Cheaking Claim Authentication
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, warehouseid = 0;
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
                        warehouseid = int.Parse(claim.Value);
                    }
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);

                //Taking Base Category Data in veriable by BasecategoryId and compid
                 bcategory = db.BaseCategoryDb.Where(x => x.BaseCategoryId == basecategory.BaseCategoryId).FirstOrDefault();

                if (bcategory != null)
                {
                    bcategory.UpdatedDate = indianTime;
                    bcategory.UpdateBy = userid.ToString();
                    bcategory.LogoUrl = basecategory.LogoUrl;
                    bcategory.BaseCategoryName = basecategory.BaseCategoryName;
                    bcategory.Discription = basecategory.Discription;
                    bcategory.IsActive = basecategory.IsActive;
                    bcategory.Message = "Successfully";
                    db.BaseCategoryDb.Attach(bcategory);
                    db.Entry(bcategory).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in addBaseCategory " + ex.Message);
                logger.Info("End  addBaseCategory: ");
                bcategory.Message = "Error";
                return bcategory;
            }
            return bcategory;
        }


        //Delete Base Category / Delete Method 
        [ResponseType(typeof(BaseCategory))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start del BaseCategory: ");
            try
            {

                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, warehouseid = 0;
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
                        warehouseid = int.Parse(claim.Value);
                    }
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                BaseCategory category = db.BaseCategoryDb.Where(x => x.BaseCategoryId == id && x.Deleted == false).FirstOrDefault();
                category.Deleted = true;
                category.IsActive = false;
                category.UpdatedDate = indianTime;
                db.BaseCategoryDb.Attach(category);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                logger.Info("End  delete BaseCategory: ");
            }
            catch (Exception ex)
            {

                logger.Error("Error in del BaseCategory " + ex.Message);


            }
        }

        //Genrat Code for base category
        public string code()
        {
            var code = db.BaseCategoryDb.OrderByDescending(e => e.Code).FirstOrDefault();
            var bcode = "";
            if (code != null)
            {
                int i = 0;
                if (code.Code != null || code.Code != "")
                {
                    i = Convert.ToInt32(code.Code);
                    i = i + 1;
                    bcode = i.ToString();
                }
            }
            else
            {
                bcode = "1001";
            }

            return bcode;
        }
    }
}



