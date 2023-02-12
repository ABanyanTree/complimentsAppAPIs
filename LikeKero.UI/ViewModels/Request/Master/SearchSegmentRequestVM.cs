using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class SearchSegmentRequestVM : BaseRequestVM
    {
        public string SegmentName { get; set; }
        public string SegmentDescription { get; set; }
    }
}
