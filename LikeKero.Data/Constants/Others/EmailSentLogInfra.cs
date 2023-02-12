namespace LikeKero.Data.Constants
{
    public class EmailSentLogInfra : BaseInfra
    {
        public const string EMAILLOGID = "EmailLogId";
        public const string EMAILTO = "EmailTo";
        public const string CC = "CC";
        public const string BCC = "BCC";
        public const string SUBJECT = "Subject";
        public const string BODY = "Body";
        public const string ISEMAILSENT = "IsEmailSent";
        public const string CREATEDDATE = "CreatedDate";
        public const string ACTUALSENTDATE = "ActualSentDate";
        public const string USERID = "UserId";

        public const string SPROC_EMAILSENTLOG_UPS = "sproc_EmailSentLog_ups";
        public const string SPROC_EMAILSENTLOG_UPDATE_EMAILSENT = "sproc_EmailSentLog_Update_EmailSent";
        public const string SPROC_GETPENDINGEMAILS = "sproc_GetPendingEmails";
        public const string SPROC_EMAILSENTLOG_LSTALL = "sproc_EmailSentLog_lstAll";
        public const string SPROC_EMAILSENTLOG_SEL = "sproc_EmailSentLog_sel";

    }
}
