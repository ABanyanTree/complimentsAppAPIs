using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels.Response
{
    public class LookupByTypeResponseVM : BaseResponseVM
    {
        public string LookUpType { get; set; }
        public string LookUpName { get; set; }
        public string LookUpValue { get; set; }
    }
}
