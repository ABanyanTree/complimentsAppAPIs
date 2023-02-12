using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LikeKero.Infra
{
    public class Utility
    {


        public static string DateDisplayFormat()
        {
            return "{0:dd-MMM-yyyy}";
        }

        public static string GeneratorUniqueId(string strPreFix)
        {
            string val = (strPreFix + System.Guid.NewGuid().ToString()).Replace("-", "");
            return val.ToUpper();
        }


        public static string GetUniqueKeyWithPrefix(string pPrefix, int pMaxSize)
        {
            char[] arrChars = new char[62];
            string str;
            str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            arrChars = str.ToCharArray();
            int size = pMaxSize;
            byte[] arrByteData = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(arrByteData);
            size = pMaxSize;
            arrByteData = new byte[size];
            crypto.GetNonZeroBytes(arrByteData);
            StringBuilder sbResult = new StringBuilder(size);
            foreach (byte b in arrByteData)
            {
                sbResult.Append(arrChars[b % (arrChars.Length - 1)]);
            }
            return pPrefix + sbResult.ToString();
        }

        public static int GenerateUniqueInteger()
        {
            Guid guid = Guid.NewGuid();
            Random random = new Random();
            int i = random.Next();
            return i;
        }

        public static string CreateRandomPassword(int length = 6)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public static string GetOnlyDate(DateTime? dt)
        {
            return dt == null ? "" : Convert.ToDateTime(dt).ToString("MMM-dd-yyyy");
        }

        public static string GetOnlyTime(DateTime? dt, bool is12Hrs = true)
        {
            return dt == null ? "" : is12Hrs == true ? Convert.ToDateTime(dt).ToString("hh:mm tt") : Convert.ToDateTime(dt).ToString("HH:mm");
        }

        public static string GetDateWithTime(DateTime? dt, bool is12Hrs = true)
        {
            return dt == null ? "" : is12Hrs == true ? Convert.ToDateTime(dt).ToString("dd-MMM-yyyy hh:mm tt") : Convert.ToDateTime(dt).ToString("dd-MMM-yyyy HH:mm");
        }

        public static string MaskCharForTestViewer(string strString)
        {
            if (!string.IsNullOrEmpty(strString))
            {
                strString = strString.Replace("'", "&#39;");
                strString = strString.Replace("\"", "&#34;");
                strString = string.Join(" ", Regex.Split(strString, @"(?:\r\n|\n|\r)"));
                return strString;
            }
            else
            {
                return strString;
            }
        }

        public static bool ConvertToBoolean(string val)
        {
            if (val == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region LMS

        public static string GetIPAddress()
        {
            string ip = "";

            return ip;
        }

        public static bool fGetBooleanValueForTestViewer(string sValue)
        {
            string sTmp = sValue.Trim().ToLower();
            int iTmp = 0;

            if (sTmp == "true")
                return true;
            else if (sTmp == "false")
                return false;
            else
            {
                if (Int32.TryParse(sValue, out iTmp))
                {
                    return Convert.ToBoolean(iTmp);
                }
                else
                {
                    return false;
                }
            }
        }


        public static int RandomNumberForTestViewer(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string fMaskCharForJS(string lString)
        {
            if (lString != "")
            {
                return lString.Replace("'", "\'");
            }
            else
            {
                return lString;
            }
        }

        #endregion


        public static string RemoveSpecialChars(string str)
        {
            str = Regex.Replace(str, @"[^0-9a-zA-Z]+", "_");
            return str;
        }
    }
}
