using LikeKero.UI.ViewModels;
//using LikeKero.UI.ViewModels.Response.Dashboard;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]
    public interface IDashboardAPI
    {
        //[Get(path: "/api/dashboard/usersummary")]
        //Task<ApiResponse<UserSummaryResponseVM>> UserSummary(string RequesterUserId);

        //[Get(path: "/api/dashboard/courseassignmentandcompletion")]
        //Task<ApiResponse<List<CourseAssignmentAndCompletionResponseVM>>> CourseAssignmentAndCompletion(string RequesterUserId, string CourseId);

        //[Get(path: "/api/dashboard/monthlyenrollmentandcompletion")]
        //Task<ApiResponse<List<MonthlyEnrollmentAndCompletionResponseVM>>> MonthlyEnrollmentAndCompletion(string RequesterUserId, int Year, string CourseId);

        //[Get(path: "/api/dashboard/acpeclaimstatus")]
        //Task<ApiResponse<List<ACPEClaimStatusResponseVM>>> ACPEClaimStatus(string RequesterUserId, int Year, string CourseId);

        //[Get(path: "/api/dashboard/groupusercount")]
        //Task<ApiResponse<List<GroupUserCountResponseVM>>> GroupUserCount(string RequesterUserId);

        //[Get(path: "/api/dashboard/getactivencpdpbymonth")]
        //Task<ApiResponse<List<ActiveNCPDPByMonthResponseVM>>> GetActiveNCPDPByMonth(string RequesterUserId, int Year);
    }
}

