using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Data.Constants.User
{
    public class SystemRoleInfra : BaseInfra
    {
        public const string SEARCHPHRASE = "SearchPhrase";
        public const string SEARCHONGROUPID = "SearchOnGroupId";
        public const string GROUPID = "GroupId";
        public const string USERROLEID = "UserRoleId";
        public const string STATUS = "Status";
        public const string ROLEID = "RoleId";

        public const string SPROC_SYSTEMROLES_GETALLROLES = "sproc_SystemRoles_GetAllRoles";
        public const string SPROC_GROUPNAMES_SEARCH_FOR_ROLEASSIGNMENT = "sproc_GroupNames_Search_For_RoleAssignment ";
        public const string SPROC_GROUPNAMES_SEARCH_FOR_ROLEASSIGNMENT_NEW = "sproc_GroupNames_Search_For_RoleAssignment_new";
        public const string SPROC_GETADMINUSERS_FOR_GROUPID = "sproc_GetAdminUsers_For_GroupID";
        public const string SPROC_SYSTEMROLES_ACTIVE_INACTIVE = "sproc_SystemRoles_Active_InActive";
        public const string SPROC_ASSIGN_SYSTEMROLE = "sproc_Assign_SystemRole";
        public const string SPROC_SYSTEMROLES_DELETE = "sproc_SystemRoles_Delete";
    }
}
