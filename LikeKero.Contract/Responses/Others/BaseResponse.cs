using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class BaseResponse
    {

        public bool IsSuccess { get; set; }
        public int TotalCount { get; set; }
        public string CreatorName { get; set; }

        public string CreatedDateDisplay { get; set; }
        public string LastModifiedDateDisplay { get; set; }

    }
}
