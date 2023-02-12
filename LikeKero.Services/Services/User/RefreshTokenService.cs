using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LikeKero.Data.Constants;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Domain.Utility;
using LikeKero.Infra;
using LikeKero.Infra.BaseUri;
using LikeKero.Infra.EmailSender;
using LikeKero.Infra.Encryption;
using LikeKero.Infra.FileSystem;
using LikeKero.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LikeKero.Services.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        IRefreshTokenRepository _IRefreshTokenRepository = null;
        private IOptions<FileSystemPath> _options;
        public RefreshTokenService(IRefreshTokenRepository IRefreshTokenRepository, IOptions<FileSystemPath> options)
        {
            _IRefreshTokenRepository = IRefreshTokenRepository;
            _options = options;
        }

        public async Task<RefreshToken> GetRefreshToken(RefreshToken RefreshToken)
        {
            return await _IRefreshTokenRepository.GetStoredRefreshToken(RefreshToken);
        }

        public async Task<int> SaveRefreshToken(RefreshToken RefreshToken)
        {
            return await _IRefreshTokenRepository.SaveRefreshToken(RefreshToken);
        }

        public async Task<int> SaveSaltTime(DateTime start, DateTime end, string APIName)
        {
            string createlog = _options.Value.CreateAPILog;
            if (createlog == "1")
            {
                RefreshToken obj = new RefreshToken()
                {
                    StartTime = start,
                    EndTime = end,
                    APIName = APIName
                };

                return await _IRefreshTokenRepository.SaveSaltTime(obj);
            }

            return 1;
        }

        public async Task<int> UpdateRefreshToken(RefreshToken RefreshToken)
        {
            return await _IRefreshTokenRepository.UpdateRefreshToken(RefreshToken);
        }

    }
}
