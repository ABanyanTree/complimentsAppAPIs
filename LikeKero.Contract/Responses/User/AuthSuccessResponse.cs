using LikeKero.Contract.Responses.Feature;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class AuthSuccessResponse : BaseResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string LoginId { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? HiringDate { get; set; }
        public string ZipCode { get; set; }
        public bool Status { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string JobCodeId { get; set; }
        public string JobCode { get; set; }
        public string JobRole { get; set; }
        public DateTime? RoleChangeDate { get; set; }
        public string TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneDescription { get; set; }
        public string ProfilePic { get; set; }
        public bool IsAdmin { get; set; }

        public List<FeatureMasterResponse> UserFeatures { get; set; } = new List<FeatureMasterResponse>();

        public UserHierarchyResponse userHierarchy { get; set; } = new UserHierarchyResponse();
        public List<UserRoleResponse> userRoles { get; set; } = new List<UserRoleResponse>();

        public bool IsPasswordChanged { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }

        public int AdminCartCount { get; set; }
        public int UserCartCount { get; set; }

        public string LoginUniqueId { get; set; }


        //
        public string URoleId { get; set; }
    }
}
