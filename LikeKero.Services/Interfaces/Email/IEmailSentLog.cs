using LikeKero.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Services.Interfaces
{
    public interface IEmailSentLog : IServiceBase<EmailSentLog>
    {

        Task<int> UpdateEmailSent(EmailSentLog obj);
        Task<IEnumerable<EmailSentLog>> GetPendingEmails();

        Task AddEmailEntry(EmailSenderEntity emailSenderEntity,object Entity);
        Task SendPendingEmails();


    }
}
