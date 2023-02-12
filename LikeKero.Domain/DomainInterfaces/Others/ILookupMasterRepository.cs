using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface ILookupMasterRepository : IRepository<LookupMaster>
    {
        Task<IEnumerable<LookupMaster>> GetAllAsync(LookupMaster obj);
        Task<IEnumerable<LookupMaster>> GetByTypeAsync(LookupMaster obj);
    }
}
