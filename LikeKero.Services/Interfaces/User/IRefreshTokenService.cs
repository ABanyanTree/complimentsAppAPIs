using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;

namespace LikeKero.Services.Interfaces
{
    public  interface IRefreshTokenService
    {
        Task<int> SaveRefreshToken(RefreshToken RefreshToken);
        Task<int> UpdateRefreshToken(RefreshToken RefreshToken);
        Task<RefreshToken> GetRefreshToken(RefreshToken RefreshToken);

        Task<int> SaveSaltTime(DateTime start, DateTime end, string APIName);
    }
}
