using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses.User
{
    public class UserCourseEnrollmentResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
        public string Group4Name { get; set; }
        public string Group5Name { get; set; }

        public bool IsEnrolled { get; set; }
        public string GroupName { get; set; }
    }
}
