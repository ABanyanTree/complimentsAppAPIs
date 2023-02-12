using LikeKero.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Services.Interfaces.User
{
    public interface ISystemRole : IServiceBase<UserRole>
    {
        Task<IEnumerable<UserRole>> AssignmentSearch(UserRole obj);
        Task<IEnumerable<UserRole>> AdminUsersForGroup(UserRole obj);
        Task<int> ActiveInActiveRole(UserRole obj);
        Task<string> AssignSystemRole(UserRole obj);
        Task<int> DeleteSystemRole(UserRole obj);
    }
}
