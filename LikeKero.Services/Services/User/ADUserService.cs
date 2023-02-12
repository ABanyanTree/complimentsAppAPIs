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
    public class ADUserService : IADUserService
    {
        private IADUserRepository _ADUserRepository;

        public ADUserService(IADUserRepository ADUserRepository)
         : base()
        {
            _ADUserRepository = ADUserRepository;

        }
        public Task<int> AddEditAsync(ADUser obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(ADUser obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ADUser>> GetAllADUser()
        {
            return await _ADUserRepository.GetAllADUser();
        }

        public async Task<IEnumerable<ADUser>> GetAllAsync(ADUser obj)
        {
            return await _ADUserRepository.GetAllAsync(obj);
        }

        /// <summary>
        /// get All AD names from comma sepearted LOBids
        /// </summary>
        /// <param name="lOBApproverObj"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ADUser>> GetAllLOBApproverDetails(ADUser obj)
        {
            return await _ADUserRepository.GetAllLOBApproverDetails(obj);
        }

        public Task<ADUser> GetAsync(ADUser obj)
        {
            throw new NotImplementedException();
        }
    }
}
