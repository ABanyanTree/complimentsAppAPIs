using LikeKero.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Infra.FileSystem
{
    public interface IFileUpload
    {
        //Task<string> UploadUserProfilePic(IFormFile ProfilePicformFile, string UserId);
        //Task<Stream> GetUserTemplateFile();
        //Task<string> UploadUserTemplateFile(IFormFile TemplateFile, string BulkImportId);
       
        //Task<Stream> GetBulkImportUploadedFile(string BulkImportId, string BulkImportFileName);
        //Task<string> UploadCourseHeaderImage(IFormFile HeaderImage, string CourseId);
        //Task<string> UploadQuestionImage(IFormFile QuestionImageFile, string QuestionId);
        //Task<bool> IsFileExists(string PathAfterWWWRoot);
       // Task<string> GetDefaultCourseHeaderImage();
        //Task<string> UploadSurveyQuestionImage(IFormFile QuestionImageFile, string QuestionId);
        //Task<string> UploadOIGDataFile(IFormFile OIGDataFile, string OIGUploadId);
        //Task<Stream> GetOIGUploadedFile(string OIGUploadId, string FileName);
        //Task<string> UploadBusinessAssignmentFile(IFormFile BusinessFile, string BusinessAssignmentId);

        //  Task<Stream> GetBusinessAssignmentFile(string OIGUploadId, string FileName);

        //Task<string> UploadVendorDataFile(IFormFile VendorDataFile, string BulkImportId);
        //Task<Stream> GetVendorUploadedFile(string FileName);

        //Task<Stream> GetVendorTemplateFile();

        //Task<string> UploadMedia(IFormFile MediaFile, string MediaId);
        //Task<string> ProgressUploadCourse(string fileKey);

        Task<Stream> ExportUserData(IEnumerable<UserMaster> lstUsers);
      

        //Task<string> UploadEventFile(IFormFile EventFile, string RecordId);
        Task<string> UploadTestCustomerFile(IFormFile TestCustomerFile, string CustID);

        //Task<string> UploadTemplateFile(string NewsLetterId, IFormFile TemplateFile);

        //KIRAN(4feb2020): Policy PDF upload and get calls
        //Task<string> UploadPolicyPDF(IFormFile PolicyFile, string PolicyId);
        //Task<Stream> GetPolicyPDF(string PolicyId, string PolicyFileName);
        //---------------------

        //Task<string> UploadPolicy(IFormFile PolicyFile, string PolicyId);

        //Task<Stream> ExportNCPDPReportData(IEnumerable<EntityName> lstAdmins);

      
        //Task<Stream> ExportOIGUserAuditData(IEnumerable<OIGData> lst);
        //Task<Stream> ExportOIGVendorAuditData(IEnumerable<OIGData> lst);
       
        //Task<Stream> ExportUserTransferData(IEnumerable<UserTransferLog> lstUsers);
      

        //Task<string> UploadAnnouncementsFile(IFormFile AnnouncementsFile, string AnnouncementsId);

        //Task<string> UploadPolicyFile(IFormFile FormFile, string PolicyId);
        //Task<string> UploadPolicyPurchaseFile(IFormFile FormFile, string PolicyPurchaseId);
       
    }
}
