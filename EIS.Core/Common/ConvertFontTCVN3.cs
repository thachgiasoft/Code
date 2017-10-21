using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Common
{
    public class ConvertFontTCVN3
    {
        private static char[] tcvnchars = {
        'µ', '¸', '¶', '·', '¹',
        '¨', '»', '¾', '¼', '½', 'Æ',
        '©', 'Ç', 'Ê', 'È', 'É', 'Ë',
        '®', 'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
        'ª', 'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
        '×', 'Ý', 'Ø', 'Ü', 'Þ',
        'ß', 'ã', 'á', 'â', 'ä',
        '«', 'å', 'è', 'æ', 'ç', 'é',
        '¬', 'ê', 'í', 'ë', 'ì', 'î',
        'ï', 'ó', 'ñ', 'ò', 'ô', '­', 'õ', 'ø', 'ö', '÷', 'ù',
        'ú', 'ý', 'û', 'ü', 'þ',
        '¡', '¢', '§', '£', '¤', '¥', '¦'
    };

        private static char[] unichars = {
        'à', 'á', 'ả', 'ã', 'ạ',
        'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
        'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
        'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
        'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
        'ì', 'í', 'ỉ', 'ĩ', 'ị',
        'ò', 'ó', 'ỏ', 'õ', 'ọ',
        'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
        'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
        'ù', 'ú', 'ủ', 'ũ', 'ụ',
        'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
        'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
        'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'
    };

        private static char[] convertTable;

        static ConvertFontTCVN3()
        {
            convertTable = new char[256];
            for (int i = 0; i < 256; i++)
                convertTable[i] = (char)i;
            for (int i = 0; i < tcvnchars.Length; i++)
                convertTable[tcvnchars[i]] = unichars[i];
        }

        public static string TCVN3ToUnicode(string value)
        {
            char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                if (chars[i] < (char)256)
                    chars[i] = convertTable[chars[i]];
            return new string(chars);
        }

        /// <summary>
        /// Descriptions: Convert Font from Unicode To VN3
        /// Mapping 1-1 theo chỉ số nếu tìm thấy
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnicodeToTCVN3(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            //Chuyển sang array character
            char[] chars = value.ToCharArray();

            //Mapping 1-1: unichar = vn3char
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];

                //Tìm chỉ số của c trong unichars
                int index = Array.IndexOf(unichars, c);

                // Nếu index > -1 (tìm thấy) thì gán chars[i] = vn3char[index]
                if (index > -1)
                {
                    char temp = tcvnchars[index];
                    chars[i] = temp;
                }

            }

            return new string(chars);
        }


        public static string TTPD0 = UnicodeToTCVN3(TrangThaiPheDuyet.ChoDuyetCapMoi);
        public static string TTPD1 = UnicodeToTCVN3(TrangThaiPheDuyet.DuyetDangKyGrid);
        public static string TTPD2 = UnicodeToTCVN3(TrangThaiPheDuyet.TuChoiDuyetCapMoi);
        //public static string TTPD5 = UnicodeToTCVN3("Chờ duyệt ngưng");
        public static string TTPD6 = UnicodeToTCVN3(TrangThaiPheDuyet.NgungHopDong);
        //public static string TTPD7 = UnicodeToTCVN3("Từ chối duyệt ngưng");
        public static string TTPD8 = UnicodeToTCVN3(TrangThaiPheDuyet.ChoDuyetDieuChinhThongTin);
        public static string TTPD9 = UnicodeToTCVN3(TrangThaiPheDuyet.DuyetDieuChinhThongTin);
        public static string TTPD10 = UnicodeToTCVN3(TrangThaiPheDuyet.TuChoiDuyetDieuChinhThongTin);
        public static string TTPD11 = UnicodeToTCVN3(TrangThaiPheDuyet.ChoDuyetActiveMin);
        public static string TTPD12 = UnicodeToTCVN3(TrangThaiPheDuyet.HuyDuyetActiveMin);
        public static string TTPD13 = UnicodeToTCVN3(TrangThaiPheDuyet.ChoDuyetActiveMax);
        public static string TTPD14 = UnicodeToTCVN3(TrangThaiPheDuyet.HuyDuyetActiveMax);
        public static string TTPD4 = UnicodeToTCVN3(TrangThaiPheDuyet.ThemMoi);

        public static string KBV1 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien01);
        public static string KBV2 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien02);
        public static string KBV3 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien03);
        public static string KBV4 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien04);
        public static string KBV5 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien05);
        public static string KBV6 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien06);
        public static string KBV7 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien07);
        public static string KBV8 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien08);
        public static string KBV9 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien09);
        public static string KBV10 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien10);
        public static string KBV11 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien11);
        public static string KBV12 = UnicodeToTCVN3(DmCsKcbTextHelper.KieuBenhVien12);

        public static string TT0 = UnicodeToTCVN3(DmCsKcbTextHelper.TrangThai0);
        public static string TT1 = UnicodeToTCVN3(DmCsKcbTextHelper.TrangThai01);
        public static string TT2 = UnicodeToTCVN3(DmCsKcbTextHelper.TrangThai02);
        public static string TT3 = UnicodeToTCVN3(DmCsKcbTextHelper.TrangThai03);
        public static string TT4 = UnicodeToTCVN3(DmCsKcbTextHelper.TrangThai04);

        public static string KHAMCHUABENH = UnicodeToTCVN3(DmCsKcbTextHelper.KhamChuaBenh02);

        public static string HINHTHUC1 = UnicodeToTCVN3(DmCsKcbTextHelper.HinhThucThanhToan01);
        public static string HINHTHUC2 = UnicodeToTCVN3(DmCsKcbTextHelper.HinhThucThanhToan02);

        public static string KCBBD0 = UnicodeToTCVN3(DmCsKcbTextHelper.DangKyKcbBanDau0);
        public static string KCBBD1 = UnicodeToTCVN3(DmCsKcbTextHelper.DangKyKcbBanDau01);
        public static string KCBBD2 = UnicodeToTCVN3(DmCsKcbTextHelper.DangKyKcbBanDau02);

        public static string HANG0 = UnicodeToTCVN3(DmCsKcbTextHelper.HangBenhVien0);
        public static string HANG5 = UnicodeToTCVN3(DmCsKcbTextHelper.HangBenhVien05);
        public static string HANG = UnicodeToTCVN3("Hạng");

        public static string TUYENTUYENCMKTTW = UnicodeToTCVN3(DmCsKcbTextHelper.TuyenCmkt01);
        public static string TUYENTUYENCMKTTINH = UnicodeToTCVN3(DmCsKcbTextHelper.TuyenCmkt02);
        public static string TUYENTUYENCMKTHUYEN = UnicodeToTCVN3(DmCsKcbTextHelper.TuyenCmkt03);
        public static string TUYENTUYENCMKTXA = UnicodeToTCVN3(DmCsKcbTextHelper.TuyenCmkt04);


        public static string CHUAPHANTUYEN = UnicodeToTCVN3("Chưa phân tuyến");
        public static string TCHU = UnicodeToTCVN3(DmCsKcbTextHelper.TuChu01);
        public static string KTUCHU = UnicodeToTCVN3(DmCsKcbTextHelper.TuChu02);

        public static string YES = UnicodeToTCVN3("Có");
        public static string NO = UnicodeToTCVN3("Không");

        public static string LOAIBENHVIENCONGLAP = UnicodeToTCVN3(DmCsKcbTextHelper.LoaiBenhVien01);
        public static string LOAIBENHVIENNGOAICONGLAP = UnicodeToTCVN3(DmCsKcbTextHelper.LoaiBenhVien02);

        public static string LOAI_DONVICHUQUAN01 = UnicodeToTCVN3(DmCsKcbTextHelper.LoaiDonViChuQuan01);
        public static string LOAI_DONVICHUQUAN02 = UnicodeToTCVN3(DmCsKcbTextHelper.LoaiDonViChuQuan02);
        public static string LOAI_DONVICHUQUAN03 = UnicodeToTCVN3(DmCsKcbTextHelper.LoaiDonViChuQuan03);
        public static string LOAI_DONVICHUQUAN04 = UnicodeToTCVN3(DmCsKcbTextHelper.LoaiDonViChuQuan04);

    }
}