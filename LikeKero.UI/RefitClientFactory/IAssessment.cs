using DelphianLMS.UI.ViewModels.Request.Assessment;
using DelphianLMS.UI.ViewModels.Response.Assessment;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DelphianLMS.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]
    public interface ITestApi
    {
        [Get(path: "/api/assessment/getassessment")]
        Task<ApiResponse<CreateAssessmentRequestVM>> GetAssessment(CreateAssessmentRequestVM model);

        [Get(path: "/api/assessment/getallassessments")]
        Task<ApiResponse<List<GetAssessmentResponseVM>>> GetAssessmentList(CreateAssessmentRequestVM model);

        [Post(path: "/api/assessment/saveassessment")]
        Task<HttpResponseMessage> SaveAssessment(CreateAssessmentRequestVM model);
    }
}
