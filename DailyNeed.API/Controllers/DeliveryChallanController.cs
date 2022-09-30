using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;
using System.Security.Claims;
using System.Data.Entity;
using System.Web.Http.Description;
using System.IO;
using System.Net.Mime;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Globalization;
using System.IdentityModel;
using System.Net.Mail;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/DeliveryChallan")]
    public class DeliveryChallanController : ApiController
    {

        iDailyNeedContext context = new DailyNeedContext();
        DailyNeedContext db = new DailyNeedContext();
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //get All Customer Challan List 
        //[Authorize]
        [Route("GetChallanList")]
        public object Get()
        {
            logger.Info("start Get CustomerServices: ");
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
                logger.Info("End  Customer: ");
                var challan = db.DeliveryChallans.Where(x => x.Company.Id == CompanyId).GroupBy(i => i.ChallanNumber)
                .Select(g => new
                {
                    ChallanNumber = g.Select(i => i.ChallanNumber).FirstOrDefault(),
                    Status = g.Select(i => i.Status).FirstOrDefault(),
                    TotalCustomer = g.Count(),
                    Quantity = g.Sum(i => i.FinalQty),
                    Date = g.Select(i => i.Date).FirstOrDefault()
                }).ToList();
                return challan;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }

        //get All Customer 
        //[Authorize]
        [Route("GetCustomerList")]
        public object GetCusotmerList(string cNumber)
        {
            logger.Info("start Get CustomerServices: ");
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
                logger.Info("End  Customer List: ");

                var challanlst = db.DeliveryChallans.Where(x => x.ChallanNumber == cNumber && x.Company.Id == CompanyId).Include("CustomerLink").Include("CustomerService").ToList();
                return challanlst.Distinct();
            }
            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End  Customer: ");
                return null;
            }
        }

        // Update Challan / POST Method
        [Route("UpdateCustomer")]
        [AcceptVerbs("PUT")]
        public DeliveryChallan CusotmerUpdate(DeliveryChallan Challanlst)
        {
            logger.Info("start addChallan: ");
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
                if (Challanlst == null)
                {
                    throw new ArgumentNullException("Challan");
                }
                logger.Info("User ID : {0} , Company Id : {1}", compid, userid);
                DeliveryChallan _Challanl = db.DeliveryChallans.Where(x => x.ChallanId == Challanlst.ChallanId && x.CustomerLink.CustomerLinkId == Challanlst.CustomerLinkId).FirstOrDefault();
                _Challanl.ChallanId = Challanlst.ChallanId;
                _Challanl.FinalQty = Challanlst.FinalQty;
                _Challanl.ChallanNumber = Challanlst.ChallanNumber;
                _Challanl.Comment = Challanlst.Comment;
                _Challanl.Status = Challanlst.Status;
                _Challanl.ItemName = Challanlst.ItemName;
                db.DeliveryChallans.Attach(_Challanl);
                db.Entry(_Challanl).State = EntityState.Modified;
                db.SaveChanges();
                _Challanl.Message = "Successfully";
                return _Challanl;
            }
            catch (Exception ex)
            {
                logger.Error("Error in UpdateChallan " + ex.Message);
                logger.Info("End  UpdateChallan: ");
                Challanlst.Message = "Error";
                return null;
            }
        }

        //Get Active Custumor list of services and post for challan creation
        [Route("GetChallanlst")]
        public List<DeliveryChallan> GetChallanlst()
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

                var monthlybill = (from e in db.CustomerServicess
                                   where e.LastBillingDate != null && e.Company.Id == CompanyId && e.IsActive == true && e.Deleted == false
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
                //return monthlybill;
                List<DeliveryChallan> DeliveryChallanlst = new List<DeliveryChallan>();
                foreach (var dechal in monthlybill)
                {
                    DeliveryChallan deliveryChallan = new DeliveryChallan();
                    deliveryChallan.FinalQty = dechal.Quantity;
                    deliveryChallan.Price = dechal.Price;
                    deliveryChallan.CustomerLinkId = dechal.CustomerLinkId;
                    deliveryChallan.ID = dechal.ID;
                    deliveryChallan.PeopleID = dechal.PeopleID;
                    DeliveryChallanlst.Add(deliveryChallan);
                }
                return DeliveryChallanlst;
            }

            catch (Exception ex)
            {
                logger.Error("Error in Customer " + ex.Message);
                logger.Info("End Customer: ");
                return null;
            }

        }

        //Generate Challan
        [ResponseType(typeof(DeliveryChallan))]
        [Route("AddCustomerBillList")]
        [AcceptVerbs("POST")]
        public List<DeliveryChallan> add(List<DeliveryChallan> _Challanlst)
        {
            List<DeliveryChallan> chalst = GetChallanlst();
            _Challanlst = chalst;
            List<DeliveryChallan> currentchallanList = new List<DeliveryChallan>();
            if (chalst != null)
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
                    int CompanyId = compid;
                    //Chaeking List Is null or not
                    //if (_Challanlst == null)
                    //{
                    //    throw new ArgumentNullException("Monthly Bill");
                    //}
                    DateTime DayOfChallan = DateTime.Now.AddDays(1);
                    DateTime startDate = DayOfChallan.Date.AddSeconds(-1);
                    DateTime endDate = DayOfChallan.Date.AddDays(1);
                    var _checkChallan = db.DeliveryChallans.Where(x => x.Date > startDate && x.Date < endDate && x.Company.Id == CompanyId).ToList();
                    People provider = db.Peoples.Where(x => x.PeopleID == userid && x.Deleted == false).FirstOrDefault();
                    Company comp = db.Companies.Where(x => x.Id == CompanyId).FirstOrDefault();
                    if (_checkChallan == null || _checkChallan.Count == 0)
                    {
                        foreach (DeliveryChallan challan in _Challanlst)
                        {
                            //People provider = db.Peoples.Where(x => x.Company.Id == CompanyId && x.Deleted == false).FirstOrDefault();

                            //CustomerServices customersrvs = db.CustomerServicess.Where(x => x.ID == challan.ID && x.Deleted == false).FirstOrDefault();

                            //Getting All Leave Dates In List 
                            List<CustomerServices> CustServices = db.CustomerServicess.Where(x => x.Company.Id == CompanyId && x.IsActive == true && x.Deleted == false).Include("PItem").Include("CustomerLink").ToList();
                            if (CustServices != null)
                            {
                                var ChallanNumber = challanNUm();
                                foreach (CustomerServices CustServlst in CustServices)
                                {
                                    CustomerLink customerlk = db.CustomerLinks.Where(x => x.CustomerLinkId == CustServlst.CustomerLink.CustomerLinkId && x.Deleted == false).FirstOrDefault();                                   

                                    DeliveryChallan myChallanlst = new DeliveryChallan();
                                    PItemLink itemlk = db.PItemLinks.Where(i=>i.PItemId == CustServlst.PItem.PItemId).Include("ItemMasters").FirstOrDefault();
                                    ItemMasterNew item = db.ItemMasterNews.Where(i => i.ItemId == itemlk.ItemMasters.ItemId).FirstOrDefault();
                                    myChallanlst.ChallanNumber = ChallanNumber;
                                    myChallanlst.Date = DayOfChallan;
                                    myChallanlst.Company = comp;
                                    myChallanlst.FinalQty = CustServlst.Quantity;
                                    myChallanlst.Price = CustServlst.Price;
                                    myChallanlst.ItemName = item.ItemName;
                                    myChallanlst.Provider = provider;
                                    myChallanlst.CustomerLink = customerlk;
                                    myChallanlst.Address = customerlk.Address;
                                    myChallanlst.MobileNumber = customerlk.Mobile;
                                    myChallanlst.CustomerService = CustServlst;
                                    myChallanlst.flag = "Regular";
                                    myChallanlst.Comment = customerlk.Comment;
                                    myChallanlst.Status = "Open";
                                    myChallanlst.Deleted = false;
                                    myChallanlst.IsActive = true;
                                    currentchallanList.Add(myChallanlst);

                                    //Getting All Leave Dates In List 
                                    List<MonthLeave> Leavelist = db.MonthLeaves.Where(x => (x.ToDate >= DayOfChallan || x.FromDate <= DayOfChallan) && x.Deleted == false && x.IsActive == true && x.CustomerLink.CustomerLinkId == challan.CustomerLinkId).ToList();

                                    foreach (MonthLeave monthleave in Leavelist)
                                    {
                                        int totaldays = Convert.ToInt32((monthleave.ToDate - monthleave.FromDate).TotalDays);

                                        for (int i = 0; i <= totaldays; i++)
                                        {
                                            string dateleave = monthleave.FromDate.AddDays(i).ToString("dd/MM/yyyy");
                                            string datechallan = DayOfChallan.ToString("dd/MM/yyyy");
                                            if (dateleave == datechallan)
                                            {
                                                DeliveryChallan leave = currentchallanList.Where(x => x.CustomerLink.CustomerLinkId == challan.CustomerLinkId).FirstOrDefault();
                                                currentchallanList.Remove(leave);
                                            }
                                        }
                                    }

                                    //Getting All Additional Services Dates In List 
                                    List<CustomerAdditionalService> CustAddServlst = db.CustomerAdditionalServicess.Where(x => x.Date > startDate && x.Date < endDate && x.Deleted == false && x.IsActive == true && x.CustomerLink.CustomerLinkId == challan.CustomerLinkId).ToList();

                                    foreach (CustomerAdditionalService custserv in CustAddServlst)
                                    {
                                        DeliveryChallan CustomerAddServ = currentchallanList.Where(x => x.CustomerLink.CustomerLinkId == challan.CustomerLinkId && x.CustomerService.ID == challan.ID).FirstOrDefault();
                                        currentchallanList.Remove(CustomerAddServ);
                                        CustomerAddServ.FinalQty = custserv.Quantity + CustomerAddServ.FinalQty;
                                        currentchallanList.Add(CustomerAddServ);
                                    }
                                }
                                if (currentchallanList.Count >= 0 || currentchallanList != null)
                                {
                                    foreach (DeliveryChallan Customere in currentchallanList)
                                    {
                                        DeliveryChallan myChallanlst = new DeliveryChallan();
                                        ItemMasterNew item = db.ItemMasterNews.Where(i => i.ItemName == Customere.ItemName).FirstOrDefault();
                                        CustomerServices custserv = db.CustomerServicess.Where(c => c.ID == Customere.CustomerService.ID).FirstOrDefault();
                                        myChallanlst.ChallanNumber = Customere.ChallanNumber;
                                        myChallanlst.Date = Customere.Date;
                                        myChallanlst.Company = Customere.Company;
                                        myChallanlst.FinalQty = Customere.FinalQty;
                                        myChallanlst.Price = Customere.Price;
                                        myChallanlst.ItemName = item.ItemName;
                                        myChallanlst.Provider = Customere.Provider;
                                        myChallanlst.CustomerLink = Customere.CustomerLink;
                                        myChallanlst.Address = Customere.Address;
                                        myChallanlst.MobileNumber = Customere.MobileNumber;
                                        myChallanlst.Comment = Customere.Comment;
                                        myChallanlst.CustomerService = custserv;
                                        myChallanlst.flag = "Regular";
                                        myChallanlst.Comment = "";
                                        myChallanlst.Status = "Open";
                                        myChallanlst.Deleted = false;
                                        myChallanlst.IsActive = true;
                                        db.DeliveryChallans.Add(myChallanlst);
                                        db.SaveChanges();
                                    }
                                }
                                return currentchallanList;
                            }
                        }
                    }
                    else
                    {
                        DeliveryChallan _DTOM = new DeliveryChallan();
                        _DTOM.Message = "Already Exists!";
                        currentchallanList.Add(_DTOM);
                        return currentchallanList;
                    }

                }

                catch (Exception ex)
                {
                    logger.Error("Error in Customer services " + ex.Message);
                    return null;
                }
            }
            return currentchallanList;
        }

        //Print Challan api with html
        [ResponseType(typeof(DeliveryChallan))]
        [Route("PrintChallan")]
        [AcceptVerbs("POST")]
        public DeliveryChallan Add(string ChallanNo)
        {
            DeliveryChallan aapReturn = new DeliveryChallan();
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
                int companyid = compid;
                var challanPath = "";
                List<MonthlyStatement> monthdata = new List<MonthlyStatement>();
                var challanLst = db.DeliveryChallans.Where(l => l.ChallanNumber == ChallanNo && l.Company.Id == compid).Include("CustomerLink").Include("CustomerService").Include("Company").Include("Provider").ToList();
                if (challanLst != null)
                {
                    Company comp = db.Companies.Where(x => x.Id == compid).FirstOrDefault();
                    People provider = db.Peoples.Where(x => x.PeopleID == userid).FirstOrDefault();
                    foreach (var dclist in challanLst)
                    {
                        MonthlyStatement _pclist = new MonthlyStatement();
                        _pclist.ChallanNumber = ChallanNo;
                        _pclist.ChallanId = dclist.ChallanId;
                        _pclist.Address = dclist.Address;
                        _pclist.Date = dclist.Date;
                        _pclist.Price = dclist.Price;
                        _pclist.FinalQty = dclist.FinalQty;
                        _pclist.ItemName = dclist.ItemName;
                        _pclist.MobileNumber = dclist.MobileNumber;
                        _pclist.CustomerLink = dclist.CustomerLink;
                        _pclist.CustomerService = dclist.CustomerService;
                        _pclist.Comment = _pclist.Comment;
                        _pclist.CreateDate = indianTime;
                        _pclist.TransactionType = "Monthly";
                        _pclist.Company = comp;
                        _pclist.Provider = provider;
                        _pclist.CreatedBy = provider.PeopleID.ToString();
                        _pclist.IsActive = true;
                        _pclist.Deleted = false;
                        _pclist.Status = dclist.Status;
                        _pclist.flag = dclist.flag;
                        _pclist.Message = "Successfully";
                        db.MonthlyStatements.Add(_pclist);
                        int id = db.SaveChanges();
                        DeliveryChallan dcl = new DeliveryChallan();
                        var challanNum = db.DeliveryChallans.Where(x => x.ChallanId == dclist.ChallanId && x.Company.Id == compid).FirstOrDefault();
                        challanNum.Status = "Close";
                        db.Entry(challanNum).State = EntityState.Modified;
                        db.SaveChanges();
                        monthdata.Add(_pclist);
                    }
                }
                var tbodystring = @"<tr>
                                    <td>{$index}</td>
                                    <td>{CustomerData1.CustomerLink.Name}</td>
                                    <td>{CustomerData1.ItemName}</td>
                                    <td>{CustomerData1.Address}</td>                                    
                                    <td>{CustomerData1.MobileNumber}</td>
                                    <td>{CustomerData1.FinalQty}</td> 
                                    <td>{CustomerData1.Comment}</td>  
                                    
                                </tr>";
                var oldtr = @"<tr>
                                    <td>{$index}</td>
                                    <td>{CustomerData1.CustomerLink.Name}</td>
                                    <td>{CustomerData1.ItemName}</td>
                                    <td>{CustomerData1.Address}</td>                                    
                                    <td>{CustomerData1.MobileNumber}</td> 
                                    <td>{CustomerData1.FinalQty}</td>
                                    <td>{CustomerData1.Comment}</td> 
                                                                       
                                </tr>";
                var completestring = "";
                WebClient client = new WebClient();
                String htmlCode = client.DownloadString(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Template/ChallanTemplate.html"));
                int index = 1;
                foreach (MonthlyStatement htmldata2 in monthdata)
                {
                    tbodystring = oldtr;
                    if (htmldata2.html == null)
                    {
                        var Cdata = db.DeliveryChallans.Where(x => x.ChallanId == htmldata2.ChallanId).Include("CustomerLink").FirstOrDefault();
                        //htmldata2.ChallanId = Cdata.ChallanId;
                        htmldata2.Name = Cdata.CustomerLink.Name;
                        htmldata2.ItemName = Cdata.ItemName;
                        htmldata2.Comment = Cdata.Comment;
                        htmldata2.Address = Cdata.Address;
                        htmldata2.MobileNumber = Cdata.MobileNumber;
                        htmldata2.Date = Cdata.Date;
                        htmlCode = htmlCode.Replace("{Date}", Convert.ToString(htmldata2.Date.ToString("dd/MM/yyyy")));

                        // Replace all html breaks for line seperators.
                        tbodystring = tbodystring.Replace("{$index}", index.ToString());
                        tbodystring = tbodystring.Replace("{CustomerData1.CustomerLink.Name}", htmldata2.Name);
                        tbodystring = tbodystring.Replace("{CustomerData1.FinalQty}", Convert.ToString(htmldata2.FinalQty));
                        tbodystring = tbodystring.Replace("{CustomerData1.ItemName}", (htmldata2.ItemName));
                        tbodystring = tbodystring.Replace("{CustomerData1.Comment}", (htmldata2.Comment));
                        tbodystring = tbodystring.Replace("{CustomerData1.Address}", (htmldata2.Address));
                        tbodystring = tbodystring.Replace("{CustomerData1.MobileNumber}", (htmldata2.MobileNumber));
                        completestring = completestring + tbodystring;

                    }
                    index++;
                }
                htmlCode = htmlCode.Replace("{tableData}", completestring);
                byte[] pdf; // result will be here
                var cssText = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/styles/bootstrap/bootstrap/bootstrap.css"));
                var month = DateTime.Now.AddMonths(-1).ToString("MMM", CultureInfo.InvariantCulture);
                using (var memoryStream = new MemoryStream())
                {

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
                    challanPath = System.Web.Hosting.HostingEnvironment.MapPath("~/SavePDF/Challans/challan" + "_" + month + "_" + ChallanNo + ".pdf");
                    File.WriteAllBytes(challanPath, memoryStream.ToArray());
                    //htmldata.Clear();
                }

                //Send Email
                List<SendMailModel> smail = new List<SendMailModel>();
                try
                {
                    People took = db.Peoples.Where(x => x.Company.Id == companyid).FirstOrDefault();
                    SendMailModel _objModelMail = new SendMailModel();
                    _objModelMail.Body = "Please find attached Invoice";
                    _objModelMail.From = "ajamil@moreyeahs.in";
                    _objModelMail.Subject = "DailyNeed Invoice";
                    _objModelMail.To = took.Email;
                    MailMessage email = new MailMessage();
                    email.To.Add(_objModelMail.To = took.Email);
                    email.From = new MailAddress(_objModelMail.From);
                    email.Subject = _objModelMail.Subject;
                    string Body = _objModelMail.Body;
                    email.Body = _objModelMail.Body;
                    FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/SavePDF/Challans/challan" + "_" + month + "_" + ChallanNo + ".pdf"), FileMode.Open, FileAccess.Read);
                    Attachment a = new Attachment(fs, "_" + month + "_" + ChallanNo + ".pdf", MediaTypeNames.Application.Octet);
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
                catch (Exception)
                {
                }
                aapReturn.Message = challanPath;
                return aapReturn;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        //Email DTO
        public class SendMailModel
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        //Funation For Genrating Challan Number
        public string challanNUm()
        {
            var _ChallanNumber = 0;
            var _challNum = db.DeliveryChallans.OrderByDescending(c => c.ChallanNumber).FirstOrDefault();
            int i = 1;
            if (_challNum != null)
            {
                _ChallanNumber = Convert.ToInt32(_challNum.ChallanNumber) + i;
                return _ChallanNumber.ToString();
            }
            else
            {
                _ChallanNumber = 1001;
                return _ChallanNumber.ToString();
            }
        }

        // Print Challan DTO Class
        public class PrintChallan
        {

            public string html { get; set; }
            public int ChallanId { get; set; }
            //public string ChallanNumber { get; set; }
            public string Name { get; set; }
            public float FinalQty { get; set; }
            public string ItemName { get; set; }
            public string Comment { get; set; }
            public string Address { get; set; }
            public string MobileNumber { get; set; }
            public DateTime Date { get; set; }

        }
    }
}
