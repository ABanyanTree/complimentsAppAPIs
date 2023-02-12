using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests
{
    public class ErrorGetRequest: BaseRequest
    {
        public string ErrorLogID { get; set; }
    }
}
