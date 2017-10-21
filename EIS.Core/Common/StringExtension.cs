using System.Text.RegularExpressions;

namespace EIS.Core.Common
{
    public static class StringExtension
    {
        /// <summary>
        /// upper and trim
        /// </summary>
        public static string ToUpperAndTrim(this string _input, bool trimgiua)
        {
            string _output = _input.ToUpper().Trim();
            if (trimgiua)
            {
                return Regex.Replace(_output, @"\s", "");
            }
            return _output;
        }
        /// <summary>
        /// thay hết các ký tự khoảng trắng đặc biệt thành dấu cách
        /// </summary>
        /// <param name="_input">string đầu vào</param>
        /// <returns>string sau khi trim và upper case</returns>
        public static string NormalizeString(this string _input)
        {
            string normalize = _input.ToUpper().Trim();
            normalize = Regex.Replace(normalize, @"\s", " ");
            return normalize;
        }
        /// <summary>
        /// longdv
        /// lấy ký tự ở cuối chuỗi
        /// </summary>
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length, length);
        }
    }
}
