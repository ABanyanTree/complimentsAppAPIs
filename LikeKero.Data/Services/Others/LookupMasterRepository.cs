using LikeKero.Data.Constants;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.Data.Services
{
    public class LookupMasterRepository : Repository<LookupMaster>, ILookupMasterRepository
    {
        public LookupMasterRepository(IOptions<ReadConfig> connStr, IDapperResolver<LookupMaster> resolver) : base(connStr, resolver)
        {
        }

        public async Task<IEnumerable<LookupMaster>> GetAllAsync(LookupMaster obj)
        {
            string[] addParams = new string[] { LookupMasterInfra.LOOKUPTYPE };
            return await GetAllAsync(obj, addParams, LookupMasterInfra.SPROC_LOOKUPMASTER_LSTALL);
        }

        public async Task<IEnumerable<LookupMaster>> GetByTypeAsync(LookupMaster obj)
        {
            string[] addParams = new string[] { LookupMasterInfra.LOOKUPTYPE };
            return await GetAllAsync(obj, addParams, LookupMasterInfra.SPROC_LOOKUPBYTYPE);
        }
    }
}
