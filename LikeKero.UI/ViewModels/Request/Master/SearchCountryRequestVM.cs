using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.UI.ViewModels
{
    public class SearchCountryRequestVM : BaseRequestVM
    {
        public string CountryName { get; set; }
        public string CountryDescription { get; set; }
    }
}
