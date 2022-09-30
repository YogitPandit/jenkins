using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;


   
namespace DailyNeed.API.Controllers
{

    [RoutePrefix("api/CattleTypeController")]
    public class CattleTypeController : ApiController
    {
        DailyNeedContext daily = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        [HttpPost]
        [Route("Add")]

        public CattleType AddCattle(CattleType obj)
        {
            
            try
            {
                var catname = daily.CattleTypes.Where(f => f.CattleName == obj.CattleName && f.IsDeleted == false).FirstOrDefault();
                if (catname != null)
                {
                    obj.CattleName = obj.CattleName;
                    obj.Message = "Already exists";
                    obj.Flag = false;
                    return obj;
                }
                else
                {

                    obj.Message = "Successfully";
                    obj.Flag = true;
                    obj.CreateDate = indianTime;
                    daily.CattleTypes.Add(obj);
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
        public List<CattleType> GetCattle()
        {
            try
            {

                var CattleList = daily.CattleTypes.Where(Cattle => Cattle.IsDeleted == false).ToList();
                return CattleList;
            }
            catch (Exception)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }


        [HttpGet]
        [Route("GetById")]
        public List<CattleType> GetCatt(int id)
        {
            try
            {

                var CatlList = daily.CattleTypes.Where(Cattle => Cattle.CattleId == id && Cattle.IsDeleted == false).ToList();
                return CatlList;
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
                CattleType cattype = daily.CattleTypes.Where(x => x.CattleId == id && x.IsDeleted == false).FirstOrDefault();
                cattype.IsDeleted = true;
                daily.CattleTypes.Attach(cattype);
                daily.Entry(cattype).State = EntityState.Modified;
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

        public CattleType PutState(CattleType objState)
        {

            CattleType states = daily.CattleTypes.Where(y => y.CattleId == objState.CattleId && y.IsDeleted == false).FirstOrDefault();
            if (states != null)
            {

                states.UpdateDate = indianTime;
                states.CattleName = objState.CattleName;
                states.CattleAlias = objState.CattleAlias;
                states.CreatedBy = objState.CreatedBy;
                states.UpdateDate = objState.UpdateDate;
                states.UpdatedBy = objState.UpdatedBy;
                states.IsDeleted = objState.IsDeleted;
                states.IsActive = objState.IsActive;

                states.CreateDate = objState.CreateDate;
                objState.Message = "Successfully";
                daily.CattleTypes.Attach(states);
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