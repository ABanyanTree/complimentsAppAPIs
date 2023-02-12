namespace LikeKero.UI.ViewModels.Response.User
{
    public class LookUpMasterVM : BaseResponseVM
    {
        public string LookupID { get; set; }
        public string LookUpType { get; set; }
        public string LookUpName { get; set; }
        public string LookUpValue { get; set; }
        public string DisplayOrder { get; set; }
        public string FieldType { get; set; }
        public bool IsGroup { get; set; }
        public string SearchonGroupId { get; set; }
        public string BRUserFieldType { get; set; }
    }
}
