using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Data.Constants
{
   public class ErrorLogsInfra: BaseInfra
    {
        public static string SPROC_APIERRORLOG_ADD = "sproc_APIErrorLog_Add";
        public const string SPROC_APIERRORLOGS_LSTALL = "sproc_APIErrorLogs_lstAll";
        public const string SPROC_APIERRORLOGS_SEL = "sproc_APIErrorLogs_sel";

        public static string ERRORLOGID = "ErrorLogID";
        public const string CONTROLLERNAME = "ControllerName";
        public const string ACTIONNAME = "ActionName";
        public const string ERRORMESSAGE = "ErrorMessage";
        public const string INNEREXCEPTION = "InnerException";
        public const string STACKTRACE = "StackTrace";
        public const string ERRORDATETIME = "ErrorDateTime";
        public const string ERRORFROMDATE = "ErrorFromDate";
        public const string ERRORTODATE = "ErrorToDate";
        public const string USERDATAXML = "UserDataXML";

    }
}
