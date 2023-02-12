using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class GetCountryResponseVM : BaseResponseVM
    {
        public string CountryID { get; set; }
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string CountryDescription { get; set; }
        public bool IsActive { get; set; }


    }
}
