using System;

namespace LikeKero.Contract.Requests
{
    public class EmailSentLogRequest : BaseRequest
    {
        public string Subject { get; set; }
        public DateTime? ActualSentDate { get; set; }
        public string EmailTo { get; set; }
    }
}
