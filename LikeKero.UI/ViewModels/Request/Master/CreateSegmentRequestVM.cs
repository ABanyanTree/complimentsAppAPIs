using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LikeKero.Contract.Responses;

namespace LikeKero.UI.ViewModels
{
    public class CreateSegmentRequestVM : BaseRequestVM
    {
        public string SegmentID { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid Segment Name.")]        
        [Required(ErrorMessage = "Please enter Segment Name")]
        [Remote("IsSegmentNameInUse", "Segment", AdditionalFields = "SegmentID", HttpMethod = "GET")]     
        public string SegmentName { get; set; }
        public string SegmentDescription { get; set; }
        public bool IsActive { get; set; }

        //Segment Country
        public string CountryList { get; set; }        
        //public List<GetSegmentCountryResponse> SegmentCountryList { get; set; } = new List<GetSegmentCountryResponse>();
    }
}
