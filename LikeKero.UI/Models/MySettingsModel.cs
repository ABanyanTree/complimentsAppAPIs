using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Models
{
    public class MySettingsModel
    {
        public string WebApiBaseUrl { get; set; }
        public string UserTemplateFileName { get; set; }
        public string OIGCheckURL { get; set; }
        public string CourseReturnURL { get; set; }
        public string CourseReturnURLAdmin { get; set; }
        public string VendorTemplateFileName { get; set; }
        public string LandingPageCourseId { get; set; }
        public string ClaimCreditVideo { get; set; }
        public string ResetPasswordVideo { get; set; }
        public string PrintCertificateVideo { get; set; }
        public string BulkPurchaseVideo { get; set; }
        public string OIGGSAVideo { get; set; }

        public string AssignmentDetailReportVideo { get; set; }
        public string GroupAdminReportVideo { get; set; }
        public string EnrollmentReportVideo { get; set; }
        public string LearnerRegReportVideo { get; set; }
        public string ACPEReportVideo { get; set; }
        public string ComplianceReportVideo { get; set; }
        public string OIGUserReportVideo { get; set; }
        public string OIGMonthlyUserReportVideo { get; set; }
        public string OIGVendorReportVideo { get; set; }
        public string OIGMonthlyVendorReportVideo { get; set; }
        public string OIGAuditReportVideo { get; set; }
        public string VendorVideo { get; set; }

        public string ShowBRTrigger { get; set; }
        public string ShowNewsLetterTrigger { get; set; }

        public string ReportHelpFile { get; set; }
        public string VendorHelpFile { get; set; }
        public string TranscriptVideo { get; set; }

        public string AddUserVideo { get; set; }
        public string AdminResetPasswordVideo { get; set; }
        public string ManualCompletionVideo { get; set; }
        public string MyLearningVideo { get; set; }

        public string IsScormDispatchEnable { get; set; }
        public string LoginPageText { get; set; }
        public string ShowLeaderNetVsMSIGraph { get; set; }

        public string BloodbornePathogensCourseId { get; set; }
        public string USP800CourseId { get; set; }
        public string DEAAuditCourseId { get; set; }
        public string SocialMediaCourseId { get; set; }

        public string AssignedCoursesFile { get; set; }
    }
}
