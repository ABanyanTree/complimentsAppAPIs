namespace LikeKero.Contract.Requests.Feature
{
    public class AdminRightsRequest : BaseRequest
    {
        public string RoleFeatureId { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public bool Status { get; set; }
    }
}
