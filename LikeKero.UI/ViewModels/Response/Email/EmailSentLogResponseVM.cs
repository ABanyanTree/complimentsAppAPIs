using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class EmailSentLogResponseVM: BaseResponseVM
    {
        public string EmailLogId { get; set; }
        public string EmailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? ActualSentDate { get; set; }
        public string ActualSentDateDisplay { get; set; }
    }
}
