using System.Globalization;
using System.Text;

namespace EIS.Core.ServiceImp
{
    public class FilterUtils
    {
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
 
            "áàạảãâấầậẩẫăắằặẳẵ",
 
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
 
            "éèẹẻẽêếềệểễ",
 
            "ÉÈẸẺẼÊẾỀỆỂỄ",
 
            "óòọỏõôốồộổỗơớờợởỡ",
 
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
 
            "úùụủũưứừựửữ",
 
            "ÚÙỤỦŨƯỨỪỰỬỮ",
 
            "íìịỉĩ",
 
            "ÍÌỊỈĨ",
 
            "đ",
 
            "Đ",
 
            "ýỳỵỷỹ",
 
            "ÝỲỴỶỸ"
        };
        /// <summary>
        /// chuyển 1 đoạn string tiếng việt có dấu thành string không dấu
        /// </summary>
        /// <param name="str">string tiếng việt có dấu</param>
        /// <returns>string tiếng việt không dấu</returns>
        public static string TVKhongDau(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        public static string TVKhongDau2(string str)
        {
            var regex = new System.Text.RegularExpressions.Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string TVKhongDau3(string str)
        {
            string stFormD = str.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static System.Drawing.Rectangle WorkingArea
        {
            get { return System.Windows.Forms.Screen.PrimaryScreen.WorkingArea; }
        }

        public static string Group(object input, string sep = ".", int num = 3)
        {
            string outp = input.ToString();
            #region cach cu
            // them ky tu phan cach nhom vao giua nhom 3 so o xau ket qua
            //int bit = outp.Length - num;
            //while (bit > 0)
            //{
            //    outp = outp.Insert(bit, sep);
            //    bit -= num;
            //} 
            #endregion
            // them ky tu phan cach nhom vao giua nhom 3 so o xau ket qua
            for (int i = outp.Length - num; i > 0; i -= num)
            {
                outp = outp.Insert(i, sep);
            }
            return outp;
        }
    }
}
