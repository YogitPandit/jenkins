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

    [RoutePrefix("api/CattleRegistrationController")]
    public class CattleRegistrationController : ApiController
    {
        DailyNeedContext daily = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        [HttpPost]
        [Route("Add")]

        public CattleRegistration AddCattle(CattleRegistration obj)
        {

            try
            {
                var catreg = daily.CattleRegistrations.Where(f => f.TagNumber == obj.TagNumber && f.IsDeleted == false).FirstOrDefault();

                var gp = daily.GroupRegistrations.Where(g => g.GroupID == obj.GroupID && g.IsDeleted == false).FirstOrDefault();
                var sp = daily.ShedRegistrations.Where(g => g.ShedId == obj.ShedId && g.IsDeleted == false).FirstOrDefault();
                var fp = daily.FarmRegistrations.Where(g => g.FarmID == obj.FarmId && g.IsDeleted == false).FirstOrDefault();
                if (catreg != null)
                {
                    obj.TagNumber = obj.TagNumber;
                    obj.Message = "Already exists";
                    obj.Flag = false;
                    return obj;
                }
                else
                {

                    obj.Message = "Successfully";
                    obj.Flag = true;
                    obj.Group = gp;
                    obj.Shed = sp;
                    obj.Farm = fp;
                    obj.CreateDate = indianTime;
                    daily.CattleRegistrations.Add(obj);
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
        public CattleListData GetCatReg()
        {
            try
            {
                CattleListData lstDto = new CattleListData();
                //List<int> totalcattle = new List<int>();
                var CattleRegList = daily.CattleRegistrations.Where(CatReg => CatReg.IsDeleted == false).ToList();
                var calfList = daily.CattleRegistrations.Where(CatReg => CatReg.CattleType == "calf" && CatReg.IsDeleted == false).ToList();
                var heifer = daily.CattleRegistrations.Where(CatReg => CatReg.CattleType == "heifer" && CatReg.IsDeleted == false).ToList();
                var milkinglist = daily.CattleRegistrations.Where(CatReg => CatReg.CattleType == "milking" && CatReg.IsDeleted == false).ToList();
                var bulllist = daily.CattleRegistrations.Where(CatReg => CatReg.CattleType == "bull" && CatReg.IsDeleted == false).ToList();
                //Get data in list
                lstDto.CattleRegList = CattleRegList;
                lstDto.calfList = calfList;
                lstDto.heifer = heifer;
                lstDto.milkinglist = milkinglist;
                lstDto.bulllist = bulllist;
                return lstDto;

            }
            catch (Exception)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }


        [HttpGet]
        [Route("GetById")]
        public List<CattleRegistration> GetCatReg(int id)
        {
            try
            {

                var CatRegList = daily.CattleRegistrations.Where(Cattle => Cattle.CattleRegId == id && Cattle.IsDeleted == false).ToList();
                return CatRegList;
            }
            catch (Exception)
            {
                return null;
                //return Request.CreateResponse(HttpStatusCode.NotFound, "NO Data Aval!");

            }

        }




        [HttpGet]
        [Route("GetGroupByShedId")]
        public List<GroupRegistration> GetGroupByShed(int id)
        {
            try
            {

                var GroupList = daily.GroupRegistrations.Where(g => g.Shed.ShedId == id && g.IsDeleted == false).ToList();
                return GroupList;
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
                CattleRegistration cattlereg = daily.CattleRegistrations.Where(x => x.CattleRegId == id && x.IsDeleted == false).FirstOrDefault();
                cattlereg.IsDeleted = true;
                daily.CattleRegistrations.Attach(cattlereg);
                daily.Entry(cattlereg).State = EntityState.Modified;
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

        public CattleRegistration PutState(CattleRegistration objState)
        {

            CattleRegistration states = daily.CattleRegistrations.Where(y => y.CattleRegId == objState.CattleRegId && y.IsDeleted == false).FirstOrDefault();
            if (states != null)
            {
                states.CattleType = objState.CattleType;
                states.TagNumber = objState.TagNumber;
                states.AnimalName = objState.AnimalName;
                states.UploadImage = objState.UploadImage;
                states.BodyWeight = objState.BodyWeight;
                states.BreedId = objState.BreedId;
                states.Dateofbirth = objState.Dateofbirth;
                states.Agency = objState.Agency;
                states.Purchasedate = objState.Purchasedate;
                states.Expirydate = objState.Expirydate;
                states.Mother = objState.Mother;
                states.Father = objState.Father;
                states.Gender = objState.Gender;
                states.Calvingdate = objState.Calvingdate;
                states.Cost = objState.Cost;
                states.Market = objState.Market;
                states.Source = objState.Source;
                states.Vendor = objState.Vendor;
                states.Bodysize = objState.Bodysize;
                states.Pregnancymonth = objState.Pregnancymonth;
                states.Lastinseminationdate = objState.Lastinseminationdate;
                states.Lactation = objState.Lactation;
                states.Milkoutput = objState.Milkoutput;
                states.FarmId = objState.FarmId;
                states.SpeciesType = objState.SpeciesType;
                states.Shed = objState.Shed;
                states.Farm = objState.Farm;
                states.Group = objState.Group;
                states.IsAnimalPregnant = objState.IsAnimalPregnant;



                states.CreateDate = objState.CreateDate;
                objState.Message = "Successfully";
                daily.CattleRegistrations.Attach(states);
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

        public class CattleListData
        {
            public List<CattleRegistration> CattleRegList { get; set; }
            public List<CattleRegistration> calfList { get; set; }
            public List<CattleRegistration> heifer { get; set; }
            public List<CattleRegistration> milkinglist { get; set; }
            public List<CattleRegistration> bulllist { get; set; }
        }

    }
}
