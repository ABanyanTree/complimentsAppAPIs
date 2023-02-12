using Microsoft.AspNetCore.Http;
using LikeKero.UI.Utility;
using LikeKero.UI.ViewModels;

namespace LikeKero.UI
{
    public static class CustomHelpers
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static HttpContext Current => _httpContextAccessor.HttpContext;

        public static FeatureMasterResponseVM GetFeatures(string FeatureId)
        {
            FeatureMasterResponseVM objRight = null;    
            var objSessionUSer = Current.Session.GetSessionUser();
            if (objSessionUSer != null)
            {
                objRight = objSessionUSer.UserFeatures.Find(delegate (FeatureMasterResponseVM obj) { return obj.FeatureId == CommonConstants.FEATURE_Region; });
            }
            return objRight;  
        }

    }
}