using LikeKero.Domain.DomainInterfaces.Feature;
using LikeKero.Domain.Feature;
using LikeKero.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Services.Services.Feature
{
    public class RoleFeatureMasterService : IRoleFeatureMaster
    {
        private IRoleFeatureMasterRepository iRoleFeatureMasterRepository;
        public RoleFeatureMasterService(IRoleFeatureMasterRepository IRoleFeatureMasterRepository) : base()
        {
            iRoleFeatureMasterRepository = IRoleFeatureMasterRepository;
        }
        public Task<int> AddEditAsync(RoleFeatureMaster obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(RoleFeatureMaster obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoleFeatureMaster>> GetAllAsync(RoleFeatureMaster obj)
        {
            throw new NotImplementedException();
        }

        public Task<RoleFeatureMaster> GetAsync(RoleFeatureMaster obj)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveFeature(RoleFeatureMaster obj)
        {
            return await iRoleFeatureMasterRepository.SaveRoleFeature(obj);
        }
    }
}
