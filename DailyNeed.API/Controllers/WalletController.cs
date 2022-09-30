using DailyNeed.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace DailyNeed.API.Controllers
{
    public class WalletController : ApiController
    {
        DailyNeedContext db = new DailyNeedContext();
        //Get Customer Walet Informatin (Perticular Customer)
        [Authorize]
        [Route("GetCustWallet")]
        public CustomerWallet GetCustWallet(int custid)
        {
            List<Warehouse> ass = new List<Warehouse>();
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                int compid = 0, userid = 0;
                int Warehouse_id = 0;

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
                var Wallet = db.CustomerWallets.Where(x => x.CustomerLink.CustomerLinkId == custid).FirstOrDefault();
                return Wallet;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
