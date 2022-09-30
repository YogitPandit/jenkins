using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/upload")]
    public class UploadController : ApiController
    {

        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile()
        {
            HttpResponseMessage op = new HttpResponseMessage();
            var data = Request.Content.IsMimeMultipartContent();
            if (Request.Content.IsMimeMultipartContent())
            {
                //fileList f = new fileList();
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                var filefromreq = provider.Contents[0];
                Stream _id = filefromreq.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(_id);
                string filename = filefromreq.Headers.ContentDisposition.FileName.Trim('\"');
                //f.originalFileName = filename;
                byte[] buffer = await filefromreq.ReadAsByteArrayAsync();
                //f.byteArray = buffer;
                string mime = filefromreq.Headers.ContentType.ToString();
                Stream stream = new MemoryStream(buffer);
                //var fs = File.WriteAllBytes("", buffer);

                var FileUrl = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadImages"), filename);
                File.WriteAllBytes(FileUrl, buffer.ToArray());


                op.Message = "Succesfully";
                op.Success = true;
            }
            else
            {
                op.Message = "Error";
                op.Success = false;
            }
            return op;
        }



        public class HttpResponseMessage
        {
            public string Message { get; set; }
            public bool Success { get; set; }
        }
    }
}

