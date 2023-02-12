using System;

namespace LikeKero.Contract.Requests
{
    public class ErrorLogRequest : BaseRequest
    {
        public DateTime? ErrorFromDate { get; set; }
        public DateTime? ErrorToDate { get; set; }
        public string ErrorMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
