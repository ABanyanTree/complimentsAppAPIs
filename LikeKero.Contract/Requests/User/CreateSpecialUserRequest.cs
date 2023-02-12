using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Contract.Requests
{
    public class CreateSpecialUserRequest
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        //public string UserType { get; set; }
        //public string PreviousUserType { get; set; }
        public string RoleID { get; set; }
        public string PreviousRoleID { get; set; }
        public string RegionList { get; set; }
        public string CountryList { get; set; }
        public bool IsAccessBreachLog { get; set; }
        public string RequesterUserId { get; set; }
    }
}
