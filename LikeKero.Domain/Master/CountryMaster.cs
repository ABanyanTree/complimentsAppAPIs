using System;
namespace LikeKero.Domain
{
    public class CountryMaster : BaseEntity
    {
        public string CountryID { get; set; }
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string CountryDescription { get; set; }
        public bool IsActive { get; set; }
        public string LOBID { get; set; }

    }
}
