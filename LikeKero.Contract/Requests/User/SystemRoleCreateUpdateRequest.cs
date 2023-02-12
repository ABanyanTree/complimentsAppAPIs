using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests.User
{
    public class SystemRoleCreateUpdateRequest: BaseRequest
    {
        public string UserRoleId { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string GroupId { get; set; }
    }
}
