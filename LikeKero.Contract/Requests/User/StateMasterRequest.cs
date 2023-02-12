using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Requests
{
   public class StateMasterRequest
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CountryId { get; set; }
    }
}
