using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LikeKero.Domain;

namespace LikeKero.Contract.Responses
{
    public class SpecialUserResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string RoleId { get; set; }
        public string UserType { get; set; }       
        public string RegionList { get; set; }      
        public string CountryList { get; set; }
        public bool IsAccessBreachLog { get; set; }
        public string ProfileImagePath { get; set; }
        public int TotalCount { get; set; }

       // public List<GetRegionResponse> RegionDataList { get; set; }
        public List<GetCountryResponse> CountryDataList { get; set; }


    }
}
