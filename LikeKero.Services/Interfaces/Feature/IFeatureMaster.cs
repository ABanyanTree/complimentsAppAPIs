using LikeKero.Domain;
using LikeKero.Domain.Feature;
using System.Threading.Tasks;

namespace LikeKero.Services.Interfaces
{
    public interface IFeatureMaster : IServiceBase<FeatureMaster>
    {
        FeatureMaster GetAllFeatures(FeatureMaster obj);
    }
}
