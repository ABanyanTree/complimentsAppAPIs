using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LikeKero.Contract.Requests.User
{
    public class ChangePasswordRequest
    {
        
        public string Password { get; set; }
        public string CurrentPassword { get; set; }
        public bool IsPasswordChanged { get; set; }
        public string RequesterUserId { get; set; }
        public string UserId { get; set; }
        public bool IsAdminPasswordChangeRequest { get; set; }
    }
}
