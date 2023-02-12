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
    public class EmailLogController : Controller
    {
        private readonly IEmailSentLog emailSentLog;
        private readonly IMapper _mapper;
        public EmailLogController(IMapper mapper, IEmailSentLog IEmailSentLog)
        {
            _mapper = mapper;
            emailSentLog = IEmailSentLog;
        }
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_EMAILLOGS)]
        [HttpPost(ApiRoutes.EmailLogRoutes.EmailLogList)]
        [ProducesResponseType(typeof(EmailSentLogResponse), statusCode: 200)]
        public async Task<IActionResult> EmailLogList([FromBody] EmailSentLogRequest request)
        {
            var obj = _mapper.Map<EmailSentLog>(request);
            var emailLogList = await emailSentLog.GetAllAsync(obj);
            var responseObj = _mapper.Map<List<EmailSentLogResponse>>(emailLogList);

            return Ok(responseObj);

        }

        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_EMAILLOGS)]
        [HttpPost(ApiRoutes.EmailLogRoutes.GetEmailLog)]
        [ProducesResponseType(typeof(EmailGetResponse), statusCode: 200)]
        public async Task<IActionResult> GetEmailLog([FromBody] EmailGetRequest request)
        {
            var obj = _mapper.Map<EmailSentLog>(request);
            var emailLog = await emailSentLog.GetAsync(obj);
            var responseObj = _mapper.Map<EmailGetResponse>(emailLog);

            return Ok(responseObj);
        }
    }
}