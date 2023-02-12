using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses.User
{
    public class WelcomeEmailLogResponse : BaseResponse
    {
        public int TotalUsers { get; set; } = 0;
        public int TotalSuccessfull { get; set; } = 0;
        public int TotalWithoutEmails { get; set; } = 0;
        public DateTime? LogDate { get; set; }
    }
}
