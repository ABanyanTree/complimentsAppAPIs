using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Domain
{
   public class ErrorLogs:BaseEntity
    {
        public string ErrorLogID { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime? ErrorDateTime { get; set; }
        public DateTime? ErrorFromDate { get; set; }
        public DateTime? ErrorToDate { get; set; }
        public string ErrorDateDisplay { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserDataXML { get; set; }

    }
}
