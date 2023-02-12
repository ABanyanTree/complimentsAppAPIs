using System; 
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LikeKero.HostedBackgroundService.RefitClientFactory
{
    
    interface IAPICalls
    {
        [Get(path: "/api/hostedservice/sendpendingemails")]
        Task<HttpResponseMessage> SendPendingEmails([Header("ApiKey")] string authorization);
        [Get(path: "/api/hostedservice/bulkimportpending")]
        Task<HttpResponseMessage> DoPendingBulkImport([Header("ApiKey")] string authorization);
        [Get(path: "/api/hostedservice/runbrrules")]
        Task<HttpResponseMessage> RunBRRUles([Header("ApiKey")] string authorization);

        [Get(path: "/api/hostedservice/bulkimportoigfiles")]
        Task<HttpResponseMessage> BulkImportOIGFiles([Header("ApiKey")] string authorization);
        [Get(path: "/api/hostedservice/bulkimportvendorfiles")]
        Task<HttpResponseMessage> BulkImportVendorFiles([Header("ApiKey")] string authorization);

        [Get(path: "/api/hostedservice/processpendingbusinessrulefiles")]
        Task<HttpResponseMessage> ProcessPendingBusinessRuleFiles([Header("ApiKey")] string authorization);
    }
}
