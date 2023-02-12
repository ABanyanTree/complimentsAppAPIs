using LikeKero.Domain;
using LikeKero.Infra.BaseUri;
using LikeKero.Infra.Encryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace LikeKero.Infra.FileSystem
{
    public class FileUpload : IFileUpload
    {
        private IHostingEnvironment _hostingEnvironment;
        private string WWWROOT = "";
        private IOptions<FileSystemPath> _options;
        private IUriService _uriService;
        private XMLLib xLib = new XMLLib();

        XmlDocument oIMSXMLObj = null;
        XmlNodeList ScoNodeList = null;
        int TotalNoOfPages = 0;
        string returnURLtoCourse = string.Empty;
        string sTreeString = "";
        string totalscos = "";
        string backstr = "&#160;&#160;&#160;&#160;&#160;&#160;&#160;";
        bool blnIsNewAttempt = true;
        int iMasteryScore = -1;
        string sAVPath = string.Empty;
        string _UserId = string.Empty;
        string _CourseId = string.Empty;
        string _UserName = string.Empty;
        string _CourseName = string.Empty;



        private XmlNamespaceManager nsmanager = null;

        public string cln = "";

        string sContentFolderUrl = string.Empty;
        string sContentFolderPath = string.Empty;
        string sLaunchParameter = string.Empty;
        string sLaunchParameterQueryString = string.Empty;
        string sLearnerLanguageId = "en-US";
        string sLanguageCode = string.Empty;

        string sStudentName = string.Empty;

        XmlDocument RequestXMLObj = null;
        string strCase;
        string strManifestId;
        string strStudentId;
        string strResponse;
        string UserDataXMLDirPath;
        XmlDocument UserDataXMLObj = null;
        XmlNode NodeList = null;
        string sUserName = string.Empty;
        string sCourseID = string.Empty;
        string sCourseName = string.Empty;
        string sLearnerId = string.Empty;
        string sClientId = string.Empty;
        string sUserDataXmlPath = string.Empty;
        string strAttemptId = string.Empty;
        bool errFound = false;
        string errMsg = string.Empty;

        public static int ProgressCnt = 0;
        public static Dictionary<string, int> FileProgressCnt = new Dictionary<string, int>();

        #region Other Methods

        public FileUpload(IHostingEnvironment hostingEnvironment, IOptions<FileSystemPath> options, IUriService uriService)
        {
            _hostingEnvironment = hostingEnvironment;
            WWWROOT = _hostingEnvironment.WebRootPath;
            _options = options;
            _uriService = uriService;
        }


        public async Task<string> UploadUserProfilePic(IFormFile ProfilePicformFile, string UserId)
        {
            if (ProfilePicformFile.Length > 0)
            {
                string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.ProfilePicPath, UserId);
                string returnPath = Path.Combine(WWWROOT, _options.Value.ProfilePicPath, UserId);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                var singleFilePath = Path.Combine(newPath, ProfilePicformFile.FileName);


                using (var stream = new FileStream(singleFilePath, FileMode.Create))
                {
                    await ProfilePicformFile.CopyToAsync(stream, new System.Threading.CancellationToken());
                }

                return string.Concat(_options.Value.ProfilePicPath, "/", UserId, "/", ProfilePicformFile.FileName);
            }

            return string.Empty;
        }

        //public async Task<Stream> GetUserTemplateFile()
        //{
        //    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.ImportTemplatesFolderName, _options.Value.UserTemplateFileName);
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

        //public async Task<string> UploadUserTemplateFile(IFormFile TemplateFile, string BulkImportId)
        //{
        //    if (TemplateFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.ImportFileBaseFolder, _options.Value.UserImportFolder, BulkImportId);
        //        string returnPath = Path.Combine(WWWROOT, _options.Value.ImportFileBaseFolder, _options.Value.UserImportFolder, BulkImportId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }

        //        var singleFilePath = Path.Combine(newPath, TemplateFile.FileName);


        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await TemplateFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return TemplateFile.FileName;
        //    }

        //    return string.Empty;
        //}


       
        //public async Task<Stream> GetBulkImportUploadedFile(string BulkImportId, string BulkImportFileName)
        //{
        //    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.ImportFileBaseFolder, _options.Value.UserImportFolder, BulkImportId, BulkImportFileName);
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

        //public async Task<string> UploadCourseHeaderImage(IFormFile HeaderImage, string CourseId)
        //{
        //    if (HeaderImage.Length > 0)
        //    {

        //        string DestinationPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.CourseHeaderImageFolder, CourseId);

        //        if (Directory.Exists(DestinationPath))
        //        {
        //            Directory.Delete(DestinationPath, true);
        //        }

        //        if (!Directory.Exists(DestinationPath))
        //        {
        //            Directory.CreateDirectory(DestinationPath);
        //        }
        //        string destinationFileName = Path.Combine(DestinationPath, HeaderImage.FileName);
        //        using (var stream = new FileStream(destinationFileName, FileMode.Create))
        //        {
        //            await HeaderImage.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return DestinationPath;
        //    }

        //    return string.Empty;
        //}

        public Task<bool> IsFileExists(string PathAfterWWWRoot)
        {
            string DestinationPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, PathAfterWWWRoot);
            if (File.Exists(DestinationPath))
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }

        //public Task<string> GetDefaultCourseHeaderImage()
        //{
        //    string DefaultHttpPath = _uriService.GetBaseUri().ToString() + Convert.ToString(_options.Value.AfterDomain) +
        //                _options.Value.CourseHeaderImageFolder + "/" + _options.Value.CourseDefaultHeaderImage;

        //    return Task.FromResult(DefaultHttpPath);

        //}


        public async Task<Stream> ExportUserData(IEnumerable<UserMaster> lstUsers)
        {
            DataTable dtRecords = UserListToDataTable(lstUsers);
            string fileName = "User Data_" + DateTime.Now.ToString("MM-dd-yyyy hh mm ss") + ".xlsx";
            string fileFullPath = ExportDataTableToExcel(fileName, dtRecords, "Users");

            var memory = new MemoryStream();
            using (var stream = new FileStream(fileFullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return memory;
        }

        private DataTable UserListToDataTable(IEnumerable<UserMaster> lstUsers)
        {
            DataTable dtRecords = new DataTable();
            dtRecords.Columns.Add("First Name");
            dtRecords.Columns.Add("Last Name");
            dtRecords.Columns.Add("Email/UserName");
            dtRecords.Columns.Add("Hiring Date");
            dtRecords.Columns.Add("Job Code");
            dtRecords.Columns.Add("Group");
            dtRecords.Columns.Add("Group Description");
            dtRecords.Columns.Add("Status");
            dtRecords.Columns.Add("Role Change Date");

            foreach (UserMaster singleUser in lstUsers)
            {
                DataRow dr = dtRecords.NewRow();

                dr["First Name"] = singleUser.FirstName;
                dr["Last Name"] = singleUser.LastName;
                dr["Email/UserName"] = singleUser.EmailAddress;
                dr["Job Code"] = singleUser.JobCode;
                dr["Group"] = singleUser.GroupName;
                dr["Group Description"] = singleUser.GroupDescription;
                dr["Status"] = singleUser.DisplayStatus;

                if (singleUser.HiringDate != null)
                {
                    dr["Hiring Date"] = Utility.GetOnlyDate(singleUser.HiringDate);
                }
                if (singleUser.RoleChangeDate != null)
                {
                    dr["Role Change Date"] = Utility.GetOnlyDate(singleUser.RoleChangeDate);
                }

                dtRecords.Rows.Add(dr);
            }

            return dtRecords;
        }

      

        private string ExportDataTableToExcel(string fileName, DataTable dtRecords, string sheetName)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, "ExportFiles", fileName);
            using (ExcelPackage pck = new ExcelPackage(new FileInfo(fullPath)))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);
                ws.Cells["A1"].LoadFromDataTable(dtRecords, true);
                ws.Cells["A1:Z1"].Style.Font.Bold = true;

                pck.Save();
            }

            return fullPath;
        }

        #endregion

        //public async Task<string> UploadQuestionImage(IFormFile QuestionImageFile, string QuestionId)
        //{
        //    if (QuestionImageFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.QuestionImagePath, QuestionId);
        //        string returnPath = Path.Combine(WWWROOT, _options.Value.QuestionImagePath, QuestionId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }

        //        var singleFilePath = Path.Combine(newPath, QuestionImageFile.FileName);


        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await QuestionImageFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return string.Concat(_options.Value.QuestionImagePath, "/", QuestionId, "/", QuestionImageFile.FileName);
        //    }

        //    return string.Empty;
        //}

        //public async Task<string> UploadSurveyQuestionImage(IFormFile QuestionImageFile, string QuestionId)
        //{
        //    if (QuestionImageFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.SurveyQuestionImagePath, QuestionId);
        //        string returnPath = Path.Combine(WWWROOT, _options.Value.SurveyQuestionImagePath, QuestionId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }

        //        var singleFilePath = Path.Combine(newPath, QuestionImageFile.FileName);


        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await QuestionImageFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return string.Concat(_options.Value.SurveyQuestionImagePath, "/", QuestionId, "/", QuestionImageFile.FileName);
        //    }

        //    return string.Empty;
        //}

        //public async Task<string> UploadOIGDataFile(IFormFile OIGDataFile, string OIGUploadId)
        //{
        //    if (OIGDataFile.Length > 0)
        //    {

        //        string DestinationPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.OIGDataFolder, OIGUploadId);

        //        if (Directory.Exists(DestinationPath))
        //        {
        //            Directory.Delete(DestinationPath, true);
        //        }

        //        if (!Directory.Exists(DestinationPath))
        //        {
        //            Directory.CreateDirectory(DestinationPath);
        //        }
        //        string destinationFileName = Path.Combine(DestinationPath, OIGDataFile.FileName);
        //        using (var stream = new FileStream(destinationFileName, FileMode.Create))
        //        {
        //            await OIGDataFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }



        //        return DestinationPath;
        //    }

        //    return string.Empty;
        //}

        //public async Task<Stream> GetOIGUploadedFile(string OIGUploadId, string FileName)
        //{
        //    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.OIGDataFolder, FileName);
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}


        //public async Task<string> UploadBusinessAssignmentFile(IFormFile BusinessFile, string BusinessAssignmentId)
        //{

        //    if (BusinessFile.Length > 0)
        //    {

        //        string DestinationPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.BusinessRulesFile, BusinessAssignmentId);


        //        if (!Directory.Exists(DestinationPath))
        //        {
        //            Directory.CreateDirectory(DestinationPath);
        //        }
        //        string destinationFileName = Path.Combine(DestinationPath, BusinessFile.FileName);
        //        using (var stream = new FileStream(destinationFileName, FileMode.Create))
        //        {
        //            await BusinessFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return DestinationPath;
        //    }

        //    return string.Empty;
        //}

        //public async Task<string> UploadVendorDataFile(IFormFile VendorDataFile, string BulkImportId)
        //{
        //    if (VendorDataFile.Length > 0)
        //    {

        //        string DestinationPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.VendorDataFolder, BulkImportId);

        //        if (Directory.Exists(DestinationPath))
        //        {
        //            Directory.Delete(DestinationPath, true);
        //        }

        //        if (!Directory.Exists(DestinationPath))
        //        {
        //            Directory.CreateDirectory(DestinationPath);
        //        }
        //        string destinationFileName = Path.Combine(DestinationPath, VendorDataFile.FileName);
        //        using (var stream = new FileStream(destinationFileName, FileMode.Create))
        //        {
        //            await VendorDataFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return DestinationPath;
        //    }

        //    return string.Empty;
        //}

        //public async Task<Stream> GetVendorUploadedFile(string FileName)
        //{
        //    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.VendorDataFolder, FileName);
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

        //public async Task<Stream> GetVendorTemplateFile()
        //{
        //    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.ImportTemplatesFolderName, _options.Value.VendorTemplateFileName);
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

        //public async Task<string> UploadMedia(IFormFile MediaFile, string MediaId)
        //{
        //    if (MediaFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.MediaFiles, MediaId);
        //        string returnPath = Path.Combine(WWWROOT, _options.Value.MediaFiles, MediaId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }

        //        var singleFilePath = Path.Combine(newPath, MediaFile.FileName);


        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await MediaFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return string.Concat(_options.Value.MediaFiles, "/", MediaId, "/", MediaFile.FileName);
        //    }

        //    return string.Empty;
        //}

        public async Task<string> ProgressUploadCourse(string fileKey)
        {
            //return ProgressCnt.ToString();
            FileProgressCnt.TryGetValue(fileKey, out int retValue);
            return retValue.ToString();
        }

       
        //public async Task<string> UploadEventFile(IFormFile EventFile, string RecordId)
        //{
        //    if (EventFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.EventFilePath, RecordId);
        //        string returnPath = Path.Combine(WWWROOT, _options.Value.EventFilePath, RecordId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }

        //        var singleFilePath = Path.Combine(newPath, EventFile.FileName);


        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await EventFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return string.Concat(_options.Value.EventFilePath, "/", RecordId, "/", EventFile.FileName);
        //    }

        //    return string.Empty;
        //}



        public async Task<string> UploadTestCustomerFile(IFormFile TestCustomerFile, string CustID)
        {
            if (TestCustomerFile.Length > 0)
            {
                string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.TestCustFilePath, CustID);
                string returnPath = Path.Combine(WWWROOT, _options.Value.TestCustFilePath, CustID);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                var singleFilePath = Path.Combine(newPath, TestCustomerFile.FileName);


                using (var stream = new FileStream(singleFilePath, FileMode.Create))
                {
                    await TestCustomerFile.CopyToAsync(stream, new System.Threading.CancellationToken());
                }

                return string.Concat(_options.Value.TestCustFilePath, "/", CustID, "/", TestCustomerFile.FileName);
            }

            return string.Empty;
        }



        //public async Task<string> UploadTemplateFile(string NewsLetterId, IFormFile TemplateFile)
        //{
        //    if (TemplateFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.NewsLetterTemplatePath, NewsLetterId);
        //        string returnPath = Path.Combine(WWWROOT, _options.Value.NewsLetterTemplatePath, NewsLetterId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }
        //        var singleFilePath = Path.Combine(newPath, TemplateFile.FileName);
        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await TemplateFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }
        //        return string.Concat(_options.Value.BaseURLPath, "/", _options.Value.NewsLetterTemplatePath, "/", NewsLetterId, "/", TemplateFile.FileName);
        //    }

        //    return string.Empty;
        //}

        #region KIRAN(4FEB2020) : Policy pdf upload/download
        //public async Task<string> UploadPolicyPDF(IFormFile PolicyFile, string PolicyId)
        //{
        //    if (PolicyFile.Length > 0)
        //    {

        //        string DestinationPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.PolicyPDFFolder, PolicyId);

        //        if (Directory.Exists(DestinationPath))
        //            Directory.Delete(DestinationPath, true);

        //        if (!Directory.Exists(DestinationPath))
        //            Directory.CreateDirectory(DestinationPath);

        //        string destinationFileName = Path.Combine(DestinationPath, PolicyFile.FileName);

        //        using (var stream = new FileStream(destinationFileName, FileMode.Create))
        //        {
        //            await PolicyFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return DestinationPath;
        //    }

        //    return string.Empty;
        //}

        //public async Task<Stream> GetPolicyPDF(string PolicyId, string PolicyFileName)
        //{
        //    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.PolicyPDFFolder, PolicyId, PolicyFileName);
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

        #endregion

        //public async Task<string> UploadPolicy(IFormFile PolicyFile, string PolicyId)
        //{
        //    if (PolicyFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.PolicyPDFFolder, PolicyId);
        //        string returnPath = Path.Combine(WWWROOT, _options.Value.PolicyPDFFolder, PolicyId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }

        //        var singleFilePath = Path.Combine(newPath, PolicyFile.FileName);


        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await PolicyFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return string.Concat(_options.Value.PolicyPDFFolder, "/", PolicyId, "/", PolicyFile.FileName);
        //    }

        //    return string.Empty;
        //}

        //public async Task<Stream> ExportNCPDPReportData(IEnumerable<EntityName> lstGroupAdmins)
        //{
        //    DataTable dtRecords = NCPDPListToDataTable(lstGroupAdmins);
        //    string fileName = "NCPDP_Report_" + DateTime.Now.ToString("MM-dd-yyyy hh mm ss") + ".xlsx";
        //    string fileFullPath = ExportDataTableToExcel(fileName, dtRecords, "NCPDP Report");

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fileFullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

      
        //private DataTable NCPDPListToDataTable(IEnumerable<EntityName> lstCourses)
        //{
        //    DataTable dtRecords = new DataTable();
        //    dtRecords.Columns.Add("NCPDP ID");
        //    dtRecords.Columns.Add("Description");
        //    dtRecords.Columns.Add("Association");
        //    dtRecords.Columns.Add("Users OIG - GSA Check");
        //    dtRecords.Columns.Add("Vendors OIG - GSA Check");
        //    dtRecords.Columns.Add("Status");

        //    foreach (EntityName singleCourse in lstCourses)
        //    {
        //        DataRow dr = dtRecords.NewRow();

        //        dr["NCPDP ID"] = singleCourse.Group3Name;
        //        dr["Description"] = singleCourse.Description;
        //        dr["Association"] = singleCourse.Group2Name;
        //        dr["Users OIG - GSA Check"] = singleCourse.IsOIGUserCheck == true ? "Yes" : "No";
        //        dr["Vendors OIG - GSA Check"] = singleCourse.IsOIGVendorCheck == true ? "Yes" : "No";
        //        dr["Status"] = singleCourse.Status == true ? "Active" : "Inactive";

        //        dtRecords.Rows.Add(dr);
        //    }

        //    return dtRecords;
        //}
       
        //public async Task<Stream> ExportOIGUserAuditData(IEnumerable<OIGData> lst)
        //{
        //    DataTable dtRecords = OIGUserAuditListToDataTable(lst);
        //    string fileName = "OIG-GSA_User_Audit_" + DateTime.Now.ToString("MM-dd-yyyy hh mm ss") + ".xlsx";
        //    string fileFullPath = ExportDataTableToExcel(fileName, dtRecords, "OIG-GSA Audit – User");

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fileFullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

        //private DataTable OIGUserAuditListToDataTable(IEnumerable<OIGData> lst)
        //{
        //    DataTable dtRecords = new DataTable();
        //    dtRecords.Columns.Add("Hits");
        //    dtRecords.Columns.Add("First Name");
        //    dtRecords.Columns.Add("Last Name");
        //    dtRecords.Columns.Add("Group Name");
        //    dtRecords.Columns.Add("Group Description");
        //    dtRecords.Columns.Add("Checked on");

        //    foreach (OIGData singleCourse in lst)
        //    {
        //        DataRow dr = dtRecords.NewRow();
        //        dr["Hits"] = singleCourse.NoOfHits;
        //        dr["First Name"] = singleCourse.FirstName;
        //        dr["Last Name"] = singleCourse.LastName;
        //        dr["Group Name"] = singleCourse.GroupName;
        //        dr["Group Description"] = singleCourse.GroupDescription;

        //        dr["Checked on"] = singleCourse.CheckedMonth;
        //        dtRecords.Rows.Add(dr);
        //    }

        //    return dtRecords;
        //}

        //public async Task<Stream> ExportOIGVendorAuditData(IEnumerable<OIGData> lst)
        //{
        //    DataTable dtRecords = OIGVendorAuditListToDataTable(lst);
        //    string fileName = "OIG-GSA_Vendor_Audit_" + DateTime.Now.ToString("MM-dd-yyyy hh mm ss") + ".xlsx";
        //    string fileFullPath = ExportDataTableToExcel(fileName, dtRecords, "OIG-GSA Audit – Vendor");

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(fileFullPath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return memory;
        //}

        //private DataTable OIGVendorAuditListToDataTable(IEnumerable<OIGData> lst)
        //{
        //    DataTable dtRecords = new DataTable();
        //    dtRecords.Columns.Add("Hits");
        //    dtRecords.Columns.Add("Business Name");
        //    dtRecords.Columns.Add("Group Name");
        //    dtRecords.Columns.Add("Group Description");
        //    dtRecords.Columns.Add("Checked on");

        //    foreach (OIGData singleVendor in lst)
        //    {
        //        DataRow dr = dtRecords.NewRow();
        //        dr["Hits"] = singleVendor.NoOfHits;
        //        dr["Business Name"] = singleVendor.VendorName;
        //        dr["Group Name"] = singleVendor.GroupName;
        //        dr["Group Description"] = singleVendor.GroupDescription;
        //        dr["Checked on"] = singleVendor.CheckedMonth;
        //        dtRecords.Rows.Add(dr);
        //    }

        //    return dtRecords;
        //}

    //public async Task<Stream> ExportUserTransferData(IEnumerable<UserTransferLog> lstUsers)
    //    {
    //        DataTable dtRecords = UserTransferListToDataTable(lstUsers);
    //        string fileName = "User Transfer Data_" + DateTime.Now.ToString("MM-dd-yyyy hh mm ss") + ".xlsx";
    //        string fileFullPath = ExportDataTableToExcel(fileName, dtRecords, "Transferred Users");

    //        var memory = new MemoryStream();
    //        using (var stream = new FileStream(fileFullPath, FileMode.Open))
    //        {
    //            await stream.CopyToAsync(memory);
    //        }
    //        memory.Position = 0;

    //        return memory;
    //    }

    //    private DataTable UserTransferListToDataTable(IEnumerable<UserTransferLog> lstUsers)
    //    {
    //        DataTable dtRecords = new DataTable();
    //        dtRecords.Columns.Add("First Name");
    //        dtRecords.Columns.Add("Last Name");
    //        dtRecords.Columns.Add("Transferred From Group");
    //        dtRecords.Columns.Add("Transferred To Group");
    //        dtRecords.Columns.Add("Transferred On");
    //        dtRecords.Columns.Add("Transferred by");
    //        dtRecords.Columns.Add("Remark");

    //        foreach (UserTransferLog singleUser in lstUsers)
    //        {
    //            DataRow dr = dtRecords.NewRow();

    //            dr["First Name"] = singleUser.FirstName;
    //            dr["Last Name"] = singleUser.LastName;
    //            dr["Transferred From Group"] = singleUser.TransferredFromName;
    //            dr["Transferred To Group"] = singleUser.TransferredToName;
    //            dr["Transferred by"] = singleUser.TransferredByName;
    //            dr["Remark"] = singleUser.Remarks;

    //            if (singleUser.TransferredOn != null)
    //            {
    //                dr["Transferred On"] = Utility.GetOnlyDate(singleUser.TransferredOn);
    //            }

    //            dtRecords.Rows.Add(dr);
    //        }

    //        return dtRecords;
    //    }

       
    
        private void setBorder(ExcelRange data)
        {
            data.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            data.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            data.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            data.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            data.Style.Border.Top.Color.SetColor(Color.Black);
            data.Style.Border.Right.Color.SetColor(Color.Black);
            data.Style.Border.Bottom.Color.SetColor(Color.Black);
            data.Style.Border.Left.Color.SetColor(Color.Black);
        }

     
        //public async Task<string> UploadAnnouncementsFile(IFormFile AnnouncementsFile, string AnnouncementsId)
        //{
        //    if (AnnouncementsFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.AnnouncementsFilePath, AnnouncementsId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }
        //        var singleFilePath = Path.Combine(newPath, AnnouncementsFile.FileName);

        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await AnnouncementsFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return AnnouncementsFile.FileName;
        //    }

        //    return string.Empty;
        //}

        //public async Task<string> UploadPolicyFile(IFormFile FormFile, string PolicyId)
        //{
        //    if (FormFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.PolicyFiles, PolicyId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }
        //        var singleFilePath = Path.Combine(newPath, FormFile.FileName);

        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await FormFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return FormFile.FileName;
        //    }

        //    return string.Empty;
        //}

        //public async Task<string> UploadPolicyPurchaseFile(IFormFile FormFile, string PolicyPurchaseId)
        //{
        //    if (FormFile.Length > 0)
        //    {
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.PurchasePolicyFiles, PolicyPurchaseId);
        //        if (!Directory.Exists(newPath))
        //        {
        //            Directory.CreateDirectory(newPath);
        //        }
        //        var singleFilePath = Path.Combine(newPath, "Policy.pdf");

        //        using (var stream = new FileStream(singleFilePath, FileMode.Create))
        //        {
        //            await FormFile.CopyToAsync(stream, new System.Threading.CancellationToken());
        //        }

        //        return FormFile.FileName;
        //    }

        //    return string.Empty;
        //}

       

       }
}


