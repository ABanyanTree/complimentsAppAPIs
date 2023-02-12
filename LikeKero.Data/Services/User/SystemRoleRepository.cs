using LikeKero.Data.Constants;
using LikeKero.Data.Constants.User;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces.User;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Data.Services.User
{
    public class SystemRoleRepository : Repository<UserRole>, ISystemRoleRepository
    {
        public SystemRoleRepository(IOptions<ReadConfig> connStr, IDapperResolver<UserRole> resolver) : base(connStr, resolver)
        {
        }

        public async Task<IEnumerable<UserRole>> GetAllSystemRoles(UserRole obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID };
            return await GetAllAsync(obj, addParams, SystemRoleInfra.SPROC_SYSTEMROLES_GETALLROLES);
        }

        public async Task<IEnumerable<UserRole>> RoleAssignmentSearch(UserRole obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP, BaseInfra.REQUESTERUSERID, SystemRoleInfra.SEARCHPHRASE, SystemRoleInfra.SEARCHONGROUPID };
            return await GetAllAsync(obj, addParams, SystemRoleInfra.SPROC_GROUPNAMES_SEARCH_FOR_ROLEASSIGNMENT_NEW);
        }

        public async Task<IEnumerable<UserRole>> GroupAdminUsers(UserRole obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID, SystemRoleInfra.GROUPID, SystemRoleInfra.SEARCHONGROUPID };
            return await GetAllAsync(obj, addParams, SystemRoleInfra.SPROC_GETADMINUSERS_FOR_GROUPID);
        }

        public async Task<int> ActiveInActiveUserRole(UserRole obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID, SystemRoleInfra.USERROLEID, SystemRoleInfra.STATUS };
            return await ExecuteNonQueryAsync(obj, addParams, SystemRoleInfra.SPROC_SYSTEMROLES_ACTIVE_INACTIVE);
        }

        public async Task<int> AssignSystemRole(UserRole obj)
        {
            string[] addParams = new string[] { UserMasterInfra.USERID, SystemRoleInfra.ROLEID, SystemRoleInfra.GROUPID, SystemRoleInfra.USERROLEID, BaseInfra.REQUESTERUSERID };
            return await ExecuteNonQueryAsync(obj, addParams, SystemRoleInfra.SPROC_ASSIGN_SYSTEMROLE);
        }

        public async Task<int> DeleteSystemRole(UserRole obj)
        {
            string[] addParams = new string[] { SystemRoleInfra.USERROLEID, BaseInfra.REQUESTERUSERID };
            return await ExecuteNonQueryAsync(obj, addParams, SystemRoleInfra.SPROC_SYSTEMROLES_DELETE);
        }
    }
}
