using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeKero.Domain;

namespace LikeKero.Services.Interfaces
{
    public interface IADUserService : IServiceBase<ADUser>
    {
        Task<IEnumerable<ADUser>> GetAllLOBApproverDetails(ADUser lOBApproverObj);
        Task<IEnumerable<ADUser>> GetAllADUser();
    }
}
