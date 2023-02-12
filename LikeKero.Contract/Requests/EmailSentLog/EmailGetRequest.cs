using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests
{
    public class EmailGetRequest: BaseRequest
    {
        public string EmailLogId { get; set; }
    }
}
