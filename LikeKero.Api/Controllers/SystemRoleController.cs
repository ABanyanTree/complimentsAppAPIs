using AutoMapper;
using LikeKero.Api.ApiPath;
using LikeKero.Api.Filters;
using LikeKero.Contract.Requests.User;
using LikeKero.Contract.Responses;
using LikeKero.Contract.Responses.SystemRole;
using LikeKero.Contract.Responses.User;
using LikeKero.Domain;
using LikeKero.Infra;
using LikeKero.Services.Interfaces.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Api.Controllers
{
    public class SystemRoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISystemRole iSystemRole;
        public SystemRoleController(IMapper mapper, ISystemRole ISystemRole)
        {
            _mapper = mapper;
            iSystemRole = ISystemRole;
        }

        /// <summary>
        /// Get all System Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.SystemRole.GetAllRoles)]
        [ProducesResponseType(typeof(List<SystemRoleResponse>), statusCode: 200)]
        [CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_MANAGESYSTEMROLE)]
        public async Task<IActionResult> GetAllRoles(string RequesterUserId)
        {

            if (string.IsNullOrEmpty(RequesterUserId))
            {
                return ReturnErrorIfUserIDIsEmpty("RequesterUserId");
            }

            UserRole ent = new UserRole();
            //ent.RequesterUserId = RequesterUserId;
            var response = await iSystemRole.GetAllAsync(ent);
            var responseObj = _mapper.Map<List<SystemRoleResponse>>(response);

            return Ok(responseObj);
        }

        private IActionResult ReturnErrorIfUserIDIsEmpty(string FieldName)
        {
            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = FieldName;
            errorModel.Message = "Field is Mandatory";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);
            return BadRequest(errorResponse);
        }


        /// <summary>
        /// Get all Groups based on given Search criteria
        /// </summary>
        /// <param name="request">Logged In User's Id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<AssignmentSearchResponse>), statusCode: 200)]
        [CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_MANAGESYSTEMROLE)]
        [HttpPost(ApiRoutes.SystemRole.SearchForAssignment)]
        public async Task<IActionResult> SearchForAssignment([FromBody] UserRole request)
        {
            //UserRole obj = new UserRole()
            //{
            //    RequesterUserId = RequesterUserId,
            //    SearchPhrase = SearchPhrase,
            //    SearchOnGroupId = SearchOnGroupId
            //};

            var response = await iSystemRole.AssignmentSearch(request);
            var responseObj = _mapper.Map<List<AssignmentSearchResponse>>(response);

            return Ok(responseObj);
        }

        ///// <summary>
        ///// Get all Admin users for specific Group
        ///// </summary>
        ///// <param name="RequesterUserId">Logged In User's Id</param>
        ///// <param name="SearchOnGroupId">Group1Id / Group2Id / Group3Id / Group4Id / Group5Id </param>
        ///// <param name="GroupId">Selected GroupId</param>
        ///// <returns></returns>
        //[ProducesResponseType(typeof(List<GroupAdminUsersResponse>), statusCode: 200)]
        //[CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_MANAGESYSTEMROLE)]
        //[HttpGet(ApiRoutes.SystemRole.GroupAdminUsers)]
        //public async Task<IActionResult> GroupAdminUsers([FromQuery] string RequesterUserId, string GroupId, string SearchOnGroupId)
        //{
        //    UserRole obj = new UserRole()
        //    {
        //        RequesterUserId = RequesterUserId,
        //        GroupId = GroupId,
        //        SearchOnGroupId = SearchOnGroupId
        //    };

        //    var response = await iSystemRole.AdminUsersForGroup(obj);
        //    var responseObj = _mapper.Map<List<GroupAdminUsersResponse>>(response);

        //    return Ok(responseObj);
        //}

        /// <summary>
        /// Make User Role Active or Inactive
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns></returns>
        [CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_MANAGESYSTEMROLE)]
        [HttpPost(ApiRoutes.SystemRole.ActiveInActiveUserRole)]
        public async Task<IActionResult> ActiveInActiveUserRole([FromBody] SystemRoleCreateUpdateRequest request)
        {
            var userRoleObj = _mapper.Map<UserRole>(request);
            await iSystemRole.ActiveInActiveRole(userRoleObj);
            return Ok();
        }

        /// <summary>
        /// Assign System Role to User
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns></returns>
        [CustomAuthorize]
        [HttpPost(ApiRoutes.SystemRole.AssignRole)]
        public async Task<IActionResult> AssignRole([FromBody] SystemRoleCreateUpdateRequest request)
        {
            var userRoleObj = _mapper.Map<UserRole>(request);
            string userRoleId = await iSystemRole.AssignSystemRole(userRoleObj);
            return Ok(userRoleId);
        }

        ///// <summary>
        ///// Delete System Role
        ///// </summary>
        ///// <param name="RequesterUserId">Logged In User's Id</param>
        ///// <param name="UserRoleId">UserRoleId </param>
        ///// <returns></returns>
        //[ProducesResponseType(typeof(List<GroupAdminUsersResponse>), statusCode: 200)]
        //[CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_MANAGESYSTEMROLE)]
        //[HttpGet(ApiRoutes.SystemRole.DeleteUserSystemRole)]
        //public async Task<IActionResult> DeleteUserSystemRole([FromQuery] string RequesterUserId, string UserRoleId)
        //{
        //    UserRole obj = new UserRole()
        //    {
        //        RequesterUserId = RequesterUserId,
        //        UserRoleId = UserRoleId
        //    };

        //    var response = await iSystemRole.DeleteSystemRole(obj);

        //    return Ok();
        //}

    }
}