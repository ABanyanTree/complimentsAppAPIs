using LikeKero.Data.Constants;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LikeKero.Data.Services
{
    public class EmailSenderEntityRepository : Repository<EmailSenderEntity>, IEmailSenderEntityRepository
    {
        public EmailSenderEntityRepository(IOptions<ReadConfig> connStr, IDapperResolver<EmailSenderEntity> resolver) : base(connStr, resolver)
        {
        }

        public async Task<EmailSenderEntity> GetEmailByType(EmailSenderEntity obj)
        {
            string[] addParams = new string[] { EmailSenderEntityInfra.EMAILTYPE, EmailSenderEntityInfra.GROUPID };
            return await GetAsync(obj, addParams, EmailSenderEntityInfra.SPROC_EMAILSENDENTITY_SEL);
        }
    }
}
