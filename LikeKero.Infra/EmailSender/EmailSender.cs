using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LikeKero.Domain;
using LikeKero.Domain.Utility;
using LikeKero.Infra.BaseUri;
using LikeKero.Infra.Encryption;
using LikeKero.Infra.FileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace LikeKero.Infra.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private IHostingEnvironment _hostingEnvironment;
        private IOptions<EmailConfig> _options;
        private IOptions<FileSystemPath> _optionsfilesystem;
        private string WWWROOT = "";
        private IUriService _uriService;

        public EmailSender(IHostingEnvironment hostingEnvironment, IOptions<EmailConfig> options, IUriService uriService, IOptions<FileSystemPath> optionsfilesystem)
        {
            _hostingEnvironment = hostingEnvironment;
            WWWROOT = _hostingEnvironment.WebRootPath;
            _options = options;
            _uriService = uriService;
            _optionsfilesystem = optionsfilesystem;
        }

        public async Task<EmailSentLog> SendInstantEmail(EmailSenderEntity emailSenderEntity, object Entity)
        {
            MailMessage mailInfo = new MailMessage();

            if (!string.IsNullOrEmpty(emailSenderEntity.EmailTo))
            {
                string[] strTO = emailSenderEntity.EmailTo.Split(',');
                foreach (string str in strTO)
                {
                    if (!string.IsNullOrEmpty(str) && str.Contains("@"))
                    {
                        mailInfo.To.Add(str);
                    }
                }
            }

            mailInfo.From = new MailAddress(_options.Value.From);
            mailInfo.Subject = emailSenderEntity.Subject;
            string strBody = ReadEmailTemplate(emailSenderEntity.HtmlFileName);

            string strBodyPlusSubject = strBody + " " + emailSenderEntity.Subject;

            string pattern = @"\<%.*?\%>";

            // Create a Regex  
            Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);


            // Get all matches  
            MatchCollection placeHolders = rg.Matches(strBodyPlusSubject);
            // Print all matched authors  
            if (placeHolders != null)
            {
                for (int count = 0; count < placeHolders.Count; count++)
                {
                    string strToBeReplaced = placeHolders[count].Value;

                    string strmanipulatedString = strToBeReplaced.Replace("<%", "").Replace("%>", "");
                    string[] ArrayClassProperty = strmanipulatedString.Split('.');
                    if (ArrayClassProperty != null && ArrayClassProperty.Length > 1)
                    {
                        string strPropertyName = ArrayClassProperty[1];
                        string Value = GetIDFieldValue(strPropertyName.Trim(), Entity);
                        if (!string.IsNullOrEmpty(Value))
                        {
                            strBody = strBody.Replace(strToBeReplaced, Value.Trim());
                            mailInfo.Subject = mailInfo.Subject.Replace(strToBeReplaced, Value.Trim());
                        }
                        else
                        {
                            EmailConfig emailConfig = _options.Value;
                            Value = GetIDFieldValue(strPropertyName.Trim(), emailConfig);
                            strBody = strBody.Replace(strToBeReplaced, Value.Trim());
                            mailInfo.Subject = mailInfo.Subject.Replace(strToBeReplaced, Value.Trim());
                        }

                    }


                }
            }





            mailInfo.Body = strBody;
            mailInfo.IsBodyHtml = true;
            string strSubject = mailInfo.Subject;
            if (!string.IsNullOrEmpty(emailSenderEntity.CC))
            {
                string[] strCC = emailSenderEntity.CC.Split(',');
                foreach (string str in strCC)
                {
                    if (!string.IsNullOrEmpty(str) && str.Contains("@"))
                    {
                        mailInfo.CC.Add(str);
                    }
                }
            }

            if (!string.IsNullOrEmpty(emailSenderEntity.BCC))
            {
                string[] strBCC = emailSenderEntity.BCC.Split(',');
                foreach (string str in strBCC)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        mailInfo.Bcc.Add(str);
                    }
                }
            }

            mailInfo.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();



            if (_options.Value.IsEmailToFolder)
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtp.PickupDirectoryLocation = Path.Combine(WWWROOT, _options.Value.SystemEmailDeliveryPath);
            }
            else
            {
                smtp.Host = EncryptionManager.Decrypt(_options.Value.SMTP_HOST);
                if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_SMTPUSERNAME))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(
                    EncryptionManager.Decrypt(_options.Value.SENDMAIL_SMTPUSERNAME),
                    EncryptionManager.Decrypt(_options.Value.SENDMAIL_SMTPUSERPASSWORD));
                }


                if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_PORT))
                {
                    smtp.Port = Convert.ToInt32(EncryptionManager.Decrypt(_options.Value.SENDMAIL_PORT));
                }

                if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_DEFAULTCREDENTIALS))
                {
                    smtp.UseDefaultCredentials = Convert.ToBoolean(EncryptionManager.Decrypt(_options.Value.SENDMAIL_DEFAULTCREDENTIALS));
                }


            }


            if (_options.Value.SendEmail)
            {
                await smtp.SendMailAsync(mailInfo);

            }

            EmailSentLog emailSentLog = new EmailSentLog();
            emailSentLog.EmailTo = emailSenderEntity.EmailTo;
            emailSentLog.CC = emailSenderEntity.CC;
            emailSentLog.BCC = emailSenderEntity.BCC;
            emailSentLog.Subject = strSubject;
            emailSentLog.Body = strBody;
            emailSentLog.IsEmailSent = true;

            return emailSentLog;



        }


        public EmailSentLog GetBodyAndSubject(EmailSenderEntity emailSenderEntity, object Entity)
        {
            EmailSentLog emailSentLog = new EmailSentLog();


            string strBody = ReadEmailTemplate(emailSenderEntity.HtmlFileName);

            string strBodyPlusSubject = strBody + " " + emailSenderEntity.Subject;

            string pattern = @"\<%.*?\%>";

            // Create a Regex  
            Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);


            // Get all matches  
            MatchCollection placeHolders = rg.Matches(strBodyPlusSubject);
            // Print all matched authors  
            if (placeHolders != null)
            {
                for (int count = 0; count < placeHolders.Count; count++)
                {
                    string strToBeReplaced = placeHolders[count].Value;

                    string strmanipulatedString = strToBeReplaced.Replace("<%", "").Replace("%>", "");
                    string[] ArrayClassProperty = strmanipulatedString.Split('.');
                    if (ArrayClassProperty != null && ArrayClassProperty.Length > 1)
                    {
                        string strPropertyName = ArrayClassProperty[1];
                        string Value = GetIDFieldValue(strPropertyName.Trim(), Entity);
                        if (!string.IsNullOrEmpty(Value))
                        {
                            strBody = strBody.Replace(strToBeReplaced, Value.Trim());
                            emailSenderEntity.Subject = emailSenderEntity.Subject.Replace(strToBeReplaced, Value.Trim());
                        }
                        else
                        {
                            EmailConfig emailConfig = _options.Value;
                            Value = GetIDFieldValue(strPropertyName.Trim(), emailConfig);
                            strBody = strBody.Replace(strToBeReplaced, Value.Trim());
                            emailSenderEntity.Subject = emailSenderEntity.Subject.Replace(strToBeReplaced, Value.Trim());

                        }

                    }


                }
            }

            emailSentLog.Body = strBody;
            emailSentLog.EmailTo = emailSenderEntity.EmailTo;
            emailSentLog.CC = emailSenderEntity.CC;
            emailSentLog.BCC = emailSenderEntity.BCC;
            emailSentLog.Subject = emailSenderEntity.Subject;


            return emailSentLog;



        }

        public async Task<bool> SendPendingEmail(EmailSentLog emailSentLog)
        {
            MailMessage mailInfo = new MailMessage();

            bool isToEmail = false;

            if (!string.IsNullOrEmpty(emailSentLog.EmailTo))
            {
                string[] strTO = emailSentLog.EmailTo.Split(',');
                foreach (string str in strTO)
                {
                    if (!string.IsNullOrEmpty(str) && str.Contains("@"))
                    {
                        isToEmail = true;
                        mailInfo.To.Add(str);
                    }
                }
            }

            mailInfo.From = new MailAddress(_options.Value.From);
            mailInfo.Subject = emailSentLog.Subject;





            mailInfo.Body = emailSentLog.Body;
            mailInfo.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(emailSentLog.CC))
            {
                string[] strCC = emailSentLog.CC.Split(',');
                foreach (string str in strCC)
                {
                    if (!string.IsNullOrEmpty(str) && str.Contains("@"))
                    {
                        mailInfo.CC.Add(str);
                    }
                }
            }

            if (!string.IsNullOrEmpty(emailSentLog.BCC))
            {
                string[] strBCC = emailSentLog.BCC.Split(',');
                foreach (string str in strBCC)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        mailInfo.Bcc.Add(str);
                    }
                }
            }

            mailInfo.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();



            if (_options.Value.IsEmailToFolder)
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtp.PickupDirectoryLocation = Path.Combine(WWWROOT, _options.Value.SystemEmailDeliveryPath);
            }
            else
            {
                smtp.Host = EncryptionManager.Decrypt(_options.Value.SMTP_HOST);
                if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_SMTPUSERNAME))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(
                    EncryptionManager.Decrypt(_options.Value.SENDMAIL_SMTPUSERNAME),
                    EncryptionManager.Decrypt(_options.Value.SENDMAIL_SMTPUSERPASSWORD));
                }


                if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_PORT))
                {
                    smtp.Port = Convert.ToInt32(EncryptionManager.Decrypt(_options.Value.SENDMAIL_PORT));
                }

                if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_DEFAULTCREDENTIALS))
                {
                    smtp.UseDefaultCredentials = Convert.ToBoolean(EncryptionManager.Decrypt(_options.Value.SENDMAIL_DEFAULTCREDENTIALS));
                }


            }


            if (_options.Value.SendEmail && isToEmail)
            {
                await smtp.SendMailAsync(mailInfo);

            }

            return true;

        }

        private string ReadEmailTemplate(string FileName)
        {
            string strFileText = string.Empty;
            string strFilePath = string.Empty;
            string EmailTemplateFilePath = string.Empty;

            if (!string.IsNullOrEmpty(FileName))
            {
                EmailTemplateFilePath = Path.Combine(Directory.GetCurrentDirectory(), WWWROOT, _options.Value.EmailTemplateFolderPath, FileName);
                strFileText = System.IO.File.ReadAllText(EmailTemplateFilePath);
            }

            return strFileText;
        }

        /// <summary>
        /// Get ID FieldValue
        /// </summary>
        /// <param name="pstrFieldID"></param>       
        /// <param name="Entity"></param>
        /// <returns></returns>
        private string GetIDFieldValue(string pstrFieldID, Object Entity)
        {
            string strRetValue = string.Empty;
            try
            {
                if (Entity != null)
                {
                    if (!string.IsNullOrEmpty(pstrFieldID))
                    {
                        PropertyInfo pi = Entity.GetType().GetProperty(pstrFieldID);
                        if (pi != null)
                        {
                            string strPropType = pi.PropertyType.FullName;
                            if (pi.PropertyType.FullName.Contains("System.DateTime"))
                            {
                                strPropType = "System.DateTime";
                            }

                            switch (strPropType)
                            {
                                case "System.String":
                                    {
                                        strRetValue = Convert.ToString(pi.GetValue(Entity, null));
                                        break;
                                    }
                                case "System.DateTime":
                                    {
                                        strRetValue = pi.GetValue(Entity, null).ToString();
                                        if (!string.IsNullOrEmpty(strRetValue))
                                        {
                                            DateTime dtPropVal = Convert.ToDateTime(strRetValue);
                                            if (DateTime.MinValue.CompareTo(dtPropVal) < 0)
                                            {
                                                strRetValue = dtPropVal.ToString("MMM-dd-yyyy");
                                            }
                                            else
                                                strRetValue = "";
                                        }
                                        break;
                                    }


                                default:
                                    {
                                        strRetValue = Convert.ToString(pi.GetValue(Entity, null));
                                        break;
                                    }

                            }
                            return strRetValue;
                        }
                        return strRetValue;
                    }
                    else
                    {
                        return strRetValue;
                    }
                }
                else
                {
                    return strRetValue;
                }
            }
            catch
            {
                return strRetValue;
            }
        }

        //public string GetPrintCertficateContent(Assignments Entity)
        //{


        //    string strBody = ReadEmailTemplate(Entity.PrintCertficateFileName);

        //    Entity.httpURL = _uriService.GetBaseUri().ToString() + Convert.ToString(_optionsfilesystem.Value.AfterDomain) +
        //                _options.Value.EmailTemplateFolderPath + "/";
        //    string pattern = @"\<%.*?\%>";

        //    // Create a Regex  
        //    Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);

        //    // Get all matches  
        //    MatchCollection placeHolders = rg.Matches(strBody);
        //    // Print all matched authors  
        //    if (placeHolders != null)
        //    {
        //        for (int count = 0; count < placeHolders.Count; count++)
        //        {
        //            string strToBeReplaced = placeHolders[count].Value;

        //            string strmanipulatedString = strToBeReplaced.Replace("<%", "").Replace("%>", "");
        //            string[] ArrayClassProperty = strmanipulatedString.Split('.');
        //            if (ArrayClassProperty != null && ArrayClassProperty.Length > 1)
        //            {
        //                string strPropertyName = ArrayClassProperty[1];
        //                string Value = GetIDFieldValue(strPropertyName.Trim(), Entity);
        //                if (!string.IsNullOrEmpty(Value))
        //                {
        //                    strBody = strBody.Replace(strToBeReplaced, Value.Trim());

        //                }


        //            }


        //        }
        //    }



        //    return strBody;



        //}


        //public async Task<EmailSentLog> SendComplianceQuickLookInstantEmail(IEnumerable<CourseCompletionReport> lstData, string EmailTo, EmailSenderEntity emailSenderEntity)
        //{
        //    var lstModel = lstData.ToList();
        //    MailMessage mailInfo = new MailMessage();
        //    string toEmail = string.Empty;

        //    if (!string.IsNullOrEmpty(EmailTo))
        //    {
        //        string[] strTO = EmailTo.Split(';');
        //        foreach (string str in strTO)
        //        {
        //            if (!string.IsNullOrEmpty(str) && str.Contains("@"))
        //            {
        //                mailInfo.To.Add(str);
        //                toEmail += str + ";";
        //            }
        //        }
        //    }

        //    mailInfo.From = new MailAddress(_options.Value.From);
        //    mailInfo.Subject = emailSenderEntity.Subject;
        //    string strBody = ReadEmailTemplate(emailSenderEntity.HtmlFileName);
        //    EmailConfig emailConfig = _options.Value;

        //    #region CREATE BODY

        //    strBody = strBody.Replace("@@ReportURL", string.Concat(emailConfig.LoginURL, "?ReturnURL=Reports/TrackingAndReports?BackPage=NCPDPQuickLook"));

        //    string reportBody = "<table border='0' cellpadding='0' cellspacing='0' style='width:100%;border:1px solid #000;border-right:none;border-top:none;margin-bottom:15px'>";
        //    reportBody += "<thead>";

        //    #region SET HEADER

        //    List<string> lstAssignmentRules = lstModel.Count == 0 ? new List<string>() : lstModel[0].lstCompletionPercentage.OrderBy(x => x.OrderNo).ThenBy(x => x.BRShortName).Select(x => x.BusinessAssignmentId).Distinct().ToList();

        //    int colSpan = lstAssignmentRules.Count + 3;
        //    reportBody += "<tr>";
        //    reportBody += "<th colspan='" + colSpan + "' style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;text-align:left;border-top:1px solid #000; border-right:1px solid #000;text-align:center'>NCPDP Status Report: " + lstModel[0].GroupName + "</th>";
        //    reportBody += "</tr>";

        //    reportBody += "<tr>";
        //    reportBody += "<th style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;text-align:left;border-top:1px solid #000; border-right:1px solid #000'>First Name</th>";
        //    reportBody += "<th style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;text-align:left;border-top:1px solid #000; border-right:1px solid #000'>Last Name</th>";
        //    reportBody += "<th style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;text-align:left;border-top:1px solid #000; border-right:1px solid #000'>Group Name</th>";


        //    if (lstAssignmentRules != null)
        //    {
        //        foreach (string singleRule in lstAssignmentRules)
        //        {
        //            string ruleName = lstModel[0].lstCompletionPercentage.Where(x => x.BusinessAssignmentId == singleRule).FirstOrDefault().BRShortName;
        //            reportBody += "<th style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;text-align:left;border-top:1px solid #000; border-right:1px solid #000;white-space: normal;word-break:break-all;'>" + ruleName + "</th>";
        //        }
        //    }

        //    reportBody += "</tr>";

        //    #endregion


        //    #region SET PROGRESS

        //    reportBody += "<tr class='smallpadding'>";
        //    reportBody += "<th colspan='3' style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;border-right:1px solid #000;border-top:1px solid #000;'></th>";

        //    foreach (var singleRule in lstAssignmentRules)
        //    {
        //        var obj = lstModel[0].lstCompletionPercentage.Where(x => x.BusinessAssignmentId == singleRule).FirstOrDefault();
        //        reportBody += "<th style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;text-align:left;border-top:1px solid #000; border-right:1px solid #000'>" + obj.CompletedPercentage + "%<br />Completion </th>";
        //    }

        //    reportBody += "</tr>";

        //    //reportBody += "<tr class='smallpadding'>";
        //    //reportBody += "<th colspan='3' style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;border-right:1px solid #000;'></th>";
        //    //foreach (var singleRule in lstAssignmentRules)
        //    //{
        //    //    reportBody += "<th style='font-size:14px;padding:8px;color:#000;font-family:Calibri;font-weight:bold;text-align:center;border-top:1px solid #000; border-right:1px solid #000;'>Status</th>";
        //    //}

        //    //reportBody += "</tr>";

        //    #endregion

        //    reportBody += "</thead>";

        //    reportBody += "<tbody>";

        //    #region SET DATA

        //    string imagePath = _uriService.GetBaseUri().ToString() + Convert.ToString(_optionsfilesystem.Value.AfterDomain) + "Images";
        //    foreach (var singleUser in lstModel)
        //    {
        //        reportBody += "<tr>";
        //        reportBody += "<td style='font-size:12px;padding:5px;color:#000;font-family:Calibri;font-weight:normal;border-top:1px solid #000; border-right:1px solid #000;text-align:left'>" + singleUser.FirstName + "</td>";
        //        reportBody += "<td style='font-size:12px;padding:5px;color:#000;font-family:Calibri;font-weight:normal;border-top:1px solid #000; border-right:1px solid #000;text-align:left'>" + singleUser.LastName + "</td>";
        //        reportBody += " <td style='font-size:12px;padding:5px;color:#000;font-family:Calibri;font-weight:normal;border-top:1px solid #000; border-right:1px solid #000;text-align:left'>" + singleUser.GroupName + "</td>";

        //        foreach (string singleRule in lstAssignmentRules)
        //        {
        //            var obj = singleUser.lstAssignmentDetails.Where(x => x.UserId == singleUser.UserId && x.BusinessAssignmentId == singleRule).FirstOrDefault();
        //            if (obj?.IsAssigned == false)
        //            {
        //                reportBody += "<td style='font-size:12px;padding:5px;color:#000;font-family:Calibri;font-weight:normal;border-top:1px solid #000; border-right:1px solid #000;text-align:center'>&nbsp;</td>";
        //            }
        //            else if (obj?.IsCompleted == true)
        //            {
        //                reportBody += "<td style='font-size:12px;padding:5px;color:#000;font-family:Calibri;font-weight:normal;border-top:1px solid #000; border-right:1px solid #000;text-align:center'><img src='" + imagePath + "/Check_mark.png' /></td>";
        //            }
        //            else if (obj?.IsOverDue == true)
        //            {
        //                reportBody += "<td style='font-size:12px;padding:5px;color:#000;font-family:Calibri;font-weight:normal;border-top:1px solid #000; border-right:1px solid #000;text-align:center'><img src='" + imagePath + "/Overdue.png' /></td>";
        //            }
        //            else
        //            {
        //                reportBody += "<td style='font-size:12px;padding:5px;color:#000;font-family:Calibri;font-weight:normal;border-top:1px solid #000; border-right:1px solid #000;text-align:center'><img src='" + imagePath + "/Not_completed.png' /></td>";
        //            }

        //        }

        //        reportBody += "</tr>";
        //    }

        //    #endregion

        //    reportBody += "</tbody>";
        //    reportBody += "</table>";

        //    #region SET LEGEND

        //    reportBody += "<table style='width: auto;'>";
        //    reportBody += "<tr>";
        //    reportBody += "<td style='font-size:14px;padding:8px; padding-left:0; color:#000;font-family:Calibri;font-weight:bold;'>";
        //    reportBody += "Legends:";
        //    reportBody += "</td>";

        //    reportBody += "<td style='font-size:14px;padding:8px; padding-left:0;color:#000;font-family:Calibri;font-weight:bold;'>";

        //    reportBody += "<table style='border:1px solid #000;width: auto;'>";
        //    reportBody += "<tbody>";
        //    reportBody += "<tr>";

        //    reportBody += "<td style='font-size:14px;padding:5px; padding-left:0;color:#000;font-family:Calibri;font-weight:bold;'>Completed </td>";
        //    reportBody += "<td style='padding:0 15px 0px 0px'><img src='" + imagePath + "/Check_mark.png' /></td>";
        //    reportBody += "<td style='font-size:14px;padding:5px; padding-left:0;color:#000;font-family:Calibri;font-weight:bold;'>Not completed </td>";
        //    reportBody += "<td style='padding:0 15px 0px 0px'><img src='" + imagePath + "/Not_completed.png' /></td>";
        //    reportBody += "<td style='font-size:14px;padding:5px; padding-left:0;color:#000;font-family:Calibri;font-weight:bold;'>Overdue</td>";
        //    reportBody += "<td style='padding:0 15px 0px 0px'><img src='" + imagePath + "/Overdue.png' /></td>";

        //    reportBody += "</tr>";
        //    reportBody += "</tbody>";
        //    reportBody += "</table>";

        //    reportBody += "</td>";
        //    reportBody += "</tr>";
        //    reportBody += "</table>";

        //    #endregion


        //    #endregion

        //    strBody = strBody.Replace("@@REPORT_BODY", reportBody);
        //    mailInfo.Body = strBody;
        //    mailInfo.IsBodyHtml = true;
        //    string strSubject = mailInfo.Subject;


        //    mailInfo.IsBodyHtml = true;

        //    SmtpClient smtp = new SmtpClient();



        //    if (_options.Value.IsEmailToFolder)
        //    {
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
        //        smtp.PickupDirectoryLocation = Path.Combine(WWWROOT, _options.Value.SystemEmailDeliveryPath);
        //    }
        //    else
        //    {
        //        smtp.Host = EncryptionManager.Decrypt(_options.Value.SMTP_HOST);
        //        if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_SMTPUSERNAME))
        //        {
        //            smtp.Credentials = new System.Net.NetworkCredential(
        //            EncryptionManager.Decrypt(_options.Value.SENDMAIL_SMTPUSERNAME),
        //            EncryptionManager.Decrypt(_options.Value.SENDMAIL_SMTPUSERPASSWORD));
        //        }


        //        if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_PORT))
        //        {
        //            smtp.Port = Convert.ToInt32(EncryptionManager.Decrypt(_options.Value.SENDMAIL_PORT));
        //        }

        //        if (!string.IsNullOrEmpty(_options.Value.SENDMAIL_DEFAULTCREDENTIALS))
        //        {
        //            smtp.UseDefaultCredentials = Convert.ToBoolean(EncryptionManager.Decrypt(_options.Value.SENDMAIL_DEFAULTCREDENTIALS));
        //        }


        //    }


        //    if (_options.Value.SendEmail)
        //    {
        //        await smtp.SendMailAsync(mailInfo);

        //    }

        //    EmailSentLog emailSentLog = new EmailSentLog();
        //    emailSentLog.EmailTo = toEmail;
        //    emailSentLog.CC = "";
        //    emailSentLog.BCC = "";
        //    emailSentLog.Subject = strSubject;
        //    emailSentLog.Body = strBody;
        //    emailSentLog.IsEmailSent = true;

        //    return emailSentLog;



        //}


        //public string GetPrintCertficateContentDispatch(Assignments Entity)
        //{


        //    string strBody = ReadEmailTemplate(Entity.PrintCertficateFileName);

        //    Entity.httpURL = _uriService.GetBaseUri().ToString() + Convert.ToString(_optionsfilesystem.Value.AfterDomain) +
        //                _options.Value.EmailTemplateFolderPath + "/";
        //    string pattern = @"\<%.*?\%>";

        //    // Create a Regex  
        //    Regex rg = new Regex(pattern, RegexOptions.IgnoreCase);

        //    // Get all matches  
        //    MatchCollection placeHolders = rg.Matches(strBody);
        //    // Print all matched authors  
        //    if (placeHolders != null)
        //    {
        //        for (int count = 0; count < placeHolders.Count; count++)
        //        {
        //            string strToBeReplaced = placeHolders[count].Value;

        //            string strmanipulatedString = strToBeReplaced.Replace("<%", "").Replace("%>", "");
        //            string[] ArrayClassProperty = strmanipulatedString.Split('.');
        //            if (ArrayClassProperty != null && ArrayClassProperty.Length > 1)
        //            {
        //                string strPropertyName = ArrayClassProperty[1];
        //                string Value = GetIDFieldValue(strPropertyName.Trim(), Entity);
        //                if (!string.IsNullOrEmpty(Value))
        //                {
        //                    strBody = strBody.Replace(strToBeReplaced, Value.Trim());

        //                }


        //            }


        //        }
        //    }



        //    return strBody;



        //}


    }
}
