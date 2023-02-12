
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Domain
{
   public class UserSearchModel 
    {
        

        //public List<DepartmentMaster> ListDepartments { get; set; } = new List<DepartmentMaster>();

        //public List<JobRoleMaster> ListJobRoles { get; set; } = new List<JobRoleMaster>();
        public List<LookupMaster> ListUserStatus { get; set; } = new List<LookupMaster>();
        //public List<CountryMaster> ListCountries { get; set; } = new List<CountryMaster>();
        //public List<TimeZoneMaster> ListTimeZones { get; set; } = new List<TimeZoneMaster>();
        //public List<StateMaster> ListStates { get; set; } = new List<StateMaster>();
        //public List<CityMaster> ListCities { get; set; } = new List<CityMaster>();
        public List<LookupMaster> ListEmpManagerJobRole { get; set; } = new List<LookupMaster>();
    }
}
