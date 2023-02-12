using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests
{
    public class UserLoginRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

    }
}
