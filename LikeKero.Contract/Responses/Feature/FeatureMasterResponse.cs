using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses.Feature
{
    public class FeatureMasterResponse : BaseResponse
    {
        public string FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string ParentFeatureId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsNoAction { get; set; }
        public bool IsShowInMenu { get; set; }
        public bool ShowOnlyToSystemAdmin { get; set; }
        public bool IsParent { get; set; }


        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsExport { get; set; }
        public bool IsView { get; set; }
        public bool IsPrint { get; set; }


        public bool IsLearnerMenu { get; set; }
    }
}
