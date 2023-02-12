using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class UserListResponse : BaseResponse
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
        public string LoginId { get; set; }
        public string ContactNumber { get; set; }
        public string ZipCode { get; set; }
        public string DisplayStatus { get; set; }
        public string DepartmentId { get; set; }
        public string JobCodeId { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }

        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }

        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsAdmin { get; set; }
        public string UserRoleId { get; set; }
        public string GroupDescription { get; set; }

    }
}
