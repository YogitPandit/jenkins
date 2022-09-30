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
    [RoutePrefix("api/GroupController")]
    public class GroupController : ApiController
    {
        DailyNeedContext daily = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        [HttpPost]
        [Route("Add")]
        public GroupRegistration AddGroup(GroupRegistration obj)
        {
            try
            {
                var grpname = daily.GroupRegistrations.Where(f => f.GroupName == obj.GroupName && f.IsDeleted == false).FirstOrDefault();
                var fgrpname = daily.FarmRegistrations.Where(f => f.FarmID == obj.FarmId && f.IsDeleted == false).FirstOrDefault();
                var sgrpname = daily.ShedRegistrations.Where(f => f.ShedId == obj.ShedId && f.IsDeleted == false).FirstOrDefault();
                var Fid = daily.GroupRegistrations.Where(x => x.GroupID == obj.GroupID && x.IsDeleted == false).FirstOrDefault();

                if (grpname != null && Fid != null)

                {
                    obj.GroupName = obj.GroupName;
                    obj.Message = "Already exists";
                    obj.Flag = false;
                    return obj;
                }
                else
                {

                    obj.Message = "Successfully";
                    obj.Flag = true;
                    obj.CreateDate = indianTime;
                    obj.Farm = fgrpname;
                    obj.Shed = sgrpname;
                    daily.GroupRegistrations.Add(obj);
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
        public List<GroupRegistration> GetGroup()
        {
            try
            {


                var GroupList = daily.GroupRegistrations.Where(Group => Group.IsDeleted == false).Include("ShedId").ToList();
                return GroupList;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }



        [HttpGet]
        [Route("GetById")]
        public List<GroupRegistration> GetSed(int id)
        {
            try
            {

                var ShedList = daily.GroupRegistrations.Where(Group => Group.GroupID == id && Group.IsDeleted == false).ToList();
                return ShedList;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }

        [HttpGet]
        [Route("GetShedByFarmId")]
        public List<ShedRegistration> GetShedByFarm(int id)
        {
            try
            {

                var ShedList = daily.ShedRegistrations.Where(s => s.FarmId.FarmID == id && s.IsDeleted == false).ToList();
                return ShedList;
            }
            catch (Exception ex)
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
                GroupRegistration grp = daily.GroupRegistrations.Where(x => x.GroupID == id && x.IsDeleted == false).FirstOrDefault();
                grp.IsDeleted = true;
                daily.GroupRegistrations.Attach(grp);
                daily.Entry(grp).State = EntityState.Modified;
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

        public GroupRegistration PutState(GroupRegistration objState)
        {

            GroupRegistration states = daily.GroupRegistrations.Where(x => x.GroupID == objState.GroupID && x.IsDeleted == false).FirstOrDefault();
            FarmRegistration fstates = daily.FarmRegistrations.Where(x => x.FarmID == objState.FarmId && x.IsDeleted == false).FirstOrDefault();
            ShedRegistration sstates = daily.ShedRegistrations.Where(x => x.ShedId == objState.ShedId && x.IsDeleted == false).FirstOrDefault();

            if (states != null)


            {

                states.UpdateDate = indianTime;
                states.GroupName = objState.GroupName;
                states.Remark = objState.Remark;
                states.CreatedBy = objState.CreatedBy;
                states.CreateDate = objState.CreateDate;
                states.UpdatedBy = objState.UpdatedBy;
                states.UpdateDate = objState.UpdateDate;
                states.DeletionDate = objState.DeletionDate;
                states.IsDeleted = objState.IsDeleted;
                states.Flag = objState.Flag;
                states.Farm = fstates;
                states.Shed = sstates;
                objState.Message = "Successfully";
                daily.GroupRegistrations.Attach(states);
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



