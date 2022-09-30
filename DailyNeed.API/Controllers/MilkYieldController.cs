using DailyNeed.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;



namespace DailyNeed.API.Controllers
{
    [RoutePrefix("api/MilkYieldController")]
    public class MilkYieldController : ApiController
    {
        DailyNeedContext daily = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //public int ShedID { get; private set; }

        //[HttpPost]
        //[Route("Add")]
        //public MilkYield AddGroup(MilkYield obj)
        //{


        //    try
        //    {

        //        foreach (MilkYield milk in obj.milkingAnimal)

        //        {
        //            if (milk.Quantity > 0)
        //            {
        //                var dateCheck = obj.Date.Date;
        //                var myield = daily.MilkYields.Where(f => f.Cattle == milk.Cattle && f.IsDeleted == false && f.Date == dateCheck && f.Shift == milk.Shift).FirstOrDefault();
        //                var Fid = daily.MilkYields.Where(x => x.MilkYieldId == milk.MilkYieldId && x.IsDeleted == false).FirstOrDefault();
        //                var tg = daily.CattleRegistrations.Where(f => f.TagNumber == milk.TagNumber && f.IsDeleted == false).Include("Group").Include("Shed").Include("Farm").FirstOrDefault();

        //                if (myield != null && Fid != null)
        //                {
        //                    milk.TagNumber = milk.TagNumber;
        //                    milk.Cattle = milk.Cattle;
        //                    milk.Message = "Already exists";
        //                    milk.Flag = false;
        //                    // return milk;
        //                }
        //                else
        //                {
        //                    milk.Message = "Successfully";
        //                    milk.Flag = true;
        //                    milk.TagNumber = tg.TagNumber;
        //                    milk.Cattle = tg;
        //                    milk.Farm = tg.Farm;
        //                    milk.Shed = tg.Shed;
        //                    milk.Shift = obj.Shift;
        //                    milk.Date = dateCheck;
        //                    milk.Group = tg.Group;
        //                    milk.CreateDate = indianTime;
        //                    daily.MilkYields.Add(milk);
        //                    daily.SaveChanges();
        //                    // return milk;
        //                }
        //            }
        //        }
        //        return obj;
        //    }


        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        [HttpPost]
        [Route("Add")]
        public MilkYield AddMilk(MilkYield obj)
        {
            try
            {


                foreach (var o in obj.milkingAnimal)
                {

                    var ctId = daily.CattleRegistrations.Where(x => x.TagNumber == o.TagNumber).Include("Group").Include("Shed").Include("Farm").FirstOrDefault();
                    var dateCheck = obj.Date.Date;
                    var cattleId = daily.MilkYields.Where(x => x.Cattle.CattleRegId == ctId.CattleRegId && x.Date == dateCheck && x.Shift == obj.Shift).FirstOrDefault();
                   // var groupid = daily.GroupRegistrations.Where(x => x.GroupID == ctId.Group.GroupID).FirstOrDefault();
                    

                    if (cattleId != null)
                    {
                        o.Message = "Already exists";
                        o.Flag = false;
                        return o;
                    }
                    else
                    {

                        o.Shift = obj.Shift;
                        o.Message = "Successfully Added";
                        o.Flag = true;
                        o.Date = dateCheck;
                        //milk.Peoples = daily.Peoples.Where(x => x.PeopleID == userid).FirstOrDefault();
                        o.Cattle = ctId;
                        o.Group = ctId.Group;
                        o.Shed = ctId.Shed;
                        o.Farm = ctId.Farm;
                        o.CreateDate = indianTime;
                        daily.MilkYields.Add(o);
                        daily.SaveChanges();
                        return o;
                    }

                }
                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("Get")]
        public List<MilkYield> GetMilk()
        {
            try
            {


                var MilkList = daily.MilkYields.Where(Milk => Milk.IsDeleted == false).ToList();
                return MilkList;


            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }



        [HttpGet]
        [Route("GetById")]
        public List<MilkYield> GetMilk(int id)
        {
            try
            {


                var MilkList = daily.MilkYields.Where(Milk => Milk.MilkYieldId == id && Milk.IsDeleted == false).ToList();
                return MilkList;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }


        [HttpGet]
        [Route("GetByDate")]
        public IEnumerable<MilkYield> GetAttendanceByDate(DateTime date)
        {
            try
            {

                var dateCheck = date.Date;
                var inven = daily.MilkYields.Where(x => x.Date == dateCheck && x.IsDeleted == false).ToList();
                return inven;

            }
            catch (Exception ex)
            {
                return null;

            }
        }





        [HttpGet]
        [Route("GetByCattle")]
        public List<CattleRegistration> GetAllCattle()
        {
            try
            {


                var CattleList = daily.CattleRegistrations.Where(Milk => Milk.CattleType == "Milking" && Milk.IsDeleted == false).ToList();
                return CattleList;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }



        [HttpGet]
        [Route("GetFarmByCattle")]
        public List<CattleRegistration> GetFarmCattle(int farmId)
        {
            try
            {


                var CattleFarmList = daily.CattleRegistrations.Where(Milk => Milk.CattleType == "Milking" && Milk.IsDeleted == false && Milk.Farm.FarmID == farmId).ToList();
                return CattleFarmList;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }








        [HttpGet]
        [Route("GetCattleByFarmSorting")]
        public List<CattleRegistration> GetCattleByFarmSorting(int farmId)
        {
            try
            {


                var CattleFarmSort = daily.CattleRegistrations.Where(Milk => Milk.CattleType == "Milking" && Milk.IsDeleted == false && Milk.Farm.FarmID == farmId).ToList();
                return CattleFarmSort;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }





        [HttpGet]
        [Route("GetCattleByShedSorting")]
        public List<CattleRegistration> GetCattleByShedSorting(int shedId)
        {
            try
            {

                var CattleSort = daily.CattleRegistrations.Where(Milk => Milk.CattleType == "Milking" && Milk.IsDeleted == false && Milk.Shed.ShedId == shedId).ToList();
                return CattleSort;
            }
            catch (Exception ex)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }


        [HttpGet]
        [Route("GetCattleByGroupSorting")]
        public List<CattleRegistration> GetCattleByGroupSorting(int groupId)
        {
            try
            {

                var CattlegrpSort = daily.CattleRegistrations.Where(Milk => Milk.CattleType == "Milking" && Milk.IsDeleted == false && Milk.Group.GroupID == groupId).ToList();
                return CattlegrpSort;
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
        public object DeleteState(int id)
        {
            try
            {


                MilkYield milk = daily.MilkYields.Where(x => x.MilkYieldId == id && x.IsDeleted == false).FirstOrDefault();
                milk.IsDeleted = true;
                daily.MilkYields.Attach(milk);
                daily.Entry(milk).State = EntityState.Modified;
                milk.DeletionDate = indianTime;
                daily.SaveChanges();
                milk.Message = "Successfully Deleted";
                return milk;
            }
            catch
            {
                return false;
            }
        }



        //Update State
        [HttpPut]
        [Route("Put")]

        public List<MilkYield> PutState(List<MilkYield> objState)

        {
            try
            {



                foreach (var o in objState)
                {

                    MilkYield states = daily.MilkYields.Where(x => x.MilkYieldId == o.MilkYieldId && x.IsDeleted == false).FirstOrDefault();
                    if (states != null)

                    {


                        states.Quantity = o.Quantity;
                        states.Fat = o.Fat;
                        states.Snf = o.Snf;
                        states.Shift = o.Shift;
                        states.TagNumber = o.TagNumber;
                        states.Cattle = o.Cattle;
                        states.Date = o.Date.Date;
                        states.Shed = o.Shed;
                        states.Group = o.Group;
                        states.Farm = o.Farm;
                        states.Cattle = o.Cattle;



                        states.UpdateDate = indianTime;
                        states.CreatedBy = states.CreatedBy;
                        states.CreateDate = states.CreateDate;
                        states.UpdatedBy = states.UpdatedBy;
                        states.DeletionDate = states.DeletionDate;
                        states.IsDeleted = states.IsDeleted;
                        states.Message = "Successfully";
                        daily.MilkYields.Attach(states);
                        daily.Entry(states).State = EntityState.Modified;
                        daily.SaveChanges();
                        //return objState;
                    }
                    else
                    {
                        //states.Message = "Error";
                        //return objState;
                    }
                }
                   return objState;
                
            }

            catch (Exception)
            {

                //objState.Message = "Failed and there is an Exception.";
                return objState;
            }



        }


        [HttpPost]
        [Route("AddMilk")]
        public MilkYield AddMilkYield(MilkYield milk)
        {


            try
            {


                var FId = daily.FarmRegistrations.Where(x => x.FarmID == milk.FarmId && x.IsDeleted == false).FirstOrDefault();
                var SId = daily.ShedRegistrations.Where(x => x.ShedId == milk.ShedId && x.IsDeleted == false).FirstOrDefault();
                var GId = daily.GroupRegistrations.Where(x => x.GroupID == milk.GroupID && x.IsDeleted == false).FirstOrDefault();
                var CId = daily.CattleRegistrations.Where(x => x.CattleRegId == milk.CattleId && x.IsDeleted == false).FirstOrDefault();

                //MilkYield milkingAnimal = null;
                var myield = daily.MilkYields.Where(f => f.Cattle == milk.Cattle && f.IsDeleted == false).FirstOrDefault();
                var Fid = daily.MilkYields.Where(x => x.MilkYieldId == milk.MilkYieldId && x.IsDeleted == false).FirstOrDefault();
                if (myield != null && Fid != null)
                {
                    milk.Cattle = milk.Cattle;
                    milk.Message = "Already exists";
                    milk.Flag = false;
                    // return milk;
                }
                else
                {
                    milk.Message = "Successfully";
                    milk.Flag = true;
                    milk.Farm = FId;
                    milk.Shed = SId;
                    milk.Group = GId;
                    milk.Cattle = CId;
                    milk.CreateDate = indianTime;
                    daily.MilkYields.Add(milk);
                    daily.SaveChanges();
                    // return milk;
                }

                return milk;
            }


            catch (Exception ex)
            {
                return null;
            }
        }



    }
}

