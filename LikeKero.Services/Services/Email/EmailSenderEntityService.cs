using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Services.Interfaces;
namespace LikeKero.Services.Services
{
    public class EmailSenderEntityService : IEmailSenderEntity
    {
        private IEmailSenderEntityRepository iEmailSenderEntityRepository;
        public EmailSenderEntityService(IEmailSenderEntityRepository IEmailSenderEntityRepository) : base()
        {
            iEmailSenderEntityRepository = IEmailSenderEntityRepository;
        }
        public Task<int> AddEditAsync(EmailSenderEntity obj)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteAsync(EmailSenderEntity obj)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmailSenderEntity>> GetAllAsync(EmailSenderEntity obj)
        {
            throw new NotImplementedException();
        }
        public Task<EmailSenderEntity> GetAsync(EmailSenderEntity obj)
        {
            return iEmailSenderEntityRepository.GetEmailByType(obj);
        }
    }
}
