using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests.User
{
    public    class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
