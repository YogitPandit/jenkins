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
using System.Web;
using System.IO;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/CategoryImage")]
    public class CategoryImageController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        CategoryAPIMethod CMethode = new CategoryAPIMethod();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                ass = CMethode.AllCategory(compid).ToList();
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


        //[Authorize]
        [Route("GetCategoryImage")]
        public IEnumerable<CategoryImageData> GetCategoryImage()
        {
            logger.Info("start Category: ");
            List<CategoryImageData> ass = new List<CategoryImageData>();
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
                ass = context.AllCategoryImages().ToList();
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

        [ResponseType(typeof(CategoryImage))]
        [Route("")]
        [AcceptVerbs("POST")]
        public int add(CategoryImage item)
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
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                int result=context.AddCategoryImage(item);
                UploadFile();
                logger.Info("End  AddCategoryImage: ");
                if(result>0)
                {
                    return result;
                }
                else
                {
                    return 0;
                }
          
            }
            catch (Exception ex)
            {
                logger.Error("Error in addCategory " + ex.Message);
                logger.Info("End  addCategory: ");
                return 0;
            }
        }

        [ResponseType(typeof(CategoryImage))]
        [Route("")]
        [AcceptVerbs("PUT")]
        public int Put(CategoryImage item)
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
                int result= context.PutCategoryImage(item);
                return result;
            }
            catch
            {
                return 0;
            }
        }

        [ResponseType(typeof(CategoryImage))]
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

                CategoryImage categoryimage = db.CategoryImageDB.Where(x => x.CategoryImageId == id && x.Deleted == false).FirstOrDefault();
                categoryimage.Deleted = true;
                categoryimage.IsActive = false;
                db.CategoryImageDB.Attach(categoryimage);
                db.Entry(categoryimage).State = EntityState.Modified;
                db.SaveChanges();

                


                logger.Info("End  delete Category: ");
            }
            catch (Exception ex)
            {
                logger.Error("Error in del Category " + ex.Message);
            }
        }

        [Route("GetCategoryImageByCId")]
        public dynamic GetCategoryImageByCId(int CategoryId)
        {
            try
            {
                List<CategoryImage> ass = new List<CategoryImage>();
                ass = db.CategoryImageDB.Where(c => c.CategoryId == CategoryId && c.Deleted == false && c.IsActive == true).ToList();
                if(ass!=null)
                {
                    return ass;
                }
                else
                {
                    var obj = new
                    {
                    };
                    return obj;
                }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public void UploadFile()
        {
            logger.Info("start image upload");
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

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files["file"];

                    if (httpPostedFile != null)
                    {
                        // Validate the uploaded image(optional)

                        // Get the complete file path
                        var ImageUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedImages"), httpPostedFile.FileName);
                        //var ImageUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);
                        //string physicalPath = ("~/images/" + ImageName);
                        //httpPostedFile.SaveAs(physicalPath);
                        //customer newRecord = new customer();
                        //newRecord.username = customer.username;
                        ////.......saving picture url......
                        //newRecord.picture = physicalPath;

                        //// Save the uploaded file to "UploadedFiles" folder
                        logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                        httpPostedFile.SaveAs(ImageUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in  image upload " + ex.Message);
                logger.Info("End  image upload: ");

            }
        }
    }

    public class CategoryImageData
    {
        public int CategoryId { get; set; }
        public int CategoryImageId { get; set; }
        public string CategoryName { get; set; }

        public string CategoryImg { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}



