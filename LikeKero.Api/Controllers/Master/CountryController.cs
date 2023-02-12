using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Api.ApiPath;
using LikeKero.Api.Filters;
using LikeKero.Contract.Requests;
using LikeKero.Contract.Responses;
using LikeKero.Domain;
using LikeKero.Infra;
using LikeKero.Services.Interfaces;

namespace LikeKero.Api.Controllers
{

    public class CountryController : Controller
    {
        private readonly ICountryMaster _iCountry;
        private readonly IMapper _mapper;


        public CountryController(ICountryMaster Country, IMapper mapper)
        {
            _iCountry = Country;
            _mapper = mapper;
        }

        /// <summary>
        /// Add Edit Country
        /// </summary>
        [HttpPost(ApiRoutes.CountryMaster.AddEditCountry), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 401)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_Country)]
        public async Task<IActionResult> AddEditCountry([FromBody] CountryRequest request)
        {
            var obj = _mapper.Map<CountryMaster>(request);
            var CountryID = await _iCountry.AddEditCountry(obj);
            return Ok(CountryID);

        }

        /// <summary>
        /// Get Country
        /// </summary>
        /// <param name="CountryID">Country ID</param>      
        /// <returns></returns>
        [HttpGet(ApiRoutes.CountryMaster.GetCountry)]
        [ProducesResponseType(typeof(GetCountryResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_Country)]
        public async Task<IActionResult> GetCountry([FromQuery] string CountryID)
        {
            var objResponse = await _iCountry.GetAsync(new CountryMaster { CountryID = CountryID });
            var responseObj = _mapper.Map<GetCountryResponse>(objResponse);
            return Ok(responseObj);
        }


        /// <summary>
        /// delete Country
        /// </summary>
        /// <param name="CountryID">Country ID</param>      
        /// <returns></returns>
        [HttpDelete(ApiRoutes.CountryMaster.DeleteCountry)]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_Country)]
        public async Task<IActionResult> DeleteCountry([FromQuery] string CountryID)
        {
            var objResponse = await _iCountry.DeleteAsync(new CountryMaster { CountryID = CountryID });
            return Ok(objResponse);
        }



        /// <summary>
        /// Get list of all Country
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.CountryMaster.GetAllCountryList)]
        [ProducesResponseType(typeof(GetCountryResponse), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_Country)]
        public async Task<IActionResult> GetAllCountryList(SearchCountryRequest request)
        {
            var obj = _mapper.Map<CountryMaster>(request);
            var objResponse = await _iCountry.GetAllAsync(obj);

            var responseObj = _mapper.Map<List<GetCountryResponse>>(objResponse);
            if (responseObj == null)
                return Ok(new List<GetCountryResponse>());

            return Ok(responseObj);
        }


        /// <summary>
        /// Check if Country Name Exists
        /// </summary>
        /// <param name="CountryName"></param>
        /// <param name="CountryID"></param> 
        /// <response code="200">True if CountryName does not exists else error message</response>         
        /// <response code="401">Unauthorized</response>
        [HttpGet(ApiRoutes.CountryMaster.IsCountryNameInUse)]
        [ProducesResponseType(typeof(bool), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_Country)]
        public async Task<IActionResult> IsCountryNameInUse([FromQuery] string CountryName)
        {
            if (string.IsNullOrEmpty(CountryName))
            {
                return ReturnErrorIfUserIDIsEmpty("CountryName");
            }

            var user = await _iCountry.IsCountryNameInUse(CountryName);
            if (user == null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }

        }


        /// <summary>
        /// Get All Country
        /// </summary>   
        /// <returns>Get All Country </returns>
        /// <response code="200">Lists of all Dropdown Values</response>         
        /// <response code="401">Unauthorized</response>
        [HttpGet(ApiRoutes.CountryMaster.GetAllCountry)]
        [ProducesResponseType(typeof(GetCountryResponse), statusCode: 200)]
        [ProducesResponseType(typeof(ErrorResponse), statusCode: 400)]
        //[CustomAuthorizeAttribute]
        public async Task<IActionResult> GetAllCountry()
        {
            var objResponse = await _iCountry.GetAllCountry();
            var responseObj = _mapper.Map<List<GetCountryResponse>>(objResponse);
            if (responseObj == null)
                return Ok(new List<GetCountryResponse>());
            return Ok(responseObj);
        }


        /// <summary>
        ///  Country IsInUseCount
        /// </summary>
        /// <param name="CountryID">Country ID</param>      
        /// <returns></returns>
        [HttpGet(ApiRoutes.CountryMaster.IsInUseCount)]
        [ProducesResponseType(typeof(string), statusCode: 200)]
        [CustomAuthorizeAttribute(FeatureId = FeatureMasterInfra.FEATURE_Country)]
        public async Task<IActionResult> IsInUseCount([FromQuery] string CountryID)
        {
            var objResponse = await _iCountry.IsInUseCount(CountryID);
            return Ok(objResponse.TotalCount);
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

    }
}