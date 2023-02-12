using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Domain
{
    public class UserMaster : BaseEntity
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProfileImagePath { get; set; }

       
        //For search request
        public string RegionId { get; set; }
        public string CountryId { get; set; }
        public string LOBId { get; set; }
        public string SubLOBId { get; set; }
        public string UserType { get; set; }



        // Shahen => 15/12/21: UserRole and respective data
        public string RoleID { get; set; }
        public string PreviousRoleID { get; set; }
        public string PreviousUserType { get; set; }
        public string RegionList { get; set; }
        public string CountryList { get; set; }
        public bool IsAccessBreachLog { get; set; }
        public string RegionNameList { get; set; }
        public string CountryNameList { get; set; }
        public bool IsActive { get; set; }




        public string Token { get; set; }
        public string Salt { get; set; }
        public string LoginId { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? HiringDate { get; set; }
        public string ZipCode { get; set; }
        public bool? Status { get; set; }
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
        public string SearchOnGroupId { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }


        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
        public string Group4Name { get; set; }
        public string Group5Name { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }

        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string BulkImportId { get; set; }


        public string DisplayStatus { get; set; }
        public List<FeatureMaster> Features { get; set; } = new List<FeatureMaster>();

        //public UserHierarchy userHierarchy { get; set; } = new UserHierarchy();
        public List<UserRole> userRoles { get; set; } = new List<UserRole>();


        public DateTime? HiringDateFrom { get; set; }
        public DateTime? HiringDateTo { get; set; }
        public DateTime? RoleChangeDateFrom { get; set; }
        public DateTime? RoleChangeDateTo { get; set; }

        public bool IsPasswordChanged { get; set; }
        public string CurrentPassword { get; set; }
        public bool IsAdminPasswordChangeRequest { get; set; }

        public bool NoPaging { get; set; }
        public int RowNo { get; set; }
        public bool? IsAdmin { get; set; }
        public string BRId { get; set; }
        public string BRActivityId { get; set; }
        public string BusinessAssignmentId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string DisplayRole { get; set; }
        public string CourseId { get; set; }

        public bool IsEnrolled { get; set; }
        public string OrderId { get; set; }

        public bool IsLogin { get; set; }
        public DateTime AssignmentDate { get; set; }
        public int TotalCartItems { get; set; }

        public string ErrorMessage { get; set; }

        public string AddedUserId { get; set; }
        public string GroupDescription { get; set; }

        public string UserRoleId { get; set; }
        public string IsMandatoryDisplay { get; set; }

        public string Remark { get; set; }
        public string SelectedUsers { get; set; }
        public int TotalRecords { get; set; } = 0;
        public string MasterLogId { get; set; }

        public string LoginUniqueId { get; set; }

        public int TotalUsers { get; set; } = 0;
        public int TotalSuccessfull { get; set; } = 0;
        public int TotalWithoutEmails { get; set; } = 0;
        public DateTime? LogDate { get; set; }
                

        //
        public string URoleId { get; set; }


       
    }
}
