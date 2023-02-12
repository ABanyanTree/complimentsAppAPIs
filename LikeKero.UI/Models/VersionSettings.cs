using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Models
{
    public class VersionSettings
    {
        public string VersionName { get; set; }
        public string ShowReportHelp { get; set; }
        public string ShowVendorHelp { get; set; }
        public string ShowMediaIcon { get; set; }
        public string FooterText { get; set; }

        public string NCPDPID_Display { get; set; }
        public string NCPDPDescr_Display { get; set; }
        public string NCPDP_Display { get; set; }
        public string Association_Display { get; set; }
        public string IsNCPDPAlphaNumeric { get; set; }

        public string StagingSiteMessage { get; set; }
    }
}
