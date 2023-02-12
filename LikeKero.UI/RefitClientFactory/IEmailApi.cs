using LikeKero.UI.ViewModels;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]
    public interface IEmailApi
    {
        [Post(path: "/api/emaillog/Emailloglist")]
        Task<ApiResponse<List<EmailSentLogResponseVM>>> EmailLogList(EmailSentLogRequestVM model);

        [Post(path: "/api/emaillog/getemaillog")]
        Task<ApiResponse<EmailGetResponseVM>> GetEmailLog(EmailGetRequestVM model);
    }
}
