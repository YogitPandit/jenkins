
using DailyNeed.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DailyNeed.API.Providers;
using System.Web;
using DailyNeed.Infrastructure;
using Microsoft.AspNet.Identity.Owin;

namespace DailyNeed.API.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        DailyNeedContext dc = new DailyNeedContext();
        public Logger logger = LogManager.GetCurrentClassLogger();
        int User_Id;
        string ac_Type;
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
         {
            context.Validated();
            return Task.FromResult<object>(null);


            //string clientId = string.Empty;
            //string clientSecret = string.Empty;
            //Client client = null;

            //if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            //{
            //    context.TryGetFormCredentials(out clientId, out clientSecret);
            //}

            //if (context.ClientId == null)
            //{
            //    //Remove the comments from the below line context.SetError, and invalidate context 
            //    //if you want to force sending clientId/secrects once obtain access tokens. 
            //    context.Validated();
            //    //context.SetError("invalid_clientId", "ClientId should be sent.");
            //    return Task.FromResult<object>(null);
            //}

            //using (AuthRepository _repo = new AuthRepository())
            //{
            //    client = _repo.FindClient(context.ClientId);
            //}

            //if (client == null)
            //{
            //    context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
            //    return Task.FromResult<object>(null);
            //}
            //if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            //{
            //    if (string.IsNullOrWhiteSpace(clientSecret))
            //    {
            //        context.SetError("invalid_clientId", "Client secret should be sent.");
            //        return Task.FromResult<object>(null);
            //    }
            //    else
            //    {
            //        if (client.Secret != Helper.GetHash(clientSecret))
            //        {
            //            context.SetError("invalid_clientId", "Client secret is invalid.");
            //            return Task.FromResult<object>(null);
            //        }
            //    }
            //}
            //if (!client.Active)
            //{
            //    context.SetError("invalid_clientId", "Client is inactive.");
            //    return Task.FromResult<object>(null);
            //}
            //context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            //context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            //context.Validated();
            //return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = "*";
            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "User did not confirm email.");
                    return;
                }

                iDailyNeedContext con = new DailyNeedContext();
               // People p = con.getPersonIdfromEmail(user.Email);
                People p = con.getPersonIdfrommobile(user.MobileNumber);
                int UserId = p.PeopleID;
                if (!p.Active)
                {
                    context.SetError("invalid_grant", "Please check your registered email address to validate email address.");
                    return;
                }
                if (UserId == 0)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                ClaimsIdentity identity = await user.GenerateUserIdentityAsync(userManager, "JWT");
                identity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
                identity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(identity));
                var companyname = dc.Companies.Where(c => c.Id == p.Company.Id).Select(c => c.CompanyName).FirstOrDefault();
                UserAccessPermission uap = con.getRoleDetail(p.Permissions); // Get Role Access Detail

                //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                //identity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
                identity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(identity));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, p.Permissions));
                identity.AddClaim(new Claim("Warehouseid", p.WarehouseId.ToString()));
                identity.AddClaim(new Claim("firsttime", "true"));
                identity.AddClaim(new Claim("compid", p.Company.Id.ToString()));
                identity.AddClaim(new Claim("companyname", companyname.ToString()));
                identity.AddClaim(new Claim("mobile", p.Mobile.ToString()));
                identity.AddClaim(new Claim("email", Convert.ToString(p.Email)));
                identity.AddClaim(new Claim("MobileNumber", user.MobileNumber.ToString()));
                identity.AddClaim(new Claim("Email", Convert.ToString(p.Email)));
                identity.AddClaim(new Claim("Level", user.Level.ToString()));
                identity.AddClaim(new Claim("userid", UserId.ToString()));
                identity.AddClaim(new Claim("Admin", uap.Admin.ToString()));
                identity.AddClaim(new Claim("Delivery", uap.Delivery.ToString()));
                identity.AddClaim(new Claim("TaxMaster ", uap.TaxMaster.ToString()));
                identity.AddClaim(new Claim("Customer", uap.Customer.ToString()));
                identity.AddClaim(new Claim("Supplier", uap.Supplier.ToString()));
                identity.AddClaim(new Claim("Warehouse", uap.Warehouse.ToString()));
                identity.AddClaim(new Claim("CurrentStock", uap.CurrentStock.ToString()));
                identity.AddClaim(new Claim("OrderMaster", uap.OrderMaster.ToString()));
                identity.AddClaim(new Claim("DamageStock", uap.DamageStock.ToString()));
                identity.AddClaim(new Claim("ItemMaster", uap.ItemMaster.ToString()));
                identity.AddClaim(new Claim("Reports", uap.Reports.ToString()));
                identity.AddClaim(new Claim("StatisticalRep", uap.StatisticalRep.ToString()));
                identity.AddClaim(new Claim("Offer", uap.Offer.ToString()));
                identity.AddClaim(new Claim("Sales", uap.Sales.ToString()));
                identity.AddClaim(new Claim("AppPromotion", uap.AppPromotion.ToString()));
                identity.AddClaim(new Claim("ItemCategory", uap.ItemCategory.ToString()));
                identity.AddClaim(new Claim("CRM", uap.CRM.ToString()));
                identity.AddClaim(new Claim("Request", uap.Request.ToString()));
                identity.AddClaim(new Claim("UnitEconomics", uap.UnitEconomics.ToString()));
                identity.AddClaim(new Claim("Promopoint", uap.PromoPoint.ToString()));
                identity.AddClaim(new Claim("News", uap.News.ToString()));
                identity.AddClaim(new Claim("DisplayName", p.DisplayName));
                identity.AddClaim(new Claim("username", (p.PeopleFirstName + " " + p.PeopleLastName).ToString()));
                User_Id = UserId;
                ac_Type = p.Permissions;
                var props = new AuthenticationProperties(new Dictionary<string, string> { });
                if (p.Permissions == "HQ Master login")
                {
                    props = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            { "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId },
                            { "userName", context.UserName },
                            { "Warehouseid", p.WarehouseId.ToString() },
                            { "compid", p.Company.Id.ToString() },
                            { "companyname",companyname.ToString() },
                            { "Level", user.Level.ToString() },
                            { "role" ,p.Permissions },
                            { "mobile" ,p.Mobile },
                            { "email" ,p.Email },
                            { "MobileNumber" ,user.MobileNumber},
                            { "userid", UserId.ToString() },
                            //{ "Skcode",  p.Skcode },

                    { "Admin", uap.Admin.ToString() },
                    { "Delivery", uap.Delivery.ToString() },
                    { "TaxMaster", uap.TaxMaster.ToString() },
                    { "Customer", uap.Customer.ToString() },
                    { "Supplier", uap.Supplier.ToString() },
                    { "Warehouse", uap.Warehouse.ToString() },
                    { "CurrentStock", uap.CurrentStock.ToString() },
                    { "OrderMaster", uap.OrderMaster.ToString() },
                    { "DamageStock", uap.DamageStock.ToString() },
                    { "ItemMaster", uap.ItemMaster.ToString() },
                    { "Reports", uap.Reports.ToString() },
                    { "StatisticalRep", uap.StatisticalRep.ToString() },
                    { "Offer", uap.Offer.ToString() },
                    { "Sales", uap.Sales.ToString() },
                    { "AppPromotion", uap.AppPromotion.ToString() },
                    { "ItemCategory", uap.ItemCategory.ToString() },
                    { "CRM", uap.CRM.ToString() },
                    { "Request", uap.Request.ToString() },
                    { "UnitEconomics", uap.UnitEconomics.ToString() },
                    { "Promopoint", uap.PromoPoint.ToString() },
                    { "News", uap.News.ToString() }
               });
                }
                else
                {
                    props = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            { "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId },
                            { "userName", context.UserName },
                            { "Warehouseid", p.WarehouseId.ToString() },
                            { "compid", p.Company.Id.ToString() },
                            { "compid", user.PhoneNumber },
                            { "role" ,p.Permissions },
                            { "mobile" ,p.Mobile },
                            { "Level", user.Level.ToString() },
                            { "email" ,p.Email },
                            { "userid", UserId.ToString() },

                    { "Admin", uap.Admin.ToString() },
                    { "Delivery", uap.Delivery.ToString() },
                    { "TaxMaster", uap.TaxMaster.ToString() },
                    { "Customer", uap.Customer.ToString() },
                    { "Supplier", uap.Supplier.ToString() },
                    { "Warehouse", uap.Warehouse.ToString() },
                    { "CurrentStock", uap.CurrentStock.ToString() },
                    { "OrderMaster", uap.OrderMaster.ToString() },
                    { "DamageStock", uap.DamageStock.ToString() },
                    { "ItemMaster", uap.ItemMaster.ToString() },
                    { "Reports", uap.Reports.ToString() },
                    { "StatisticalRep", uap.StatisticalRep.ToString() },
                    { "Offer", uap.Offer.ToString() },
                    { "Sales", uap.Sales.ToString() },
                    { "AppPromotion", uap.AppPromotion.ToString() },
                    { "ItemCategory", uap.ItemCategory.ToString() },
                    { "CRM", uap.CRM.ToString() },
                    { "Request", uap.Request.ToString() },
                    { "UnitEconomics", uap.UnitEconomics.ToString() },
                    { "Promopoint", uap.PromoPoint.ToString() },
                    { "News", uap.News.ToString() }
                        });
                }
                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                logger.Error("Unable to validate user {0}", context.UserName);
                logger.Error(ex.Message);
            }
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;
            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }
            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            //User traking Code strated....
            UserTraking us = new UserTraking();
            us.PeopleId = User_Id + "";
            us.Type = ac_Type;
            us.LoginTime = DateTime.Now;
            us.Remark = "login page ,";
            dc.UserTrakings.Add(us);
            dc.SaveChanges();
            //END User traking Code....
            return Task.FromResult<object>(null);
        }
    }
}