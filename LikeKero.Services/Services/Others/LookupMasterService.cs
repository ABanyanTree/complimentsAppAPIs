using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Services.Interfaces;
namespace LikeKero.Services.Services
{
    public class LookupMasterService : ILookupMaster
    {
        private ILookupMasterRepository iLookupMasterRepository;
        public LookupMasterService(ILookupMasterRepository ILookupMasterRepository) : base()
        {
            iLookupMasterRepository = ILookupMasterRepository;
        }
        public Task<int> AddEditAsync(LookupMaster obj)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteAsync(LookupMaster obj)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<LookupMaster>> GetAllAsync(LookupMaster obj)
        {
            return iLookupMasterRepository.GetAllAsync(obj);
        }
        public Task<LookupMaster> GetAsync(LookupMaster obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LookupMaster>> GetByType(LookupMaster obj)
        {
            return await iLookupMasterRepository.GetByTypeAsync(obj);
        }
    }
}
