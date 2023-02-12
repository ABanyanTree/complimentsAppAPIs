using LikeKero.Domain.Feature;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces.Feature
{
    public interface IRoleFeatureMasterRepository : IRepository<RoleFeatureMaster>
    {
        Task<int> SaveRoleFeature(RoleFeatureMaster obj);
    }
}
