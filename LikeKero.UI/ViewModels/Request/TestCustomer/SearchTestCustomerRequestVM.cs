using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class SearchTestCustomerRequestVM : BaseRequestVM
    {
        public string CustID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobNo { get; set; }
    }
}
