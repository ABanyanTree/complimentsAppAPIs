using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests.User
{
    public class UserTransferLogRequest : BaseRequest
    {
        public string TransferredByName { get; set; }
        public DateTime? TransferredOn { get; set; }
        public string TransferredToId { get; set; }
        public string MasterLogId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
