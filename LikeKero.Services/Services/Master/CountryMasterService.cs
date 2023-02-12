using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Data.Constants;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Infra;
using LikeKero.Services.Interfaces;
namespace LikeKero.Services.Services
{
    public class CountryMasterService : ICountryMaster
    {
        private ICountryMasterRepository iCountryMasterRepository;
        public CountryMasterService(ICountryMasterRepository ICountryMasterRepository) : base()
        {
            iCountryMasterRepository = ICountryMasterRepository;
        }
        public Task<int> AddEditAsync(CountryMaster obj)
        {
            throw new NotImplementedException();
        }

        public async Task<string> AddEditCountry(CountryMaster obj)
        {
            if (string.IsNullOrEmpty(obj.CountryID))
                obj.CountryID = Utility.GeneratorUniqueId("COU");
            await iCountryMasterRepository.AddEditCountry(obj);
            return obj.CountryID;
        }

        public async Task<int> DeleteAsync(CountryMaster obj)
        {
            return await iCountryMasterRepository.DeleteCountryMaster(obj.CountryID);
        }
        public async Task<IEnumerable<CountryMaster>> GetAllAsync(CountryMaster obj)
        {
            return await iCountryMasterRepository.GetAllCountryList(obj);
        }



        public async Task<CountryMaster> GetAsync(CountryMaster obj)
        {
            return await iCountryMasterRepository.GetCountry(obj.CountryID);
        }

        public async Task<CountryMaster> IsCountryNameInUse(string CountryName)
        {
            return await iCountryMasterRepository.IsCountryNameInUse(CountryName);
        }

        public async Task<IEnumerable<CountryMaster>> GetAllCountry()
        {
            return await iCountryMasterRepository.GetAllCountry();
        }

        public async Task<CountryMaster> IsInUseCount(string CountryID)
        {
            return await iCountryMasterRepository.IsInUseCount(CountryID);
        }

        public async Task<IEnumerable<CountryMaster>> GetAllLOBCountryDetails(CountryMaster obj)
        {
            return await iCountryMasterRepository.GetAllLOBCountryDetails(obj);
        }

        public async Task<IEnumerable<CountryMaster>> GetAllCountryByIDs(CountryMaster countryMaster)
        {
            return await iCountryMasterRepository.GetAllCountryByIDs(countryMaster);
        }
    }
}
