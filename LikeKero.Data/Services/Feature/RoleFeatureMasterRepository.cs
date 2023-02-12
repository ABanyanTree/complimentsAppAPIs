using LikeKero.Data.Constants;
using LikeKero.Data.Constants.User;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces.Feature;
using LikeKero.Domain.Feature;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Data.Services.Feature
{
    public class RoleFeatureMasterRepository : Repository<RoleFeatureMaster>, IRoleFeatureMasterRepository
    {
        public RoleFeatureMasterRepository(IOptions<ReadConfig> connStr, IDapperResolver<RoleFeatureMaster> resolver) : base(connStr, resolver)
        {
        }

        public async Task<int> SaveRoleFeature(RoleFeatureMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID, FeatureMasterInfra.ROLEFEATUREID, SystemRoleInfra.ROLEID, FeatureMasterInfra.FEATUREID, FeatureMasterInfra.STATUS };
            return await ExecuteNonQueryAsync(obj, addParams, FeatureMasterInfra.SPROC_FEATURES_ROLES_ADDMAPPING);
        }
    }
}
