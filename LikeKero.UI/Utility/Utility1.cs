using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LikeKero.UI.Utility
{
    public class Utility1
    {
        public enum ControlTypes
        {
            TextBoxControl = 0,
            DropDown = 1,
            Label = 2,
            INCONDITION = 3,
            CheckBox = 4

        }

        #region LMS
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

        #region "Email/Phone Format"
        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex re = new Regex(strRegex, RegexOptions.IgnoreCase);
            if ((!string.IsNullOrEmpty(inputEmail)) && re.IsMatch(inputEmail))
                return (true);
            else
                return (false);

        }

        public static bool IsValidPhone(string Phone)
        {
            if (string.IsNullOrEmpty(Phone))
            {
                return false;
            }
            else
            {
                return Phone.Except("-").Except(" ").Except("+").Except("(").Except(")").All(x => Char.IsNumber(x));
            }
        }

        public static bool IsOnlyCharacters(string str)
        {
            bool IsValid;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            else
            {
                IsValid = str.Except(" ").Except(".").All(x => Char.IsLetter(x));
            }
            return IsValid;
        }

        public static bool IsNullTrim(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(str.Trim()))
            {
                return true;
            }
            return false;
        }


        #endregion

        public static DateTime ConvertDateTimeToEST(DateTime dt)
        {
            var timeToConvert1 = (dt != default(DateTime)) ? dt : DateTime.Now;
            var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var ESTtargetTime = TimeZoneInfo.ConvertTime(timeToConvert1, est);
            return ESTtargetTime;
        }


        #region Function to get the startup route based on the instance
        public static string GetStartupRoute(string instance)
        {
            if (instance == "retail")
            {
                return "{controller=NblsRetail}/{action=Login}/{id?}";
            }
            else

            {
                return "{controller=Login}/{action=Login}/{id?}";
            }
        }
        #endregion
    }
}
