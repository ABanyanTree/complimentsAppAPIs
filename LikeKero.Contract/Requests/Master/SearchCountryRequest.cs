using System;

namespace LikeKero.Contract.Requests
{
    public class SearchCountryRequest : BaseRequest
    {     
        public string CountryName { get; set; }
        public string RegionID { get; set; }        
    }
}
