using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class BaseResponseVM
    {
        public bool IsSuccess { get; set; }
        public int TotalCount { get; set; }
        public List<ErrorModelVM> Errors { get; set; } = new List<ErrorModelVM>();
    }
}
