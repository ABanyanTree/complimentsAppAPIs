using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class BaseRequestVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortExp { get; set; }
        public string CreatorName { get; set; }
        public string RequesterUserId { get; set; }
    }
}
