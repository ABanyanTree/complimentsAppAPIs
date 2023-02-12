using System;
using System.Collections.Generic;
using System.Text;
namespace LikeKero.Domain
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime Creationdate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool InValidated { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string APIName { get; set; }
    }
}
