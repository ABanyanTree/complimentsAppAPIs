using System;
namespace LikeKero.Domain
{
    public class TestCustomer : BaseEntity
    {
        public string CustID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }      
        public string FileName { get; set; }
    }
}
