using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class SearchRegionRequestVM : BaseRequestVM
    {
        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
    }
}
