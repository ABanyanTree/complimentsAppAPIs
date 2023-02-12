using System;
using System.Collections.Generic;
using System.Text;
using LikeKero.Contract.Responses;

namespace LikeKero.UI.ViewModels
{
    public class GetLOBResponseVM : BaseResponseVM
    {
        public string LOBID { get; set; }
        public string LOBName { get; set; }
        public string LOBDescription { get; set; }
        public bool IsActive { get; set; }

        public string CountryList { get; set; }
        public string LOBApproverList { get; set; }

    }
}
