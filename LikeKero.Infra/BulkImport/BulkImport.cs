using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LikeKero.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using LikeKero.Infra.FileSystem;
using System.Data;
using OfficeOpenXml;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace LikeKero.Infra.BulkImport
{
    public class BulkImport : IBulkImport
    {
        private IHostingEnvironment _hostingEnvironment;
        private string WWWROOT = "";
        private IOptions<FileSystemPath> _options;
        private const string FILE_NOT_FOUND = "Error - Bulk Import File Not Found";
        private const string UNABLE_TO_READ_FILE = "Error - Unable to Read File";
        private const string UNABLE_INVALID_FILE = "Error - Invalid File Uploaded";
        

        public BulkImport(IHostingEnvironment hostingEnvironment, IOptions<FileSystemPath> options)
        {
            _hostingEnvironment = hostingEnvironment;
            WWWROOT = _hostingEnvironment.WebRootPath;
            _options = options;
        }

      

        public DataTable ReadExcelToDataTable(string FilePath, ref string ErrorMessage)
        {
            DataTable dtExcelDataTable = new DataTable();
            try
            {

                var package = new ExcelPackage(new FileInfo(FilePath));
                ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                //Get the values in the sheet
                object[,] valueArray = workSheet.Cells.GetValue<object[,]>();
                int maxRows = workSheet.Dimension.End.Row;
                //int maxColumns = workSheet.Dimension.End.Column;
                int maxColumns = 7;
                //Column Headers
                for (int col = 0; col < maxColumns; col++)
                {
                    dtExcelDataTable.Columns.Add(valueArray[1, col].ToString());
                }
                //Import Excel Data ot DataTable
                for (int row = 2; row < maxRows; row++)
                {
                    DataRow dr = dtExcelDataTable.NewRow();
                    for (int col = 0; col < maxColumns; col++)
                    {
                        dr[col] = valueArray[row, col]?.ToString();

                        // DateTime.FromOADate(dateNum)
                    }

                    if (AreAllColumnsEmpty(dr))
                    {
                        break;
                    }
                    dtExcelDataTable.Rows.Add(dr);
                }
                workSheet.Dispose();
                package.Dispose();
            }
            catch (Exception e)
            {
                var str = e.Message;
                ErrorMessage = str;
                //TempData["XlsConvertErrorMsg"] = "Converting fail, check if your data is correct";
            }
            return dtExcelDataTable;
        }

        private bool AreAllColumnsEmpty(DataRow dr)
        {
            if (dr == null)
            {
                return true;
            }
            else
            {
                foreach (var value in dr.ItemArray)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(value)))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        //private UserMaster GetUser(DataRow dr)
        //{
        //    UserMaster userMaster = new UserMaster();
        //    try
        //    {
        //        userMaster.FirstName = Convert.ToString(dr[_options.Value.UserImport_FirstName]).Trim();
        //        userMaster.LastName = Convert.ToString(dr[_options.Value.UserImport_LastName]).Trim();
        //        userMaster.EmailAddress = Convert.ToString(dr[_options.Value.UserImport_EmailAddress]).Trim();
        //        userMaster.JobCodeId = Convert.ToString(dr[_options.Value.UserImport_JobCode]).Trim();
        //        userMaster.GroupId = Convert.ToString(dr[_options.Value.UserImport_GroupID]).Trim();
        //        if (!string.IsNullOrEmpty(Convert.ToString(dr[_options.Value.UserImport_HiringDate]).Trim()))
        //        {
        //            try
        //            {
        //                userMaster.HiringDate = DateTime.FromOADate(Convert.ToDouble(dr[_options.Value.UserImport_HiringDate]));
        //            }
        //            catch
        //            {

        //            }
        //            //ResolveExcelDateInput(dr[_options.Value.UserImport_HiringDate].ToString());

        //        }


        //        if (!string.IsNullOrEmpty(Convert.ToString(dr[_options.Value.UserImport_RoleChangeDate]).Trim()))
        //        {
        //            try
        //            {
        //                userMaster.RoleChangeDate = DateTime.FromOADate(Convert.ToDouble(dr[_options.Value.UserImport_RoleChangeDate]));
        //            }
        //            catch
        //            {

        //            }

        //            //ResolveExcelDateInput(dr[_options.Value.UserImport_RoleChangeDate].ToString());

        //        }


        //        return userMaster;
        //    }
        //    catch (Exception ex)
        //    {
        //        return userMaster;
        //    }
        //}

        //private bool IsValidUserUploadFile(DataTable dt)
        //{
        //    if (dt == null)
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.UserImport_FirstName))
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.UserImport_LastName))
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.UserImport_EmailAddress))
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.UserImport_HiringDate))
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.UserImport_JobCode))
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.UserImport_RoleChangeDate))
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.UserImport_GroupID))
        //        return false;

        //    //dt.Columns[_options.Value.UserImport_HiringDate].DataType =typeof(DateTime);
        //    //dt.Columns[_options.Value.UserImport_RoleChangeDate].DataType = typeof(DateTime);

        //    return true;
        //}

        private static DateTime? ResolveExcelDateInput(string inputDate)
        {


            // = DATE(2016, 11, 1)
            inputDate = inputDate.Replace("=DATE(", "").Replace(")", "").Replace(" ", "");
            string[] strArray = inputDate.Split(',');
            if (strArray.Length < 3)
            {
                return null;
            }

            try
            {
                return new DateTime(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
            }
            catch
            {
                return null;
            }


        }

    
        //private List<OIGData> ReadOIGUploadCSV(string FilePath, ref string ErrorMessage)
        //{
        //    StreamReader sr = new StreamReader(FilePath);
        //    string[] headers = sr.ReadLine().Split(',');
        //    if (headers.Length < 18)
        //    {
        //        ErrorMessage = "Current columns (" + headers.Length + ") are less than expected (18)";
        //        return null;
        //    }

        //    List<OIGData> lstCsvData = new List<OIGData>();

        //    #region CSV TO DATATABLE

        //    DataTable dtRecords = new DataTable();
        //    foreach (string header in headers)
        //    {
        //        dtRecords.Columns.Add(header.ToString());
        //    }
        //    while (!sr.EndOfStream)
        //    {
        //        string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
        //        DataRow dr = dtRecords.NewRow();
        //        for (int i = 0; i < headers.Length; i++)
        //        {
        //            dr[i] = rows[i];
        //        }
        //        dtRecords.Rows.Add(dr);
        //    }

        //    #endregion

        //    int firstcheck = 1;

        //    bool isValid = IsValidOIGUploadFile(dtRecords, ref ErrorMessage);

        //    if (isValid == false)
        //    {
        //        firstcheck = 2;
        //        ErrorMessage = "";
        //        isValid = IsValidOIGUploadFile_Excel(dtRecords, ref ErrorMessage);
        //        if (isValid == false)
        //        {
        //            ErrorMessage = ErrorMessage;
        //            return null;
        //        }

        //    }

        //    #region ASSIGN DATA
        //    int rowNo = 1;

        //    if (firstcheck == 2)
        //    {
        //        foreach (DataRow dr in dtRecords.Rows)
        //        {
        //            OIGData obj = new OIGData();
        //            obj.LastName = dr[_options.Value.OIGUpload_LASTNAME_EXCEL].ToString();
        //            obj.FirstName = dr[_options.Value.OIGUpload_FIRSTNAME_EXCEL].ToString();
        //            obj.MiddleName = dr[_options.Value.OIGUpload_MIDNAME_EXCEL].ToString();
        //            obj.BusinessName = dr[_options.Value.OIGUpload_BUSNAME_EXCEL].ToString();
        //            obj.General = dr[_options.Value.OIGUpload_GENERAL_EXCEL].ToString();
        //            obj.Specialty = dr[_options.Value.OIGUpload_SPECIALTY_EXCEL].ToString();
        //            obj.UPIN = dr[_options.Value.OIGUpload_UPIN_EXCEL].ToString();
        //            obj.NPI = dr[_options.Value.OIGUpload_NPI_EXCEL].ToString();
        //            obj.DOB = dr[_options.Value.OIGUpload_DOB_EXCEL].ToString();
        //            obj.Address = dr[_options.Value.OIGUpload_ADDRESS_EXCEL].ToString();
        //            obj.City = dr[_options.Value.OIGUpload_CITY_EXCEL].ToString();
        //            obj.State = dr[_options.Value.OIGUpload_STATE_EXCEL].ToString();
        //            obj.Zip = dr[_options.Value.OIGUpload_ZIP_EXCEL].ToString();
        //            obj.ExclusionType = dr[_options.Value.OIGUpload_EXCLTYPE_EXCEL].ToString();
        //            obj.ExclusionDate = dr[_options.Value.OIGUpload_EXCLDATE_EXCEL].ToString();
        //            obj.ReinDate = dr[_options.Value.OIGUpload_REINDATE_EXCEL].ToString();
        //            obj.WaiverDate = dr[_options.Value.OIGUpload_WAIVERDATE_EXCEL].ToString();
        //            obj.WaiverState = dr[_options.Value.OIGUpload_WVRSTATE_EXCEL].ToString();

        //            obj.RowNo = rowNo++;

        //            lstCsvData.Add(obj);
        //        }
        //    }
        //    else if (firstcheck == 1)
        //    {
        //        foreach (DataRow dr in dtRecords.Rows)
        //        {
        //            OIGData obj = new OIGData();
        //            obj.LastName = dr[_options.Value.OIGUpload_LASTNAME].ToString();
        //            obj.FirstName = dr[_options.Value.OIGUpload_FIRSTNAME].ToString();
        //            obj.MiddleName = dr[_options.Value.OIGUpload_MIDNAME].ToString();
        //            obj.BusinessName = dr[_options.Value.OIGUpload_BUSNAME].ToString();
        //            obj.General = dr[_options.Value.OIGUpload_GENERAL].ToString();
        //            obj.Specialty = dr[_options.Value.OIGUpload_SPECIALTY].ToString();
        //            obj.UPIN = dr[_options.Value.OIGUpload_UPIN].ToString();
        //            obj.NPI = dr[_options.Value.OIGUpload_NPI].ToString();
        //            obj.DOB = dr[_options.Value.OIGUpload_DOB].ToString();
        //            obj.Address = dr[_options.Value.OIGUpload_ADDRESS].ToString();
        //            obj.City = dr[_options.Value.OIGUpload_CITY].ToString();
        //            obj.State = dr[_options.Value.OIGUpload_STATE].ToString();
        //            obj.Zip = dr[_options.Value.OIGUpload_ZIP].ToString();
        //            obj.ExclusionType = dr[_options.Value.OIGUpload_EXCLTYPE].ToString();
        //            obj.ExclusionDate = dr[_options.Value.OIGUpload_EXCLDATE].ToString();
        //            obj.ReinDate = dr[_options.Value.OIGUpload_REINDATE].ToString();
        //            obj.WaiverDate = dr[_options.Value.OIGUpload_WAIVERDATE].ToString();
        //            obj.WaiverState = dr[_options.Value.OIGUpload_WVRSTATE].ToString();

        //            obj.RowNo = rowNo++;

        //            lstCsvData.Add(obj);
        //        }
        //    }


        //    #endregion

        //    return lstCsvData;

        //}

        //private bool IsValidOIGUploadFile(DataTable dt, ref string ErrorMessage)
        //{
        //    if (dt == null)
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_LASTNAME))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_LASTNAME + " is missing, ";
        //        return false;
        //    }
        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_FIRSTNAME))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_FIRSTNAME + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_MIDNAME))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_MIDNAME + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_BUSNAME))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_BUSNAME + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_GENERAL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_GENERAL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_SPECIALTY))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_SPECIALTY + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_UPIN))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_UPIN + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_NPI))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_NPI + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_DOB))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_DOB + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_ADDRESS))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_ADDRESS + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_CITY))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_CITY + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_STATE))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_STATE + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_ZIP))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_ZIP + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_EXCLTYPE))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_EXCLTYPE + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_EXCLDATE))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_EXCLDATE + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_REINDATE))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_REINDATE + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_WAIVERDATE))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_WAIVERDATE + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_WVRSTATE))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_WVRSTATE + " is missing, ";
        //        return false;
        //    }

        //    return true;
        //}

        //private bool IsValidOIGUploadFile_Excel(DataTable dt, ref string ErrorMessage)
        //{
        //    if (dt == null)
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_LASTNAME_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_LASTNAME_EXCEL + " is missing, ";
        //        return false;
        //    }
        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_FIRSTNAME_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_FIRSTNAME_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_MIDNAME_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_MIDNAME_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_BUSNAME_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_BUSNAME_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_GENERAL_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_GENERAL_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_SPECIALTY_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_SPECIALTY_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_UPIN_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_UPIN_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_NPI_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_NPI_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_DOB_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_DOB_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_ADDRESS_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_ADDRESS_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_CITY_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_CITY_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_STATE_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_STATE_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_ZIP_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_ZIP_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_EXCLTYPE_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_EXCLTYPE_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_EXCLDATE_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_EXCLDATE_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_REINDATE_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_REINDATE_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_WAIVERDATE_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_WAIVERDATE_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.OIGUpload_WVRSTATE_EXCEL))
        //    {
        //        ErrorMessage += _options.Value.OIGUpload_WVRSTATE_EXCEL + " is missing, ";
        //        return false;
        //    }

        //    return true;
        //}


       
        //private bool IsValidVendorUploadFile(DataTable dt, ref string ErrorMessage)
        //{
        //    if (dt == null)
        //        return false;

        //    if (Convert.ToBoolean(dt.Columns?.Contains(_options.Value.VendorUpload_VendorName)) == false)
        //    {
        //        ErrorMessage += _options.Value.VendorUpload_VendorName + " is missing, ";
        //        return false;
        //    }

        //    if (Convert.ToBoolean(dt.Columns?.Contains(_options.Value.VendorUpload_GroupName)) == false)
        //    {
        //        ErrorMessage += _options.Value.VendorUpload_GroupName + " is missing, ";
        //        return false;
        //    }

        //    return true;
        //}

        #region BUSINESS RULE FILE

       
        //private bool IsValidBusinessRuleUploadFile(DataTable dt, ref string ErrorMessage)
        //{
        //    if (dt == null)
        //        return false;

        //    if (Convert.ToBoolean(dt.Columns?.Contains(_options.Value.UserImport_EmailAddress)) == false)
        //    {
        //        ErrorMessage += _options.Value.UserImport_EmailAddress + " is missing, ";
        //        return false;
        //    }

        //    return true;
        //}

        #endregion

        //public OIGUploadHistory AutoDownloadOIGFile(DateTime dt)
        //{
        //    string urlToDownload = _options.Value.OIGAutoDownload;

        //    string oigUploadId = Utility.GeneratorUniqueId("AUTO_");
        //    urlToDownload = urlToDownload.Replace("yyyy", dt.Year.ToString()).Replace("yy", dt.ToString("yy")).Replace("MM", dt.ToString("MM"));

        //    string fileName = Path.GetFileName(urlToDownload);
        //    string dirName = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.OIGDataFolder, oigUploadId);
        //    if (!Directory.Exists(dirName))
        //    {
        //        Directory.CreateDirectory(dirName);
        //    }
        //    string fullPath = Path.Combine(dirName, fileName);
        //    try
        //    {
        //        WebClient webClient = new WebClient();
        //        webClient.DownloadFile(urlToDownload, fullPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        Directory.Delete(dirName);
        //        return null;
        //    }

        //    OIGUploadHistory obj = new OIGUploadHistory()
        //    {
        //        OIGUploadId = oigUploadId,
        //        FileName = fileName
        //    };
        //    return obj;
        //}

      
        private string setDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                string[] arr;
                if (date.Contains("-"))
                {
                    arr = date.Split('-');
                    if (arr.Length > 2)
                        date = string.Concat(arr[2], "", arr[0], "", arr[1]);
                }
                else if (date.Contains("/"))
                {
                    arr = date.Split('/');
                    if (arr.Length > 2)
                        date = string.Concat(arr[2], "", arr[0], "", arr[1]);
                }
            }

            return date;
        }

        //private bool IsValidGSAUploadFile(DataTable dt, ref string ErrorMessage)
        //{
        //    if (dt == null)
        //        return false;

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Classification))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Classification + " is missing, ";
        //        return false;
        //    }
        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Name))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Name + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Prefix))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Prefix + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_First))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_First + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Middle))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Middle + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Last))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Last + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Suffix))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Suffix + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_NPI))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_NPI + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Address_1))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Address_1 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Address_2))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Address_2 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Address_3))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Address_3 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Address_4))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Address_4 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_City))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_City + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Zip_Code))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Zip_Code + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_DUNS))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_DUNS + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Exclusion_Program))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Exclusion_Program + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Excluding_Agency))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Excluding_Agency + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_CT_Code))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_CT_Code + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Exclusion_Type))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Exclusion_Type + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Additional_Comments))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Additional_Comments + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Active_Date))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Active_Date + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Termination_Date))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Termination_Date + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Record_Status))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Record_Status + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Cross_Reference))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Cross_Reference + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_SAM_Number))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_SAM_Number + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_CAGE))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_CAGE + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_NPI))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_NPI + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_Creation_Date))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_Creation_Date + " is missing, ";
        //        return false;
        //    }

        //    return true;
        //}

        //private bool IsValidGSAUploadFile_Excel(DataTable dt, ref string ErrorMessage)
        //{
        //    if (dt == null)
        //        return false;
        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Classification))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Classification + " is missing, ";
        //        return false;
        //    }
        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Name))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Name + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Prefix))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Prefix + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_First))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_First + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Middle))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Middle + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Last))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Last + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Suffix))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Suffix + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_NPI))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_NPI + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Address_1))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Address_1 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Address_2))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Address_2 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Address_3))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Address_3 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Address_4))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Address_4 + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_City))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_City + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Zip_Code))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Zip_Code + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_DUNS))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_DUNS + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Exclusion_Program))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Exclusion_Program + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Excluding_Agency))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Excluding_Agency + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_CT_Code))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_CT_Code + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Exclusion_Type))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Exclusion_Type + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Additional_Comments))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Additional_Comments + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Active_Date))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Active_Date + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Termination_Date))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Termination_Date + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Record_Status))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Record_Status + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Cross_Reference))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Cross_Reference + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_SAM_Number))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_SAM_Number + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_CAGE))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_CAGE + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_NPI))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_NPI + " is missing, ";
        //        return false;
        //    }

        //    if (!dt.Columns.Contains(_options.Value.GSAUpload_EXCEL_Creation_Date))
        //    {
        //        ErrorMessage += _options.Value.GSAUpload_EXCEL_Creation_Date + " is missing, ";
        //        return false;
        //    }

        //    return true;

        //}
    }
}
