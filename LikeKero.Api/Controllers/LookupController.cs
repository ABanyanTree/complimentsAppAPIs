using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LikeKero.Api.ApiPath;
using LikeKero.Contract.Requests.Others;
using LikeKero.Contract.Responses.Others;
using LikeKero.Domain;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LikeKero.Api.Controllers
{
    public class LookupController : Controller
    {
        private readonly ILookupMaster iLookup;
        private readonly IMapper _mapper;
        public LookupController(IMapper mapper, ILookupMaster lookupMaster)
        {
            _mapper = mapper;
            iLookup = lookupMaster;
        }

        /// <summary>
        /// Get all Lookups by type
        /// </summary>
        /// <param name="LookupType">Lookup Type</param>
        /// <returns>Lookup data</returns>
        /// <response code="200">Lookup List</response>
        [HttpPost(ApiRoutes.Lookup.GetLookupByType)]
        [ProducesResponseType(typeof(List<LookupByTypeResponse>), statusCode: 200)]
        public async Task<IActionResult> GetLookupByType([FromQuery] string LookupType)
        {
            LookupByTypeRequest request = new LookupByTypeRequest() { LookUpType = LookupType };

            var lookupobj = _mapper.Map<LookupMaster>(request);
            var lookuplist = await iLookup.GetByType(lookupobj);

            var responseObj = _mapper.Map<List<LookupByTypeResponse>>(lookuplist);

            return Ok(responseObj);
        }
    }
}