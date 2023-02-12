using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses.User
{
    public class GetUserResponse: BaseResponse
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
        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
        public string Group4Name { get; set; }
        public string Group5Name { get; set; }
        public bool? IsAdmin { get; set; }

    }
}
