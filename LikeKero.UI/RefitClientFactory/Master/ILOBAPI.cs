using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LikeKero.Contract.Responses;
using LikeKero.UI.ViewModels;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface ILOBAPI
    {
        [Post(path: "/api/lob/addeditlob")]
        Task<HttpResponseMessage> AddEditLOB(CreateLOBRequestVM request);

        [Get(path: "/api/lob/getlob")]
        Task<ApiResponse<CreateLOBRequestVM>> GetLOB(string LOBID);

        [Delete(path: "/api/LOB/deleteLOB")]
        Task<HttpResponseMessage> DeleteLOB(string LOBID);

        [Get(path: "/api/lob/getallloblist")]
        Task<ApiResponse<List<GetLOBResponseVM>>> GetAllLOBList(SearchLOBRequestVM request);

        [Get(path: "/api/lob/islobnameinuse")]
        Task<HttpResponseMessage> IsLOBNameInUse(string LOBName, string LOBID);

        [Get(path: "/api/lob/getalllob")]
        Task<ApiResponse<List<GetLOBResponseVM>>> GetAllLOB();

        [Get(path: "/api/lob/isinusecount")]
        Task<HttpResponseMessage> IsInUseCount(string LOBID);

    }
}
