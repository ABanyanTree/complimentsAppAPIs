namespace LikeKero.UI.ViewModels
{
    public class FeatureMasterResponseVM : BaseResponseVM
    {
        public string FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string ParentFeatureId { get; set; }
        public int DisplayOrder { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string CssClass { get; set; }
        public bool IsShowInMenu { get; set; }
        public string IconName { get; set; }
        public bool IsLearnerMenu { get; set; }

        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsExport { get; set; }
        public bool IsView { get; set; }
        public bool IsPrint { get; set; }

    }
}
