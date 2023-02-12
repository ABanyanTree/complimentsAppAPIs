using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels.Response
{
    public class RoleFeatureResponseVM : FeatureMasterResponseVM
    {
        public string RoleId { get; set; }
        public string DisplayRole { get; set; }
        public string RoleName { get; set; }
        public int SequenceNo { get; set; }
        public bool Status { get; set; }
    }
}
