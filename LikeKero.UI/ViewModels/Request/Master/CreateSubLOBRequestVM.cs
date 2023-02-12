using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class CreateSubLOBRequestVM : BaseRequestVM
    {
        public string SubLOBID { get; set; }

        [Required(ErrorMessage = "Please select LOB")]
        public string LOBID { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid SubLOB Name.")]        
        [Required(ErrorMessage = "Please enter SubLOB Name")]
        [Remote("IsSubLOBNameInUse", "SubLOB", AdditionalFields = "SubLOBID", HttpMethod = "GET")]
        public string SubLOBName { get; set; }
        public string SubLOBDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
