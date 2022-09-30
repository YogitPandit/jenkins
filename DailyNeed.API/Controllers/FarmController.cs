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
    [RoutePrefix("api/FarmController")]
    public class FarmController : ApiController
    {

        DailyNeedContext daily = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        [HttpPost]
        [Route("Add")]
        public FarmRegistration AddFarm(FarmRegistration obj)
        {
            //FarmRegistration farmregistration = new FarmRegistration();
            try
            {
                var framnae = daily.FarmRegistrations.Where(f => f.FarmName == obj.FarmName && f.IsDeleted == false).FirstOrDefault();
                if (framnae != null)
                {
                    obj.FarmName = obj.FarmName;
                    obj.Message = "Already exists";
                    obj.Flag = false;
                    return obj;
                }
                else
                {

                    obj.Message = "Successfully";
                    obj.Flag = true;
                    obj.CreateDate = indianTime;
                    daily.FarmRegistrations.Add(obj);
                    daily.SaveChanges();
                    return obj;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpGet]
        [Route("Get")]
        public List<FarmRegistration> GetFarm()
        {
            try
            {

                var FarmList = daily.FarmRegistrations.Where(farm => farm.IsDeleted == false).ToList();
                return FarmList;
            }
            catch (Exception)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }


        [HttpGet]
        [Route("GetById")]
        public List<FarmRegistration> GetSed(int id)
        {
            try
            {

                var ShedList = daily.FarmRegistrations.Where(Farm => Farm.FarmID == id && Farm.IsDeleted == false).ToList();
                return ShedList;
            }
            catch (Exception)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }




        private object GetDetail()
        {
            return null;
        }

        // Delete Method
        [HttpDelete]
        [Route("Delete")]
        public bool DeleteState(int id)
        {
            try
            {
                FarmRegistration frm = daily.FarmRegistrations.Where(x => x.FarmID == id && x.IsDeleted == false).FirstOrDefault();
                frm.IsDeleted = true;
                daily.FarmRegistrations.Attach(frm);
                daily.Entry(frm).State = EntityState.Modified;
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

        public FarmRegistration PutState(FarmRegistration objState)
        {

            FarmRegistration states = daily.FarmRegistrations.Where(x => x.FarmID == objState.FarmID && x.IsDeleted == false).FirstOrDefault();
            if (states != null)
            {

                states.UpdateDate = indianTime;
                states.FarmName = objState.FarmName;
                states.Address = objState.Address;
                states.Contact = objState.Contact;
                states.PrimaryContactNumber = objState.PrimaryContactNumber;
                states.EmailId = objState.EmailId;
                states.Remark = objState.Remark;
                states.IsDefault = objState.IsDefault;
                states.CreatedBy = objState.CreatedBy;
                states.UpdateDate = objState.UpdateDate;

                states.UpdatedBy = objState.UpdatedBy;

                states.DeletionDate = objState.DeletionDate;

                states.IsDeleted = objState.IsDeleted;

                states.Company = objState.Company;


                states.CreateDate = objState.CreateDate;
                objState.Message = "Successfully";
                daily.FarmRegistrations.Attach(states);
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
