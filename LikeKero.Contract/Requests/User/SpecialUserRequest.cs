using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Contract.Requests
{
    public class SpecialUserRequest
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string RegionId { get; set; }
        public string CountryId { get; set; }
        public string UserType { get; set; }
        public string RequesterUserId { get; set; }

    }
}
