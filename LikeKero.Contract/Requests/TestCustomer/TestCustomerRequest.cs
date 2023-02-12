using System;

namespace LikeKero.Contract.Requests
{
    public class TestCustomerRequest : BaseRequest
    {
        public string CustID { get; set; }
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        public string MobNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
