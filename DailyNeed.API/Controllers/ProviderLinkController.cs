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
    [RoutePrefix("api/ProviderLink")]
    public class ProviderLinkController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        DailyNeedContext db = new DailyNeedContext();
        CustomerLinkController CutomerLink = new CustomerLinkController();
        CustomerLink CustomerTable = new CustomerLink();
        public static Logger logger = LogManager.GetCurrentClassLogger();

        //Get All WareHouse By CompanyId 
        [Route("GetWareHouse")]
        public IEnumerable<PWarehouseLink> GetWarehouse()
        {
            logger.Info("start Warehouse: ");
            Warehouse Warehouse = new Warehouse();
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
                var warehouse = db.PWarehouseLinks.Where(x => x.Company.Id == CompanyId && x.Deleted == false && x.IsActive == true).Include("Warehouse").ToList();
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

        //Get All pWareHouse By CompanyId which have selected
        [Route("GetSelectedWarehouse")]
        public object GetSelectedWarehouse()
        {
            logger.Info("start Warehouse: ");
            PWarehouseLink pwlink = new PWarehouseLink();
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
                //var pwarehouselink = db.PWarehouseLinks.Where(c => c.Deleted == false && c.Company.Id == CompanyId).ToList();
                var pwarehouselink = (from e in db.PWarehouseLinks
                                      where e.Company.Id == CompanyId && e.Deleted == false
                                      select new
                                      {
                                          e.PWarehouseId,
                                          e.Warehouse.WarehouseName
                                      }).OrderByDescending(x => x.PWarehouseId).ToList();
                logger.Info("End  warehouse: ");
                return pwarehouselink;
            }
            catch (Exception ex)
            {
                logger.Error("Error in warehouse " + ex.Message);
                logger.Info("End  warehouse: ");
                return null;
            }
        }


        //Get All BaseCategory By CompanyId which have selected
        [Route("GetSelectedBCategory")]
        public object GetSelectedBCategory()
        {
            logger.Info("start Warehouse: ");
            PBaseCategoryLink pwlink = new PBaseCategoryLink();
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
                //var pwarehouselink = db.PWarehouseLinks.Where(c => c.Deleted == false && c.Company.Id == CompanyId).ToList();
                var PBaseCategoryLink = (from e in db.PBaseCategoryLinks
                                         where e.Company.Id == CompanyId && e.Deleted == false
                                         select new
                                         {
                                             e.PBaseCategoryId,
                                             e.BaseCategorys.BaseCategoryName
                                         }).OrderByDescending(x => x.PBaseCategoryId).ToList();
                logger.Info("End  warehouse: ");
                return PBaseCategoryLink;
            }
            catch (Exception ex)
            {
                logger.Error("Error in warehouse " + ex.Message);
                logger.Info("End  warehouse: ");
                return null;
            }
        }

        [ResponseType(typeof(PWarehouseLink))]
        [Route("addPWarehouse")]
        [AcceptVerbs("POST")]
        public List<PWarehouseLink> AddWarehouse(List<PWarehouseLink> pwlink)
        {
            logger.Info("start PWarehouseLink: ");
            List<PWarehouseLink> providerwlink = new List<PWarehouseLink>();
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
                int CompanyId = compid;
                Company comp = db.Companies.Where(x => x.Id == CompanyId && x.Deleted == false).FirstOrDefault();
                People provider = db.Peoples.Where(x => x.Company.Id == CompanyId && x.Deleted == false).FirstOrDefault();
                if (pwlink != null)
                {
                    foreach (var warehouse in pwlink)
                    {
                        PWarehouseLink PWhLink = db.PWarehouseLinks.Where(x => x.Warehouse.WarehouseId == warehouse.PWarehouseId && x.Company.Id == CompanyId && x.Provider.PeopleID == userid).FirstOrDefault();

                        if (PWhLink == null)
                        {
                            var wh = db.Warehouses.Where(x => x.WarehouseId == warehouse.PWarehouseId).ToList();
                            foreach (var pwhl in wh)
                            {
                                PWarehouseLink pwl = new PWarehouseLink();
                                pwl.CreatedBy = userid.ToString();
                                pwl.Warehouse = pwhl;
                                pwl.Provider = provider;
                                pwl.Company = comp;
                                pwl.IsActive = true;
                                pwl.Deleted = false;
                                db.PWarehouseLinks.Add(pwl);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                return providerwlink;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }


        //Get All Item list On the bases of Basecategory Id in PCategoryLink table
        [ResponseType(typeof(ItemMasterNew))]
        [Route("GetItemByBaseCateId")]
        [AcceptVerbs("POST")]
        public List<ItemMasterNew> Get(List<ItemMasterNew> bcategory)
        {
            logger.Info("start Category: ");
            List<ItemMasterNew> itemlist = new List<ItemMasterNew>();
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
                int CompanyId = compid;
                //Company comp = db.Companies.Where(x => x.Id == CompanyId && x.Deleted == false).FirstOrDefault();
                //People provider = db.Peoples.Where(x => x.PeopleID == CompanyId && x.Deleted == false).FirstOrDefault();

                if (bcategory != null)
                {
                    foreach (var ct in bcategory)
                    {
                        logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                        List<ItemMasterNew> items = db.ItemMasterNews.Where(x => x.Deleted == false && x.BaseCategory.BaseCategoryId == ct.id).ToList();
                        foreach (ItemMasterNew itemslst in items)
                        {
                            ItemMasterNew tm = new ItemMasterNew();
                            tm.ItemId = itemslst.ItemId;
                            tm.ItemName = itemslst.ItemName;
                            itemlist.Add(tm);
                        }
                    }
                }
                return itemlist;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }


        //Get All Sub Category On the bases of category and add category in PCategoryLink table
        [ResponseType(typeof(SubCategory))]
        [Route("GetSubCategory")]
        [AcceptVerbs("POST")]
        public List<SubCategory> Get(List<SubCategory> scategory)
        {
            logger.Info("start Category: ");
            List<SubCategory> subCtgryList = new List<SubCategory>();
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
                int CompanyId = compid;
                Company comp = db.Companies.Where(x => x.Id == CompanyId && x.Deleted == false).FirstOrDefault();
                People provider = db.Peoples.Where(x => x.PeopleID == CompanyId && x.Deleted == false).FirstOrDefault();
                if (scategory != null)
                {
                    foreach (var sct in scategory)
                    {
                        var p = db.Categorys.Where(x => x.Categoryid == sct.Categoryid).ToList();
                        foreach (var pcl in p)
                        {
                            PCategoryLink pclist = new PCategoryLink();
                            pclist.CreatedBy = userid.ToString();
                            pclist.Categoryes = pcl;
                            pclist.Provider = provider;
                            pclist.Company = comp;
                            pclist.IsActive = true;
                            pclist.Deleted = false;
                            db.PCategoryLinks.Add(pclist);
                            db.SaveChanges();
                        }
                        logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                        List<SubCategory> subcategory = db.SubCategorys.Where(x => x.Deleted == false && x.Categoryid == sct.Categoryid).ToList();
                        foreach (SubCategory sct1 in subcategory)
                        {
                            SubCategory scd = new SubCategory();
                            scd.SubCategoryId = sct1.SubCategoryId;
                            scd.SubcategoryName = sct1.SubcategoryName;
                            subCtgryList.Add(scd);
                        }
                    }
                }
                return subCtgryList;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }


    

        [ResponseType(typeof(PItemLink))]
        [Route("addPItems")]
        [AcceptVerbs("POST")]
        public void AddItems(List<ItemMasterNew> pilink)
        {
            logger.Info("start PWarehouseLink: ");
            List<PItemLink> providerilink = new List<PItemLink>();
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
                int CompanyId = compid;
                var WarehouseId = db.PWarehouseLinks.Where(x => x.Company.Id == CompanyId && x.Deleted == false).Select(x => x.Warehouse.WarehouseId).FirstOrDefault();
                List<ItemMasterNew> itemlst = new List<ItemMasterNew>();
                foreach (ItemMasterNew item in pilink)
                {
                    ItemMasterNew itemId = new ItemMasterNew();
                    itemId.ItemId = item.id;
                    itemlst.Add(itemId);
                }
                Providerlink(itemlst, WarehouseId);
            }
            catch (Exception)
            {
            }
        }

        //** Android Use **//
        //Link Warehouse,BaseCategory,Category,SubCategory,Item With Provider
        [Route("ProviderLink")]
        [AcceptVerbs("POST")]
        public List<ProviderLinks> Providerlink(List<ItemMasterNew> Itmlist, int whid)
        {
            List<ProviderLinks> providerLinks = new List<ProviderLinks>();
            logger.Info("start ProviderLink: ");

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
                int CompanyId = compid;
                Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                People user = db.Peoples.Where(x => x.PeopleID == userid).FirstOrDefault();
                Warehouse wh = db.Warehouses.Where(x => x.WarehouseId == whid).FirstOrDefault();

                if (Itmlist != null && CompanyId != 0 && userid != 0)
                {
                    foreach (var itm in Itmlist)
                    {
                        ProviderLinks pL = new ProviderLinks();
                        //Finding Item Allredy Subscribe by Provider Or Not
                        ItemMasterNew itms = db.ItemMasterNews.Where(x => x.ItemId == itm.ItemId).Include("BaseCategory").Include("Category").Include("SubCategory").FirstOrDefault();
                        
                        if (itms != null)
                        {
                            SubCategory subcategory = db.SubCategorys.Where(x => x.SubCategoryId == itms.SubCategory.SubCategoryId).FirstOrDefault();
                            Category category = db.Categorys.Where(x => x.Categoryid == itms.Category.Categoryid).FirstOrDefault();
                            BaseCategory baseCategory = db.BaseCategoryDb.Where(x => x.BaseCategoryId == itms.BaseCategory.BaseCategoryId).FirstOrDefault();
                            pL.bCategory = baseCategory;
                            pL.category = category;
                            pL.subCategory = subcategory;
                            pL.Items = itms;
                            if (baseCategory != null)
                            {
                                //Finding Basecategory Allredy Subscribe by Provider Or Not
                                PBaseCategoryLink pbaseCatLink = db.PBaseCategoryLinks.Where(x => x.BaseCategorys.BaseCategoryId == baseCategory.BaseCategoryId && x.Company.Id == CompanyId && x.Provider.PeopleID == userid).FirstOrDefault();

                                //If Provider Not Founded Then POST the Base Category
                                if (pbaseCatLink == null)
                                {
                                    PBaseCategoryLink pbasecatLink = new PBaseCategoryLink();
                                    pbasecatLink.CreatedBy = userid.ToString();
                                    pbasecatLink.BaseCategorys = baseCategory;
                                    pbasecatLink.Provider = user;
                                    pbasecatLink.Company = comp;
                                    pbasecatLink.Warehouse = wh;
                                    pbasecatLink.IsActive = true;
                                    pbasecatLink.Deleted = false;
                                    db.PBaseCategoryLinks.Add(pbasecatLink);
                                    db.SaveChanges();
                                }

                                //Finding Category Allredy Subscribe by Provider Or Not
                                PCategoryLink pcatLink = db.PCategoryLinks.Where(x => x.Categoryes.Categoryid == category.Categoryid && x.Company.Id == CompanyId && x.Provider.PeopleID == userid).FirstOrDefault();

                                //If Provider Not Founded Then POST the Category
                                if (pcatLink == null)
                                {
                                    PCategoryLink pcatoryLink = new PCategoryLink();
                                    pcatoryLink.CreatedBy = userid.ToString();
                                    pcatoryLink.Provider = user;
                                    pcatoryLink.Company = comp;
                                    pcatoryLink.Warehouse = wh;
                                    pcatoryLink.Basecategory = baseCategory;
                                    pcatoryLink.Categoryes = category;
                                    pcatoryLink.IsActive = true;
                                    pcatoryLink.Deleted = false;
                                    db.PCategoryLinks.Add(pcatoryLink);
                                    db.SaveChanges();
                                }

                                //Finding Sub Category Allredy Subscribe by Provider Or Not
                                PSubCategoryLink pSubCatLink = db.PSubCategoryLinks.Where(x => x.SubCategoryes.SubCategoryId == subcategory.SubCategoryId && x.Company.Id == CompanyId && x.Provider.PeopleID == userid).FirstOrDefault();

                                //If Provider Not Founded Then POST the Sub Category
                                if (pSubCatLink == null)
                                {
                                    PSubCategoryLink pSubCatoryLink = new PSubCategoryLink();
                                    pSubCatoryLink.CreatedBy = userid.ToString();
                                    pSubCatoryLink.Provider = user;
                                    pSubCatoryLink.Company = comp;
                                    pSubCatoryLink.Warehouse = wh;
                                    pSubCatoryLink.Basecategory = baseCategory;
                                    pSubCatoryLink.Category = category;
                                    pSubCatoryLink.SubCategoryes = subcategory;
                                    pSubCatoryLink.IsActive = true;
                                    pSubCatoryLink.Deleted = false;
                                    db.PSubCategoryLinks.Add(pSubCatoryLink);
                                    db.SaveChanges();
                                }

                                //Finding Item Allredy Subscribe by Provider Or Not
                                PItemLink pItmLink = db.PItemLinks.Where(x => x.ItemMasters.ItemId == itm.ItemId && x.Company.Id == CompanyId && x.Provider.PeopleID == userid).FirstOrDefault();

                                //If Provider Not Founded Then POST the Item
                                if (pItmLink == null)
                                {
                                    PItemLink pItmLinks = new PItemLink();
                                    pItmLinks.CreatedBy = userid.ToString();
                                    pItmLinks.Provider = user;
                                    pItmLinks.Company = comp;
                                    pItmLinks.Warehouse = wh;
                                    pItmLinks.Basecategory = baseCategory;
                                    pItmLinks.Category = category;
                                    pItmLinks.Subcategory = subcategory;
                                    pItmLinks.ItemMasters = itms;
                                    pItmLinks.IsActive = true;
                                    pItmLinks.Deleted = false;
                                    db.PItemLinks.Add(pItmLinks);
                                    db.SaveChanges();
                                }
                            }
                        }
                        providerLinks.Add(pL);
                    }
                }
                return providerLinks;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Category " + ex.Message);
                logger.Info("End  Category: ");
                return null;
            }
        }

        //** DTO OF Above POST **//
        //Reponse For Android In Object List Of Objects
        public class ProviderLinks
        {
            public BaseCategory bCategory { get; set; }
            public Category category { get; set; }
            public SubCategory subCategory { get; set; }
            public ItemMasterNew Items { get; set; }
        }

        //** End Android POST **//

        //** Android Use - Get Provider Link List
        //Get All Category,SubCategory,Item on basis of BaseCategory
        [Route("GetProviderlst")]
        public HttpResponseMessage GetProviderlst()
        {
            logger.Info("start GetAllProviderlst:");
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

                //Making Object Of Provider Link Base Category
                PBaseCategoryLink pBaseCategory = new PBaseCategoryLink();
                BaseCat basecat = new BaseCat();
                //Geting Provider Link Base Category form table by CompID
                pBaseCategory = db.PBaseCategoryLinks.Where(x => x.Company.Id == CompanyId).Include("BaseCategorys").FirstOrDefault();
                basecat.BaseCategoryId = pBaseCategory.BaseCategorys.BaseCategoryId;
                basecat.BaseCategoryName = pBaseCategory.BaseCategorys.BaseCategoryName;
                //Geting Provider Link Category from table by Provider Link Base Category ID in list 
                basecat.ProviderCategory = db.PCategoryLinks.Where(x => x.Basecategory.BaseCategoryId == pBaseCategory.BaseCategorys.BaseCategoryId && x.Company.Id == CompanyId).Include("Categoryes").Select(c => new Cate { Categoryid = c.Categoryes.Categoryid, CategoryName = c.Categoryes.CategoryName }).ToList();
                //Foreach for geting Provider Link subcategory list by one by one Provider Link category
                foreach (Cate provdrCat in basecat.ProviderCategory)
                {
                    provdrCat.ProviderSubCategory = db.PSubCategoryLinks.Where(x => x.Category.Categoryid == provdrCat.Categoryid && x.Company.Id == CompanyId).Include("SubCategoryes").Select(c => new SubCate { SubCategoryId = c.SubCategoryes.SubCategoryId, SubcategoryName = c.SubCategoryes.SubcategoryName }).ToList();

                    //Foreach for geting Provider Link Item list by one by one Provider Link Sucategory
                    foreach (SubCate psubcat in provdrCat.ProviderSubCategory)
                    {
                        psubcat.ProviderItem = db.PItemLinks.Where(x => x.Subcategory.SubCategoryId == psubcat.SubCategoryId && x.Company.Id == CompanyId).Include("ItemMasters").Select(c => new Itm {ItemId= c.PItemId , PItemId = c.ItemMasters.ItemId, itemname = c.ItemMasters.ItemName, LogoUrl = c.ItemMasters.LogoUrl ,}).ToList();
                    }
                }
                //Response Return Basecategory in Jason form
                return Request.CreateResponse(HttpStatusCode.OK, basecat);
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetAllProviderlst " + ex.Message);
                logger.Info("End  GetAllProviderlst: ");
                return null;
            }
        }

        //** DTO OF Above GET **//

        public class BaseCat
        {
            public int BaseCategoryId { get; set; }
            public string BaseCategoryName { get; set; }
            public ICollection<Cate> ProviderCategory { get; set; }
        }

        public class Cate
        {
            public int Categoryid { get; set; }
            public string CategoryName { get; set; }
            public ICollection<SubCate> ProviderSubCategory { get; set; }
        }

        public class SubCate
        {
            public int SubCategoryId { get; set; }
            public string SubcategoryName { get; set; }
            public ICollection<Itm> ProviderItem { get; set; }
        }

        public class Itm
        {
            public int ItemId { get; set; }
            public int PItemId { get; set; }
            public string itemname { get; set; }
            public string LogoUrl { get; set; }
        }

        //** End Android GET Provider Link List **//

        //**Android Use**//
        //Get All Category,SubCategory,Item on basis of BaseCategory
        [Route("GetAllCentralCatdb")]
        public HttpResponseMessage GetCentralCategory(int bCategoryId)
        {
            logger.Info("start GetAllByBaseCategory: ");
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

                //Making Object Of Base Category
                BaseCategory baseCategory = new BaseCategory();
                CentralBaseCatetegory _centralbasecategory = new CentralBaseCatetegory();

                //Geting Base Category form table by BaceCategoryID
                baseCategory = db.BaseCategoryDb.Where(x => x.BaseCategoryId == bCategoryId).FirstOrDefault();
                _centralbasecategory.BaseCategoryId = baseCategory.BaseCategoryId;
                _centralbasecategory.BaseCategoryName = baseCategory.BaseCategoryName;
                //Geting Category from table by Base Category ID in list 
                _centralbasecategory.CentralCategory = db.Categorys.Where(x => x.BaseCategory.BaseCategoryId == bCategoryId).Include("BaseCategory").Select(c => new CentralCategory { Categoryid = c.Categoryid, CategoryName = c.CategoryName }).ToList();

                //Foreach for geting subcategory list by one by one category
                foreach (CentralCategory cat1 in _centralbasecategory.CentralCategory)
                {
                    cat1.CentralSubCategory = db.SubCategorys.Where(x => x.Categoryid == cat1.Categoryid).Select(c => new CentralSubCategory { SubCategoryId = c.SubCategoryId, SubcategoryName = c.SubcategoryName }).ToList();

                    //Foreach for geting Item list by one by one Sucategory
                    foreach (CentralSubCategory subcat in cat1.CentralSubCategory)
                    {
                        subcat.CentralItem = db.ItemMasterNews.Where(x => x.SubCategory.SubCategoryId == subcat.SubCategoryId).Select(c => new CentralItm { ItemId = c.ItemId, itemname = c.ItemName, LogoUrl = c.LogoUrl }).ToList();
                    }
                }
                //Response Return Basecategory in Jason form
                return Request.CreateResponse(HttpStatusCode.OK, _centralbasecategory);

            }
            catch (Exception ex)
            {
                logger.Error("Error in GetAllByBaseCategory " + ex.Message);
                logger.Info("End GetAllByBaseCategory: ");
                return null;
            }
        }

        //** For Above DTO Class
        //Central Base Category DTO Class
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

        //* End For Above DTO Class *//
    }
}
