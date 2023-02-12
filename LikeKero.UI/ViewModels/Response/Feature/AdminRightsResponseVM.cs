using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels.Response
{
    public class AdminRightsResponseVM
    {
        public List<RoleFeatureResponseVM> RoleFeatureMasterList { get; set; } = new List<RoleFeatureResponseVM>();
    }
}
