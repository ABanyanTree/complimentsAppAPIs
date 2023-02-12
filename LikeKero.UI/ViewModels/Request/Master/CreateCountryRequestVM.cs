using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class CreateCountryRequestVM : BaseRequestVM
    {
        public string CountryID { get; set; }

        [Required(ErrorMessage = "Please select Region")]
        public string RegionID { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid Country Name.")]        
        [Required(ErrorMessage = "Please enter Country Name")]
        [Remote("IsCountryNameInUse", "Country", AdditionalFields = "CountryID", HttpMethod = "GET")]
        public string CountryName { get; set; }
        public string CountryDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
