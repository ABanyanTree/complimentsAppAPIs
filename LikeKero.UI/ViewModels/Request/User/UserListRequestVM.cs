using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class UserListRequestVM  : BaseRequestVM
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string FirstNameSearch { get; set; }
        public string LastName { get; set; }
        public string LastNameSearch { get; set; }
        public string SearchOnGroupId { get; set; }
        public string GroupId { get; set; }
        
        public string DepartmentId { get; set; }
        public string JobCodeId { get; set; }
        public bool? Status { get; set; }
        public string DisplayStatus { get; set; }
        public string Group1Name { get; set; }
        public DateTime? HiringDateFrom { get; set; }
        public DateTime? HiringDateTo { get; set; }
        public DateTime? RoleChangeDateFrom { get; set; }
        public DateTime? RoleChangeDateTo { get; set; }
        public bool NoPaging { get; set; }

        public bool? IsAdmin { get; set; }
        public List<FeatureMasterResponseVM> UserFeatures { get; set; } = new List<FeatureMasterResponseVM>();
    }
}
