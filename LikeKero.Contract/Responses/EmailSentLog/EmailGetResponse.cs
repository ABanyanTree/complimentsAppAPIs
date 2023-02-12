using System;

namespace LikeKero.Contract.Responses
{
    public class EmailGetResponse : BaseResponse
    {
        public string EmailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? ActualSentDate { get; set; }
    }
}
