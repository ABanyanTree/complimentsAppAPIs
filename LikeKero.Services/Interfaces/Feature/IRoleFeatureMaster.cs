using LikeKero.Domain;
using LikeKero.Domain.Feature;
using System.Threading.Tasks;

namespace LikeKero.Services.Interfaces
{
    public interface IRoleFeatureMaster : IServiceBase<RoleFeatureMaster>
    {
        Task<int> SaveFeature(RoleFeatureMaster obj);
    }
}
