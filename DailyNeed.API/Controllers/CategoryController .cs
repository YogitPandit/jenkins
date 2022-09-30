using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using System.Security.Claims;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        CategoryAPIMethod context = new CategoryAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Get All Category
        [Authorize]
        [Route("")]
        public IEnumerable<Category> Get()
        {
            logger.Info("start Category: ");
            List<Category> ass = new List<Category>();
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
                ass = context.AllCategory(compid).ToList();
                logger.Info("End  Category: ");
                return ass;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }

        //Get All Category On Basecategory Id
        [Route("GetByBasecategoryId")]
        public IEnumerable<Category> Get(int baseId)
        {
            logger.Info("start Category: ");
            List<Category> ass = new List<Category>();
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
                var category = db.Categorys.Where(x => x.Deleted == false && x.BaseCategory.BaseCategoryId == baseId).ToList();
                //ass = context.AllCategory(compid).ToList();
                logger.Info("End  Category: ");
                return category;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }


        //Export all category table data in excel
        [Route("export")]
        [HttpGet]
        public dynamic export()
        {
            logger.Info("start Category: ");
            dynamic Category = null;
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

                Category = db.Categorys.ToList();

                logger.Info("End  Category: ");
                return Category;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }

        // Add Category / POST MEthod
        [ResponseType(typeof(Category))]
        [Route("")]
        [AcceptVerbs("POST")]
        public Category add(Category ctegory)
        {
            logger.Info("start addCategory: ");
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
                if (ctegory == null)
                {
                    throw new ArgumentNullException("ctegory");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                if (ctegory.Code == null)
                {
                    ctegory.Code = code();
                }
                int CompanyId = compid;
                context.AddCategory(ctegory, CompanyId);
                logger.Info("End  addCategory: ");
                return ctegory;
            }
            catch (Exception ex)
            {
                logger.Error("Error in addCategory " + ex.Message);
                logger.Info("End  addCategory: ");
                return null;
            }
        }


        // Update Category / PUT MEthod
        [ResponseType(typeof(Category))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public Category Put(Category item)
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

                return context.PutCategory(item);
            }
            catch
            {
                return null;
            }
        }

        // Delete Category / Delete MEthod
        [ResponseType(typeof(Category))]
        [Route("")]
        [AcceptVerbs("Delete")]
        public void Remove(int id)
        {
            logger.Info("start del Category: ");
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

                Category category = db.Categorys.Where(x => x.Categoryid == id && x.Deleted == false).FirstOrDefault();
                category.Deleted = true;
                category.IsActive = false;
                db.Categorys.Attach(category);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                logger.Info("End  delete Category: ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in del Category " + ex.Message);
            }
        }

        //Genrating Code 
        public string code()
        {
            var code = db.Categorys.OrderByDescending(e => e.Code).FirstOrDefault();
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



