using DailyNeed.API.Models;
using DailyNeed.API.Results;
using DailyNeed.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NLog;
using DailyNeed.Infrastructure;
using DailyNeed.Controllers;
using System.Data.Entity;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        string company;
        private DailyNeedRepository _repo = null;
        DailyNeedContext db = new DailyNeedContext();
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AccountController()
        {
            _repo = new DailyNeedRepository();
        }

        public Logger logger = LogManager.GetCurrentClassLogger();

        // POST API/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel, string flag)
        {
            People presult = null;
            var identity = User.Identity as ClaimsIdentity;
            DailyNeedContext context = new DailyNeedContext();
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
            logger.Info("Get async: ");
            if (userModel.Email == "" || userModel.Email == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Valid Email Is Requaired !";
                return Ok(PeopleDTO);
            }
            if (userModel.PeopleFirstName == "" || userModel.PeopleFirstName == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "People FirstName Is Requaired !";
                return Ok(PeopleDTO);
            }
            if (userModel.PeopleLastName == "" || userModel.PeopleLastName == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "People LastName Is Requaired !";
                return Ok(PeopleDTO);
            }
            if (userModel.Password == "" && userModel.Password == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Password Is Requaired !";
                return Ok(PeopleDTO);
            }
            if (userModel.CompanyName == "" || userModel.CompanyName == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "CompanyName Is Requaired !";
                return Ok(PeopleDTO);
            }
            //if (userModel.Address == "" || userModel.Address == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "Company Address Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            if (userModel.CompanyPhone == "" || userModel.CompanyPhone == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Company Phone Is Required !";
                return Ok(PeopleDTO);
            }
            if (userModel.CompanyPhone == "" || userModel.CompanyPhone == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Company Phone Is Required !";
                return Ok(PeopleDTO);
            }
            if (flag == "" || flag == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Flag Is Required !";
                return Ok(PeopleDTO);
            }
            var _Comp = context.Companies.Where(x => x.CompanyName == userModel.CompanyName || x.Name == userModel.CompanyName).FirstOrDefault();
            if (_Comp != null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Company Is Already Exists !";
                return Ok(PeopleDTO);
            }
            var _Email = context.Peoples.Where(x => x.Email == userModel.Email).FirstOrDefault();

            if (_Email != null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "User Email Is Already Exists !";
                return Ok(PeopleDTO);
            }
            try
            {
                byte Levels = 4;
                //User Info
                var user = new ApplicationUser()
                {
                    UserName = userModel.UserName,
                    Email = userModel.Email,
                    FirstName = userModel.PeopleFirstName,
                    LastName = userModel.PeopleLastName,
                    Level = Levels,
                    JoinDate = DateTime.Now.Date,
                    EmailConfirmed = true
                };
                userModel.UserName = userModel.Email;
                user.UserName = userModel.Email;
                string displayname = userModel.UserName;

                //Get company name from email address
                int index1 = userModel.Email.IndexOf("@") + 1;
                int index2 = userModel.Email.LastIndexOf(".");
                int length = index2 - index1;
                //string company = userModel.Email.Substring(index1, length);
                if (compid == 0)
                {
                    company = userModel.CompanyName;
                    var company1 = context.Companies.Where(x => x.Name == company).Select(x => x.CompanyName).FirstOrDefault();
                    if (company1 != null)
                    {
                        company = company1;
                    }
                    else
                    {
                        company = userModel.CompanyName;
                    }
                }
                if (compid != 0)
                {
                    company = context.Companies.Where(x => x.Id == compid).Select(x => x.CompanyName).FirstOrDefault();
                }
                //check if company is there or not 
                bool existingcompany = true;
                existingcompany = context.CompanyExists(company);
                Company c = null;
                //company info
                if (!existingcompany)
                {
                    c = new Company();
                    c.CompanyName = userModel.CompanyName;
                    c.Address = userModel.Address;
                    c.CompanyPhone = userModel.CompanyPhone;
                    c.CompanyZip = userModel.CompanyZip;
                    c.EmployeesCount = userModel.Employees;
                    c.GSTIN = userModel.GSTIN;
                    c.Name = company;
                    c = context.AddCompany(c);
                }
                if (c == null)
                {
                    c = context.Companies.Where(x => x.Name == company).FirstOrDefault();
                }
                userModel.DepartmentId = c.Id.ToString();

                IdentityResult result = await this.AppUserManager.CreateAsync(user, userModel.Password);
                Warehouse wh = context.Warehouses.Where(w => w.WarehouseId == userModel.PWarehouseId).FirstOrDefault();
                State st = context.States.Where(s => s.Stateid == userModel.Stateid).FirstOrDefault();
                City ct = context.Cities.Where(l => l.Cityid == userModel.Cityid).FirstOrDefault();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new DailyNeedContext()));
                var adminUser = manager.FindByName(displayname);
                // assign User role for super admin
                if (userModel.RoleName == null)
                {
                    userModel.RoleName = "HQ Master login";
                }
                manager.AddToRole(adminUser.Id, userModel.RoleName);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                if (result.Errors.Count() > 0)
                {
                    IHttpActionResult errorResult1 = GetErrorResult(result);
                    return errorResult1;
                }

                People p = null;
                try
                {
                    p = context.GetPeoplebyCompanyId(c.Id).Where(a => a.Email.Equals(userModel.Email)).SingleOrDefault();
                }
                catch (Exception ex) { }
                if (p == null)
                {
                    p = new People();
                    p.Email = userModel.Email;
                    p.Company = c;
                    p.Password = userModel.Password;
                    p.PeopleFirstName = userModel.PeopleFirstName;
                    p.PeopleLastName = userModel.PeopleLastName;
                    p.Warehouse = wh;
                    p.State = st;
                    p.City = ct;
                    p.UserLoginType = flag;
                    p.Mobile = userModel.Mobile;
                    if (compid == 0)
                    {
                        p.Active = true;

                    }
                    else
                    {
                        p.Active = true;
                    }
                    p.Permissions = userModel.RoleName;
                    IHttpActionResult errorResult1 = GetErrorResult(result);
                    if (errorResult1 != null)
                    {
                        return errorResult1;
                    }
                    else
                    {
                        context.AddPeople(p);
                        presult = p;
                    }
                }
                else
                {
                    userModel.Message = "Company Already Exits!";
                    //return Ok(userModel);
                }
                //Util.NotifyUsersForConfirmingRegistration(userModel.Email, userModel.Password);
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting async " + ex.Message);
            }

            logger.Info("end async ");
            presult.Message = "Successfully";
            return Ok(presult);
        }







        [AllowAnonymous]
        [Route("RegisterByMobile")]
        public async Task<IHttpActionResult> RegisterByMobile(UserModel userModel)
        {
            People presult = null;
            var identity = User.Identity as ClaimsIdentity;
            DailyNeedContext context = new DailyNeedContext();
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
            logger.Info("Get async: ");
            //if (userModel.Email == "" || userModel.Email == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "Valid Email Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (userModel.PeopleFirstName == "" || userModel.PeopleFirstName == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "People FirstName Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (userModel.PeopleLastName == "" || userModel.PeopleLastName == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "People LastName Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (userModel.Password == "" && userModel.Password == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "Password Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (userModel.CompanyName == "" || userModel.CompanyName == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "CompanyName Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (userModel.Address == "" || userModel.Address == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "Company Address Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (userModel.CompanyPhone == "" || userModel.CompanyPhone == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "Company CompanyPhone Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (userModel.CompanyPhone == "" || userModel.CompanyPhone == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "Company CompanyPhone Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            //if (flag == "" || flag == null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "Flag Is Requaired !";
            //    return Ok(PeopleDTO);
            //}
            var _Comp = context.Companies.Where(x => x.CompanyName == userModel.CompanyName || x.Name == userModel.CompanyName).FirstOrDefault();
            if (_Comp != null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Company Is Already Exists !";
                return Ok(PeopleDTO);
            }
            //var _Email = context.Peoples.Where(x => x.Email == userModel.Email).FirstOrDefault();

            //if (_Email != null)
            //{
            //    PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
            //    PeopleDTO.Message = "User Email Is Allready Exists !";
            //    return Ok(PeopleDTO);
            //}
            try
            {
                byte Levels = 4;
                //User Info
                var user = new ApplicationUser()
                {
                    UserName = userModel.UserName,
                    Email = userModel.MobileNumber,
                    FirstName = userModel.PeopleFirstName,
                    LastName = userModel.PeopleLastName,
                    Level = Levels,
                    MobileNumber = userModel.MobileNumber,
                    JoinDate = DateTime.Now.Date,
                    EmailConfirmed = true
                };
                userModel.UserName = userModel.MobileNumber;
                user.UserName = userModel.MobileNumber;
                string displayname = userModel.UserName;

                //Get company name from email address
                //int index1 = userModel.Email.IndexOf("@") + 1;
                //int index2 = userModel.Email.LastIndexOf(".");
                //int length = index2 - index1;
                //string company = userModel.Email.Substring(index1, length);
                //if (compid == 0)
                //{
                company = userModel.CompanyName;
                var company1 = context.Companies.Where(x => x.Name == company).Select(x => x.CompanyName).FirstOrDefault();
                if (company1 != null)
                {
                    company = company1;
                }
                else
                {
                    company = "DEMO";
                    userModel.Password = userModel.Password;
                }
                //}
                //if (compid != 0)
                //{
                //    company = context.Companies.Where(x => x.Id == compid).Select(x => x.CompanyName).FirstOrDefault();
                //}
                //check if company is there or not 
                //bool existingcompany = true;
                //existingcompany = context.CompanyExists(company);
                Company c = null;
                ////company info
                //if (!existingcompany)
                //{
                c = new Company();
                c.CompanyName = company;
                c.Address = userModel.Address;
                c.CompanyPhone = userModel.CompanyPhone;
                c.CompanyZip = userModel.CompanyZip;
                c.EmployeesCount = userModel.Employees;
                c.GSTIN = userModel.GSTIN;
                c.Name = company;
                c = context.AddCompany(c);
                //}
                //if (c == null)
                //{
                //    c = context.Companies.Where(x => x.Name == company).FirstOrDefault();
                //}
                userModel.DepartmentId = c.Id.ToString();

                IdentityResult result = await this.AppUserManager.CreateAsync(user, userModel.Password);
                //Warehouse wh = context.Warehouses.Where(w => w.WarehouseId == userModel.PWarehouseId).FirstOrDefault();
                //State st = context.States.Where(s => s.Stateid == userModel.Stateid).FirstOrDefault();
                //City ct = context.Cities.Where(l => l.Cityid == userModel.Cityid).FirstOrDefault();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new DailyNeedContext()));
                var adminUser = manager.FindByName(displayname);
                // assign User role for super admin
                if (userModel.RoleName == null)
                {
                    userModel.RoleName = "HQ Master login";
                }
                manager.AddToRole(adminUser.Id, userModel.RoleName);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                if (result.Errors.Count() > 0)
                {
                    IHttpActionResult errorResult1 = GetErrorResult(result);
                    return errorResult1;
                }

                People p = null;
                try
                {
                    p = context.GetPeoplebyCompanyId(c.Id).Where(a => a.Mobile.Equals(userModel.MobileNumber)).FirstOrDefault();
                }
                catch (Exception ex) { }
                if (p == null)
                {
                    p = new People();
                    p.Email = userModel.Email;
                    p.Company = c;
                    p.Password = userModel.Password;
                    p.PeopleFirstName = userModel.PeopleFirstName;
                    p.PeopleLastName = userModel.PeopleLastName;
                    //p.Warehouse = wh;
                    //p.State = st;
                    //p.City = ct;
                    //p.UserLoginType = flag;
                    p.Mobile = userModel.MobileNumber;
                    if (compid == 0)
                    {
                        p.Active = true;

                    }
                    else
                    {
                        p.Active = true;
                    }
                    p.Permissions = userModel.RoleName;
                    IHttpActionResult errorResult1 = GetErrorResult(result);
                    if (errorResult1 != null)
                    {
                        return errorResult1;
                    }
                    else
                    {
                        context.AddPeople(p);
                        presult = p;
                    }
                }
                else
                {
                    userModel.Message = "Company Already Exits!";
                    //return Ok(userModel);
                }
                //Util.NotifyUsersForConfirmingRegistration(userModel.Email, userModel.Password);
            }
            catch (Exception ex)
            {
                logger.Error("Error in getting async " + ex.Message);
            }

            logger.Info("end async ");
            presult.Message = "Successfully";
            return Ok(presult);
        }

      
        
        
        
        
        
        
        
        //PUT API/Acount/Profile Update
        [AllowAnonymous]
        [Route("UpdateProfile")]
        public object UpdateProfile(UserModel userModel)
        {
            People presult = null;
            var identity = User.Identity as ClaimsIdentity;
            DailyNeedContext context = new DailyNeedContext();
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
            logger.Info("Get async: ");

            //Finding Company By Company Id (Coming by token access)
            var _company = context.Companies.Where(x => x.Id == compid).FirstOrDefault();
            if (_company == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Company Is Not Exists !";
                return PeopleDTO;
            }
            //Finding Employee By Employee Number and CompanyId (Coming by token access)
            var _people = context.Peoples.Where(x => x.Mobile == userModel.MobileNumber && x.Company.Id == compid).FirstOrDefault();
            if (_people == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "People Is Not Exists !";
                return PeopleDTO;
            }

            if (userModel.PeopleFirstName == "" || userModel.PeopleFirstName == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "People FirstName Is Required !";
                return PeopleDTO;
            }
            if (userModel.PeopleLastName == "" || userModel.PeopleLastName == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "People LastName Is Required !";
                return PeopleDTO;
            }
            if (userModel.Address == "" || userModel.Address == null)
            {
                PeopleExitsDTO PeopleDTO = new PeopleExitsDTO();
                PeopleDTO.Message = "Company Address Is Required !";
                return PeopleDTO;
            }

            if (userModel.CompanyName != "")
            {
                _company.CompanyName = userModel.CompanyName;
                _company.Name = userModel.CompanyName;
            }
            else
            {
                _company.CompanyName = userModel.PeopleFirstName + " " + userModel.PeopleLastName;
                _company.Name = userModel.PeopleFirstName + " " + userModel.PeopleLastName;
            }
            _company.EmployeesCount = userModel.Employees;
            _company.CompanyZip = userModel.CompanyZip;
            _company.CompanyPhone = userModel.CompanyPhone;
            _company.Address = userModel.Address;
            _company.Active = true;
            _company.Deleted = false;

            db.Companies.Attach(_company);
            db.Entry(_company).State = EntityState.Modified;
            db.SaveChanges();

            _people.Email = userModel.Email;
            _people.Company = _company;
            _people.Password = "123456";
            _people.PeopleFirstName = userModel.PeopleFirstName;
            _people.PeopleLastName = userModel.PeopleLastName;
            _people.Active = true;
            _people.Deleted = false;
            //p.Warehouse = wh;
            //p.State = st;
            //p.City = ct;
            //p.UserLoginType = flag;
            _people.Mobile = userModel.MobileNumber;

            db.Peoples.Attach(_people);
            db.Entry(_people).State = EntityState.Modified;
            db.SaveChanges();
            presult = _people;


            logger.Info("end async ");
            presult.Message = "Successfully";
            return presult;
        }

        //GET API/Acount/Profile GET
        [Route("GETProfile")]
        public UserModel GetProfile()
        {
            var identity = User.Identity as ClaimsIdentity;
            DailyNeedContext context = new DailyNeedContext();
            int compid = 0, userid = 0;
            string MobileNumber = "";
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
                if (claim.Type == "MobileNumber")
                {
                    MobileNumber = claim.Value;
                }
            }
            logger.Info("Get async: ");

            //Finding Profile By Company Id (Coming by token access)
            // var _userProfile = context.Peoples.Where(x => x.Mobile == MobileNumber && x.Company.Id==compid).FirstOrDefault();
            UserModel _userProFile = (from e in db.Peoples
                                      join comp in db.Companies on e.Company.Id equals comp.Id
                                      where e.Mobile == MobileNumber && e.Active == true && e.Deleted == false && e.Company.Id == compid
                                      select new UserModel
                                      {
                                          Email = e.Email,
                                          PeopleFirstName = e.PeopleFirstName,
                                          PeopleLastName = e.PeopleLastName,
                                          CompanyName = comp.CompanyName,
                                          Address = comp.Address,
                                          CompanyPhone = comp.CompanyPhone,
                                          CompanyZip = comp.CompanyZip,
                                          Message="Success",
                                          Success=true
                                         
                                      }).FirstOrDefault();
            return _userProFile;
        }


        public class PeopleExitsDTO
        {
            public string Message { get; set; }
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);

        }

        // POST api/Account/RegisterExternal
        [AllowAnonymous]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            user = new IdentityUser() { UserName = model.UserName };

            IdentityResult result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

            return Ok(accessTokenResponse);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

            return Ok(accessTokenResponse);

        }


        [Route("ChangePassword")]
        public async Task<IHttpActionResult> UpdatePassword(UserModel userModel)
        {

            string userName = userModel.UserName;

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new DailyNeedContext()));

            var user = manager.FindByName(userName);
            if (user.PasswordHash != null)
            {
                manager.RemovePassword(user.Id);
            }
            try
            {
                manager.AddPassword(user.Id, userModel.Password);
            }
            catch (Exception es)
            {
            }


            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private new IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client = _repo.FindClient(clientId);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

            if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            }

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "xxxxxx";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                          new JProperty("CompanyId", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }

        #endregion
    }
}