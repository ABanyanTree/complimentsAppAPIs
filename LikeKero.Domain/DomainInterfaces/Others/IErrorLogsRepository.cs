using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface IErrorLogsRepository : IRepository<ErrorLogs>
    {
        Task<int> AddEditAsync(ErrorLogs obj);
        Task<IEnumerable<ErrorLogs>> GetAllAsync(ErrorLogs obj);
        Task<ErrorLogs> GetAsync(ErrorLogs obj);
    }
}
