using DailyNeed.Model;
using System;
using System.Collections.Generic;
using DailyNeed.API.Controllers;

using GenricEcommers.Models;
//using GenricEcommers.Models;
//using DailyNeed.API.DN.Models;

namespace DailyNeed.API
{
    /// <summary>
    /// This interface contains all defination of DailyNeedContext Class 
    /// </summary>
    public interface iDailyNeedContext
    {
        string skcode();
        Customer Resetpassword(Customer customer);    
        People getPersonIdfromEmail(string email);
        People getPersonIdfrommobile(string mobile);
        IEnumerable<Company> AllCompanies { get; }
        Company AddCompany(Company company);       
        bool DeleteCompany(int id);
        bool CompanyExists(string companyName);   
        UserAccessPermission getRoleDetail(string RoleName);
        List<People> GetPeoplebyCompanyId(int id);
        People GetPeoplebyId(int compid, string email);
        People AddPeople(People people);
        People PutPeople(People people);
        IEnumerable<Role> AllRoles(int compid);
        Role AddRole(Role role);
        Role PutRoles(Role role);
        bool DeleteRole(int id);
       // Warehouse AddWarehouse(Warehouse warehouse);
        IEnumerable<SubCategory> AllSubCategory(int compid);
        IEnumerable<SubCategory> AllSubCategoryy(int subcat, int CompanyId);
        SubCategory AddSubCategory(SubCategory subCategory);
        SubCategory PutSubCategory(SubCategory subCategory,int userid);
        bool DeleteSubCategory(int id, int CompanyId);
        // List<SubsubCategory> subcategorybyWarehouse(int id, int compid);
        //List<SubsubCategory> Updatebrands(List<SubsubCategory> sub, int compid);
        //List<SubsubCategory> UpdateExclusivebrands(List<SubsubCategory> sub, int compid);
        //List<SubsubCategory> subcategorybyPramotion(int id, int compid);
        //List<SubsubCategory> subcategorybyPramotionExlusive();
        //  List<SubsubCategory> subcategorybycity(int id, int compid);
        IEnumerable<Zone> AllZones(int compid);
        IEnumerable<Zone> AllZoness(int zne, int CompanyId);
        Zone AddZone(Zone zone, int compid, int userid);
        Zone PutZone(Zone zone, int CompanyId, int UserId);
        bool DeleteZone(int id, int CompanyId);


        int AddCategoryImage(CategoryImage item);
        int PutCategoryImage(CategoryImage item);
     
        List<CategoryImageData> AllCategoryImages();
     
  
      
    }
}
