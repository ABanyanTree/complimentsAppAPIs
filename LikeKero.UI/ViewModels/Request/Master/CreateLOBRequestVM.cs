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
    public class CreateLOBRequestVM : BaseRequestVM
    {
        public string LOBID { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid LOB Name.")]        
        [Required(ErrorMessage = "Please enter LOB Name")]
        [Remote("IsLOBNameInUse", "LOB", AdditionalFields = "LOBID", HttpMethod = "GET")]     
        public string LOBName { get; set; }
        public string LOBDescription { get; set; }
        public bool IsActive { get; set; }

        //LOB Country
        public string CountryList { get; set; }        
        
    }
}
