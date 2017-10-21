using System;

namespace EIS.Core.Common
{
    public class StringUtilHelperGDDM_CSKCB
    {
        private static readonly StringUtilHelperGDDM_CSKCB _instance = new StringUtilHelperGDDM_CSKCB();

        private StringUtilHelperGDDM_CSKCB()
        {
        }

        public static StringUtilHelperGDDM_CSKCB Instance
        {
            get
            {
                return _instance;
            }
        }

        public DateTime? TryToConvertNgayApDungToDate(long? ngayApDung)
        {
            try
            {
                if (ngayApDung.HasValue)
                {
                    var str = ngayApDung.ToString().Trim();
                    var year = str.Substring(0, 4);
                    var month = str.Substring(4, 2);
                    var day = str.Substring(6, 2);
                    var hour = str.Substring(8, 2);
                    var minute = str.Substring(10, 2);

                    var currentYear = 0;
                    int.TryParse(year, out currentYear);

                    var currentMonth = 0;
                    int.TryParse(month, out currentMonth);

                    var currentDay = 0;
                    int.TryParse(day, out currentDay);

                    var r = new DateTime(currentYear, currentMonth, currentDay, 0, 0, 0);

                    return r;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public long ConvertStartNgayApDungToLong(DateTime ngayApDung)
        {
            var year = ngayApDung.Year;
            var month = ngayApDung.Month;
            var day = ngayApDung.Day;

            string m = month < 10 ? "0" + month.ToString() : month.ToString();
            string d = day < 10 ? "0" + day.ToString() : day.ToString();

            string res = year.ToString() + m + d + "00" + "00";
            long r = 0;
            long.TryParse(res, out r);
            return r;
        }

        public long ConvertEndNgayApDungToLong(DateTime ngayApDung)
        {
            var year = ngayApDung.Year;
            var month = ngayApDung.Month;
            var day = ngayApDung.Day;

            string m = month < 10 ? "0" + month.ToString() : month.ToString();
            string d = day < 10 ? "0" + day.ToString() : day.ToString();

            string res = year.ToString() + m + d + "99" + "99";
            long r = 0;
            long.TryParse(res, out r);
            return r;
        }
    }
}