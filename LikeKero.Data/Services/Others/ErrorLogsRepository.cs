using System.Collections.Generic;
using System.Threading.Tasks;
using LikeKero.Data.Constants;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using Microsoft.Extensions.Options;

namespace LikeKero.Data.Services
{
    public class ErrorLogsRepository : Repository<ErrorLogs>, IErrorLogsRepository
    {
        public ErrorLogsRepository(IOptions<ReadConfig> connStr, IDapperResolver<ErrorLogs> resolver) : base(connStr, resolver)
        {

        }

        public async Task<int> AddEditAsync(ErrorLogs obj)
        {
            string[] addParams = new string[] { ErrorLogsInfra.CONTROLLERNAME, ErrorLogsInfra.ACTIONNAME,
                ErrorLogsInfra.ERRORMESSAGE, ErrorLogsInfra.INNEREXCEPTION,
            ErrorLogsInfra.STACKTRACE, BaseInfra.REQUESTERUSERID, ErrorLogsInfra.USERDATAXML};
            return await ExecuteNonQueryAsync(obj, addParams, ErrorLogsInfra.SPROC_APIERRORLOG_ADD);
        }

        public async Task<IEnumerable<ErrorLogs>> GetAllAsync(ErrorLogs obj)
        {
            string[] addParams = new string[] { BaseInfra.PAGEINDEX, BaseInfra.PAGESIZE, BaseInfra.SORTEXP, ErrorLogsInfra.ERRORFROMDATE, ErrorLogsInfra.ERRORTODATE, ErrorLogsInfra.ERRORMESSAGE, UserMasterInfra.FIRSTNAME, UserMasterInfra.LASTNAME };
            return await GetAllAsync(obj, addParams, ErrorLogsInfra.SPROC_APIERRORLOGS_LSTALL);
        }

        public async Task<ErrorLogs> GetAsync(ErrorLogs obj)
        {
            string[] addParams = new string[] { ErrorLogsInfra.ERRORLOGID };
            return await GetAsync(obj, addParams, ErrorLogsInfra.SPROC_APIERRORLOGS_SEL);
        }
    }
}
