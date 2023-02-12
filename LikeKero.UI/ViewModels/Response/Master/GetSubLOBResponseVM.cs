using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class GetSubLOBResponseVM : BaseResponseVM
    {
        public string SubLOBID { get; set; }
        public string LOBID { get; set; }
        public string LOBName { get; set; }
        public string SubLOBName { get; set; }
        public string SubLOBDescription { get; set; }
        public bool IsActive { get; set; }


    }
}
