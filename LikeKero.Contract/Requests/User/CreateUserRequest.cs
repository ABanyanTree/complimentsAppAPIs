using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests
{
    public class CreateUserRequest : BaseRequest
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public string LoginId { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? HiringDate { get; set; }
        public string ZipCode { get; set; }
        public bool? Status { get; set; }
        public string DepartmentId { get; set; }

        public string JobCodeId { get; set; }

        public DateTime? RoleChangeDate { get; set; }
        public string TimeZoneId { get; set; }

        public string ProfilePic { get; set; }

        public string GroupId { get; set; }

        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
       // public string RequesterUserId { get; set; }
        public string BRId { get; set; }
        public bool? IsAdmin { get; set; }

        // Shahen => 15/12/21: UserRole and respective data
        public string UserType { get; set; }
        public string PriousvUserType { get; set; }
        public string RegionList { get; set; }
        public string CountryList { get; set; }
        public bool IsAccessBreachLog { get; set; }


    }
}
