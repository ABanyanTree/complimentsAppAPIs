using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LikeKero.Contract.Responses;
using LikeKero.UI.ViewModels;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface ISegmentAPI
    {
        [Post(path: "/api/segment/addeditsegment")]
        Task<HttpResponseMessage> AddEditSegment(CreateSegmentRequestVM request);

        [Get(path: "/api/segment/getsegment")]
        Task<ApiResponse<CreateSegmentRequestVM>> GetSegment(string SegmentID);

        [Delete(path: "/api/segment/deletesegment")]
        Task<HttpResponseMessage> DeleteSegment(string SegmentID);

        [Get(path: "/api/segment/getallsegmentlist")]
        Task<ApiResponse<List<GetSegmentResponseVM>>> GetAllSegmentList(SearchSegmentRequestVM request);

        [Get(path: "/api/segment/issegmentnameinuse")]
        Task<HttpResponseMessage> IsSegmentNameInUse(string SegmentName, string SegmentID);

        //[Get(path: "/api/segment/getallsegment")]
        //Task<ApiResponse<List<GetSegmentResponseVM>>> GetAllSegment();

        //[Get(path: "/api/segment/getallsegmentcountry")]
        //Task<ApiResponse<List<GetSegmentCountryResponse>>> GetAllSegmentCountry(string SegmentID);

        [Get(path: "/api/segment/isinusecount")]
        Task<HttpResponseMessage> IsInUseCount(string SegmentID);

    }
}
