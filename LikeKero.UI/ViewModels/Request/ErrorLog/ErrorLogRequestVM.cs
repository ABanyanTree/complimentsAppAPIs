using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class ErrorLogRequestVM : BaseRequestVM
    {
        public DateTime? ErrorFromDate { get; set; }
        public DateTime? ErrorToDate { get; set; }
        public string ErrorMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
