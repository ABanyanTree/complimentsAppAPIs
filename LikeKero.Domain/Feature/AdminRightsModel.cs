using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Domain.Feature
{
    public class AdminRightsModel
    {
        public List<FeatureMaster> FeatureMasterList { get; set; }
        public List<UserRole> UserRoleList { get; set; }
        public List<RoleFeatureMaster> RoleFeatureMasterList { get; set; }
    }
}
