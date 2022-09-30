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


namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/SubsubCategory")]
    public class SubsubCategoryController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Get All Sub Sub Category
        [Authorize]
        [Route("")]
        public IEnumerable<SubsubCategory> Get()
        {
            logger.Info("start Subsubategory: ");
            List<SubsubCategory> ass = new List<SubsubCategory>();
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

                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                ass = context.AllSubsubCat(compid).ToList();
                logger.Info("End  Subsubategory: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Subsubategory " + ex.Message);
                logger.Info("End  Subsubategory: ");
                return null;
            }
        }

        //Export all subsubcategory table data in excel
        [Route("export")]
        [HttpGet]
        public dynamic export()
        {
            logger.Info("start SubSubCategory: ");
            dynamic SubSubCategory = null;
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

                SubSubCategory = db.SubsubCategorys.ToList();

                logger.Info("End  SubSubCategory: ");
                return SubSubCategory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in SubSubCategory " + ex.Message);
                logger.Info("End  SubSubCategory: ");
                return null;
            }
        }

        //skcode generate 
        [HttpGet]
        [Route("GenerateSubSubCode")]
        public dynamic GenerateSubSubCode()
        {
            logger.Info("start Subsubategory: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                DailyNeedContext db = new DailyNeedContext();
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

                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                var ssc = db.GenerateSubSubCode(compid);
                var atm = Int32.Parse(ssc);
                logger.Info("End  Subsubategory: ");
                return atm;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Subsubategory " + ex.Message);
                logger.Info("End  Subsubategory: ");
                return null;
            }
        }

        //Add Sub Sub category//POST Method
        [ResponseType(typeof(SubsubCategory))]
        [Route("")]
        [AcceptVerbs("POST")]
        public SubsubCategory add(SubsubCategory subsubcategory)
        {
            logger.Info("start Subsubategory: ");
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
                if (subsubcategory == null)
                {
                    throw new ArgumentNullException("subsubcategory");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                
                context.AddSubsubCat(subsubcategory);

                logger.Info("End  Subsubategory: ");
                return subsubcategory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Subsubategory " + ex.Message);
                logger.Info("End  Subsubategory: ");
                return null;
            }
        }

        //Update Sub Sub Category //PUT method
        [ResponseType(typeof(SubsubCategory))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public SubsubCategory Put(SubsubCategory subsubcategory)
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
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                context.PutSubsubCat(subsubcategory);
                return subsubcategory;
            }
            catch
            {
                return null;
            }
        }

        //Delete record by subsubcategory id
        [ResponseType(typeof(SubsubCategory))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start Subsubategory: ");
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
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                int CompanyId = compid;
                context.DeleteSubsubCat(id, CompanyId);
                logger.Info("End  delete Subsubategory: ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in Subsubategory " + ex.Message);
            }
        }
        
        //for Brand All 
        [Route("GetAllBrand")]
        public dynamic GetBrand()
        {
            logger.Info("start Subsubategory: ");
            List<SubsubCategory> ass = new List<SubsubCategory>();
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
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                List<string> BrandName = new List<string>();
                var SubBrand = db.SubsubCategorys.Where(p => p.Deleted == false && p.IsActive==true).ToList();
                
                if (SubBrand != null)
                {
                    foreach (var subdate in SubBrand) {

                        if (subdate != null && !BrandName.Any(x => x == subdate.SubsubcategoryName))
                        {
                            ass.Add(subdate);
                            BrandName.Add(subdate.SubsubcategoryName);
                        }                       
                    }               
                }
                logger.Info("End  Subsubategory: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Subsubategory " + ex.Message);
                logger.Info("End  Subsubategory: ");
                return null;
            }
        }


        [Route("GetSubSubCategoryById")]
        public IEnumerable<SubsubCategory> Get(int subcat)
        {
            logger.Info("start Subsubategory: ");
            List<SubsubCategory> ass = new List<SubsubCategory>();
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

                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                //ass = context.AllSubsubCat(compid).ToList();
                var SubSubcategory = db.SubsubCategorys.Where(x => x.Deleted == false && x.SubCategoryId == subcat).ToList();
                logger.Info("End  Subsubategory: ");
                return SubSubcategory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Subsubategory " + ex.Message);
                logger.Info("End  Subsubategory: ");
                return null;
            }
        }


    }

    public class SubSubCategoryData
    {
        public  IEnumerable<string> subsubcategoryname
        {
            get;set;
        }
    }
}
       




