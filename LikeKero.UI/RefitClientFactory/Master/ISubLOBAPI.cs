using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LikeKero.UI.ViewModels;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface ISubLOBAPI
    {
        [Post(path: "/api/sublob/addeditsublob")]
        Task <HttpResponseMessage> AddEditSubLOB(CreateSubLOBRequestVM request);

        [Get(path: "/api/sublob/getsublob")]
        Task<ApiResponse<CreateSubLOBRequestVM>> GetSubLOB(string SubLOBID);

        [Delete(path: "/api/sublob/deletesublob")]
        Task<HttpResponseMessage> DeleteSubLOB(string SubLOBID);

        [Get(path: "/api/sublob/getallsubloblist")]
        Task<ApiResponse<List<GetSubLOBResponseVM>>> GetAllSubLOBList(SearchSubLOBRequestVM request);

        [Get(path: "/api/SubLOB/issublobnameinuse")]
        Task<HttpResponseMessage> IsSubLOBNameInUse(string SubLOBName, string SubLOBID);

        //[Get(path: "/api/sublob/getallsublob")]
        //Task<ApiResponse<List<GetSubLOBResponseVM>>> GetAllSubLOB();

        [Get(path: "/api/sublob/isinusecount")]
        Task<HttpResponseMessage> IsInUseCount(string SubLOBID);

    }
}
