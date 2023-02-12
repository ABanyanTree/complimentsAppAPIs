using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using LikeKero.Data.Constants;

namespace LikeKero.Data.Services
{
    public class ADUserRepository : Repository<ADUser>, IADUserRepository
    {
        public ADUserRepository(IOptions<ReadConfig> connStr, IDapperResolver<ADUser> resolver) : base(connStr, resolver)
        {

        }

        public async Task<IEnumerable<ADUser>> GetAllADUser()
        {
            string[] addParams = new string[] { };
            return await GetAllAsync(new ADUser() , addParams, ADUserInfra.SPROC_ADUSER_GETALL);
        }

        public async Task<IEnumerable<ADUser>> GetAllAsync(ADUser obj)
        {
            string[] addParams = new string[] { ADUserInfra.USERNAME };
            return await GetAllAsync(obj, addParams, ADUserInfra.SPROC_ADUSER_LSTALL);
        }

        public async Task<IEnumerable<ADUser>> GetAllLOBApproverDetails(ADUser obj)
        {
            string[] addParams = new string[] { ADUserInfra.LOBID };
            return await GetAllAsync(obj, addParams, ADUserInfra.SPROC_GETALLLOBAPPROVERDETAILS_LSTALL);

        }
    }
}
