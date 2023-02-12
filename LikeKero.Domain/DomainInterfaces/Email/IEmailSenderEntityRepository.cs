using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface IEmailSenderEntityRepository : IRepository<EmailSenderEntity>
    {
        Task<EmailSenderEntity> GetEmailByType(EmailSenderEntity obj);
    }
}
