using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Framework.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Server;
using System.Web.Http;

namespace AngularJSAuthentication.API.Controllers
{

    public class WeekTaskEventViewModel
    {

        public int  taskid { get; set; }

        public string projectname { get; set; }
        public int projectid { get; set; }
        public string AssignedTo { get; set; }
        public string clientname { get; set; }
        public double HoursAllotted { get; set; }
        public double HoursBilled { get; set; }
        public int clientid { get; set; }
        public string taskname { get; set; }
        public int tasktypeid { get; set; }
        public string tasktype { get; set; }
        public double d1 { get; set; }
        public double d2 { get; set; }
        public double d3 { get; set; }
        public double d4 { get; set; }
        public double d5 { get; set; }
        public double d6 { get; set; }
        public double d7 { get; set; }
        public double total { get; set; }
        public DateTime startdate { get; set; }
    }
    public class TFSController : ApiController
    {
        public class TFSTask
        {
            public int Id { get; set; }
            public string Title { get; set; }
          
          
        }

        List<WeekTaskEventViewModel> abc = new List<WeekTaskEventViewModel>();

        public string strProjectName = "Altruit SharePoint";
        public string strCollName = "https://tfs.webfortis.com/tfs/Webfortis Customer Solutions/";
        public IEnumerable<WeekTaskEventViewModel> Get(string startdate)
        {
            Uri tfsUri = new Uri("https://tfs.webfortis.com/tfs");


            try
            {
                System.Net.ICredentials credentials = new System.Net.NetworkCredential("agarcia", "Pass@word1", "webfortis.com");
                Microsoft.TeamFoundation.Client.TfsTeamProjectCollection teamProjectCollection = new Microsoft.TeamFoundation.Client.TfsTeamProjectCollection(new Uri(@"https://tfs.webfortis.com/tfs/Webfortis Customer Solutions"), credentials);
                teamProjectCollection.EnsureAuthenticated();
                Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore Store = (Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore)
                teamProjectCollection.GetService(typeof(Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore));
                string query = "SELECT * FROM WorkItems WHERE [System.TeamProject] = '" + strProjectName + "' AND [State] <> 'Closed' AND [State] <> 'Removed' AND [State] <> 'Done' AND [Assigned To] = 'Akhilesh Gandhi' ORDER BY [System.WorkItemType], [System.Id]";
                Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemCollection WIC = Store.Query(query);

   
                
                foreach (WorkItem wi in WIC)
                {
                    string AssignedTo = "";
                    //foreach (Revision revision in wi.Revisions)
                    //{
                    //    // Get value of fields in the work item revision
                    //    foreach (Field field in wi.Fields)
                    //    {
                    //        //if (revision.Fields[field.Name].Equals("Assigned To"))
                    //        //{
                    //        //    AssignedTo = revision.Fields[field.Name].Value.ToString();
                    //        //}
                    //        Console.WriteLine(revision.Fields[field.Name].Value);
                    //    }
                    //}
                 
                    WeekTaskEventViewModel tt = new WeekTaskEventViewModel();
                    tt.taskid = wi.Id;
                    tt.taskname = wi.Title;
                    tt.AssignedTo = wi.Fields["Assigned To"].Value.ToString();
                    tt.projectname = strProjectName;
                  
                   // AddHoursToField()
                    double hoursa = 0;
                    try
                    {
                    hoursa =   GetHoursFromField(wi,"Microsoft.VSTS.Scheduling.CompletedWork"); //   double.TryParse(wi.Fields["Original Estimate"].Value.ToString(), out hoursa);
                    }
                    catch (Exception ex) { }
                    double hoursb = 0;
                    try
                    {
                        hoursb = GetHoursFromField(wi, "Microsoft.VSTS.Scheduling.RemainingWork");  //double.TryParse(wi.Fields["Completed Work"].Value.ToString(), out hoursb);
                    }
                    catch (Exception ex) { }
                        tt.HoursAllotted = hoursa;
                    tt.HoursBilled = hoursb;
                    abc.Add(tt);
                }

            }
            catch (Exception ex)
            { 
            
            
            }
            return abc.AsEnumerable();


        }
         private double GetHoursFromField(WorkItem workItem, string fieldRef)
        {
               double num = 0.0;
            if (!string.IsNullOrEmpty(fieldRef) && workItem.Fields.Contains(fieldRef))
            {
                Field field = workItem.Fields[fieldRef];
                if (field.FieldDefinition.FieldType == FieldType.Double)
                {
                  
                    if (field.Value != null)
                    {
                        num = (double)field.Value;
                    }
                    return num; 
                    //double num2 = num + hours;
                    //if (num2 < 0.0)
                    //{
                    //    num2 = 0.0;
                    //}
                    //field.Value = Math.Round(num2, 2);
                }
            }
             return num ;
        }
        private void AddHoursToField(WorkItem workItem, string fieldRef, double hours)
        {
            if (!string.IsNullOrEmpty(fieldRef) && workItem.Fields.Contains(fieldRef))
            {
                Field field = workItem.Fields[fieldRef];
                if (field.FieldDefinition.FieldType == FieldType.Double)
                {
                    double num = 0.0;
                    if (field.Value != null)
                    {
                        num = (double)field.Value;
                    }
                    double num2 = num + hours;
                    if (num2 < 0.0)
                    {
                        num2 = 0.0;
                    }
                    field.Value = Math.Round(num2, 2);
                }
            }
        }

        public void Post()
        {

            //WorkItem item = WorkItemStore.GetWorkItem(Convert.ToInt32(current.WorkItemId));
            //item.State = rcbState.SelectedValue;
            //item.Save();
        }

        public void UpdateWorkItemHours(string userName, Guid collectionGuid, int workItemId, string remainingWorkReferenceField, string completedWorkReferenceField, double hours)
        {
            System.Net.ICredentials credentials = new System.Net.NetworkCredential("agarcia", "Pass@word1", "webfortis.com");
            Microsoft.TeamFoundation.Client.TfsTeamProjectCollection teamProjectCollection = new Microsoft.TeamFoundation.Client.TfsTeamProjectCollection(new Uri(@"https://tfs.webfortis.com/tfs/Webfortis Customer Solutions"), credentials);
            teamProjectCollection.EnsureAuthenticated();

            Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore Store = (Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore)
                teamProjectCollection.GetService(typeof(Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore));
            WorkItem workItem = Store.GetWorkItem(workItemId);
            if (workItem == null)
            {
                throw new ArgumentException(string.Format("Cannot find work item with id {0} in collection {1}", workItemId, teamProjectCollection.Name));
            }
            this.AddHoursToField(workItem, remainingWorkReferenceField, -hours);
            this.AddHoursToField(workItem, completedWorkReferenceField, hours);
            workItem.History = "Updated via Timesheet";
            try
            {
                workItem.Save();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}