using LikeKero.UI.ViewModels.Response;
using LikeKero.UI.ViewModels.Response.User;
using System;
using System.Collections.Generic;

namespace LikeKero.UI.ViewModels
{
    public class UserListResponseVM : BaseResponseVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string JobCode { get; set; }
        public string JobRole { get; set; }
        public string Group1Name { get; set; }

        public string Group2Name { get; set; }
        public string Group3Name { get; set; }

        public string Group4Name { get; set; }

        public string Group5Name { get; set; }
        public bool Status { get; set; }
        public string DisplayStatus { get; set; }
        public string LoginId { get; set; }
        public string ContactNumber { get; set; }
        public string ZipCode { get; set; }
        public string DepartmentId { get; set; }
        public string JobCodeId { get; set; }
        public DateTime? HiringDateFrom { get; set; }
        public DateTime? HiringDateTo { get; set; }
        public DateTime? RoleChangeDateFrom { get; set; }
        public DateTime? RoleChangeDateTo { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }
        public bool IsAdmin { get; set; }
        public string SearchOnGroupId { get; set; }
        public string GroupDescription { get; set; }

        //public List<DepartmentMasterVM> ListDepartments { get; set; } = new List<DepartmentMasterVM>();
        //public List<JobRoleMasterVM> ListJobRoles { get; set; } = new List<JobRoleMasterVM>();
        public List<LookUpMasterVM> ListUserStatus { get; set; } = new List<LookUpMasterVM>();
        public List<CountryMasterResponseVM> ListCountries { get; set; } = new List<CountryMasterResponseVM>();
        public List<TimeZoneMasterResponseVM> ListTimeZones { get; set; } = new List<TimeZoneMasterResponseVM>();
        public List<StateMasterResponseVM> ListStates { get; set; } = new List<StateMasterResponseVM>();
        public List<CityMasterResponseVM> ListCities { get; set; } = new List<CityMasterResponseVM>();
        public List<LookUpMasterVM> ListEmpManagerJobRole { get; set; } = new List<LookUpMasterVM>();
    }
}
