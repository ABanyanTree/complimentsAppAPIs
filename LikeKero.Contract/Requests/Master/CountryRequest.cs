using System;

namespace LikeKero.Contract.Requests
{
    public class CountryRequest
    {
        public string CountryID { get; set; }
        public string CountryName { get; set; }
        public string RegionID { get; set; }
        public string CountryDescription { get; set; }
        public bool IsActive { get; set; }
        public string RequesterUserId { get; set; }
    }
}
