using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LikeKero.Contract.Responses;
using LikeKero.UI.ViewModels;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface ILOBChapterAPI
    {
        [Post(path: "/api/lobchapter/addeditlobchapter")]
        Task<HttpResponseMessage> AddEditLOBChapter(CreateLOBChapterRequestVM request);

        [Get(path: "/api/lobchapter/getlobchapter")]
        Task<ApiResponse<CreateLOBChapterRequestVM>> GetLOBChapter(string LOBChapterID);

        [Delete(path: "/api/lobchapter/deletelobchapter")]
        Task<HttpResponseMessage> DeleteLOBChapter(string LOBChapterID);

        [Get(path: "/api/lobchapter/getalllobchapterlist")]
        Task<ApiResponse<List<GetLOBChapterResponseVM>>> GetAllLOBChapterList(SearchLOBChapterRequestVM request);

        [Get(path: "/api/lobchapter/islobchapternameinuse")]
        Task<HttpResponseMessage> IsLOBChapterNameInUse(string LOBChapterName, string LOBChapterID);

        //[Get(path: "/api/lobchapter/getalllobchapter")]
        //Task<ApiResponse<List<GetLOBChapterResponseVM>>> GetAllLOBChapter();
        
        [Get(path: "/api/lobchapter/isinusecount")]
        Task<HttpResponseMessage> IsInUseCount(string LOBChapterID);

    }
}
