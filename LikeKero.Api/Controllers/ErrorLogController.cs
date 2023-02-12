using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LikeKero.Api.ApiPath;
using LikeKero.Api.Filters;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Responses;
using LikeKero.Domain;
using LikeKero.Infra;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LikeKero.Api.Controllers
{
    public class ErrorLogController : Controller
    {
        private readonly IErrorLogs errorLogs;
        private readonly IMapper _mapper;
        public ErrorLogController(IMapper mapper, IErrorLogs IErrorLogs)
        {
            _mapper = mapper;
            errorLogs = IErrorLogs;
        }

        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ERRORLOGS)]
        [HttpPost(ApiRoutes.ErrorLogRoutes.ErrorLogList)]
        [ProducesResponseType(typeof(ErrorLogResponse), statusCode: 200)]
        public async Task<IActionResult> ErrorLogList([FromBody] ErrorLogRequest request)
        {
            var obj = _mapper.Map<ErrorLogs>(request);
            var errorLogList = await errorLogs.GetAllAsync(obj);
            var responseObj = _mapper.Map<List<ErrorLogResponse>>(errorLogList);

            return Ok(responseObj);
        }

        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_ERRORLOGS)]
        [HttpPost(ApiRoutes.ErrorLogRoutes.GetErrorLog)]
        [ProducesResponseType(typeof(ErrorGetResponse), statusCode: 200)]
        public async Task<IActionResult> GetErrorLog([FromBody] ErrorGetRequest request)
        {
            var obj = _mapper.Map<ErrorLogs>(request);
            var errorLog = await errorLogs.GetAsync(obj);
            var responseObj = _mapper.Map<ErrorGetResponse>(errorLog);

            return Ok(responseObj);
        }

    }
}