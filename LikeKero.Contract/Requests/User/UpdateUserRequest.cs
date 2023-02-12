using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests.User
{
    public class GetUserRequest
    {
        public string UserId { get; set; }
        public string RequesterUserId { get; set; }
    }
}
