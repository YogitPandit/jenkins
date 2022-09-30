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
using System.Data;
using System.Data.Entity;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Globalization;
using System.IdentityModel;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/MonthlyBill")]
    public class MonthlyBillController : ApiController
    {
        iDailyNeedContext context = new DailyNeedContext();
        DailyNeedContext db = new DailyNeedContext();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        float fQty, fPrice;
        float custAmt;
        DateTime lastDayOfMonth;


        //Get Customers by name
        //[Authorize]
        [Route("GetBillByName")]
        public object Get(string name)
        {
            logger.Info("start Get MonthlyBill: ");
            List<CustomerServices> customerservice = new List<CustomerServices>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
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
                        Warehouse_id = int.Parse(claim.Value);
                    }
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End Customer: ");

                var monthlybill = (from e in db.CustomerServicess
                                   join f in db.CustomerLinks on e.CustomerLink.Name equals name
                                   where
                                   e.CustomerLink.Customer.CustomerId == f.CustomerId
                                   select new
                                   {
                                       e.ID,
                                       e.CustomerLink.Name,
                                       e.CustomerLink.CustomerLinkId,
                                       e.Provider.PeopleID,
                                       e.Price,
                                       e.Quantity,
                                       e.BillingType,
                                       e.LastBillingDate
                                   }).OrderByDescending(x => x.ID).ToList();
                return monthlybill;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End Customer: ");
                return null;
            }

        }

        //Get All Customers
        //[Authorize]
        [Route("")]
        public object Get()
        {
            logger.Info("start Get MonthlyBill: ");
            List<CustomerServices> customerservice = new List<CustomerServices>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
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
                        Warehouse_id = int.Parse(claim.Value);
                    }
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End Customer: ");

                var monthlybill = (from e in db.CustomerServicess
                                   join csut in db.CustomerLinks on e.CustomerLink.CustomerLinkId equals csut.CustomerLinkId
                                   where e.LastBillingDate != null && e.Company.Id == compid && e.IsActive == true && e.Deleted == false
                                   select new
                                   {
                                       e.ID,
                                       e.CustomerLink.Name,
                                       e.CustomerLink.CustomerLinkId,
                                       e.Provider.PeopleID,
                                       e.Price,
                                       e.Quantity,
                                       e.BillingType,
                                       e.LastBillingDate,
                                       csut.Address,
                                       csut.Mobile
                                   }).OrderByDescending(x => x.ID).ToList();
                return monthlybill;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End Customer: ");
                return null;
            }

        }


        [ResponseType(typeof(MonthlyStatement))]
        [Route("AddCustomerBillList")]
        [AcceptVerbs("POST")]
        public List<MonthlyStatement> add(List<MonthlyStatement> billlist1)
        {
            logger.Info("start add Monthly Bill List: ");
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
                //Chaeking List Is null or not
                if (billlist1 == null)
                {
                    throw new ArgumentNullException("Monthly Bill");
                }
                foreach (var Customers in billlist1)
                {
                    People provider = db.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
                    CustomerLink customerlk = db.CustomerLinks.Where(x => x.CustomerLinkId == Customers.CustomerLinkId && x.Deleted == false).FirstOrDefault();
                    CustomerServices customerSrvice = db.CustomerServicess.Where(x => x.CustomerLink.CustomerLinkId == Customers.CustomerLinkId && x.Deleted == false).Include("PItem").FirstOrDefault();
                    PItemLink Pitem = db.PItemLinks.Where(x => x.Company.Id == compid && x.PItemId == customerSrvice.PItem.PItemId).Include("ItemMasters").FirstOrDefault();
                    ItemMasterNew itm = db.ItemMasterNews.Where(x => x.ItemId == Pitem.ItemMasters.ItemId).FirstOrDefault();
                    Company comp = db.Companies.Where(c => c.Id == compid && c.Deleted == false).FirstOrDefault();
                    //Assuming Date Of Recent Day
                    DateTime firstDayOfMonth;

                    //Frist Day And Last Day Of Month
                    firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    //finding Monthaly statetnet by first and last date
                    List<MonthlyStatement> Mothlystatetment = db.MonthlyStatements.Where(x => (x.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth) && x.Deleted == false && x.IsActive == true && x.CustomerLink.CustomerLinkId == Customers.CustomerLinkId && x.Company.Id == compid && x.Status == "Open").ToList();

                    //Consolidated Data For Invoice Creation
                    if (Mothlystatetment != null && Mothlystatetment.Count != 0)
                    {
                        foreach (MonthlyStatement monthSatetment in Mothlystatetment)
                        {
                            monthSatetment.Status = "Close";
                            db.MonthlyStatements.Attach(monthSatetment);
                            db.Entry(monthSatetment).State = EntityState.Modified;
                            db.SaveChanges();
                            fQty += monthSatetment.FinalQty;
                            fPrice += monthSatetment.Price;
                        }
                        CustomerInvoice Invoce = new CustomerInvoice();
                        Invoce.FromDate = firstDayOfMonth;
                        Invoce.ToDate = lastDayOfMonth;
                        Invoce.TotalQty = fQty;
                        Invoce.TotalPrice = fPrice;
                        Invoce.Company = comp;
                        Invoce.customerServiceID = customerSrvice.ID;
                        Invoce.Customerlk = customerlk;
                        Invoce.CreateDate = indianTime;
                        Invoce.CreatedBy = userid.ToString();
                        Invoce.Deleted = false;
                        Invoce.IsActive = true;
                        Invoce.Status = "Open";
                        db.CustomerInvoices.Add(Invoce);
                        db.SaveChanges();
                        int Invoice = Invoce.Id;
                        //fQty = 0;


                        ////finding Invoice by first and last date
                        List<CustomerInvoice> custInvoice = db.CustomerInvoices.Where(x => (x.FromDate >= firstDayOfMonth || x.ToDate <= lastDayOfMonth) && x.Deleted == false && x.IsActive == true && x.Customerlk.CustomerLinkId == Customers.CustomerLinkId && x.Status == "Open").ToList();

                        //Consolidated Data For Wallet Creation
                        if (custInvoice != null && custInvoice.Count != 0)
                        {
                            var cWallet = db.CustomerWallets.Where(x => x.CustomerLink.CustomerLinkId == Customers.CustomerLinkId).FirstOrDefault();
                            if (cWallet == null)
                            {
                                custAmt = fPrice;
                                CustomerWallet custWallet = new CustomerWallet();
                                custWallet.AvailableBalance = 0 + (-custAmt);
                                if (custWallet.AvailableBalance > 0)
                                {
                                    custWallet.BalanceType = "Cr.";
                                }
                                else
                                {
                                    custWallet.BalanceType = "Dr.";
                                    custWallet.AvailableBalance = -(custWallet.AvailableBalance);
                                }

                                //custWallet.ServiceId = customerSrvice.ID;
                                custWallet.WalletLimit = 2000;
                                custWallet.Provider = provider;
                                custWallet.CustomerLink = customerlk;
                                custWallet.CreateDate = indianTime;
                                custWallet.Deleted = false;
                                custWallet.IsActive = true;
                                custAmt = 0;
                                //fPrice = 0;
                                db.AddWallet(custWallet, custWallet.AvailableBalance);
                            }
                            else
                            {
                                custAmt = fPrice;
                                CustomerWallet custWallet = new CustomerWallet();
                                custWallet.AvailableBalance = (cWallet.AvailableBalance) + (-custAmt);
                                if (custWallet.AvailableBalance > 0)
                                {
                                    custWallet.BalanceType = "Cr.";
                                }
                                else
                                {
                                    custWallet.BalanceType = "Dr.";
                                    custWallet.AvailableBalance = -(custWallet.AvailableBalance);
                                }
                                //custWallet.ServiceId = customerSrvice.ID;
                                custWallet.WalletLimit = 2000;
                                custWallet.Provider = provider;
                                custWallet.CustomerLink = customerlk;
                                custWallet.CreateDate = indianTime;
                                custWallet.Deleted = false;
                                custWallet.IsActive = true;
                                custAmt = 0;
                                //fPrice = 0;
                                db.AddWallet(custWallet, custWallet.AvailableBalance);
                            }
                        }
                        var custservise = db.CustomerServicess.Where(x => x.CustomerLink.CustomerLinkId == Customers.CustomerLinkId && x.ID == Customers.ID).FirstOrDefault();
                        if (custservise != null)
                        {
                            custservise.LastBillingDate = lastDayOfMonth.AddDays(1);
                            custservise.Message = "MonthlyStatement";
                            db.CustomerServicess.Attach(custservise);
                            db.Entry(custservise).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        HtmDataBulk printList = new HtmDataBulk();
                        printList.CustomerLinkId = customerlk.CustomerLinkId;
                        printList.InvoiceNumber = Invoice.ToString();
                        printList.Name = customerlk.Name;
                        printList.BillingAddress = customerlk.Address;
                        printList.Mobile = customerlk.Mobile;
                        printList.ToDate = indianTime.ToString("dd/MM/yyyy");
                        printList.TotalPrice = fPrice.ToString();
                        printList.itemname = itm.ItemName;
                        printList.TotalQty = fQty.ToString();
                        printList.Price = customerSrvice.Price.ToString();
                        printList.CompanyName = comp.CompanyName;
                        printList.Address = comp.Address;
                        printList.CompanyPhone = comp.CompanyPhone;
                        printList.DiscountAmount = "0";
                        printList.TaxAmount = "0";
                        printList.deliveryCharge ="0";
                        printList.WalletAmount = "0";

                        Add(printList);
                    }
                    else
                    {
                        Customers.Message = "No Trancation For Invoice / Error";
                    }
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End Monthly Bill: ");
                return billlist1;

            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer services " + ex.Message);
                return null;
            }
        }

        //[ResponseType(typeof(CustomerInvoice))]
        [Route("BulkEmail")]
        [AcceptVerbs("POST")]
        public HtmDataBulk Add(HtmDataBulk bulkHtmlData)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                //Access claims
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
                var tbodystring = @"<tr>
                                        <td>{$index}</td>
                                        <td>{itemname}</td>
                                        <td>{Price}</td>                                        
                                        <td>{TotalQty}</td>
                                        <td>{TotalPrice}</td>
                                    </tr>";
                var completestring = "";
                WebClient client = new WebClient();
                //String htmlCode = client.DownloadString(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Template/BulkEmailWithInvoice.html"));
                int index = 1;

                //foreach (HtmDataBulk htmldata in bulkHtmlData)
                //{
                    String htmlCode = client.DownloadString(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Template/BulkEmailWithInvoice.html"));
                    //Header Data
                    htmlCode = htmlCode.Replace("{Name}", bulkHtmlData.Name);
                    htmlCode = htmlCode.Replace("{Mobile}", bulkHtmlData.Mobile);
                    htmlCode = htmlCode.Replace("{CompanyPhone}", bulkHtmlData.CompanyPhone);
                    htmlCode = htmlCode.Replace("{Address}", bulkHtmlData.Address);
                    htmlCode = htmlCode.Replace("{CompanyName}", bulkHtmlData.CompanyName);
                    htmlCode = htmlCode.Replace("{BillingAddress}", bulkHtmlData.BillingAddress);
                    htmlCode = htmlCode.Replace("{TotalPrice}", bulkHtmlData.TotalPrice);
                    htmlCode = htmlCode.Replace("{CompanyPhone}", bulkHtmlData.CompanyPhone);
                    htmlCode = htmlCode.Replace("{Price}", bulkHtmlData.Price);
                    htmlCode = htmlCode.Replace("{deliveryCharge}", bulkHtmlData.deliveryCharge);
                    htmlCode = htmlCode.Replace("{WalletAmount}", bulkHtmlData.WalletAmount);
                    htmlCode = htmlCode.Replace("{DiscountAmount}", bulkHtmlData.DiscountAmount);
                    htmlCode = htmlCode.Replace("{TaxAmount}", bulkHtmlData.TaxAmount);
                    htmlCode = htmlCode.Replace("{ToDate}", Convert.ToString(bulkHtmlData.ToDate));
                    htmlCode = htmlCode.Replace("{Id}", Convert.ToString(bulkHtmlData.InvoiceNumber));

                    //Table Data
                    tbodystring = tbodystring.Replace("{$index}", index.ToString());
                    tbodystring = tbodystring.Replace("{itemname}", bulkHtmlData.itemname);
                    tbodystring = tbodystring.Replace("{Price}", bulkHtmlData.Price);
                    tbodystring = tbodystring.Replace("{TotalQty}", Convert.ToString(bulkHtmlData.TotalQty));
                    tbodystring = tbodystring.Replace("{TotalPrice}", bulkHtmlData.TotalPrice);
                    completestring = tbodystring;

                    htmlCode = htmlCode.Replace("{tableData}", completestring);
                    byte[] pdf; // result will be here
                    var cssText = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/styles/bootstrap/bootstrap/bootstrap.css"));
                    var month = DateTime.Now.AddMonths(-1).ToString("MMM", CultureInfo.InvariantCulture);
                    using (var memoryStream = new MemoryStream())
                    {
                        //var document = new Document(PageSize.A4, 50, 50, 60, 60);
                        var document = new Document(PageSize.A4, 25, 25, 30, 30);
                        var writer = PdfWriter.GetInstance(document, memoryStream);
                        document.Open();
                        using (var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText)))
                        {
                            using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlCode)))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, htmlMemoryStream, cssMemoryStream);
                            }
                        }
                        document.Close();
                        pdf = memoryStream.ToArray();
                        File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/SavePDF/" + bulkHtmlData.Name + "_" + bulkHtmlData.InvoiceNumber + "_" + month + ".pdf"), memoryStream.ToArray());
                        //FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/SavePDF/test123.pdf"), FileMode.CreateNew);
                    }
                    List<SendMailModel> smail = new List<SendMailModel>();
                    try
                    {
                        CustomerLink took = db.CustomerLinks.Where(x => x.CustomerLinkId == bulkHtmlData.CustomerLinkId).FirstOrDefault();
                        SendMailModel _objModelMail = new SendMailModel();
                        _objModelMail.Body = "Please find attached Invoice";
                        _objModelMail.From = "ajamil@moreyeahs.in";
                        _objModelMail.Subject = "DailyNeed Invoice";
                        _objModelMail.To = took.EmailId;
                        MailMessage email = new MailMessage();
                        email.To.Add(_objModelMail.To = took.EmailId);
                        email.From = new MailAddress(_objModelMail.From);
                        email.Subject = _objModelMail.Subject;
                        string Body = _objModelMail.Body;
                        email.Body = _objModelMail.Body;
                        FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/SavePDF/" + bulkHtmlData.Name + "_" + bulkHtmlData.InvoiceNumber + "_" + month + ".pdf"), FileMode.Open, FileAccess.Read);
                        Attachment a = new Attachment(fs, bulkHtmlData.Name + "_" + bulkHtmlData.InvoiceNumber + "_" + month + ".pdf", MediaTypeNames.Application.Octet);
                        email.Attachments.Add(a);
                        email.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("ajamil@moreyeahs.in", "Oneplus@5"); // Enter senders User name and password 
                        smtp.EnableSsl = true;
                        smtp.Send(email);
                    }
                    catch (Exception ex)
                    {
                    }
                //}
                return bulkHtmlData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //get All Customer Monthly Statement
        ////[Authorize]
        //[Route("MonthlyStatement")]
        //[AcceptVerbs("GET")]
        //public object MonthlyStatement()
        //{
        //    List<MonthlyStatement> statement = new List<MonthlyStatement>();
        //    try
        //    {
        //        var identity = User.Identity as ClaimsIdentity;
        //        int compid = 0, userid = 0;
        //        //Access claims
        //        foreach (Claim claim in identity.Claims)
        //        {
        //            if (claim.Type == "compid")
        //            {
        //                compid = int.Parse(claim.Value);
        //            }
        //            if (claim.Type == "userid")
        //            {
        //                userid = int.Parse(claim.Value);
        //            }
        //        }
        //        var custStatement = (from e in db.MonthlyStatements
        //                             where e.IsActive == true && e.Deleted == false && e.Provider.PeopleID == userid
        //                             select new
        //                             {
        //                                 e.Id,
        //                                 e.Customer.Name,
        //                                 e.Customer.CustomerId,
        //                                 e.Provider.PeopleID,
        //                                 e.Provider.PeopleFirstName,
        //                                 e.Date,
        //                                 e.FinalQty,
        //                                 e.Price
        //                             }).OrderByDescending(x => x.Id).ToList();
        //        return custStatement;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Error in getting Statement " + ex.Message);
        //        return null;
        //    }
        //}

        //get Monthly Statement Customerid and date wise
        //[Authorize]
        //[Route("MonthlyDateStatement")]
        //[AcceptVerbs("GET")]
        //public object MonthlyDateStatement(int custId, DateTime fDate, DateTime tDate)
        //{
        //    List<MonthlyStatement> statement = new List<MonthlyStatement>();
        //    try
        //    {
        //        var identity = User.Identity as ClaimsIdentity;
        //        int compid = 0, userid = 0;
        //        //Access claims
        //        foreach (Claim claim in identity.Claims)
        //        {
        //            if (claim.Type == "compid")
        //            {
        //                compid = int.Parse(claim.Value);
        //            }
        //            if (claim.Type == "userid")
        //            {
        //                userid = int.Parse(claim.Value);
        //            }
        //        }
        //        var custStatement = (from e in db.MonthlyStatements
        //                             where e.IsActive == true && e.Deleted == false && e.Provider.PeopleID == userid
        //                             && e.Customer.CustomerId == custId && (e.Date >= fDate && e.Date <= tDate)
        //                             select new
        //                             {
        //                                 e.Id,
        //                                 e.Customer.Name,
        //                                 e.Customer.CustomerId,
        //                                 e.Provider.PeopleID,
        //                                 e.Provider.PeopleFirstName,
        //                                 e.flag,
        //                                 e.Date,
        //                                 e.FinalQty,
        //                                 e.Price
        //                             }).ToList();
        //        return custStatement;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Error in getting Statement " + ex.Message);
        //        return null;
        //    }
        //}

        [Route("AllInvoice")]
        public object GetAllInvoice()
        {
            logger.Info("start Get MonthlyBill: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
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
                        Warehouse_id = int.Parse(claim.Value);
                    }
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End Customer: ");

                var allInvoice = (from e in db.CustomerInvoices
                                  where e.Company.Id == CompanyId
                                  select new
                                  {
                                      e.Id,
                                      e.Customerlk,
                                      e.customerServiceID,
                                      e.TotalQty,
                                      e.TotalPrice,
                                      e.Status,
                                      e.FromDate,
                                      e.ToDate,
                                  }).OrderByDescending(x => x.Id).ToList();
                return allInvoice;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End Customer: ");
                return null;
            }

        }

        [Route("AllInvoiceByCust")]
        public object GetAllInvoiceByCust(int custId)
        {
            logger.Info("start Get MonthlyBill: ");
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0, Warehouse_id = 0;
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
                        Warehouse_id = int.Parse(claim.Value);
                    }
                }
                int CompanyId = compid;
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                logger.Info("End Customer: ");

                var allInvoiceByCust = (from e in db.CustomerInvoices
                                        where e.Company.Id == CompanyId && e.Customerlk.CustomerLinkId == custId
                                        select new
                                        {
                                            e.Id,
                                            e.Customerlk,
                                            e.customerServiceID,
                                            e.TotalQty,
                                            e.TotalPrice,
                                            e.Status,
                                            e.FromDate,
                                            e.ToDate,
                                        }).OrderByDescending(x => x.Id).ToList();
                return allInvoiceByCust;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End Customer: ");
                return null;
            }

        }


        //Dto Classes for bulk data 
        public class HtmDataBulk
        {

            public int CustomerLinkId { get; set; }
            public string InvoiceNumber { get; set; }
            public string Name { get; set; }
            public string BillingAddress { get; set; }
            public string Mobile { get; set; }
            public string ToDate { get; set; }
            public string TotalPrice { get; set; }
            public string itemname { get; set; }
            public string TotalQty { get; set; }
            public string Price { get; set; }
            public string CompanyName { get; set; }
            public string Address { get; set; }
            public string CompanyPhone { get; set; }
            public string DiscountAmount { get; set; }
            public string TaxAmount { get; set; }
            public string deliveryCharge { get; set; }
            public string WalletAmount { get; set; }
        }

        public class SendMailModel
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }

        }

    }
}