using LikeKero.UI.ViewModels;
using LikeKero.UI.ViewModels.Request.User;
using LikeKero.UI.ViewModels.Response.User;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]
    public interface IErrorLogApi
    {
        [Post(path: "/api/errorlog/errorloglist")]
        Task<ApiResponse<List<ErrorLogResponseVM>>> GetErrorLogList(ErrorLogRequestVM model);

        [Post(path: "/api/errorlog/geterrorlog")]
        Task<ApiResponse<ErrorLogGetResponseVM>> GetErrorLog(ErrorLogGetRequestVM model);
    }
}
