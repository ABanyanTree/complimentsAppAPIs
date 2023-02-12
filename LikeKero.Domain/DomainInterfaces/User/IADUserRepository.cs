using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface IADUserRepository : IRepository<ADUser>
    {
        Task<IEnumerable<ADUser>> GetAllAsync(ADUser obj);
        Task<IEnumerable<ADUser>> GetAllLOBApproverDetails(ADUser obj);
        Task<IEnumerable<ADUser>> GetAllADUser();
    }
}
