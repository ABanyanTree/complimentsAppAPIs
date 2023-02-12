using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class UserRoleResponse : UserHierarchyResponse
    {
        public string RoleId { get; set; }
        public string DisplayRole { get; set; }        
        public string RoleName { get; set; }
        public string SearchonGroupId { get; set; }
        public int SequenceNo { get; set; }
    }
}
