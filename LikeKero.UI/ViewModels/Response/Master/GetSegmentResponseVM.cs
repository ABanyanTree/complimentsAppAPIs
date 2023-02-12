using System;
using System.Collections.Generic;
using System.Text;
using LikeKero.Contract.Responses;

namespace LikeKero.UI.ViewModels
{
    public class GetSegmentResponseVM : BaseResponseVM
    {
        public string SegmentID { get; set; }
        public string SegmentName { get; set; }
        public string SegmentDescription { get; set; }
        public bool IsActive { get; set; }

        public string CountryList { get; set; }
        //public List<GetSegmentCountryResponse> SegmentCountryList { get; set; } = new List<GetSegmentCountryResponse>();
    }
}
