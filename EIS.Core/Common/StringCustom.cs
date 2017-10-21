using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EIS.Core.Common
{
    public class StringCustom
    {
        private static readonly StringCustom _instance = new StringCustom();
        public static StringCustom Instance
        {
            get
            {
                return _instance;
            }
        }
        public StringCustom()
        {

        }
        public string ClearWhitePlace(string input)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            input = input.Replace("\n", " ");
            input = input.Replace("  ", " ").Trim();
            input = regex.Replace(input, " ");
            input = Regex.Replace(input, @"^\s{1,}$", " ");

            return input;
        }

        public bool ConvertThangNamToKyQT(int thang, int nam, out string result)
        {
            result = "0000000";
            if (thang > 0 && thang < 13 && nam.ToString().Length == 4)
            {
                if (thang < 10)
                {
                    result = nam.ToString() + "0" + thang.ToString();
                    return true;
                }
                else
                {
                    result = nam.ToString() + thang.ToString();
                    return true;
                }
            }
            return false;
        }

        public bool TryParstMonthToQuarter(int month, out int quarter)
        {
            if (month == 1 || month == 2 || month == 3)
            {
                quarter = 1;
                return true;
            }
            else if (month == 4 || month == 5 || month == 6)
            {
                quarter = 2;
                return true;
            }
            else if (month == 7 || month == 8 || month == 9)
            {
                quarter = 3;
                return true;
            }
            else if (month == 10 || month == 11 || month == 12)
            {
                quarter = 4;
                return true;
            }
            else
            {
                quarter = -1;
                return false;
            }
        }

        public List<int> RenderQuaterToMonth(int quater)
        {
            if (quater == 1)
            {
                return new List<int>
                {
                    1, 2 ,3
                };
            }
            else if (quater == 2)
            {
                return new List<int>
                {
                    4, 5 ,6
                };
            }
            else if (quater == 3)
            {
                return new List<int>
                {
                    7, 8 , 9
                };
            }
            else if (quater == 4)
            {
                return new List<int>
                {
                    10, 11 ,12
                };
            }
            else
            {
                return new List<int>();
            }
        }

        public int RenderMonthAndYearToKyQT(int month, int year)
        {
            int result = 0;
            if (month < 10)
            {
                var str = year.ToString() + "0" + month.ToString();
                int.TryParse(str, out result);
            }
            else
            {
                var str = year.ToString() + month.ToString();
                int.TryParse(str, out result);
            }

            return result;
        }

        public string RenderQuyNumberToText(int quy)
        {
            if (quy == 1)
            {
                return "I";
            }
            else if (quy == 2)
            {
                return "II";
            }
            if (quy == 3)
            {
                return "III";
            }
            else
            {
                return "IV";
            }
        }
    }
}
