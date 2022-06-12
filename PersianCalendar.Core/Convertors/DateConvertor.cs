namespace PersianCalendar.Core.Convertors
{
    public static class DateConvertor
    {
        private static readonly System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
        private static readonly System.Globalization.GregorianCalendar gregorianCalendar = new System.Globalization.GregorianCalendar();
        private static readonly System.Globalization.HijriCalendar hijriCalendar = new System.Globalization.HijriCalendar();

        #region Shamsi
        public static string ToShamsiDateOnly(this DateTime value)
        {
            return
                $"{persianCalendar.GetYear(value)}/" +
                $"{persianCalendar.GetMonth(value)}/" +
                $"{persianCalendar.GetDayOfMonth(value)}";
        }
        public static string ToShamsiDateTime(this DateTime value)
        {
            return
                $"{ToShamsiDateOnly(value)}  " +
                $"{value.ToString("HH:mm:ss")}";
        }

        public static string GetShamsiDayOfMonth(this DateTime value)
        {
            return persianCalendar.GetDayOfMonth(value).ToString("00");
        }

        public static string GetShamsiMonth(this DateTime value)
        {
            return persianCalendar.GetMonth(value).ToString("00");
        }
        #endregion

        #region Gregorian
        public static string ToGregorianDateOnly(this DateTime value)
        {
            return
               $"{gregorianCalendar.GetYear(value)}/" +
               $"{gregorianCalendar.GetMonth(value)}/" +
               $"{gregorianCalendar.GetDayOfMonth(value)}";
        }

        public static string GetGregorianDayOfMonth(this DateTime value)
        {
            return gregorianCalendar.GetDayOfMonth(value).ToString("00");
        }

        public static string GetGregorianMonth(this DateTime value)
        {
            return gregorianCalendar.GetMonth(value).ToString("00");
        }
        #endregion

        #region Hijri
        public static string ToHijriDateOnly(this DateTime value)
        {
            return
               $"{hijriCalendar.GetYear(value)}/" +
               $"{hijriCalendar.GetMonth(value)}/" +
               $"{hijriCalendar.GetDayOfMonth(value)}";
        }

        public static string GetHijriDayOfMonth(this DateTime value)
        {
            return hijriCalendar.GetDayOfMonth(value).ToString("00");
        }

        public static string GetHijriMonth(this DateTime value)
        {
            return hijriCalendar.GetMonth(value).ToString("00");
        }
        #endregion
    }
}
