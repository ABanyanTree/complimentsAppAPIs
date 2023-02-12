using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class EmailEntityResponse
    {
        public string EmailType { get; set; }
        public string EmailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string HtmlFileName { get; set; }
    }
}
