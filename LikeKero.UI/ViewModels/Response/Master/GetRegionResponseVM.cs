using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class GetRegionResponseVM : BaseResponseVM
    {
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
        public bool IsActive { get; set; }


    }
}
