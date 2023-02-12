using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
 { 
  public interface IEmailSentLogRepository : IRepository<EmailSentLog> 
  {
        Task<int> AddEditAsync(EmailSentLog obj);
        Task<int> UpdateEmailSent(EmailSentLog obj);
        Task<IEnumerable<EmailSentLog>> GetPendingEmails();
        Task<IEnumerable<EmailSentLog>> GetAllAsync(EmailSentLog obj);
        Task<EmailSentLog> GetAsync(EmailSentLog obj);
  }
} 
