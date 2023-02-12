using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class ErrorResponse: BaseResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
