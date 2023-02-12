﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class ErrorLogGetResponseVM : BaseResponseVM
    {
        public string ErrorLogID { get; set; }        
        public string ErrorMessage { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime? ErrorDateTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
