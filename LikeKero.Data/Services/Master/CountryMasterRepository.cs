using System;
using LikeKero.Data.Constants;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LikeKero.Data.Services
{
    public class CountryMasterRepository : Repository<CountryMaster>, ICountryMasterRepository
    {
        public CountryMasterRepository(IOptions<ReadConfig> connStr, IDapperResolver<CountryMaster> resolver) : base(connStr, resolver)
        {
        }

        public async Task<int> AddEditCountry(CountryMaster obj)
        {
            string[] addParams = new string[] { CountryMasterInfra.COUNTRYID, CountryMasterInfra.REGIONID, CountryMasterInfra.COUNTRYNAME, CountryMasterInfra.COUNTRYDESCRIPTION, CountryMasterInfra.ISACTIVE, BaseInfra.REQUESTERUSERID };
            return await ExecuteNonQueryAsync(obj, addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_UPS);
        }

        public async Task<int> DeleteCountryMaster(string CountryID)
        {
            CountryMaster obj = new CountryMaster() { CountryID = CountryID };
            string[] addParams = new string[] { CountryMasterInfra.COUNTRYID };
            return await ExecuteNonQueryAsync(obj, addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_DEL);
        }

        public async Task<IEnumerable<CountryMaster>> GetAllCountryList(CountryMaster obj)
        {
            string[] addParams = new string[] { CountryMasterInfra.COUNTRYNAME, CountryMasterInfra.REGIONID, BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP };
            return await GetAllAsync(obj, addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_LSTALL);
        }

        public async Task<CountryMaster> GetCountry(string CountryID)
        {
            CountryMaster obj = new CountryMaster() { CountryID = CountryID };
            string[] addParams = new string[] { CountryMasterInfra.COUNTRYID };
            return await GetAsync(obj, addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_SEL);
        }

        public async Task<CountryMaster> IsCountryNameInUse(string CountryName)
        {
            CountryMaster obj = new CountryMaster() { CountryName = CountryName };
            string[] addParams = new string[] { CountryMasterInfra.COUNTRYNAME };
            return await GetAsync(obj, addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_ISCOUNTRYNAMEINUSE);
        }

        public async Task<IEnumerable<CountryMaster>> GetAllCountry()
        {
            string[] addParams = new string[] { };
            return await GetAllAsync(new CountryMaster(), addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_GETALL);

        }

        public async Task<CountryMaster> IsInUseCount(string CountryID)
        {
            CountryMaster obj = new CountryMaster() { CountryID = CountryID };
            string[] addParams = new string[] { CountryMasterInfra.COUNTRYID };
            return await GetAsync(obj, addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_ISINUSECOUNT);
        }

        public async Task<IEnumerable<CountryMaster>> GetAllLOBCountryDetails(CountryMaster obj)
        {
            string[] addParams = new string[] { CountryMasterInfra.LOBID };
            return await GetAllAsync(obj, addParams, CountryMasterInfra.SPROC_GETALLLOBCOUNTRYDETAILS_LSTALL);
        }

        public async Task<IEnumerable<CountryMaster>> GetAllCountryByIDs(CountryMaster obj)
        {
            string[] addParams = new string[] { CountryMasterInfra.COUNTRYID };
            return await GetAllAsync(obj, addParams, CountryMasterInfra.SPROC_COUNTRYMASTER_GETALLBYIDS);
        }
    }
}
