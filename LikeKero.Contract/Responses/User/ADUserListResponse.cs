using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Contract.Responses
{
    public class ADUserListResponse
    {
        public string ADUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfileImagePath { get; set; }
        public string IsActive { get; set; }
        public string CreatedDate { get; set; }
    }
}
