using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class GetTestCustomerResponseVM : BaseResponseVM
    {
        public string CustID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

     
    }
}
