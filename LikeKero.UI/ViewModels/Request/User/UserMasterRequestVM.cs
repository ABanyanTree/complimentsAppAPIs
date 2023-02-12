using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace LikeKero.UI.ViewModels.Request.User
{
    public class UserMasterRequestVM : BaseRequestVM
    {
        public string UserId { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid First Name.")]
        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid Last Name.")]
        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Login ID")]
        [Remote("IsLoginIDExists", "User", AdditionalFields = "UserId", HttpMethod = "GET")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Please enter Email Or UserName")]
        [Remote("IsEmailExists", "User", AdditionalFields = "UserId", HttpMethod = "GET")]
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        
        public DateTime? HiringDate { get; set; }

        public string ZipCode { get; set; }
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "Please enter Jobe Code")]
        public string JobCodeId { get; set; }
        public string TimeZoneId { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }

        public bool Status { get; set; }
        public string RequesterUserId { get; set; }

        public string StateName { get; set; }
        public string CityName { get; set; }

        public IFormFile ProfilePicformFile { get; set; }
        public string ProfilePic { get; set; }
        public string FileName { get; set; }
        public bool UserStatus { get; set; }
        public bool IsOIGChecked { get; set; }
        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
        public string Group4Name { get; set; }
        public string Group5Name { get; set; }

        public DateTime? RoleChangeDate { get; set; }
        public string Message { get; set; }

        public bool IsAdmin { get; set; }
    }
}
