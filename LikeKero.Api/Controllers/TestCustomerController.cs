using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LikeKero.Api.ApiPath;
using LikeKero.Api.Filters;
using LikeKero.Api.options;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Requests.User;
using LikeKero.Contract.Responses;
using LikeKero.Contract.Responses.Feature;
using LikeKero.Contract.Responses.User;
using LikeKero.Domain;
using LikeKero.Infra;
using LikeKero.Infra.Encryption;
using LikeKero.Infra.FileSystem;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LikeKero.Api.Controllers
{

    public class TestCustomerController : Controller
    {
        private readonly ITestCustomer _iTestCustomer;
        private readonly IMapper _mapper;


        public TestCustomerController(ITestCustomer testCustomer, IMapper mapper)
        {
            _iTestCustomer = testCustomer;
            _mapper = mapper;
        }
      
        /// <summary>
        /// Add Test Customer
        /// </summary>
        /// <param name="request"></param>       
        /// <returns>UserID</returns>
        /// <response code="200">CustId of Customer</response>
        /// <response code="400">ErrorResponse</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.TestCustomer.AddEditTestCustomer), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_TESTCUSTOMER)]
        public async Task<IActionResult> AddEditTestCustomer([FromBody] TestCustomerRequest request)
        {
            var Custobj = _mapper.Map<TestCustomer>(request);
            var CustID = await _iTestCustomer.SaveCustomer(Custobj);
            return Ok(CustID);

        }

        /// <summary>
        /// Get Test Customer
        /// </summary>
        /// <param name="CustId">Cust Id</param>      
        /// <returns></returns>
        [HttpGet(ApiRoutes.TestCustomer.GetTestCustomer)]
        [ProducesResponseType(typeof(TestCustomerGetResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_TESTCUSTOMER)]
        public async Task<IActionResult> GetTestCustomer([FromQuery] string CustId)
        {
            var objResponse = await _iTestCustomer.GetTestCustomer(CustId);           
            return Ok(objResponse);
        }


        /// <summary>
        /// delete Test Customer
        /// </summary>
        /// <param name="CustId">Cust Id</param>      
        /// <returns></returns>
        [HttpGet(ApiRoutes.TestCustomer.DeleteTestCustomer)]
        [ProducesResponseType(typeof(TestCustomerGetResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_TESTCUSTOMER)]
        public async Task<IActionResult> DeleteTestCustomer([FromQuery] string CustId)
        {
            var objResponse = await _iTestCustomer.DeleteTestCustomer(CustId);
            return Ok(objResponse);
        }



        /// <summary>
        /// Get list of all Test Customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.TestCustomer.GetAllTestCustomer)]
        [ProducesResponseType(typeof(TestCustomerGetResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_TESTCUSTOMER)]       
        public async Task<IActionResult> GetAllTestCustomer(SearchTestCustomerRequest request)
        {
            var objEvent = _mapper.Map<TestCustomer>(request);
            var objResponse = await _iTestCustomer.GetAllAsync(objEvent);

            var responseObj = _mapper.Map<List<TestCustomerGetResponse>>(objResponse);
            if (responseObj == null)
                return Ok(new List<TestCustomerGetResponse>());

            return Ok(responseObj);
        }


        /// <summary>
        /// Upload Test customer
        /// </summary>       
        /// <param name="CustId"></param>
        /// <param name="uploadedFile">Event Id</param>       
        /// <returns>IsSuccess as Boolean</returns>
        /// <response code="200">True or False</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost(ApiRoutes.TestCustomer.UploadTestCustomerFile), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(bool), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_TESTCUSTOMER)]
        public async Task<IActionResult> UploadTestCustomerFile([FromForm] string CustId, [FromForm] IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var IsSuccess = await _iTestCustomer.SaveFile(CustId, uploadedFile);
                return Ok(true);
            }

            return BadRequest(false);

        }


    }
}