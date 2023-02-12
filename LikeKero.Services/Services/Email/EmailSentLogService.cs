using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Infra.EmailSender;
using LikeKero.Services.Interfaces;
namespace LikeKero.Services.Services
{
    public class EmailSentLogService : IEmailSentLog
    {
        private IEmailSentLogRepository iEmailSentLogRepository;
        private IEmailSender _emailSender;
        public EmailSentLogService(IEmailSentLogRepository IEmailSentLogRepository,
            IEmailSender emailSender
            ) : base()
        {
            iEmailSentLogRepository = IEmailSentLogRepository;
            _emailSender = emailSender;
        }
        public async Task<int> AddEditAsync(EmailSentLog obj)
        {
            return await iEmailSentLogRepository.AddEditAsync(obj);
        }
        public Task<int> DeleteAsync(EmailSentLog obj)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<EmailSentLog>> GetAllAsync(EmailSentLog obj)
        {
            return await iEmailSentLogRepository.GetAllAsync(obj);
        }
        public async Task<EmailSentLog> GetAsync(EmailSentLog obj)
        {
            return await iEmailSentLogRepository.GetAsync(obj);
        }

        public async Task<int> UpdateEmailSent(EmailSentLog obj)
        {
            return await iEmailSentLogRepository.UpdateEmailSent(obj);
        }

        public async Task<IEnumerable<EmailSentLog>> GetPendingEmails()
        {
            return await iEmailSentLogRepository.GetPendingEmails();
        }

        public async Task AddEmailEntry(EmailSenderEntity emailSenderEntity, object Entity)
        {
            EmailSentLog emailSentLog = _emailSender.GetBodyAndSubject(emailSenderEntity, Entity);

            await iEmailSentLogRepository.AddEditAsync(emailSentLog);
        }

        public async Task SendPendingEmails()
        {
            var lstpendingEmails = await iEmailSentLogRepository.GetPendingEmails();
            if (lstpendingEmails != null)
            {
                foreach (var PendingEmail in lstpendingEmails)
                {
                    await _emailSender.SendPendingEmail(PendingEmail);
                    await UpdateEmailSent(PendingEmail);
                }
            }
        }
    }
}
