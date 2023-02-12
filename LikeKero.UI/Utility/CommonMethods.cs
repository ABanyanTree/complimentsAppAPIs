using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LikeKero.UI.Utility
{
    public class CommonMethods
    {
        public static string GetOnlyDate(DateTime? dt)
        {
            return dt == null || dt == DateTime.MinValue ? "" : Convert.ToDateTime(dt).ToString("MMM-dd-yyyy");
            //return dt == null ? "" : Convert.ToDateTime(dt).ToString("dd-MMM-yyyy");
        }

        public static string GetOnlyTime(DateTime? dt, bool is12Hrs = true)
        {
            return dt == null || dt == DateTime.MinValue ? "" : is12Hrs == true ? Convert.ToDateTime(dt).ToString("hh:mm tt") : Convert.ToDateTime(dt).ToString("HH:mm");
        }

        public static string GetDateWithTime(DateTime? dt, bool is12Hrs = true)
        {
            //return dt == null ? "" : is12Hrs == true ? Convert.ToDateTime(dt).ToString("dd-MMM-yyyy hh:mm tt") : Convert.ToDateTime(dt).ToString("dd-MMM-yyyy HH:mm");
            return dt == null || dt == DateTime.MinValue ? "" : is12Hrs == true ? Convert.ToDateTime(dt).ToString("MMM-dd-yyyy hh:mm tt") : Convert.ToDateTime(dt).ToString("MMM-dd-yyyy HH:mm");
        }

        public static string GetDateWithTimeForCalendar(DateTime? dt, bool is12Hrs = true)
        {
            //return dt == null ? "" : is12Hrs == true ? Convert.ToDateTime(dt).ToString("dd-MMM-yyyy hh:mm tt") : Convert.ToDateTime(dt).ToString("dd-MMM-yyyy HH:mm");
            return dt == null || dt == DateTime.MinValue ? "" : is12Hrs == true ? Convert.ToDateTime(dt).ToString("yyyy-MM-dd") : Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
        }

        public static string GetCourseDuration(int CourseDurationHH, int CourseDurationMM)
        {

            if (CourseDurationHH == 0)
            {
                return (CourseDurationMM < 9 ? "0" : "") + CourseDurationMM + " mins";
            }
            else if (CourseDurationMM == 0)
            {
                return (CourseDurationHH < 9 ? "0" : "") + CourseDurationHH + " hr";
            }
            else
            {
                return (CourseDurationHH < 9 ? "0" : "") + CourseDurationHH + " hr " + (CourseDurationMM < 9 ? "0" : "") + CourseDurationMM + " mins";
            }



        }

        public static string TruncateString(string value, int maxChars)
        {
            return value == null ? "" : (value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...");
        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip", "application/x-zip-compressed"},
                {".mp4","video/mp4" }
            };
        }

        public static string FormatOIGDate(string InputDate)
        {
            try
            {
                return GetOnlyDate(DateTime.ParseExact(InputDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                return string.IsNullOrEmpty(InputDate) ? "-" : InputDate;
            }
        }

        public static string RemoveSpecialChars(string str)
        {
            str = Regex.Replace(str, @"[^0-9a-zA-Z]+", "_");
            return str;
        }
    }
}
