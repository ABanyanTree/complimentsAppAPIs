using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests
{
    public class UserListRequest: BaseRequest
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SearchOnGroupId { get; set; }
        public string GroupId { get; set; }      
        public string DepartmentId { get; set; }
        public string JobCodeId { get; set; }
        public bool? Status { get; set; }

        public DateTime? HiringDateFrom { get; set; }
        public DateTime? HiringDateTo { get; set; }
        public DateTime? RoleChangeDateFrom { get; set; }
        public DateTime? RoleChangeDateTo { get; set; }
        public bool NoPaging { get; set; }
        public bool? IsAdmin { get; set; }

        public string UserName { get; set; }


    }
}
