using AutoMapper;
using LikeKero.Api.ApiPath;
using LikeKero.Api.Filters;
using LikeKero.Contract.Responses;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Api.Controllers
{
    public class DashboardController : Controller
    {
        //private readonly IMapper _mapper;
        //private readonly IDashboardUserSummary iDashboardUserSummary;
        //private readonly ICourseAssignmentAndCompletion iCourseAssignmentAndCompletion;
        //private readonly IMonthlyEnrollmentAndCompletion iMonthlyEnrollmentAndCompletion;
        //private readonly IACPEClaimStatus iACPEClaimStatus;
        //private readonly IGroupUserCount iGroupUserCount;
        //private readonly IActiveNCPDPByMonth iActiveNCPDPByMonth;
        //public DashboardController(IMapper mapper, IDashboardUserSummary IDashboardUserSummary, ICourseAssignmentAndCompletion ICourseAssignmentAndCompletion, IMonthlyEnrollmentAndCompletion IMonthlyEnrollmentAndCompletion, IACPEClaimStatus IACPEClaimStatus, IGroupUserCount IGroupUserCount, IActiveNCPDPByMonth IActiveNCPDPByMonth)
        //{
        //    _mapper = mapper;
        //    iDashboardUserSummary = IDashboardUserSummary;
        //    iCourseAssignmentAndCompletion = ICourseAssignmentAndCompletion;
        //    iMonthlyEnrollmentAndCompletion = IMonthlyEnrollmentAndCompletion;
        //    iACPEClaimStatus = IACPEClaimStatus;
        //    iGroupUserCount = IGroupUserCount;
        //    iActiveNCPDPByMonth = IActiveNCPDPByMonth;
        //}

        ///// <summary>
        ///// Get User summary data for dashboard 
        ///// </summary>
        ///// <param name="RequesterUserId"></param>
        ///// <returns></returns>
        //[HttpGet(ApiRoutes.DashboardRoute.UserSummary)]
        //[ProducesResponseType(typeof(UserSummaryResponse), statusCode: 200)]
        //[CustomAuthorize]
        //public async Task<IActionResult> UserSummary(string RequesterUserId)
        //{
        //    if (string.IsNullOrEmpty(RequesterUserId))
        //    {
        //        return ReturnErrorIfIsEmpty("RequesterUserId");
        //    }
        //    UserSummary obj = new UserSummary()
        //    {
        //        RequesterUserId = RequesterUserId
        //    };

        //    var objResponse = await iDashboardUserSummary.GetAsync(obj);
        //    var responseObj = _mapper.Map<UserSummaryResponse>(objResponse);

        //    return Ok(responseObj);
        //}

        //private IActionResult ReturnErrorIfIsEmpty(string FieldName)
        //{
        //    ErrorModel errorModel = new ErrorModel();
        //    errorModel.FieldName = FieldName;
        //    errorModel.Message = "Field is Mandatory";
        //    ErrorResponse errorResponse = new ErrorResponse();
        //    errorResponse.Errors = new List<ErrorModel>();
        //    errorResponse.Errors.Add(errorModel);
        //    return BadRequest(errorResponse);
        //}

        ///// <summary>
        ///// Get Course Assignment and Completion Details
        ///// </summary>
        ///// <param name="RequesterUserId"></param>
        ///// <param name="CourseId"></param>
        ///// <returns></returns>
        //[HttpGet(ApiRoutes.DashboardRoute.CourseAssignmentAndCompletion)]
        //[ProducesResponseType(typeof(CourseAssignmentAndCompletionResponse), statusCode: 200)]
        //[CustomAuthorize]
        //public async Task<IActionResult> CourseAssignmentAndCompletion(string RequesterUserId, string CourseId)
        //{
        //    if (string.IsNullOrEmpty(RequesterUserId))
        //    {
        //        return ReturnErrorIfIsEmpty("RequesterUserId");
        //    }
        //    CourseAssignment obj = new CourseAssignment()
        //    {
        //        RequesterUserId = RequesterUserId,
        //        CourseId = CourseId
        //    };

        //    var objResponse = await iCourseAssignmentAndCompletion.GetCourseAssignmentAndCompletionAsync(obj);
        //    var responseObj = _mapper.Map<List<CourseAssignmentAndCompletionResponse>>(objResponse);

        //    return Ok(responseObj);
        //}

        ///// <summary>
        ///// Get Monthly Enrollment and Completion Details
        ///// </summary>
        ///// <param name="RequesterUserId"></param>
        ///// <param name="year"></param>
        ///// <param name="CourseId"></param>
        ///// <returns></returns>
        //[HttpGet(ApiRoutes.DashboardRoute.MonthlyEnrollmentAndCompletion)]
        //[ProducesResponseType(typeof(CourseAssignmentAndCompletionResponse), statusCode: 200)]
        //[CustomAuthorize]
        //public async Task<IActionResult> MonthlyEnrollmentAndCompletion(string RequesterUserId, int year, string CourseId)
        //{
        //    if (string.IsNullOrEmpty(RequesterUserId))
        //    {
        //        return ReturnErrorIfIsEmpty("RequesterUserId");
        //    }
        //    MonthlyEnrollmentAndCompletion obj = new MonthlyEnrollmentAndCompletion()
        //    {
        //        RequesterUserId = RequesterUserId,
        //        Year = year,
        //        CourseId = CourseId
        //    };

        //    var objResponse = await iMonthlyEnrollmentAndCompletion.GetMonthlyEnrollmentAndCompletion(obj);
        //    var responseObj = _mapper.Map<List<MonthlyEnrollmentAndCompletionResponse>>(objResponse);

        //    return Ok(responseObj);
        //}

        ///// <summary>
        ///// Get Monthly Enrollment and Completion Details
        ///// </summary>
        ///// <param name="RequesterUserId"></param>
        ///// <param name="year"></param>
        ///// <param name="CourseId"></param>
        ///// <returns></returns>
        //[HttpGet(ApiRoutes.DashboardRoute.ACPEClaimStatus)]
        //[ProducesResponseType(typeof(CourseAssignmentAndCompletionResponse), statusCode: 200)]
        //[CustomAuthorize]
        //public async Task<IActionResult> ACPEClaimStatus(string RequesterUserId, int year, string CourseId)
        //{
        //    if (string.IsNullOrEmpty(RequesterUserId))
        //    {
        //        return ReturnErrorIfIsEmpty("RequesterUserId");
        //    }
        //    ACPEClaimStatus obj = new ACPEClaimStatus()
        //    {
        //        RequesterUserId = RequesterUserId,
        //        Year = year,
        //        CourseId = CourseId
        //    };

        //    var objResponse = await iACPEClaimStatus.GetACPEClaimStatus(obj);
        //    var responseObj = _mapper.Map<List<ACPEClaimStatusResponse>>(objResponse);

        //    return Ok(responseObj);
        //}

        ///// <summary>
        ///// Get Group User Count
        ///// </summary>
        ///// <param name="RequesterUserId"></param>
        ///// <returns></returns>
        //[HttpGet(ApiRoutes.DashboardRoute.GroupUserCount)]
        //[ProducesResponseType(typeof(GroupUserCountResponse), statusCode: 200)]
        //[CustomAuthorize]
        //public async Task<IActionResult> GroupUserCount(string RequesterUserId)
        //{
        //    if (string.IsNullOrEmpty(RequesterUserId))
        //    {
        //        return ReturnErrorIfIsEmpty("RequesterUserId");
        //    }
        //    GroupUserCount obj = new GroupUserCount()
        //    {
        //        RequesterUserId = RequesterUserId
        //    };

        //    var objResponse = await iGroupUserCount.GetGroupUserCount(obj);
        //    var responseObj = _mapper.Map<List<GroupUserCountResponse>>(objResponse);

        //    return Ok(responseObj);
        //}

        ///// <summary>
        ///// Get Active NCPDP By Month
        ///// </summary>
        ///// <param name="RequesterUserId"></param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //[HttpGet(ApiRoutes.DashboardRoute.GetActiveNCPDPByMonth)]
        //[ProducesResponseType(typeof(ActiveNCPDPByMonthResponse), statusCode: 200)]
        //[CustomAuthorize]
        //public async Task<IActionResult> GetActiveNCPDPByMonth(string RequesterUserId, int year)
        //{
        //    if (string.IsNullOrEmpty(RequesterUserId))
        //    {
        //        return ReturnErrorIfIsEmpty("RequesterUserId");
        //    }
        //    ActiveNCPDPByMonth obj = new ActiveNCPDPByMonth()
        //    {
        //        RequesterUserId = RequesterUserId,
        //        Year = year
        //    };

        //    var objResponse = await iActiveNCPDPByMonth.GetActiveNCPDPByMonth(obj);
        //    var responseObj = _mapper.Map<List<ActiveNCPDPByMonthResponse>>(objResponse);

        //    return Ok(responseObj);
        //}
    }
}