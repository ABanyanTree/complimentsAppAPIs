using System;
namespace LikeKero.Domain
{
    public class EmailSentLog : BaseEntity
    {
        public string EmailLogId { get; set; }
        public string EmailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsEmailSent { get; set; }

        public DateTime? ActualSentDate { get; set; }
        public string ActualSentDateDisplay { get; set; }

        public string UserId { get; set; }
    }
}
