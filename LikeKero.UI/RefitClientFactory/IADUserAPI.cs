using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LikeKero.UI.ViewModels;


namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]
    public interface IADUserAPI
    {
        [Get(path: "api/user/getalladuser")]
        Task<ApiResponse<List<ADUserRequest>>> GetAllADUser();
       
    }
}
