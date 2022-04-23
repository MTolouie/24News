using System.Globalization;

namespace News_Web.Utilities
{
    public static class DateConvertor
    {
        public static string ToGregorian(this DateTime value)
        {
            var gc = new GregorianCalendar();
            return gc.GetYear(value) + "/" + gc.GetMonth(value).ToString("00") + "/" + gc.GetDayOfMonth(value).ToString("00");

        }
    }
}
