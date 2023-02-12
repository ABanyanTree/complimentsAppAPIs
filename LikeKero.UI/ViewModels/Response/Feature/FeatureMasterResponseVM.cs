using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels.Response
{
    public class FeatureMasterResponseVM
    {
        public string FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string ParentFeatureId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsNoAction { get; set; }
        public bool IsShowInMenu { get; set; }
        public bool ShowOnlyToSystemAdmin { get; set; }
        public bool IsParent { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
