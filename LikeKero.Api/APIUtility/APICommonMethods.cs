using System;
using System.Collections.Generic;

namespace LikeKero.Api.APIUtility
{
    public class APICommonMethods
    {
        public static string GetOnlyDate(DateTime? dt)
        {
            return dt == null || dt == DateTime.MinValue ? "" : Convert.ToDateTime(dt).ToString("MMM-dd-yyyy");
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
    }
}
