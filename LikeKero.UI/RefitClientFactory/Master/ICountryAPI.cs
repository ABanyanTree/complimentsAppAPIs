using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LikeKero.UI.ViewModels;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface ICountryAPI
    {
        [Post(path: "/api/country/addeditcountry")]
        Task <HttpResponseMessage> AddEditCountry(CreateCountryRequestVM request);

        [Get(path: "/api/country/getcountry")]
        Task<ApiResponse<CreateCountryRequestVM>> GetCountry(string CountryID);

        [Delete(path: "/api/country/deletecountry")]
        Task<HttpResponseMessage> DeleteCountry(string CountryID);

        [Get(path: "/api/country/getallcountrylist")]
        Task<ApiResponse<List<GetCountryResponseVM>>> GetAllCountryList(SearchCountryRequestVM request);

        [Get(path: "/api/country/iscountrynameinuse")]
        Task<HttpResponseMessage> IsCountryNameInUse(string CountryName, string CountryID);

        [Get(path: "/api/country/getallcountry")]
        Task<ApiResponse<List<GetCountryResponseVM>>> GetAllCountry();

        [Get(path: "/api/country/isinusecount")]
        Task<HttpResponseMessage> IsInUseCount(string CountryID);

    }
}
