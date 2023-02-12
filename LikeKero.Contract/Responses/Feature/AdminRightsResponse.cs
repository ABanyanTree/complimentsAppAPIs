using System.Collections.Generic;

namespace LikeKero.Contract.Responses.Feature
{
    public class AdminRightsResponse : BaseResponse
    {
        public List<RoleFeatureResponse> RoleFeatureMasterList { get; set; } = new List<RoleFeatureResponse>();
    }
}
