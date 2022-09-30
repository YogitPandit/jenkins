using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NLog;
using DailyNeed.API.Controllers;
using System.Runtime.Caching;
using DailyNeed.Model;
using GenricEcommers.Models;
using DailyNeed.Model.NotMapped;
using System.Net;
using System.Text;
using System.IO;

namespace DailyNeed.API.Controllers
{
    public class StateAPIMethod
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        DailyNeedContext db = new DailyNeedContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        //Get all States
        public IEnumerable<State> Allstates()
        {
            if (db.States.AsEnumerable().Count() > 0)
            {
                return db.States.Where(p => p.Deleted == false).AsEnumerable();
            }
            else
            {
                List<State> state = new List<State>();
                return state.AsEnumerable();
            }
        }

        //State add method
        public State AddState(State state)
        {
            List<State> states = db.States.Where(c => c.StateName.Trim().Equals(state.StateName.Trim()) && c.Deleted == false).ToList();
            State objState = new State();
            if (states.Count == 0)
            {
                state.CreatedBy = state.CreatedBy;
                state.CreatedDate = indianTime;
                state.UpdatedDate = indianTime;
                db.States.Add(state);
                int id = db.SaveChanges();
                return state;
            }
            else
            {
                return objState;
            }
        }

        //Update State
        public State PutState(State objState)
        {

            State states = db.States.Where(x => x.Stateid == objState.Stateid && x.Deleted == false).FirstOrDefault();
            if (states != null)
            {
                states.UpdatedDate = indianTime;
                states.StateName = objState.StateName;
                states.AliasName = objState.AliasName;
                states.CreatedBy = objState.CreatedBy;
                states.CreatedDate = objState.CreatedDate;
                states.UpdateBy = objState.UpdateBy;
                objState.Message = "Successfully";
                db.States.Attach(states);
                db.Entry(states).State = EntityState.Modified;
                db.SaveChanges();
                return objState;
            }
            else
            {
                objState.Message = "Error";
                return objState;
            }
        }

        //Delete Method
        public bool DeleteState(int id)
        {
            try
            {
                State states = db.States.Where(x => x.Stateid == id && x.Deleted == false).FirstOrDefault();
                states.Deleted = true;
                db.States.Attach(states);
                db.Entry(states).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}