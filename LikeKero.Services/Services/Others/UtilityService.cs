using LikeKero.Infra;
using LikeKero.Services.Interfaces.Others;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Services.Services.Others
{
    public class UtilityService : IUtilityService
    {
        public string MaskCharForTestViewer(string strString)
        {
            return Utility.MaskCharForTestViewer(strString);
        }
    }
}
