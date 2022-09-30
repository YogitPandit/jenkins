
using DailyNeed.Model;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/customerupload")]

    public class CustomerUploadController : ApiController
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        WarehouseAPIMethod warehouseapi = new WarehouseAPIMethod();
        CustomersController customerupload = new CustomersController();
        public static Logger logger = LogManager.GetCurrentClassLogger();
        string msg; string msgitemname;
        string col1, col2, col3, col4, col5, col6, col7, col8, col9, col10, col11, col12, col13, col14, col15, col16, col17;
        int sprovider = 1, sbasecategory = 1;

        public string Msgitemname
        {
            get
            {
                return Msgitemname2;
            }

            set
            {
                Msgitemname2 = value;
            }
        }

        public string Msgitemname1
        {
            get
            {
                return Msgitemname2;
            }

            set
            {
                Msgitemname2 = value;
            }
        }

        public string Msgitemname2
        {
            get
            {
                return msgitemname;
            }

            set
            {
                msgitemname = value;
            }
        }

        [HttpPost]
        public string UploadFile(int compid, int userid)
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                logger.Info("start Item Upload Exel File: ");
                // Get the uploaded image from the Files collection
                System.Web.HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files["file"];

                if (httpPostedFile != null)
                {
                    // Validate the uploaded image(optional)
                    byte[] buffer = new byte[httpPostedFile.ContentLength];
                    using (BinaryReader br = new BinaryReader(httpPostedFile.InputStream))
                    {
                        br.Read(buffer, 0, buffer.Length);
                    }
                    XSSFWorkbook hssfwb;
                    //   XSSFWorkbook workbook1;
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        BinaryFormatter binForm = new BinaryFormatter();
                        memStream.Write(buffer, 0, buffer.Length);
                        memStream.Seek(0, SeekOrigin.Begin);
                        hssfwb = new XSSFWorkbook(memStream);
                        string sSheetName = hssfwb.GetSheetName(0);
                        ISheet sheet = hssfwb.GetSheet(sSheetName);
                        DailyNeedContext context = new DailyNeedContext();
                        IRow rowData;
                        ICell cellData = null;
                        try
                        {
                            List<Customer> CustCollection = new List<Customer>();
                            for (int iRowIdx = 0; iRowIdx <= sheet.LastRowNum; iRowIdx++)
                            {
                                if (iRowIdx == 0)
                                {
                                }
                                else
                                {

                                    rowData = sheet.GetRow(iRowIdx);
                                    cellData = rowData.GetCell(0);
                                    rowData = sheet.GetRow(iRowIdx);
                                    if (rowData != null)
                                    {
                                        Customer cust = new Customer();
                                        try
                                        {
                                            //get company by compid
                                            Company com = context.Companies.Where(x => x.Id == compid && x.Deleted == false).Select(x => x).FirstOrDefault();
                                            People provider = context.Peoples.Where(x => x.Company.Id == compid).FirstOrDefault();

                                            cellData = rowData.GetCell(0);
                                            col1 = cellData == null ? "" : cellData.ToString();
                                            if (col1.Trim() == "" || col1 == null || col1 == "null")
                                                break;
                                            cust.Name = col1.Trim();

                                            cellData = rowData.GetCell(1);
                                            col2 = cellData == null ? "" : cellData.ToString();
                                            cust.Mobile = col2.Trim();

                                            cellData = rowData.GetCell(2);
                                            col3 = cellData == null ? "" : cellData.ToString();
                                            cust.Address = col3.Trim();

                                            cellData = rowData.GetCell(3);
                                            col4 = cellData == null ? "" : cellData.ToString();
                                            if (col4.Trim() == "" || col4 == null)
                                                break;
                                            cust.EmailId = col4.Trim();

                                            //create warehouse object
                                            Warehouse whobj = new Warehouse();

                                            cellData = rowData.GetCell(4);
                                            col5 = cellData == null ? "" : cellData.ToString();
                                            whobj.WarehouseName = col5.Trim();

                                            //get seleted provider and company from ui and add in all tables
                                            People providers = context.Peoples.Where(x => x.PeopleID == sprovider).Include("Company").FirstOrDefault();
                                            Company comps = context.Companies.Where(x => x.Id == providers.Company.Id).FirstOrDefault();
                                            //check warehouse if  exist then add it in customer
                                            var whsearch = customerupload.serachWarehouse(whobj.WarehouseName, comps.Id);

                                            if (whsearch == true)
                                            {

                                                var whid = context.Warehouses.Where(x => x.WarehouseName == whobj.WarehouseName && x.Company.Id == comps.Id).FirstOrDefault();
                                                cust.WarehouseId = whid.WarehouseId;
                                            }
                                            else
                                            {
                                                //check warehouse if not exist then create warehouse for that provider
                                                whobj.CreatedBy = provider.PeopleID.ToString();
                                                whobj.CreatedDate = indianTime;
                                                whobj.UpdatedDate = indianTime;
                                                whobj.WarehouseName = whobj.WarehouseName;
                                                whobj.Company = comps;
                                                whobj.Active = true;
                                                whobj.Deleted = false;
                                                context.Warehouses.Add(whobj);
                                                context.SaveChanges();
                                                var pwid = whobj.WarehouseId;
                                                cust.WarehouseId = pwid;
                                            }

                                            //Create Depot object
                                            Depot dpobj = new Depot();

                                            cellData = rowData.GetCell(5);
                                            col6 = cellData == null ? "" : cellData.ToString();
                                            dpobj.DepotName = col6.Trim();


                                            //check warehouse is exist the add it in all tables where is linking
                                            Warehouse wh = context.Warehouses.Where(w => w.WarehouseId == cust.PWarehouseId && w.Deleted == false).FirstOrDefault();
                                            //check Depot if  exist then add it in customer
                                            var dpsearch = customerupload.serachDepot(dpobj.DepotName, comps.Id);
                                            if (dpsearch == true)
                                            {
                                                var dpid = context.Depots.Where(x => x.DepotName == dpobj.DepotName && x.Company.Id == comps.Id).FirstOrDefault();
                                                cust.DepoId = dpid.Depotid;
                                            }
                                            else
                                            {
                                                //check depot if not exist then create depot for that provider
                                                dpobj.CreatedBy = provider.PeopleID.ToString();
                                                dpobj.CreatedDate = indianTime;
                                                dpobj.UpdatedDate = indianTime;
                                                dpobj.DepotName = dpobj.DepotName;
                                                dpobj.Warehouse = wh;
                                                dpobj.Provider = providers;
                                                dpobj.Company = comps;
                                                dpobj.IsActive = true;
                                                dpobj.Deleted = false;
                                                context.Depots.Add(dpobj);
                                                context.SaveChanges();
                                                var dpid = dpobj.Depotid;
                                                cust.DepoId = dpid;
                                            }

                                            //Create Depot object
                                            Zone znobj = new Zone();

                                            cellData = rowData.GetCell(6);

                                            col7 = cellData == null ? "" : cellData.ToString();
                                            znobj.ZoneName = col7.Trim();

                                            //check Zone if  exist then add it in customer
                                            var znsearch = customerupload.serachZone(znobj.ZoneName, comps.Id);
                                            if (znsearch == true)
                                            {
                                                var znid = context.Zones.Where(x => x.ZoneName == znobj.ZoneName && x.Company.Id == comps.Id).FirstOrDefault();
                                                cust.ZoneId = znid.Zoneid;
                                            }
                                            else
                                            {
                                                //check depot is exist the add it in zone
                                                Depot dt = context.Depots.Where(w => w.Depotid == cust.DepoId && w.Deleted == false).FirstOrDefault();
                                                //check zone if not exist then create zone for that provider
                                                znobj.CreatedBy = provider.PeopleID.ToString();
                                                znobj.CreatedDate = indianTime;
                                                znobj.UpdatedDate = indianTime;
                                                znobj.Warehouse = wh;
                                                znobj.Depot = dt;
                                                znobj.ZoneName = znobj.ZoneName;
                                                znobj.Provider = providers;
                                                znobj.Company = comps;
                                                znobj.IsActive = true;
                                                znobj.Deleted = false;
                                                context.Zones.Add(znobj);
                                                context.SaveChanges();
                                                var znid = znobj.Zoneid;
                                                cust.ZoneId = znid;
                                            }

                                            //Create Category object
                                            Category cgobj = new Category();

                                            cellData = rowData.GetCell(7);
                                            col8 = cellData == null ? "" : cellData.ToString();
                                            cgobj.CategoryName = col8.Trim();


                                            //check basecategy is exist then add it in all table where basecategory is linked
                                            BaseCategory baseCategory = context.BaseCategoryDb.Where(x => x.BaseCategoryId == sbasecategory).FirstOrDefault();
                                            PBaseCategoryLink pbasecate = context.PBaseCategoryLinks.Where(x => x.BaseCategorys.BaseCategoryId == baseCategory.BaseCategoryId && x.Company.Id == comps.Id).FirstOrDefault();
                                            PBaseCategoryLink pbclink = new PBaseCategoryLink();
                                            if (pbasecate == null)
                                            {
                                                pbclink.CreatedBy = provider.PeopleID.ToString();
                                                pbclink.Provider = providers;
                                                pbclink.Company = comps;
                                                pbclink.Warehouse = wh;
                                                pbclink.BaseCategorys = baseCategory;
                                                pbclink.IsActive = true;
                                                pbclink.Deleted = false;
                                                context.PBaseCategoryLinks.Add(pbclink);
                                                context.SaveChanges();
                                            }

                                            PCategoryLink pcatoryLink = new PCategoryLink();
                                            //get category by category name and add in linked table
                                            var category = context.Categorys.Where(x => x.CategoryName == cgobj.CategoryName && x.Company.Id == comps.Id).FirstOrDefault();
                                            if(category != null)
                                            {
                                                //check Zone if  exist then add it in customer
                                                var cgsearch = customerupload.serachCategory(cgobj.CategoryName, comps.Id);
                                                if (cgsearch == true)
                                                {
                                                    var pcate = context.PCategoryLinks.Where(x => x.Categoryes.Categoryid == category.Categoryid && x.Company.Id == comps.Id).FirstOrDefault();

                                                    if (pcate == null)
                                                    {
                                                        pcatoryLink.CreatedBy = provider.PeopleID.ToString();
                                                        pcatoryLink.Provider = providers;
                                                        pcatoryLink.Company = comps;
                                                        pcatoryLink.Warehouse = wh;
                                                        pcatoryLink.Basecategory = baseCategory;
                                                        pcatoryLink.Categoryes = category;
                                                        pcatoryLink.IsActive = true;
                                                        pcatoryLink.Deleted = false;
                                                        context.PCategoryLinks.Add(pcatoryLink);
                                                        context.SaveChanges();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //add category when it is not exist in category table
                                                cgobj.CreatedBy = provider.PeopleID.ToString();
                                                cgobj.CreatedDate = indianTime;
                                                cgobj.UpdatedDate = indianTime;
                                                cgobj.Company = comps;
                                                cgobj.BaseCategory = baseCategory;
                                                cgobj.CategoryName = cgobj.CategoryName;
                                                cgobj.IsActive = true;
                                                cgobj.Deleted = false;
                                                context.Categorys.Add(cgobj);
                                                context.SaveChanges();
                                                //get category object when created in category table
                                                category = context.Categorys.Where(x => x.Categoryid == cgobj.Categoryid).FirstOrDefault();
                                                cust.categoryId = category.Categoryid;
                                                //add category in pcategory link table 
                                                pcatoryLink.CreatedBy = provider.PeopleID.ToString();
                                                pcatoryLink.Provider = providers;
                                                pcatoryLink.Company = comps;
                                                pcatoryLink.Warehouse = wh;
                                                pcatoryLink.Basecategory = baseCategory;
                                                pcatoryLink.Categoryes = category;
                                                pcatoryLink.IsActive = true;
                                                pcatoryLink.Deleted = false;
                                                context.PCategoryLinks.Add(pcatoryLink);
                                                context.SaveChanges();
                                            }

                                            //Create Category object
                                            SubCategory sbcgobj = new SubCategory();

                                            cellData = rowData.GetCell(8);
                                            col9 = cellData == null ? "" : cellData.ToString();
                                            sbcgobj.SubcategoryName = col9.Trim();
                                            //check subcategory by subcategory name and get that record
                                            var subcategory = context.SubCategorys.Where(x => x.SubcategoryName == sbcgobj.SubcategoryName && x.Company.Id == comps.Id).FirstOrDefault();
                                            PSubCategoryLink psubcatoryLink = new PSubCategoryLink();
                                            if(subcategory != null)
                                            {
                                                //check Subcategory in subcategory table if  exist then add it in customer
                                                var sbcgsearch = customerupload.serachSubCategory(sbcgobj.SubcategoryName, comps.Id);
                                                if (sbcgsearch == true)
                                                {
                                                    //check subcategory in psubcategory link table
                                                    var psubcate = context.PSubCategoryLinks.Where(x => x.SubCategoryes.SubCategoryId == subcategory.SubCategoryId && x.Company.Id == comps.Id).FirstOrDefault();
                                                    if (psubcate == null)
                                                    {
                                                        psubcatoryLink.CreatedBy = provider.PeopleID.ToString();
                                                        psubcatoryLink.Provider = providers;
                                                        psubcatoryLink.Company = comps;
                                                        psubcatoryLink.Warehouse = wh;
                                                        psubcatoryLink.Basecategory = baseCategory;
                                                        psubcatoryLink.Category = category;
                                                        psubcatoryLink.SubCategoryes = subcategory;
                                                        psubcatoryLink.IsActive = true;
                                                        psubcatoryLink.Deleted = false;
                                                        context.PSubCategoryLinks.Add(psubcatoryLink);
                                                        context.SaveChanges();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //add subcategory when it is not exist in subcategory table
                                                sbcgobj.CreatedBy = provider.PeopleID.ToString();
                                                sbcgobj.CreatedDate = indianTime;
                                                sbcgobj.UpdatedDate = indianTime;
                                                sbcgobj.Company = comps;
                                                sbcgobj.SubcategoryName = sbcgobj.SubcategoryName;
                                                sbcgobj.IsActive = true;
                                                sbcgobj.Deleted = false;
                                                context.SubCategorys.Add(sbcgobj);
                                                context.SaveChanges();
                                                //get subcategory object when created in subcategory table
                                                 subcategory = context.SubCategorys.Where(x => x.SubCategoryId == sbcgobj.SubCategoryId).FirstOrDefault();
                                                cust.subCategoryId = subcategory.SubCategoryId;
                                                //add category in pcategory link table 
                                                psubcatoryLink.CreatedBy = provider.PeopleID.ToString();
                                                psubcatoryLink.Provider = providers;
                                                psubcatoryLink.Company = comps;
                                                psubcatoryLink.Warehouse = wh;
                                                psubcatoryLink.Basecategory = baseCategory;
                                                psubcatoryLink.Category = category;
                                                psubcatoryLink.SubCategoryes = subcategory;
                                                psubcatoryLink.IsActive = true;
                                                psubcatoryLink.Deleted = false;
                                                context.PSubCategoryLinks.Add(psubcatoryLink);
                                                context.SaveChanges();
                                            }

                                            //Create Category object
                                            ItemMasterNew itemobj = new ItemMasterNew();

                                            cellData = rowData.GetCell(9);
                                            col10 = cellData == null ? "" : cellData.ToString();
                                            itemobj.ItemName = col10.Trim();
                                            
                                            cellData = rowData.GetCell(10);
                                            col11 = cellData == null ? "" : cellData.ToString();
                                            cust.ShiftName = col11.Trim();

                                            cellData = rowData.GetCell(11);
                                            col12 = cellData == null ? "" : cellData.ToString();
                                            cust.StartDate = col12;

                                            cellData = rowData.GetCell(12);
                                            col13 = cellData == null ? "" : cellData.ToString();
                                            itemobj.BasePrice = Convert.ToDouble(col13);

                                            //product BasePrice of customer service
                                            cust.Price = (float)itemobj.BasePrice;

                                            cellData = rowData.GetCell(13);
                                            col14 = cellData == null ? "" : cellData.ToString();
                                            itemobj.Quantity = Convert.ToDouble(col14);

                                            //product quantity of customer service
                                            cust.Quantity = (float)itemobj.Quantity;

                                            //check ItemMaster by ItemMaster name and get that record
                                            var items = context.ItemMasterNews.Where(x => x.ItemName == itemobj.ItemName && x.Company.Id == comps.Id).FirstOrDefault();
                                            PItemLink pitemLink = new PItemLink();
                                            if (items != null)
                                            {
                                                //check Item in item table if  exist then add it in customer
                                                var itemsearch = customerupload.serachItem(itemobj.ItemName, comps.Id);
                                                if (itemsearch == true)
                                                {
                                                    //check item in pitem link table
                                                    var pitem = context.PItemLinks.Where(x => x.ItemMasters.ItemId == items.ItemId && x.Company.Id == comps.Id).FirstOrDefault();
                                                    cust.PItemId = pitem.PItemId;
                                                    if (pitem == null)
                                                    {
                                                        pitemLink.CreatedBy = provider.PeopleID.ToString();
                                                        pitemLink.Provider = providers;
                                                        pitemLink.Company = comps;
                                                        pitemLink.Warehouse = wh;
                                                        pitemLink.Basecategory = baseCategory;
                                                        pitemLink.Category = category;
                                                        pitemLink.Subcategory = subcategory;
                                                        pitemLink.ItemMasters = items;
                                                        pitemLink.IsActive = true;
                                                        pitemLink.Deleted = false;
                                                        context.PItemLinks.Add(pitemLink);
                                                        context.SaveChanges();
                                                        var PItemId = pitemLink.PItemId;
                                                        cust.PItemId = PItemId;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //add item when it is not exist in item table
                                                itemobj.CreatedBy = provider.PeopleID.ToString();
                                                itemobj.CreatedDate = indianTime;
                                                itemobj.UpdatedDate = indianTime;
                                                itemobj.Company = comps;
                                                itemobj.Warehouse = wh;
                                                itemobj.BaseCategory = baseCategory;
                                                itemobj.Category = category;
                                                itemobj.SubCategory = subcategory;
                                                itemobj.ItemName = itemobj.ItemName;
                                                itemobj.BasePrice = itemobj.BasePrice;
                                                itemobj.Quantity = itemobj.Quantity;
                                                itemobj.Active = true;
                                                itemobj.Deleted = false;
                                                context.ItemMasterNews.Add(itemobj);
                                                context.SaveChanges();
                                                //get subcategory object when created in subcategory table
                                                items = context.ItemMasterNews.Where(x => x.ItemId == itemobj.ItemId).FirstOrDefault();
                                                //add category in pcategory link table 
                                                pitemLink.CreatedBy = provider.PeopleID.ToString();
                                                pitemLink.Provider = providers;
                                                pitemLink.Company = comps;
                                                pitemLink.Warehouse = wh;
                                                pitemLink.Basecategory = baseCategory;
                                                pitemLink.Category = category;
                                                pitemLink.ItemMasters = items;
                                                pitemLink.Subcategory = subcategory;
                                                pitemLink.IsActive = true;
                                                pitemLink.Deleted = false;
                                                context.PItemLinks.Add(pitemLink);
                                                context.SaveChanges();
                                                var PItemId = pitemLink.PItemId;
                                                cust.PItemId = PItemId;
                                            }

                                            cellData = rowData.GetCell(14);
                                            col15 = cellData == null ? "" : cellData.ToString();
                                            cust.Comment = col15.Trim();

                                            cellData = rowData.GetCell(15);
                                            col16 = cellData == null ? "" : cellData.ToString();
                                            cust.DeliveryDays = col16.Trim();

                                            cellData = rowData.GetCell(16);
                                            col17 = cellData == null ? "" : cellData.ToString();
                                            cust.DeliveryTime = Convert.ToDateTime(col17);

                                            cust.Active = true;
                                            cust.Password = "123456";

                                            CustCollection.Add(cust);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error("Error adding customer in collection " + "\n\n" + ex.Message + "\n\n" + ex.InnerException + "\n\n" + ex.StackTrace + cust.Name);
                                        }
                                    }
                                }
                            }
                            customerupload.AddBulkcustomer(CustCollection, sprovider);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Error loading for \n\n" + ex.Message + "\n\n" + ex.InnerException + "\n\n" + ex.StackTrace);

                        }
                    }
                    var FileUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

                    httpPostedFile.SaveAs(FileUrl);
                }
            }
            if (Msgitemname != null)
            {
                return Msgitemname;
            }
            msg = "Your Exel data is succesfully saved";
            return msg;
        }
    }
}
