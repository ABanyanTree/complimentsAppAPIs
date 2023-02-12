using System;

namespace LikeKero.Contract.Responses.Feature
{
    public class RoleFeatureResponse : FeatureMasterResponse
    {
        public string RoleId { get; set; }
        public string DisplayRole { get; set; }
        public string RoleName { get; set; }
        public int SequenceNo { get; set; }
        public bool Status { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
