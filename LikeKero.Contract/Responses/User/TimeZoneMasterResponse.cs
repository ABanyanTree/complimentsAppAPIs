using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class TimeZoneMasterResponse : BaseResponse
    {
        public string TimeZoneId { get; set; }
        public string Description { get; set; }
    }
}
