using System;

namespace LikeKero.UI.ViewModels
{
    public class EmailGetResponseVM : BaseResponseVM
    {
        public string EmailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? ActualSentDate { get; set; }
    }
}
