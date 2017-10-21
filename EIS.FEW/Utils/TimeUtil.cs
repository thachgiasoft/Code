using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIS.FEW.Utils
{
    public class TimeUtil
    {
        public static List<int> GetListForYear()
        {
            var year = DateTime.Now.Year;
            List<int> result = new List<int>();
            for(var i = 0; i < 5; i++)
            {
                result.Add(year);
                year -= 1;
            }
            return result;
        }

        public static List<int> GetListForMonth()
        {
            List<int> result = new List<int>();
            for(int i = 0; i < 12; i++)
            {
                result.Add(i + 1);
            }
            return result;
        }

        public static int CurrentMonthIndex
        {
            get
            {
                return DateTime.Now.Month - 1;
            }
        }
    }
}