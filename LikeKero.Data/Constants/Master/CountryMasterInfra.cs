using System;
namespace LikeKero.Data.Constants
{
    public class CountryMasterInfra : BaseInfra
    {
        public const string COUNTRYID = "CountryID";
        public const string REGIONID = "RegionID";
        public const string COUNTRYNAME = "CountryName";
        public const string COUNTRYDESCRIPTION = "CountryDescription";
        public const string ISACTIVE = "IsActive";
        public const string LOBID = "LOBID";

        public const string SPROC_COUNTRYMASTER_DEL = "sproc_CountryMaster_del";
        public const string SPROC_COUNTRYMASTER_LSTALL = "sproc_CountryMaster_lstAll";
        public const string SPROC_COUNTRYMASTER_SEL = "sproc_CountryMaster_sel";
        public const string SPROC_COUNTRYMASTER_UPS = "sproc_CountryMaster_ups";
        public const string SPROC_COUNTRYMASTER_ISCOUNTRYNAMEINUSE = "sproc_CountryMaster_IsCountryNameInUse";
        public const string SPROC_COUNTRYMASTER_GETALL = "sproc_CountryMaster_GetAll";
        public const string SPROC_COUNTRYMASTER_ISINUSECOUNT = "sproc_CountryMaster_IsInUseCount";

        public const string SPROC_GETALLLOBCOUNTRYDETAILS_LSTALL = "sproc_GetAllLOBCountryDetails_lstAll";

        public const string SPROC_COUNTRYMASTER_GETALLBYIDS = "sproc_CountryMaster_GetAllByIds";
        



    }
}
