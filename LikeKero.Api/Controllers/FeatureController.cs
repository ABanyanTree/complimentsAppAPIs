using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LikeKero.Api.ApiPath;
using LikeKero.Api.Filters;
using LikeKero.Contract.Requests.Feature;
using LikeKero.Contract.Responses;
using LikeKero.Contract.Responses.Feature;
using LikeKero.Domain;
using LikeKero.Domain.Feature;
using LikeKero.Infra;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LikeKero.Api.Controllers
{
    public class FeatureController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFeatureMaster iFeatureMaster;
        private readonly IRoleFeatureMaster iRoleFeatureMaster;
        public FeatureController(IMapper mapper, IFeatureMaster IFeatureMaster, IRoleFeatureMaster IRoleFeatureMaster)
        {
            _mapper = mapper;
            iFeatureMaster = IFeatureMaster;
            iRoleFeatureMaster = IRoleFeatureMaster;
        }

        /// <summary>
        /// Get Features for Logged In User
        /// </summary>
        /// <param name="RequesterUserId">Logged In User Id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(FeatureMaster), statusCode: 200)]
        [HttpGet(ApiRoutes.FeatureMasterRoutes.GetAdminFeatures)]
        [CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_AdminRights)]
        public IActionResult GetAdminFeatures([FromQuery] string RequesterUserId)
        {
            FeatureMaster obj = new FeatureMaster()
            {
                RequesterUserId = RequesterUserId
            };
            var featureList = iFeatureMaster.GetAllFeatures(obj);
            var responseObj = _mapper.Map<AdminRightsResponse>(featureList);

            return Ok(responseObj);
        }

        /// <summary>
        /// Save Features for LoggdIn User
        /// </summary>
        /// <param name="singleFeature">Features object with RequesterUserId set as Logged In User Id</param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.FeatureMasterRoutes.SaveAdminRights)]
        [CustomAuthorize(FeatureId = FeatureMasterInfra.FEATURE_AdminRights)]
        public async Task<IActionResult> SaveAdminRights([FromBody] AdminRightsRequest singleFeature)
        {
            if (string.IsNullOrEmpty(singleFeature.RequesterUserId))
            {
                return ReturnErrorIfEmpty("RequesterUserId");
            }

            RoleFeatureMaster objFeature = _mapper.Map<RoleFeatureMaster>(singleFeature);
            if (string.IsNullOrEmpty(objFeature.RoleFeatureId))
            {
                objFeature.RoleFeatureId = Utility.GeneratorUniqueId("RF_");
            }
            await iRoleFeatureMaster.SaveFeature(objFeature);

            return Ok();
        }

        private IActionResult ReturnErrorIfEmpty(string FieldName)
        {
            ErrorModel errorModel = new ErrorModel();
            errorModel.FieldName = FieldName;
            errorModel.Message = "Field is Mandatory";
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Errors = new List<ErrorModel>();
            errorResponse.Errors.Add(errorModel);
            return BadRequest(errorResponse);
        }
    }
}