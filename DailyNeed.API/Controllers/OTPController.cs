using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/OTP")]
    public class OTPController : ApiController
    {
        DailyNeedContext db = new DailyNeedContext();
        #region Generate Random OTP
        /// <summary>
        /// Created by 18/12/2018 
        /// Create rendom otp
        /// </summary>
        /// <param name="iOTPLength"></param>
        /// <param name="saAllowedCharacters"></param>
        /// <returns></returns>
        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }
        #endregion
        [HttpPost]
        [Route("Genotp")]
        public OTP Getotp(string MobileNumber)
        {
            OTP a = null;
            //logger.Info("start Gen OTP: ");
            try
            {
                var mNumber = db.Peoples.Where(x => x.Mobile==MobileNumber).FirstOrDefault();
                if(mNumber == null)
                {
                    string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                    string sRandomOTP = GenerateRandomOTP(4, saAllowedCharacters);
                    string OtpMessage = " is Your DailyNeed Verification Code. :)";
                    string CountryCode = "91";
                    string Sender = "RAFETS";
                    string authkey = "a09db0959e8eca7040fe90b2693f4ecb";
                    int route = 4;
                    string path = "http://sms.bulksmsserviceproviders.com/api/send_http.php?authkey=" + authkey + "&mobiles=" + MobileNumber + "&message=" + sRandomOTP + " :" + OtpMessage + " &sender=" + Sender + "&route=" + route + "&country=" + CountryCode;

                    //string path ="http://sms.bulksmsserviceproviders.com/api/send_http.php?authkey=100498AhbWDYbtJT56af33e3&mobiles=9770838685&message= SK OTP is : " + sRandomOTP + " &sender=SHOPKR&route=4&country=91";

                    var webRequest = (HttpWebRequest)WebRequest.Create(path);
                    webRequest.Method = "GET";
                    webRequest.ContentType = "application/json";
                    webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:28.0) Gecko/20100101 Firefox/28.0";
                    webRequest.ContentLength = 0; // added per comment 
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                    webRequest.Accept = "*/*";
                    var webResponse = (HttpWebResponse)webRequest.GetResponse();
                    if (webResponse.StatusCode != HttpStatusCode.OK) Console.WriteLine("{0}", webResponse.Headers);
                    //logger.Info("OTP Genrated: " + sRandomOTP);
                    a = new OTP()
                    {
                        OtpNo = sRandomOTP,
                        MobileNumber = MobileNumber,
                        Message = "Success"
                    };                   
                }
                else
                {
                    a = new OTP()
                    {
                        OtpNo = "",
                        MobileNumber = "",
                        Message = "User Already Exists!"
                    };
                }
                return a;
            }
            catch (Exception)
            {
                //logger.Error("Error in OTP Genration.");
                return null;
            }
        }

        public class OTP
        {
            public string OtpNo { get; set; }
            public string MobileNumber { get; set; }
            public string Message { get; set; }
        }
    }
}
