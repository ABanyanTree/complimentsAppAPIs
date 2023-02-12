using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LikeKero.UI.ViewModels;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface IRegionAPI
    {
        [Post(path: "/api/region/addeditregion")]
        Task<HttpResponseMessage> AddEditRegion(CreateRegionRequestVM request);

        [Get(path: "/api/region/getregion")]
        Task<ApiResponse<CreateRegionRequestVM>> GetRegion(string RegionID);

        [Delete(path: "/api/region/deleteregion")]
        Task<HttpResponseMessage> DeleteRegion(string RegionID);

        [Get(path: "/api/region/getallregionlist")]
        Task<ApiResponse<List<GetRegionResponseVM>>> GetAllRegionList(SearchRegionRequestVM request);

        [Get(path: "/api/region/isregionnameinuse")]
        Task<HttpResponseMessage> IsRegionNameInUse(string RegionName, string RegionID);
        
        [Get(path: "/api/region/getallregion")]
        Task<ApiResponse<List<GetRegionResponseVM>>> GetAllRegion();

        [Get(path: "/api/region/isinusecount")]
        Task<HttpResponseMessage> IsInUseCount(string RegionID);

        

    }
}
