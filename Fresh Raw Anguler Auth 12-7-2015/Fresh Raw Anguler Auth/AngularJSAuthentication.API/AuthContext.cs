
using AngularJSAuthentication.Model;

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AngularJSAuthentication.API
{
    public interface iAuthContext
    {        
          
        // ORiginal Authcontext

        People getPersonIdfromEmail(string email);

        IEnumerable<Company> AllCompanies { get; }
        Company AddCompany(Company company);
        Company PutCompany(Company company);
        Company GetCompanybyCompanyId(int id);
        bool DeleteCompany(int id);
        bool CompanyExists(string companyName);

        IEnumerable<People> singleuser(int i);
        
       
        IEnumerable<People> AllPeoples(int compid);
        List<People> GetPeoplebyCompanyId(int id);
        List<People> GetPeoplebyEmail(string email);
        People GetPeoplebyId(int id);
        People AddPeople(People people);
        People AddPeoplebyAdmin(People people);
        People PutPeople(People people);
        People PutPeoplebyAdmin(People people);
        bool DeletePeople(int id);


        IEnumerable<Category> AllCategory();

    }

    public class AuthContext : IdentityDbContext<IdentityUser>, iAuthContext
    {
        public AuthContext() : base("AuthContext")
        {

        }
       

        public DbSet<People> Peoples { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categorys { get; set; }
        

        //-------------Start All Categories----------------//
        public IEnumerable<Category> AllCategory()
        {

            if (Categorys.AsEnumerable().Count() > 0)
            {
                var data= Categorys.AsEnumerable();
                return data;

            }
            else

            {
                List<Category> category = new List<Category>();
                return category.AsEnumerable();
            }

        }
        //-------------End All Categories----------------//

        //.............End Customer...................//

        public IEnumerable<People> singleuser(int i)
        {
            return Peoples.Where(c => c.PeopleID == i).AsEnumerable();

        }
        

        /// <summary>
        /// people
        /// </summary>
        /// <param name="compid"></param>
        /// <returns></returns>
        public IEnumerable<People> AllPeoples(int compid)
        {
            if (Peoples.AsEnumerable().Count() > 0)
            {
                List<People> person = new List<People>();
                person = Peoples.Where(e => e.CompanyID == compid).ToList();
                return person.AsEnumerable();
            }
            else
            {
                List<People> people = new List<People>();
                return people.AsEnumerable();
            }
        }
        public People AddPeople(People people)
        {
            List<People> peoples = Peoples.Where(c => c.Email.Trim().Equals(people.Email.Trim())).ToList();

            People objPeople = new People();
            if (peoples.Count == 0)
            {
                people.DisplayName = people.PeopleFirstName + " " + people.PeopleLastName;

                people.CreatedBy = people.CreatedBy;
                people.CreatedDate = DateTime.Now;
                people.UpdatedDate = DateTime.Now;
                Peoples.Add(people);
                int id = this.SaveChanges();
                return people;
            }
            else
            {
                //objProject.Exception = "Already";
                return objPeople;
            }
        }
        public People AddPeoplebyAdmin(People people)
        {
            List<People> peoples = Peoples.Where(c => c.Email.Trim().Equals(people.Email.Trim())).ToList();
            //State state = States.Where(x => x.Stateid == people.Stateid).Select(x => x).SingleOrDefault();
            //City city = Cities.Where(x => x.Cityid == people.Cityid).Select(x => x).SingleOrDefault();
            People objPeople = new People();
            if (peoples.Count == 0)
            {
                people.DisplayName = people.PeopleFirstName + " " + people.PeopleLastName;
                //people.state = state.StateName;
                //people.city = city.CityName;
                people.CreatedBy = people.CreatedBy;
                people.CreatedDate = DateTime.Now;
                people.UpdatedDate = DateTime.Now;
                //people.EmailConfirmed = true;
                Peoples.Add(people);
                int id = this.SaveChanges();
                return people;
            }
            else
            {
                //objProject.Exception = "Already";
                return objPeople;
            }
        }
        public People PutPeople(People objCust)
        {
            People proj = Peoples.Where(x => x.PeopleID == objCust.PeopleID).FirstOrDefault();
            //State state = States.Where(x => x.Stateid == objCust.Stateid).Select(x => x).SingleOrDefault();
            //City city = Cities.Where(x => x.Cityid == objCust.Cityid).Select(x => x).SingleOrDefault();
            if (proj != null)
            {
                proj.UpdatedDate = DateTime.Now;
                //proj.Client = objCust.Client;
                proj.PeopleFirstName = objCust.PeopleFirstName;
                proj.PeopleLastName = objCust.PeopleLastName;
                proj.Email = objCust.Email;
                //proj.state = state.StateName;
                //proj.city = city.CityName;
                proj.Mobile = objCust.Mobile;
                proj.Department = objCust.Department;
                proj.BillableRate = objCust.BillableRate;
                proj.CostRate = objCust.CostRate;
                proj.Permissions = objCust.Permissions;
                proj.Type = objCust.Type;
                proj.ImageUrl = objCust.ImageUrl;
                proj.DisplayName = objCust.PeopleFirstName + " " + objCust.PeopleLastName;
                proj.CreatedBy = objCust.CreatedBy;
                proj.CreatedDate = objCust.CreatedDate;
                proj.UpdateBy = objCust.UpdateBy;
                proj.EmailConfirmed = objCust.EmailConfirmed;
                Peoples.Attach(proj);
                this.Entry(proj).State = EntityState.Modified;
                this.SaveChanges();
                return objCust;
            }
            else
            {
                return objCust;
            }
        }
        public bool DeletePeople(int id)
        {
            try
            {
                People DL = new People();
                DL.PeopleID = id;
                Entry(DL).State = EntityState.Deleted;
                SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public People PutPeoplebyAdmin(People objCust)
        {
            People proj = Peoples.Where(x => x.PeopleID == objCust.PeopleID).FirstOrDefault();
            //State state = States.Where(x => x.Stateid == objCust.Stateid).Select(x => x).SingleOrDefault();
            //City city = Cities.Where(x => x.Cityid == objCust.Cityid).Select(x => x).SingleOrDefault();
            if (proj != null)
            {
                proj.UpdatedDate = DateTime.Now;
                //proj.Client = objCust.Client;
                proj.PeopleFirstName = objCust.PeopleFirstName;
                proj.PeopleLastName = objCust.PeopleLastName;
                proj.Email = objCust.Email;
               // proj.state = state.StateName;
                //proj.city = city.CityName;
                proj.Mobile = objCust.Mobile;
                proj.Department = objCust.Department;
                proj.BillableRate = objCust.BillableRate;
                proj.CostRate = objCust.CostRate;
                proj.Permissions = objCust.Permissions;
                proj.Type = objCust.Type;
                proj.ImageUrl = objCust.ImageUrl;
                proj.DisplayName = objCust.PeopleFirstName + " " + objCust.PeopleLastName;
                proj.CreatedBy = objCust.CreatedBy;
                proj.CreatedDate = objCust.CreatedDate;
                proj.UpdateBy = objCust.UpdateBy;
                proj.EmailConfirmed = objCust.EmailConfirmed;
                Peoples.Attach(proj);
                this.Entry(proj).State = EntityState.Modified;
                this.SaveChanges();
                return objCust;
            }
            else
            {
                return objCust;
            }
        }
    
        //public bool DeletePeople(int id)
        //{
        //    try
        //    {
        //        People DL = new People();
        //        DL.PeopleID = id;
        //        Entry(DL).State = EntityState.Deleted;
        //        SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public IEnumerable<Company> AllCompanies
        {
            get { return Companies.AsEnumerable(); }
        }
        public Company GetCompanybyCompanyId(int id)
        {
            Company p = this.Companies.Where(c => c.Id == id).SingleOrDefault();
            if (p != null)
            {

            }
            else
            {

                p = new Company();

            }
            return p;
        }
        public Company AddCompany(Company company)
        {
            List<Company> cmp = Companies.Where(c => c.Name.Trim().Equals(company.Name.Trim())).ToList();
            // Company objCompany = new Company();
            if (cmp.Count == 0)
            {
                //   objCompany.Name = companyname;
                company.CreatedBy = "System";
                company.CreatedDate = DateTime.Now;
                company.UpdatedDate = DateTime.Now;
                Companies.Add(company);
                int id = this.SaveChanges();
                //Company.CompanyId = id;
                return company;
            }
            else
            {
                return cmp[0];
            }
        }
        public Company PutCompany(Company company)
        {
            Company proj = Companies.Where(x => x.Id == company.Id).FirstOrDefault();
            if (proj != null)
            {
                proj.UpdatedDate = DateTime.Now;
                //proj.Name = cmp.Name;
                proj.AlertDay = company.AlertDay;
                proj.AlertTime = company.AlertTime;
                proj.FreezeDay = company.FreezeDay;
                proj.TFSUrl = company.TFSUrl;
                proj.TFSUserId = company.TFSUserId;
                proj.TFSPassword = company.TFSPassword;
                proj.LogoUrl = company.LogoUrl;
                proj.Address = company.Address;
                proj.CompanyName = company.CompanyName;
                proj.contactinfo = company.contactinfo;
                proj.currency = company.currency;
                proj.dateformat = company.dateformat;
                proj.fiscalyear = company.fiscalyear;
                proj.startweek = company.startweek;
                proj.timezone = company.timezone;
                proj.Webaddress = company.Webaddress;
                //  proj.UpdateBy = company.UpdateBy;
                Companies.Attach(proj);
                this.Entry(proj).State = EntityState.Modified;
                this.SaveChanges();
                return proj;
            }
            else
            {
                return null;
            }
        }
        public bool DeleteCompany(int id)
        {
            throw new NotImplementedException();
        }
        public bool CompanyExists(string companyName)
        {
            List<Company> cmp = Companies.Where(c => c.Name.Trim().Equals(companyName.Trim())).ToList();
            Company objCompany = new Company();
            if (cmp.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        
        public People getPersonIdfromEmail(string email)
        {
            People ps = new People();
            ps = Peoples.Where(p => p.Email.Trim().Equals(email.Trim())).SingleOrDefault();
            int id = 0;
            if (ps != null)
            {
                id = ps.PeopleID;
            }
            return ps;
        } 
    
      
      
     //------------------------------------------------------------------------------
        public List<People> GetPeoplebyCompanyId(int id)
        {
            List<People> p = this.Peoples.Where(c => c.CompanyID == id).ToList();
            if (p != null)
            {

            }
            else
            {

                p = new List<People>();

            }
            return p;
        }

        public List<People> GetPeoplebyEmail(string email)
        {
            List<People> p = this.Peoples.Where(c => c.Email == email).ToList();
            if (p != null)
            {

            }
            else
            {

                p = new List<People>();

            }
            return p;
        }

        public People GetPeoplebyId(int id)
        {
            People p = this.Peoples.Where(c => c.PeopleID == id).SingleOrDefault();
            if (p != null)
            {

            }
            else
            {

                p = new People();

            }
            return p;
        }
        
    }
}