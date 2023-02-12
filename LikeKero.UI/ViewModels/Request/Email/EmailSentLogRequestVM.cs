using System;

namespace LikeKero.UI.ViewModels
{
    public class EmailSentLogRequestVM : BaseRequestVM
    {
        public string Subject { get; set; }
        public DateTime? ActualSentDate { get; set; }
        public string EmailTo { get; set; }
        public string RequesterUserId { get; set; }
    }
}
