using System;

namespace LikeKero.Contract.Responses
{
    public class GetCountryResponse
    {
        public string CountryID { get; set; }
        public string CountryName { get; set; }
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string CountryDescription { get; set; }
        public bool IsActive { get; set; }
        public string LOBID { get; set; }
        public int TotalCount { get; set; }
    }
}
