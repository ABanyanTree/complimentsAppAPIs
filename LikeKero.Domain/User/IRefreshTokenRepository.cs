using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace LikeKero.Domain.DomainInterfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<int> SaveRefreshToken(RefreshToken RefreshToken);
        Task<int> UpdateRefreshToken(RefreshToken RefreshToken);
        Task<RefreshToken> GetStoredRefreshToken(RefreshToken RefreshToken);
        Task<int> SaveSaltTime(RefreshToken obj);
    }
}
