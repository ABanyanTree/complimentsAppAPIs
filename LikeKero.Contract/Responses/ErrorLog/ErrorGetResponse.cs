using System;

namespace LikeKero.Contract.Responses
{
    public class ErrorGetResponse : BaseResponse
    {
        public string ErrorMessage { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime? ErrorDateTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
