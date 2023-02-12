using LikeKero.Domain.Feature;
using System.Threading.Tasks;

namespace LikeKero.Domain.DomainInterfaces
{
    public interface IFeatureMasterRepository : IRepository<FeatureMaster>
    {
        FeatureMaster GetAllAdminFeatures(FeatureMaster obj);
    }
}
