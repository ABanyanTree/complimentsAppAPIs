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
    public class CreateLOBChapterRequestVM : BaseRequestVM
    {
        public string LOBChapterID { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid LOBChapter Name.")]        
        [Required(ErrorMessage = "Please enter LOBChapter Name")]
        [Remote("IsLOBChapterNameInUse", "LOBChapter", AdditionalFields = "LOBChapterID", HttpMethod = "GET")]     
        public string LOBChapterName { get; set; }
        public string LOBChapterDescription { get; set; }
        public bool IsActive { get; set; }

        //LOB List
        public string LOBList { get; set; }


    }
}
