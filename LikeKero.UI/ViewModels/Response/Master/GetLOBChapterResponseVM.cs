using System;
using System.Collections.Generic;
using System.Text;
using LikeKero.Contract.Responses;

namespace LikeKero.UI.ViewModels
{
    public class GetLOBChapterResponseVM : BaseResponseVM
    {
        public string LOBChapterID { get; set; }
        public string LOBChapterName { get; set; }
        public string LOBChapterDescription { get; set; }
        public bool IsActive { get; set; }
        public string LOBList { get; set; }
       
    }
}
