using System.Globalization;

namespace PersianCalendar.Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsiDateOnly(this DateTime value)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            string date = pc.GetYear(value) + "-" + pc.GetMonth(value).ToString("00") + "-" +
                   pc.GetDayOfMonth(value).ToString("00");

            return date;
        }
        public static string ToShamsiDateTime(this DateTime value)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            string date = pc.GetYear(value) + "-" + pc.GetMonth(value).ToString("00") + "-" +
                   pc.GetDayOfMonth(value).ToString("00") + $" {value.ToString("HH:mm")}";

            return date;
        }

        public static string GetShamsiDay(this DateTime value)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            string day = pc.GetDayOfMonth(value).ToString("00");

            return day;
        }

        public static string GetShamsiMonth(this DateTime value)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            string day = pc.GetMonth(value).ToString("00");

            return day;
        }

    }
}
