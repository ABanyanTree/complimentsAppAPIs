using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces.User;
using LikeKero.Infra;
using LikeKero.Services.Interfaces.User;

namespace LikeKero.Services.Services.User
{
    public class SystemRoleService : ISystemRole
    {
        private ISystemRoleRepository iSystemRoleRepository;

        public SystemRoleService(ISystemRoleRepository ISystemRoleRepository) : base()
        {
            iSystemRoleRepository = ISystemRoleRepository;
        }

        public Task<int> AddEditAsync(UserRole obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteAsync(UserRole obj)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync(UserRole obj)
        {
            return await iSystemRoleRepository.GetAllSystemRoles(obj);
        }

        public Task<UserRole> GetAsync(UserRole obj)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<UserRole>> AssignmentSearch(UserRole obj)
        {
            return await iSystemRoleRepository.RoleAssignmentSearch(obj);
        }

        public async Task<IEnumerable<UserRole>> AdminUsersForGroup(UserRole obj)
        {
            return await iSystemRoleRepository.GroupAdminUsers(obj);
        }

        public async Task<int> ActiveInActiveRole(UserRole obj)
        {
            return await iSystemRoleRepository.ActiveInActiveUserRole(obj);
        }

        public async Task<string> AssignSystemRole(UserRole obj)
        {
            if (string.IsNullOrEmpty(obj.UserRoleId))
            {
                obj.UserRoleId = Utility.GetUniqueKeyWithPrefix("USRROLE_", 8);

            }
            string strUserRoleId = obj.UserRoleId;

            await iSystemRoleRepository.AssignSystemRole(obj);

            return strUserRoleId;
        }

        public async Task<int> DeleteSystemRole(UserRole obj)
        {
            return await iSystemRoleRepository.DeleteSystemRole(obj);
        }
    }
}
