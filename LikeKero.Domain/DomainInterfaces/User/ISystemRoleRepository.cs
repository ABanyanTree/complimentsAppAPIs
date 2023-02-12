using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces.User
{
    public interface ISystemRoleRepository : IRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetAllSystemRoles(UserRole obj);
        Task<IEnumerable<UserRole>> RoleAssignmentSearch(UserRole obj);
        Task<IEnumerable<UserRole>> GroupAdminUsers(UserRole obj);
        Task<int> ActiveInActiveUserRole(UserRole obj);
        Task<int> AssignSystemRole(UserRole obj);
        Task<int> DeleteSystemRole(UserRole obj);
    }
}
