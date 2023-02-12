using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses.User
{
    public class UserTransferLogResponse : BaseResponse
    {
        public string MasterLogId { get; set; }
        public int TotalRecords { get; set; }
        public string TransferredById { get; set; }
        public string TransferredByName { get; set; }
        public DateTime TransferredOn { get; set; }
        public string TransferredToId { get; set; }
        public string TransferredToName { get; set; }
        public string Remarks { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string TransferredFromName { get; set; }
    }
}
