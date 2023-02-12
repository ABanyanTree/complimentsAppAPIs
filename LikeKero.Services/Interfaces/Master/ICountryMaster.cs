using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;
namespace LikeKero.Services.Interfaces
{
    public interface ICountryMaster : IServiceBase<CountryMaster>
    {
        Task<string> AddEditCountry(CountryMaster obj);
        Task<CountryMaster> IsCountryNameInUse(string CountryName);
        Task<IEnumerable<CountryMaster>> GetAllCountry();
        Task<CountryMaster> IsInUseCount(string CountryID);
        Task<IEnumerable<CountryMaster>> GetAllLOBCountryDetails(CountryMaster countryMaster);
        Task<IEnumerable<CountryMaster>> GetAllCountryByIDs(CountryMaster countryMaster);
    }
}
