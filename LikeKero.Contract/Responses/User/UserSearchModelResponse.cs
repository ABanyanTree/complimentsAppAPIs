using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class UserSearchModelResponse : BaseResponse
    {
        // public List<DepartmentMasterResponse> ListDepartments { get; set; } = new List<DepartmentMasterResponse>();

        //public List<JobRoleMasterResponse> ListJobRoles { get; set; } = new List<JobRoleMasterResponse>();
        public List<LookUpMasterResponse> ListUserStatus { get; set; } = new List<LookUpMasterResponse>();
        //public List<CountryMasterResponse> ListCountries { get; set; } = new List<CountryMasterResponse>();
        public List<TimeZoneMasterResponse> ListTimeZones { get; set; } = new List<TimeZoneMasterResponse>();

        //public List<StateMasterResponse> ListStates { get; set; } = new List<StateMasterResponse>();
        public List<LookUpMasterResponse> ListEmpManagerJobRole { get; set; } = new List<LookUpMasterResponse>();

    }
}
