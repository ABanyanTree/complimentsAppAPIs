using LikeKero.Data.Constants;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Data.Services
{
    public class EmailSentLogRepository : Repository<EmailSentLog>, IEmailSentLogRepository
    {
        public EmailSentLogRepository(IOptions<ReadConfig> connStr, IDapperResolver<EmailSentLog> resolver) : base(connStr, resolver)
        {
        }

        public async Task<int> AddEditAsync(EmailSentLog obj)
        {
            string[] addParams = new string[] { EmailSentLogInfra.EMAILTO, EmailSentLogInfra.CC, EmailSentLogInfra.BCC, EmailSentLogInfra.SUBJECT, EmailSentLogInfra.BODY, EmailSentLogInfra.ISEMAILSENT, EmailSentLogInfra.USERID };
            return await ExecuteNonQueryAsync(obj, addParams, EmailSentLogInfra.SPROC_EMAILSENTLOG_UPS);
        }

        public async Task<int> UpdateEmailSent(EmailSentLog obj)
        {
            string[] addParams = new string[] { EmailSentLogInfra.EMAILLOGID };
            return await ExecuteNonQueryAsync(obj, addParams, EmailSentLogInfra.SPROC_EMAILSENTLOG_UPDATE_EMAILSENT);
        }

        public async Task<IEnumerable<EmailSentLog>> GetPendingEmails()
        {
            string[] addParams = new string[] { };
            return await GetAllAsync(new EmailSentLog(), addParams, EmailSentLogInfra.SPROC_GETPENDINGEMAILS);
        }

        public async Task<IEnumerable<EmailSentLog>> GetAllAsync(EmailSentLog obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP, EmailSentLogInfra.BODY, EmailSentLogInfra.SUBJECT, EmailSentLogInfra.ACTUALSENTDATE, EmailSentLogInfra.EMAILTO, BaseInfra.REQUESTERUSERID };
            return await GetAllAsync(obj, addParams, EmailSentLogInfra.SPROC_EMAILSENTLOG_LSTALL);
        }

        public async Task<EmailSentLog> GetAsync(EmailSentLog obj)
        {
            string[] addParams = new string[] { EmailSentLogInfra.EMAILLOGID };
            return await GetAsync(obj, addParams, EmailSentLogInfra.SPROC_EMAILSENTLOG_SEL);
        }
    }
}
