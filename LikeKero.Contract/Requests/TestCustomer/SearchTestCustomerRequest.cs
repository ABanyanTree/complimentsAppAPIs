using System;

namespace LikeKero.Contract.Requests
{
    public class SearchTestCustomerRequest : BaseRequest
    {
        public string CustID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobNo { get; set; }      
    }
}
