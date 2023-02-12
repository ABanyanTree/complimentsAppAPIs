using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Domain.Feature;
using LikeKero.Services.Interfaces;
namespace LikeKero.Services.Services
{
    public class FeatureMasterService : IFeatureMaster
    {
        private IFeatureMasterRepository iFeatureMasterRepository;
        public FeatureMasterService(IFeatureMasterRepository IFeatureMasterRepository) : base()
        {
            iFeatureMasterRepository = IFeatureMasterRepository;
        }
        public Task<int> AddEditAsync(FeatureMaster obj)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteAsync(FeatureMaster obj)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<FeatureMaster>> GetAllAsync(FeatureMaster obj)
        {
            throw new NotImplementedException();
        }
        public Task<FeatureMaster> GetAsync(FeatureMaster obj)
        {
            throw new NotImplementedException();
        }

        public FeatureMaster GetAllFeatures(FeatureMaster obj)
        {
            return iFeatureMasterRepository.GetAllAdminFeatures(obj);
        }

        
    }
}
