using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface ICountryMasterRepository : IRepository<CountryMaster>
    {
        Task<int> AddEditCountry(CountryMaster obj);
        Task<CountryMaster> GetCountry(string CountryID);
        Task<IEnumerable<CountryMaster>> GetAllCountryList(CountryMaster obj);
        Task<int> DeleteCountryMaster(string CountryID);
        Task<CountryMaster> IsCountryNameInUse(string CountryName);
        Task<IEnumerable<CountryMaster>> GetAllCountry();
        Task<CountryMaster> IsInUseCount(string CountryID);
        Task<IEnumerable<CountryMaster>> GetAllLOBCountryDetails(CountryMaster obj);
        Task<IEnumerable<CountryMaster>> GetAllCountryByIDs(CountryMaster obj);
    }
}
