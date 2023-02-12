using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests
{
    public class BaseRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }       
        public string SortExp { get; set; }
        public string RequesterUserId { get; set; }
        public string CreatorName { get; set; }
    }
}
