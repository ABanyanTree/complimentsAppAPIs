using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class CreateRegionRequestVM : BaseRequestVM
    {
        public string RegionID { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid Region Name.")]        
        [Required(ErrorMessage = "Please enter Region Name")]
        [Remote("IsRegionNameInUse", "Region", AdditionalFields = "RegionID", HttpMethod = "GET")]
        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
