namespace LikeKero.Domain.Feature
{
    public class RoleFeatureMaster : FeatureMaster
    {
        public string RoleFeatureId { get; set; }
        public string RoleId { get; set; }
        public string DisplayRole { get; set; }
        public string RoleName { get; set; }
        public int SequenceNo { get; set; }
        public bool Status { get; set; }
    }
}
