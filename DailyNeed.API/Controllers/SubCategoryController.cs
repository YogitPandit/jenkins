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
    [RoutePrefix("api/SubCategory")]
    public class SubCategoryController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();

        //Get All Sub Category
        //[Authorize]
        [Route("")]
        public IEnumerable<SubCategory> Get()
        {
            logger.Info("start SubCategory: ");
            List<SubCategory> subcategories = new List<SubCategory>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
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
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                subcategories = context.AllSubCategory(compid).ToList();
                logger.Info("End  SubCategory: ");                
                return subcategories;
            }
            catch (Exception ex)
            {
                logger.Error("Error in SubCategory " + ex.Message);
                logger.Info("End  SubCategory: ");
                return null;
            }
        }

        //Get sub category by id
        [Route("GetSubcategoryById")]
        public IEnumerable<SubCategory> Get(int id)
        {
            logger.Info("start SubCategory: ");
            List<SubCategory> ass = new List<SubCategory>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
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
                }

                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                int CompanyId = compid;
                ass = context.AllSubCategoryy(id,compid).ToList();
                logger.Info("End  SubCategory: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in SubCategory " + ex.Message);
                logger.Info("End  SubCategory: ");
                return null;
            }
        }

        //Export all category table data in excel
        [Route("export")]
        [HttpGet]
        public dynamic export()
        {
            logger.Info("start City: ");
            dynamic SubCategory = null;
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

                SubCategory = db.SubCategorys.ToList();

                logger.Info("End  SubCategory: ");
                return SubCategory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in SubCategory " + ex.Message);
                logger.Info("End  SubCategory: ");
                return null;
            }
        }

       

        //Add Sub category//POST Method
        [ResponseType(typeof(SubCategory))]
        [Route("")]
        [AcceptVerbs("POST")]
        public SubCategory add(SubCategory subcategory)
        {
            logger.Info("start add SubCategory: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
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
                }
         
                if (subcategory == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                context.AddSubCategory(subcategory);
                logger.Info("End  add SubCategory: ");
                return subcategory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in add SubCategory " + ex.Message);
                logger.Info("End  add SubCategory: ");
                return null;
            }
        }

        //Update Sub Category //PUT method
        [ResponseType(typeof(SubCategory))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public SubCategory Put(SubCategory sbcategory)
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
                context.PutSubCategory(sbcategory, userid);
                return sbcategory;
            }
            catch
            {
                return null;
            }
        }

        //Delete record by subcategory id
        [ResponseType(typeof(SubCategory))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start delete SubCategory: ");
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
                context.DeleteSubCategory(id, CompanyId);
                logger.Info("End  delete  SubCategory: ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in delete SubCategory " + ex.Message);
            }
        }
    }
}



