using LikeKero.UI.ViewModels;

using LikeKero.UI.ViewModels.Request.User;
using LikeKero.UI.ViewModels.Response;
using LikeKero.UI.ViewModels.Response.User;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]

    public interface ITestCustomerAPI
    {
        [Post(path: "/api/testcutomer/addedittestcustomer")]
        Task<HttpResponseMessage> SaveTestCustomer(CreateTestCustomerRequestVM request);

        [Get(path: "/api/testcustomer/gettestcustomer")]
        Task<ApiResponse<CreateTestCustomerRequestVM>> GetTestCustomer(string CustId);

        [Get(path: "/api/testcustomer/deletetestcustomer")]
        Task<HttpResponseMessage> DeleteTestCustomer(string CustId);

        [Get(path: "/api/testcustomer/getalltestcustomer")]
        Task<ApiResponse<List<GetTestCustomerResponseVM>>> GetAllTestCustomer(SearchTestCustomerRequestVM request);

        [Multipart]
        [Post(path: "/api/testcustomer/uploadtestcustomerfile")]
        Task<HttpResponseMessage> UploadTestCustomerFile(string CustId, StreamPart uploadedFile);

    }
}
