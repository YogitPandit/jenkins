using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/ShedController")]
    public class ShedController : ApiController
    {
        DailyNeedContext daily = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //public int ShedID { get; private set; }

        [HttpPost]
        [Route("Add")]
        public ShedRegistration AddGroup(ShedRegistration obj)
        {
            try
            {
                var shdname = daily.ShedRegistrations.Where(f => f.ShedName == obj.ShedName && f.IsDeleted == false).FirstOrDefault();
                var Fid = daily.ShedRegistrations.Where(x => x.ShedId == obj.ShedId && x.IsDeleted == false).FirstOrDefault();
                var fshedname = daily.FarmRegistrations.Where(f => f.FarmID == obj.FarmID && f.IsDeleted == false).FirstOrDefault();
                if (shdname != null && Fid !=null)
                {
                    obj.ShedName = obj.ShedName;
                    obj.Message = "Already exists";
                    obj.Flag = false;
                    return obj;
                }
                else
                {

                    obj.Message = "Successfully";
                    obj.Flag = true;
                    obj.CreateDate = indianTime;
                    obj.FarmId = fshedname;
                    daily.ShedRegistrations.Add(obj);
                    daily.SaveChanges();                    
                    return obj;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        [Route("Get")]
        public List<ShedRegistration> GetShed()
        {
            try
            {

                var ShedList = daily.ShedRegistrations.Where(Shed => Shed.IsDeleted == false).Include("FarmId").ToList();
                return ShedList;
               

            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }



        [HttpGet]
        [Route("GetById")]
        public List<ShedRegistration> GetSed(int id)
        {
            try
            {

                var ShedList = daily.ShedRegistrations.Where(Shed => Shed.ShedId == id && Shed.IsDeleted == false).ToList();
                return ShedList;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }

        

        // Delete Method
        [HttpDelete]
        [Route("Delete")]
        public bool DeleteState(int id)
        {
            try
            {
                ShedRegistration shed = daily.ShedRegistrations.Where(x => x.ShedId == id && x.IsDeleted == false).FirstOrDefault();
                shed.IsDeleted = true;
                daily.ShedRegistrations.Attach(shed);
                daily.Entry(shed).State = EntityState.Modified;
                daily.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }



        //Update State
        [HttpPut]
        [Route("Put")]

        public ShedRegistration PutState(ShedRegistration objState)
        {

            ShedRegistration states = daily.ShedRegistrations.Where(x => x.ShedId == objState.ShedId && x.IsDeleted == false).FirstOrDefault();
            FarmRegistration fstates = daily.FarmRegistrations.Where(x => x.FarmID == objState.FarmID && x.IsDeleted == false).FirstOrDefault();
            if (states != null)

            {

                states.UpdateDate = indianTime;
                states.ShedName = objState.ShedName;
                states.Remark = objState.Remark;
                states.CreatedBy = objState.CreatedBy;
                states.CreateDate = objState.CreateDate;
                states.UpdatedBy = objState.UpdatedBy;
                states.FarmId = fstates;
                objState.Message = "Successfully";
                daily.ShedRegistrations.Attach(states);
                daily.Entry(states).State = EntityState.Modified;
                daily.SaveChanges();
                return objState;
            }
            else
            {
                objState.Message = "Error";
                return objState;
            }
        }


    }
}
