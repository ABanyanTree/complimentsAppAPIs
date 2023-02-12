using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Data.Constants;
using LikeKero.Data.Constants.User;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;

namespace LikeKero.Data.Services
{
    public class RefreshTokenRepository : Repository<RefreshToken> , IRefreshTokenRepository
    { 

        public RefreshTokenRepository(IOptions<ReadConfig> connStr, IDapperResolver<RefreshToken> resolver) : base(connStr, resolver)
        {
        }
        public  async Task<int> SaveRefreshToken(RefreshToken RefreshToken)
        {
            string[] Params = new string[] { RefreshTokenInfra.Token, RefreshTokenInfra.JwtId,
                 RefreshTokenInfra.Creationdate,
                 RefreshTokenInfra.ExpiryDate,
                 RefreshTokenInfra.Used,
                 RefreshTokenInfra.InValidated,
                 RefreshTokenInfra.UserId,
                 RefreshTokenInfra.EmailAddress
            }; 
            var userResponse = await ExecuteNonQueryAsync(RefreshToken, Params, RefreshTokenInfra.sproc_RefreshToken_Ups);
            return userResponse;
        } 
        public async Task<int> UpdateRefreshToken(RefreshToken RefreshToken)
        {
            string[] Params = new string[] { RefreshTokenInfra.Token 
            };
            var userResponse = await ExecuteNonQueryAsync(RefreshToken, Params, RefreshTokenInfra.sproc_RefreshToken_Ups_Used);
            return userResponse;
        }

        public async  Task<RefreshToken> GetStoredRefreshToken(RefreshToken RefreshToken)
        {
            string[] Params = new string[] { RefreshTokenInfra.Token
            };
            var userResponse = await GetAsync(RefreshToken, Params, RefreshTokenInfra.sproc_RefreshToken_Get);
            return userResponse;
        }

        public async Task<int> SaveSaltTime(RefreshToken obj)
        {
            string[] addparams = new string[] { "StartTime", "EndTime", "APIName" };
            return await ExecuteNonQueryAsync(obj, addparams, "sproc_SaltTime");
        }
    }
}
