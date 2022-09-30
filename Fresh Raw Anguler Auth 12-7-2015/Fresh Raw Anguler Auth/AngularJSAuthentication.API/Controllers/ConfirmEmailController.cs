using AngularJSAuthentication.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AngularJSAuthentication.API.Controllers
{
    public class Confirmed
    {
         public bool isConfirmed { get; set; }
        public string Error { get; set; }
    }
    public class ConfirmEmailController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("api/ConfirmEmail")]
        [System.Web.Http.AcceptVerbs("GET")]
        public async Task<Confirmed> Get(string id)
        {
           string uid = Encoding.ASCII.GetString(HttpServerUtility.UrlTokenDecode(id));
            Confirmed c = new Confirmed();
            string fullstring = Util.Decrypt(uid, true);
            int index = fullstring.IndexOf("{GreenTime}");
           string  UserName = fullstring.Substring(0, index);
            string Password = fullstring.Substring(index + 11);

            AuthContext context = new AuthContext();
           IdentityUser user = null;
          
            People ps = context.Peoples.Where(p => p.Email == UserName).SingleOrDefault();
            ps.EmailConfirmed = true;

            using (AuthRepository _repo = new AuthRepository())
            {

                user = await _repo.FindUser(UserName, Password);


                if (user != null)
                {
                    context.PutPeople(ps);
                    c.isConfirmed = true;

                    return c;
                }
            }
        
            return c;
        }
    }
}
