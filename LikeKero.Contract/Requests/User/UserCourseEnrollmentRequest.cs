using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests.User
{
    public class UserCourseEnrollmentRequest : BaseRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SearchOnGroupId { get; set; }
        public string GroupId { get; set; }
        public string JobCodeId { get; set; }
        public string CourseId { get; set; }
    }
}
