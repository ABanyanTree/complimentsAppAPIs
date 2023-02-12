using LikeKero.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Services.Interfaces
{
    public interface ILookupMaster : IServiceBase<LookupMaster>
    {
        Task<IEnumerable<LookupMaster>> GetByType(LookupMaster obj);
    }
}
