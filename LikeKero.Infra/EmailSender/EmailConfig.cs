using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Infra.EmailSender
{
    public class EmailConfig
    {
        public string From { get; set; }
        public bool SendEmail { get; set; }
        public string SMTP_HOST { get; set; }
        public string SENDMAIL_SMTPUSERNAME { get; set; }
        public string SENDMAIL_SMTPUSERPASSWORD { get; set; }
        public string SENDMAIL_PORT { get; set; }
        public string SENDMAIL_DEFAULTCREDENTIALS { get; set; }
        public string EmailTemplateFolderPath { get; set; }
        public bool IsEmailToFolder { get; set; }
        public string SystemEmailDeliveryPath { get; set; }
        public string LoginURL { get; set; }


    }
}
