using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EIS.Core.Common
{

    public class Common
    {
        #region "convert tien tu so sang chu"

        public static string moneyToString(decimal number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3)
                        j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0))
                            str = "lẻ " + str;
                        if (chuc == 1)
                            str = "mười " + str;
                        if (chuc > 1)
                            str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0)
                        break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0))
                            str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm)
                str = "Âm " + str;
            if (str == "không ")
            {
                return "";
            }
            return str + "đồng";
        }
        public static string moneyToString(double number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = "";
            bool booAm = false;
            double decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDouble(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3)
                        j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        //str = "một " + str;
                        str = "mốt " + str;//longdv
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0))
                            str = "lẻ " + str;
                        if (chuc == 1)
                            str = "mười " + str;
                        if (chuc > 1)
                            str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0)
                        break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0))
                            str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm)
                str = "Âm " + str;
            if (str == "không")
            {
                return "";
            }
            return !string.IsNullOrEmpty(str) ? string.Format("{0}đồng.", ConvertInHoa(str)) : string.Format("{0}đồng.", str);
        }
        #endregion
        public static string SplitWordBySpace(string text, int numberofword, char separator)
        {
            string split = text;
            int countspace = text.Split(separator).Length - 1;
            if (countspace < numberofword)
            {
                return split;
            }
            int index = -1;
            for (int i = 0; i < numberofword; i++)
            {
                index = text.IndexOf(separator, index + 1);
            }
            return split.Substring(0, index);
        }
        private static string ConvertInHoa(string txt)
        {
            var convert_tien = txt.TrimStart();
            char[] array = convert_tien.ToCharArray();
            string txtone = array[0].ToString().ToUpper();
            return string.Format("{0}{1}", txtone, convert_tien.Substring(1, convert_tien.Length - 1));
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        /// <summary>
        /// Duyệt = 1; Hủy Duyệt = 2; Khôi phục  = 3; Ánh xạ = 4; Hủy ánh xạ = 5
        /// </summary>
        public enum eTacVu
        {
            Duyet = 1,
            HuyDuyet = 2,
            KhoiPhuc = 3,
            AnhXa = 4,
            HuyAnhXa = 5,
        }
        public enum DM_THUOC_KK_LoaiKeKhai
        {
            [Description("4: Thuốc kê khai trong nước")]
            KK_TrongNuoc = 4,
            [Description("3: Thuốc kê khai ngoài nước")]
            KK_NgoaiNuoc = 3,
            [Description("2: Thuốc kê khai lại trong nước")]
            KKL_TrongNuoc = 2,
            [Description("1: Thuốc kê khai lại ngoài nước")]
            KKL_NgoaiNuoc = 1
        }
        public enum DMCSKCB_TuChu
        {
            KhongTuChu = 0,
            TuChu
        }
        public enum DMCSKCB_TrangThai
        {
            [Description("0: Không có hiệu lực.")]
            KhongHieuLuc = 0,
            [Description("1: Có hiệu lực.")]
            CoHieuLuc,
            [Description("2: Tạm ngưng hợp đồng.")]
            TamNgungHD,
            [Description("3: Đã xóa.")]
            Daxoa,
        }

        public enum KQGD_CBB
        {
            [Description("99: Tất cả")]
            TatCa = 99,
            [Description("0: Chưa giám định")]
            ChuaGiamDinh = 0,
            [Description("1: Cảnh báo")]
            CanhBao = 1,
            [Description("2: Chấp nhận chủ động")]
            ChapNhanChuDong = 2,
            [Description("3: Chấp nhận tự động")]
            ChapNhanTuDong = 3,
            [Description("4: Từ chối chủ động")]
            TuChoiChuDong = 4,
            [Description("5: Từ chối tự động")]
            TuChoiTuDong = 5
        }
        public enum LoaiUser
        {
            Quantri = 0,
            TW = 1,
            Tinh = 2,
            Huyen = 3
        }
        /// <summary>
        /// Tổng hợp = 0; Tân dược = 1; Chế phẩm = 2; Vị thuốc = 3; Phóng xạ = 4; Vật tư = 5; Dịch vụ = 6, Tân dược tự bào chế = 7, Chế phẩm tự bào chế = 8
        /// </summary>
        public enum GoiThau_Loai
        {
            [Description("0 : Tổng hợp")]
            TongHop = 0,
            [Description("1 : Tân dược")]
            TanDuoc,
            [Description("2 : Chế phẩm")]
            ChePham,
            [Description("3 : Vị thuốc")]
            ViThuoc,
            [Description("4 : Phóng xạ")]
            PhongXa,
            [Description("5 : Vật tư")]
            VatTu,
            [Description("6 : Dịch vụ")]
            DichVu,
            [Description("7 : Tân dược tự bào chế")]
            TanDuocTuBaoChe,
            [Description("8 : Chế phẩm tự bào chế")]
            ChePhamTuBaoChe
        }
        public enum ThuocThau_Loai_IS5173
        {
            [Description("0 : Tổng hợp")]
            TongHop = 0,
            [Description("1 : Tân dược")]
            TanDuoc,
            [Description("2 : Chế phẩm")]
            ChePham,
            [Description("3 : Vị thuốc")]
            ViThuoc,
            [Description("4 : Phóng xạ")]
            PhongXa,
            [Description("5 : Tân dược tự bào chế")]
            TanDuocTuBaoChe,
            [Description("6 : Chế phẩm tự bào chế")]
            ChePhamTuBaoChe
        }

        /// <summary>
        /// Thuốc thầu  = 1; Giá dịch vụ = 2; Vật tư thầu = 3;
        /// </summary>
        public enum GoiThau_Nhom
        {
            [Description("1: Thuốc thầu")]
            ThuocThau = 1,
            [Description("2: Dịch vụ")]
            GiaDichVu,
            [Description("3: Vật tư thầu")]
            VatTuThau
        }
        /// <summary>
        /// Mới = 0;  Hoàn tất = 1;  Đang bổ sung = 2
        /// </summary>
        public enum GoiThau_TrangThai
        {
            Moi = 0,
            HoanTat,
            DangBoSung
        }

        /// <summary>
        /// NgoaiThau = 0; Thầu tập chung = 1; Thầu riêng lẻ = 2
        /// </summary>
        public enum GoiThau_HinhThuc
        {
            NgoaiThau = 0,
            ThauTapChung,
            ThauRiengLe
        }
        /// <summary>
        /// Tân dược = 1; Chế phẩm = 2; Vị thuốc = 3; Phóng xạ = 4
        /// </summary>
        public enum Thuoc_Loai
        {
            TanDuoc = 1,
            ChePham,
            ViThuoc,
            PhongXa
        }
        public enum MaLoaiThuoc
        {
            TanDuoc = 1,
            Phongxa,
            ChePham,
            ViThuoc
        }
        public enum ShowerrorCosoKcb
        {
            success = 0,
            errorFtp,
            errorTrungMa,
            errorKhongDungTrangThai,
            errorLoiTryCatch,
            errorLoiInportCsVc,
            errorLoiInportNvyt,
            errorLoiAddCsvcLoaiGia,
            errorHetPhienLamViec,
            errorChuyenDuyet,
            errorDongY,
            errorTuChoi,
            errorGhiChuQuaGioiHan,
            errorMaqua1000,
            errorKhongTonTaiCoSo
        }

        public enum errorInport
        {
            dinhdangfile = 1,
            loicskcb,
            thanhcong,
            xuatfileloi,
            kimportduoc,
            error_banghi,
            loi_file_format,
            checktrungbanghikhithemmoi,
            loiFileName
        }

        /// <summary>
        /// Chờ duyệt = 0; 
        /// Đã duyệt = 1;
        /// Từ chối duyệt = 2;
        /// Mới = 4;
        /// Chờ duyệt tạm ngưng = 5;
        /// Đã duyệt tạm ngưng = 6;
        /// Từ chối duyệt tạm ngưng = 7;
        /// Chờ duyệt ngưng = 8;
        /// Đã duyệt ngưng = 9;
        /// Từ chối duyệt ngưng = 10;
        /// Chờ duyệt active lại = 11;
        /// Từ chối duyệt active lại = 12;
        /// Tất cả = 99;
        /// </summary>
        public enum COSOKCB_TrangThai
        {
            Moi = 4,
            ChoDuyetDangKy = 0,
            DaDuyetDangKy = 1,
            DuyetActivate = 3,
            NgungHopDong = 6,
            ChoDuyetActiveBe = 11,
            DongYDuyetActiveBe = 15,
            ChoduyetactivehdLon = 13,
            DongYDuyetActivateLon = 16,
            TuChoiDuyetDangKy = 2,
            TuChoiDuyetActiveBe = 12,
            TuchoiduyetactivehdLon = 14,

            ChoDuyetDieuChinhTt = 8,
            DaDuyetDieuChinhTt = 9,
            TuChoiDuyetDieuChinhTt = 10,
            TatCa = 99
        }


        public enum COSOKCB_TrangThaiLuoi
        {
            ChoDuyetDangKy = 0,
            DaDuyetDangKy = 1,
            TuChoiDuyetDangKy = 2,
            Moi = 4,
            //ChoDuyetTamNgung = 5,
            NgungHopDong = 6,
            //TuChoiDuyetTamNgung = 7,
            ChoDuyetDieuChinhTt = 8,
            DaDuyetDieuChinhTt = 9,
            TuChoiDuyetDieuChinhTt = 10,
            ChoDuyetActiveBe = 11,
            TuChoiDuyetActiveBe = 12,
            ChoduyetactivehdLon = 13,
            TuchoiduyetactivehdLon = 14,
        }

        /*  1. Khám bệnh (Kiểu khai báo theo Khoa)
            2. Điều trị ngoại trú (Kiểu khai báo theo Khoa)
            3. Thận nhân tạo (Kiểu khai báo theo Khoa)
            4. Ngoại trú (Kiểu khai báo theo Nội trú/Ngoại trú)
            5. Nội trú (Kiểu khai báo theo Nội trú/Ngoại trú).
         */

        public enum coSoKcb_LOAIBENHVIEN
        {
            congLap = 1,
            ngoaiCongLap = 2
        }

        public enum coSoKCb_HANGBENHVIEN
        {
            hangDacBiet = 0,
            hang1 = 1,
            hang2 = 2,
            hang3 = 3,
            hang4 = 4,
            chuaXepHang = 5
        }

        public enum coSoKcb_DKKCBBD
        {
            khongNhanDK_KCBBĐ = 0,
            nhanDK_KCBBĐNT = 1,
            nhanDK_Noi_NgoaiTinh = 2,
        }

        public enum coSoKcb_KIEUBV
        {
            bv_BenhVienDaKhoa = 1,
            pk_PhongKhamDaKhoa = 2,
            bbvcssk_Banbaovechamsocsuckhoe = 3,
            ttytck_trungtamytechuyenkhoa = 4,
            ytcq_Ytecoquan = 5,
            ttyt_Trungtamyte = 6,
            tyt = 7,
            bx_Benhxa = 8,
            nhs_Nhahosinh = 9,
            bv_BenhvienYHCT = 10,
            bv_Benhvienchuyenkhoa = 11,
            pk_Phongkhamchuyenkhoa = 12,
        }

        public enum coSoKcb_HINHTHUCTT
        {
            theoPhiDichVu = 1,
            theoDinhSuat = 2
        }

        public enum coSoKcb_KCB
        {
            khongKCB = 0,
            coKCb = 1
        }

        public enum coSoKcb_TUCHU
        {
            tuChu = 1,
            khongTuChu = 0
        }

        public enum coSoKcb_LOAIHOPDONG
        {
            kcb_BHYT_NoiTru = 0,
            kcb_BHYT_NgoaiTru = 1,
            kcb_BHYT_NoiTru_NgoaiTru = 2,
            thanhtoantructiep = 3
        }

        public enum TenLoaiKhamBenh
        {
            KhamBenh = 1,
            DieuTriNgoaiTru = 2,
            ThanNhanTao = 3,
            NgoaiTru = 4,
            NoiTru = 5
        }


        public enum hienThiBangGhi
        {
            TatCa = 1,
            Loi,
            HoanThanh

        }
        public enum VaiTroND
        {
            TrungUong = 1,
            Tinh,
            TruongNhom,
            GDVCoSo
        }

        public enum VaiTroNDKhongCoQuyen
        {
            KhongCoQuyen = 0,
        }

        public enum VaiTroND_KDL
        {
            LanhDaoTinh = 2,
            TruongPhongTinh = 1,
            NhanVien = 0
        }
        public enum eLyDoVaoVien
        {
            dungTuyen = 1,
            capCuu = 2,
            traiTuyen = 3
        }
        public enum thu7chunhat
        {
            thu7 = 1,
            chunhat = 2,
            // traiTuyen = 3
        }
        public enum eLyDoVaoVienShow
        {
            kcbbd = eLyDoVaoVien.dungTuyen - 1,
            chuyenDen = eLyDoVaoVien.dungTuyen,
            capCuu = eLyDoVaoVien.capCuu,
            traiTuyen = eLyDoVaoVien.traiTuyen,
        }
        public enum eTinhTrangRaVien
        {
            raVien = 1,
            chuyenVien = 2,
            tronVien = 3,
            xinRaVien = 4
        }
        public enum ephannhomdtbhyt
        {
            nhom1 = 1,
            nhom2 = 2,
            nhom3 = 3,
            nhom4 = 4,
            nhom5 = 5,
            nhom6 = 6,
            nhom7 = 7,
            nhom8 = 8,
            nhom9 = 9

        }
        public enum eLoaiKCB
        {
            all = 0,
            ngoaiTru = 1,
            noiTru = 2
        }
        public enum eLoaiKCBTL
        {
            all = 0,
            ngoaiTru = 1,
            kcb = 2,
            noitru = 3,
        }
        public enum ephamvi
        {
            all = 0,
            noivien = 1,
            lienvientrongtinh = 2,
            lienvienngoaitinh = 3
        }
        public enum eLoaiCanhBao
        {
            canhBao = 1,
            xuatToan = 2
        }
        public enum eHinhThuc
        {
            ngoaiTru = 2,
            noiTru = 3
        }
        public enum eLuaChonIn
        {
            tatCa = 1,
            noiTinhBD = 2,
            noiTinhDen = 3,
            ngoaiTinhDen = 4
        }
        public enum eLayMau
        {
            ngay = 1,
            hoSo = 2,
            khoaNgay = 3,
            khoa = 4
        }
        public enum eMauTT
        {
            mauA = 1,
            mauB = 2,
            lan2 = 3
        }
        public enum emau192021
        {
            mauA = 1,
            mauB = 2,
        }
        public enum eKieuBC
        {
            chiTiet = 1,
            theoBenhVien = 2,
            theoTinh = 3,
            theoTTYT = 4
        }
        public enum eLoaiBC
        {
            ngoaiTru = 1,
            noiTru = 2,
            noiTruCoCongKham = 3
        }
        public enum eTuyenKCB
        {
            tatCa = 1,
            dungTuyen = 2,
            traiTuyen = 3,
            capCuu = 4,
            thongTuyen = 5
        }
        public enum eBaoCaoEmpty
        {
            bvDeNghi = 1,
            giamDinhDuyet = 2
        }

        public enum Number
        {
            all = -1,
            zero = 0,
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            night = 9
        }
        public enum eThongKeRP
        {
            doiTuong = 1,
            loaiKCB = 2,
            tuyenBN = 3
        }
        public enum eMauIn
        {
            mauBenhVien = 1,
            mauTinh = 2
        }
        public enum eLoaiHinhKCB
        {
            tatCa = 1,
            ngoaiTru = 2,
            noiTru = 3
        }
        public enum eGioiTinh
        {
            tatCa = -1,
            Nu = 2,
            Nam = 1

        }


        public enum eLoaiBaoCao
        {
            mau01 = 0,
            mauTonghop = 1,
            mauChiTiet = 2
        }

        public enum eGioiTinhssss
        {
            tatCa = 3,
            Nu = 2,
            Nam = 1

        }
        public enum eLoaiGiamDinh
        {
            MoiTao = 0,
            DangGiamDinh = 1,
            DaGiamDinh = 2,
            GiamDinhLai = 3,
            ThuHoi = 4,
            dangxacnhan,
            danghuyxacnhan,
        }
        public enum eLoaiHS
        {
            tatca = 0,
            NgoaiMau = 1,
            TrongMau = 2
        }
        public enum eTCTT
        {
            tatca = 0,
            thuoc = 1,
            dvdk = 2,
            vtyt = 3
        }
        public enum eGioi_Tinh
        {

            Nu = 0,
            Nam = 1

        }
        public enum eTrangthai
        {
            Chuaxl = 0,
            Daxl = 1
        }
        public enum eTrunglap
        {
            motphan = 0,
            toanbo = 1
            //toanBo = 0,
            //ngoaiNgoai = 1,
            //ngoaiNoi = 2,
            //noiNoi = 3
        }
        public enum eTuyenBv
        {
            trungUong = 1,
            tinh = 2,
            huyen = 3,
            xa = 4
        }
        public enum eChidinh
        {
            muc1 = 1,
            muc2 = 2,
            muc3 = 3,
            muc4 = 4,
        }

        public enum eTuongtac
        {
            muc1 = 1,
            muc2 = 2,
            muc3 = 3,
            muc4 = 4,
        }

        public enum eNguon
        {
            who = 1,
            nhasx = 2,
            boyt = 3,
            nguonkhac = 4,
        }
        public enum Status
        {
            dangxuly = 0,
            daxuly = 1,
            tuchoi = 2
        }

        public enum eTrangThaiBC
        {
            moi = 0,
            daGui = 1,
            biTraLai = 2,
            daDuyet = 3,
            daGuiTW = 4
        }

        public enum eTrangThaiBCBHYT12
        {
            TatCa = -1,
            MoiGuiKHTC = 0,
            KHTCTraLai = 1,
            KHTCDaDuyet = 2,
            TWTraLai = 3,
            TWDaDuyet = 4

        }
        public enum eLOAIBN_CHITIET
        {
            noikcbbd = 1,
            noiden,
            ngoaiden,
        }

        public enum eNhomQuyTac
        {
            [Description("-1: Tất cả")]
            TatCa = -1,
            [Description("0: Quy tắc thẻ")]
            QuyTacThe = 0,
            [Description("1: Quy tắc mức hưởng")]
            QuyTacMucHuong = 1,
            [Description("2: Quy tắc về thuốc")]
            QuyTacVeThuoc = 2,
            [Description("3: Quy tắc VTYT")]
            QuyTacVTYT = 3,
            [Description("4: Quy tắc DVKT")]
            QuyTacDVKT = 4
        }

        public enum eKetQuaQuyTac
        {
            [Description("-1: Tất cả")]
            TatCa = -1,
            [Description("0: Xuất toán (1,2)")]
            XuatToan = 0,
            [Description("1: Cảnh báo (0)")]
            CanhBao = 1

        }
        public enum eLoaivanbanhienthi
        {
            thuoc = 1,
            khunggiadichvu = 2,
            dichvu = 3,
            dichvukhac = 4,
            vattu = 5,
            cacloaivanbankhac = 6
        }
        public enum eTrangThais
        {
            DeNghi = 1,//
            TuChoi = 3,//
            DaApDung = 4,//
            ChoPheDuyet = 5,//
            CanhBao = 6,//
            ChuaGanMa = 7,//
        }
        public enum ETUCHU
        {
            Tatca = 3,
            Tuchu = 1,
            Khongtuchu = 0,
        }
        public enum EHIEULUC
        {
            Tatca = 3,
            Hieuluc = 1,
            Khonghieuluc = 0,
            ChuaDuyet = 2
        }
        public enum DVKT
        {
            Tatca = 4,
            Daduyet = 1,
            Chuaduyet = 0,
            Chocapnhat = 3,
            Chuaapdung = 2,
        }

        public enum coSoKcb_LdvCq
        {
            BYT = 1,
            SYT = 2,
            YTBN = 3,
            Khac = 4
        }
        public enum loaiCM
        {
            tatca = 0,
            benhvien,
            chacon,
            benhvienvapk
        }
        public enum chaylaiGD_trangthai
        {
            tatca = 4,
            chogd=0,
            giamdinhTD=1,
            giamdinhCD=2,
            huy=3
        }
        public enum chaylaiGD_ketqua
        {
            chapnhan = 1,
            canhbao,
            XT1phan,
            XTtoanbo,
           trunglap
        }
        public enum loaiTL
        {
            trunghoantoan=0,
            ngoaingoai,
            ngoainoi,
            noinoi,
            tatca
        }


        public enum ketquaDt
        {
            khoi = 1,
            Do,
            khongthaydoi,
            nanghon,
            tuvong
        }

        public enum maloaiKcb
        {
            khambenh = 1,
            dieutrinoitru,
            dieutringoaitru
        }

        public static Dictionary<int, string> LoaiTrungLap = new Dictionary<int, string>()
        {
             {(int)loaiTL.tatca, "Tất cả"},
             {(int)loaiTL.trunghoantoan, "Trùng hoàn toàn"},
             {(int)loaiTL.ngoaingoai, "Trùng ngoại - ngoại"},
             {(int)loaiTL.ngoainoi, "Trùng ngoại - nội"},
             {(int)loaiTL.noinoi, "Trùng nội - nội"},
        };
        public static Dictionary<int, string> GVLoaiTrungLap = new Dictionary<int, string>()
        {
             {(int)loaiTL.trunghoantoan, "Trùng hoàn toàn"},
             {(int)loaiTL.ngoaingoai, "Trùng ngoại - ngoại"},
             {(int)loaiTL.ngoainoi, "Trùng ngoại - nội"},
             {(int)loaiTL.noinoi, "Trùng nội - nội"},
        };
        public static Dictionary<int, string> LoaiChonMau = new Dictionary<int, string>()
        {
            {(int)loaiCM.tatca, "Tất cả"},
            {(int)loaiCM.benhvien, "Lấy mẫu của bệnh viện"},
            {(int)loaiCM.chacon, "Cơ sở khám chữa bệnh con áp riêng tỷ lệ"},
           {(int)loaiCM.benhvienvapk, "Bệnh viện và phòng khám"},
        };
        public static Dictionary<int, string> coSoKCb_LdvCq = new Dictionary<int, string>()
        {
            {(int)coSoKcb_LdvCq.BYT, DmCsKcbTextHelper.LoaiDonViChuQuan01},
            {(int)coSoKcb_LdvCq.SYT, DmCsKcbTextHelper.LoaiDonViChuQuan02},
            {(int)coSoKcb_LdvCq.YTBN, DmCsKcbTextHelper.LoaiDonViChuQuan03},
            {(int)coSoKcb_LdvCq.Khac, DmCsKcbTextHelper.LoaiDonViChuQuan04},
        };

        public static Dictionary<int, string> coSoKcb_LoaiBenhVien = new Dictionary<int, string>()
        {
            {(int)coSoKcb_LOAIBENHVIEN.congLap, DmCsKcbTextHelper.LoaiBenhVien01},
            {(int)coSoKcb_LOAIBENHVIEN.ngoaiCongLap, DmCsKcbTextHelper.LoaiBenhVien02}
        };

        public static Dictionary<int, string> coSoKCb_HangBenhVien = new Dictionary<int, string>()
        {
            {(int)coSoKCb_HANGBENHVIEN.hangDacBiet, DmCsKcbTextHelper.HangBenhVien0},
            {(int)coSoKCb_HANGBENHVIEN.hang1, DmCsKcbTextHelper.HangBenhVien01},
            {(int)coSoKCb_HANGBENHVIEN.hang2, DmCsKcbTextHelper.HangBenhVien02},
            {(int)coSoKCb_HANGBENHVIEN.hang3, DmCsKcbTextHelper.HangBenhVien03},
            {(int)coSoKCb_HANGBENHVIEN.hang4, DmCsKcbTextHelper.HangBenhVien04},
            {(int)coSoKCb_HANGBENHVIEN.chuaXepHang, DmCsKcbTextHelper.HangBenhVien05}
        };

        public static Dictionary<int, string> coSoKCb_DkKcbBd = new Dictionary<int, string>()
        {
            {(int)coSoKcb_DKKCBBD.khongNhanDK_KCBBĐ,  DmCsKcbTextHelper.DangKyKcbBanDau0},
            {(int)coSoKcb_DKKCBBD.nhanDK_KCBBĐNT, DmCsKcbTextHelper.DangKyKcbBanDau01},
            {(int)coSoKcb_DKKCBBD.nhanDK_Noi_NgoaiTinh, DmCsKcbTextHelper.DangKyKcbBanDau02},
        };

        public static Dictionary<int, string> coSoKCb_KieuBenhVien = new Dictionary<int, string>()
        {
            {(int)coSoKcb_KIEUBV.bv_BenhVienDaKhoa, DmCsKcbTextHelper.KieuBenhVien01},
            {(int)coSoKcb_KIEUBV.pk_PhongKhamDaKhoa, DmCsKcbTextHelper.KieuBenhVien02},
            {(int)coSoKcb_KIEUBV.bbvcssk_Banbaovechamsocsuckhoe, DmCsKcbTextHelper.KieuBenhVien03},
            {(int)coSoKcb_KIEUBV.ttytck_trungtamytechuyenkhoa, DmCsKcbTextHelper.KieuBenhVien04},
            {(int)coSoKcb_KIEUBV.ytcq_Ytecoquan, DmCsKcbTextHelper.KieuBenhVien05},
            {(int)coSoKcb_KIEUBV.ttyt_Trungtamyte, DmCsKcbTextHelper.KieuBenhVien06},
            {(int)coSoKcb_KIEUBV.tyt, DmCsKcbTextHelper.KieuBenhVien07},
            {(int)coSoKcb_KIEUBV.bx_Benhxa, DmCsKcbTextHelper.KieuBenhVien08},
            {(int)coSoKcb_KIEUBV.nhs_Nhahosinh, DmCsKcbTextHelper.KieuBenhVien09},
            {(int)coSoKcb_KIEUBV.bv_BenhvienYHCT, DmCsKcbTextHelper.KieuBenhVien10},
            {(int)coSoKcb_KIEUBV.bv_Benhvienchuyenkhoa, DmCsKcbTextHelper.KieuBenhVien11},
            {(int)coSoKcb_KIEUBV.pk_Phongkhamchuyenkhoa, DmCsKcbTextHelper.KieuBenhVien12},
        };


        public static Dictionary<int, string> coSoKCb_HinhThucThanhToan = new Dictionary<int, string>()
        {
            {(int)coSoKcb_HINHTHUCTT.theoPhiDichVu, DmCsKcbTextHelper.HinhThucThanhToan01},
            {(int)coSoKcb_HINHTHUCTT.theoDinhSuat, DmCsKcbTextHelper.HinhThucThanhToan02},
        };
        public static Dictionary<int, string> coSoKCb_KCB = new Dictionary<int, string>()
        {
            {(int)coSoKcb_KCB.khongKCB, DmCsKcbTextHelper.KhamChuaBenh01},
            {(int)coSoKcb_KCB.coKCb, DmCsKcbTextHelper.KhamChuaBenh02},
        };

        public static Dictionary<int, string> coSoKCb_LOAITRANGTHAIDMCSKCB = new Dictionary<int, string>()
        {
            {(int)LOAITRANGTHAIDMCSKCB.KHONG_HL, DmCsKcbTextHelper.TrangThai0},
            {(int)LOAITRANGTHAIDMCSKCB.CO_HL, DmCsKcbTextHelper.TrangThai01},
            {(int)LOAITRANGTHAIDMCSKCB.DE_NGHI_TAM_NGUNG_HD, DmCsKcbTextHelper.TrangThai02},
            {(int)LOAITRANGTHAIDMCSKCB.DE_NGHI_KH_HD_NHO_HON_6, DmCsKcbTextHelper.TrangThai03},
            {(int)LOAITRANGTHAIDMCSKCB.DE_NGHI_KH_HD_LON_HON_6, DmCsKcbTextHelper.TrangThai04},
        };

        public static Dictionary<int, string> coSoKcb_LoaiHopDong = new Dictionary<int, string>()
        {
            {(int)coSoKcb_LOAIHOPDONG.kcb_BHYT_NoiTru,  DmCsKcbTextHelper.LoaiHopDong01},
            {(int)coSoKcb_LOAIHOPDONG.kcb_BHYT_NgoaiTru,DmCsKcbTextHelper.LoaiHopDong02},
            {(int)coSoKcb_LOAIHOPDONG.kcb_BHYT_NoiTru_NgoaiTru, DmCsKcbTextHelper.LoaiHopDong03},
            {(int)coSoKcb_LOAIHOPDONG.thanhtoantructiep, DmCsKcbTextHelper.LoaiHopDong04},
        };

        public static Dictionary<int, string> coSoKCb_eTuyenBv = new Dictionary<int, string>()
        {
            {(int)eTuyenBv.trungUong, DmCsKcbTextHelper.TuyenCmkt01},
            {(int)eTuyenBv.tinh, DmCsKcbTextHelper.TuyenCmkt02},
            {(int)eTuyenBv.huyen, DmCsKcbTextHelper.TuyenCmkt03},
            {(int)eTuyenBv.xa, DmCsKcbTextHelper.TuyenCmkt04},
        };

        public static Dictionary<int, string> coSoKcb_TuChu = new Dictionary<int, string>()
        {
            {(int)coSoKcb_TUCHU.tuChu,DmCsKcbTextHelper.TuChu01},
            {(int)coSoKcb_TUCHU.khongTuChu, DmCsKcbTextHelper.TuChu02},
        };

        public static Dictionary<int, string> LOAIKCBBD_CHITIET = new Dictionary<int, string>()
        {
             {(int)eLOAIBN_CHITIET.noikcbbd, "Nội tỉnh KCB ban đầu"},
             {(int)eLOAIBN_CHITIET.noiden, "Nội tỉnh đến"},
             {(int)eLOAIBN_CHITIET.ngoaiden, "Ngoại tỉnh đến"},
        };
        public static Dictionary<int, string> TypeDVKTBANGGIA = new Dictionary<int, string>()
        {
             {(int)DVKT.Tatca, "Tất cả"},
             {(int)DVKT.Daduyet, "Đã duyệt"},
             {(int)DVKT.Chuaduyet, "Chưa duyệt"},
             {(int)DVKT.Chocapnhat, "Chờ cập nhật"},
             {(int)DVKT.Chuaapdung, "Chưa áp dụng"},
        };
        public enum eLOAICSKCB
        {
            LayMauBenhVien = 1,
            CSKCBApRiengTyLe = 2,
            BenhVienPK = 3
        }
        public readonly static Dictionary<int, string> LoaiCSKCBs = new Dictionary<int, string>()
        {
             {(int)eLOAICSKCB.LayMauBenhVien, "Lấy mẫu của bệnh viện"},
             {(int)eLOAICSKCB.CSKCBApRiengTyLe, "Cơ sở khám chữa bệnh con áp riêng tỷ lệ"},
             {(int)eLOAICSKCB.BenhVienPK, "Bệnh viện và phòng khám"},
        };

        public static Dictionary<int, string> Hieulucs = new Dictionary<int, string>()
        {
             {(int)EHIEULUC.Tatca, "Tất cả"},
             {(int)EHIEULUC.Hieuluc, "Hiệu lực"},
             {(int)EHIEULUC.Khonghieuluc, "Không hiệu lực"},
             {(int)EHIEULUC.ChuaDuyet, "Chưa duyệt"},
        };
        public static Dictionary<int, string> BgHieulucs = new Dictionary<int, string>()
        {
             {(int)EHIEULUC.Tatca, "Tất cả"},
             {(int)EHIEULUC.Hieuluc, "Hiệu lực"},
             {(int)EHIEULUC.Khonghieuluc, "Không hiệu lực"}
        };
        public enum LOAI_MHK
        {
            KhoaDT = 1,
            KhoaCLS = 2,
        }
        public static Dictionary<int, string> phamvi = new Dictionary<int, string>()
        {
             {(int)ephamvi.noivien, "Nội viện"},
             {(int)ephamvi.lienvientrongtinh, "Liên viện nội tỉnh"},
              {(int)ephamvi.lienvienngoaitinh, "Liên viện ngoại tỉnh"},
        };
        public static Dictionary<int, string> Loaimhk = new Dictionary<int, string>()
        {
             {(int)LOAI_MHK.KhoaDT, "Khoa điều trị"},
             {(int)LOAI_MHK.KhoaCLS, "Khoa cận lâm sàng"},
        };
        public enum trangthai_thuockk
        {
            Moi = 1,
            Cu = 2,
        }
        public static Dictionary<int, string> trangthai_tkk = new Dictionary<int, string>()
        {
             {(int)trangthai_thuockk.Moi, "Mới"},
             {(int)trangthai_thuockk.Cu, "Cũ"},
        };
        public static Dictionary<int, string> Tuchus = new Dictionary<int, string>()
        {
             {(int)ETUCHU.Tatca, "Tất cả"},
             {(int)ETUCHU.Tuchu, "Tự chủ"},
             {(int)ETUCHU.Khongtuchu, "Không tự chủ"},
        };
        public static Dictionary<int, string> TrangThais = new Dictionary<int, string>()
        {
             {(int)eTrangThais.DeNghi, "Đề nghị"},
             {(int)eTrangThais.TuChoi, "Từ chối"},
             {(int)eTrangThais.DaApDung, "Đã áp dụng"},
             {(int)eTrangThais.ChoPheDuyet, "Chờ phê duyệt"},
             {(int)eTrangThais.CanhBao, "Cảnh báo"},
             {(int)eTrangThais.ChuaGanMa, "Chưa gán mã"},
        };
        public readonly static Dictionary<int, string> eloaivanbanhienthi = new Dictionary<int, string>()
        {

            { (int)eLoaivanbanhienthi.thuoc, "Thuốc" },
            { (int)eLoaivanbanhienthi.khunggiadichvu, "Khung giá dịch vụ"},
            { (int)eLoaivanbanhienthi.dichvu, "Dịch vụ" },
            { (int)eLoaivanbanhienthi.dichvukhac, "Dịch vụ khác"},
            { (int)eLoaivanbanhienthi.vattu, "Vật tư"},
            { (int)eLoaivanbanhienthi.cacloaivanbankhac, "Các loại văn bản khác"},
        };


        public readonly static Dictionary<int, string> etttttuongtac = new Dictionary<int, string>()
        {

            { (int)eTuongtac.muc1, "Tương tác cần theo dõi" },
            { (int)eTuongtac.muc2, "Tương tác cần thận trọng"},
            { (int)eTuongtac.muc3, "Cân nhắc nguy cơ / lợi ích" },
            { (int)eTuongtac.muc4, "Phối hợp nguy hiểm"},
        };
        public readonly static Dictionary<int, string> enhomdtbhyt = new Dictionary<int, string>()
        {

            { (int)ephannhomdtbhyt.nhom1, "Nhóm 1" },
            { (int)ephannhomdtbhyt.nhom2, "Nhóm 2"},
            { (int)ephannhomdtbhyt.nhom3, "Nhóm 3" },
            { (int)ephannhomdtbhyt.nhom4, "Nhóm 4"},
            { (int)ephannhomdtbhyt.nhom5, "Nhóm 5"},
            //{ (int)ephannhomdtbhyt.nhom6, "Nhóm 6"},
            //{ (int)ephannhomdtbhyt.nhom7, "Nhóm 7"},
            //{ (int)ephannhomdtbhyt.nhom8, "Nhóm 8"},
            //{ (int)ephannhomdtbhyt.nhom9, "Nhóm 9"},
        };
        //public readonly static Dictionary<int, string> etttttuongtac = new Dictionary<int, string>()
        //{

        //    { (int)eTuongtac.muc1, "Tương tác cần theo dõi" },
        //    { (int)eTuongtac.muc2, "Tương tác cần thận trọng"},
        //    { (int)eTuongtac.muc3, "Cân nhắc nguy cơ / lợi ích" },
        //    { (int)eTuongtac.muc4, "Phối hợp nguy hiểm"},
        //};
        public readonly static Dictionary<int, string> ettttNguon = new Dictionary<int, string>()
        {

            { (int)eNguon.nhasx, "Nhà sản xuất" },
            { (int)eNguon.boyt, "Bộ y tế"},
            { (int)eNguon.who, "WHO" },
            { (int)eNguon.nguonkhac, "Nguồn khác"}
        };
        public readonly static Dictionary<int, string> eTrang_thai_yc = new Dictionary<int, string>()
        {
            { (int)Status.dangxuly, "Đang xử lý" },
            { (int)Status.daxuly, "Đã xử lý"},
             { (int)Status.tuchoi, "Từ chối xử lý"}
        };
        public readonly static Dictionary<int, string> eTrung_Lap = new Dictionary<int, string>()
        {
            { (int)eTrunglap.motphan, "Trùng 1 phần" },
            { (int)eTrunglap.toanbo, "Trùng toàn bộ"}
            //{ (int)eTrunglap.toanBo, "Trùng toàn bộ" },
            //{ (int)eTrunglap.ngoaiNgoai, "Trùng ngoại trú - ngoại trú"},
            //{ (int)eTrunglap.ngoaiNoi, "Trùng ngoại trú - nội trú"},
            //{ (int)eTrunglap.noiNoi, "Trùng nội trú - nội trú"}
        };
        public readonly static Dictionary<int, string> eTrang_thai = new Dictionary<int, string>()
        {

            { (int)eTrangthai.Chuaxl, "Chưa xử lý" },
            { (int)eTrangthai.Daxl, "Đã xử lý"}
        };
        public readonly static Dictionary<int, string> eGioi_Tinhs = new Dictionary<int, string>()
        {

            { (int)eGioiTinh.Nam, "Nam" },
            { (int)eGioiTinh.Nu, "Nữ"}
        };
        public static readonly Dictionary<int, string> eThongKeTCTTs = new Dictionary<int, string>()
        { // { (int) eTCTT.tatca, "Tất cả" },
            { (int) eTCTT.thuoc, "Thuốc" },
            { (int) eTCTT.dvdk, "Dịch vụ kỹ thuật" },
            { (int) eTCTT.vtyt, "Vật tư y tế"}
        };
        public static readonly Dictionary<int, string> eThongKeRPs = new Dictionary<int, string>()
        {
            { (int) eThongKeRP.doiTuong, "Đối tượng BHYT" },
            { (int) eThongKeRP.loaiKCB, "Loại KCB" },
            { (int) eThongKeRP.tuyenBN, "Tuyến BN"}
        };
        public static readonly Dictionary<int, string> eLoaiHSs = new Dictionary<int, string>()
        {
            { (int) eLoaiHS.tatca, "Tất cả" },
            { (int) eLoaiHS.TrongMau, "Trong mẫu" },
            { (int) eLoaiHS.NgoaiMau, "Ngoài mẫu"}
        };
        public static readonly Dictionary<int, string> LoaiGiamDinhs = new Dictionary<int, string>()
        {

            { (int)eLoaiGiamDinh.MoiTao,"Mới tạo" },
            { (int)eLoaiGiamDinh.DangGiamDinh,"Đang giám định" },
            { (int)eLoaiGiamDinh.DaGiamDinh,"Đã xác nhận" },
            { (int)eLoaiGiamDinh.GiamDinhLai,"Giám định lại" },
            { (int)eLoaiGiamDinh.ThuHoi,"Thu hồi" },
            { (int)eLoaiGiamDinh.dangxacnhan,"Đang xác nhận" },
            { (int)eLoaiGiamDinh.danghuyxacnhan,"Đang huỷ xác nhận" }

        };
        public readonly static Dictionary<int, string> eMauIns = new Dictionary<int, string>()
        {
            { (int)eMauIn.mauBenhVien, "In mẫu BV" },
            { (int)eMauIn.mauTinh, "In mẫu tỉnh" },
        };
        public readonly static Dictionary<int, string> eLoaiHinhKCBs = new Dictionary<int, string>()
        {                              
            { (int)eLoaiHinhKCB.tatCa, "Tất cả" },
            { (int)eLoaiHinhKCB.ngoaiTru, "Ngoại Trú" },
            { (int)eLoaiHinhKCB.noiTru, "Nội Trú" },

        };

        public readonly static Dictionary<int, string> eLoaiHinhKCBTreemKts = new Dictionary<int, string>()
        {                              
            { (int)eLoaiHinhKCB.tatCa, "Tất cả" },
            { (int)eLoaiHinhKCB.ngoaiTru, "Ngoại Trú(79)" },
            { (int)eLoaiHinhKCB.noiTru, "Nội Trú(80)" },

        };

        public readonly static Dictionary<int, string> eGioiTinhs = new Dictionary<int, string>()
        {
              { (int)eGioiTinh.tatCa, "Tất cả" },
            { (int)eGioiTinh.Nam, "Nam" },
            { (int)eGioiTinh.Nu, "Nữ"}
        };
        public readonly static Dictionary<int, string> eGioiTinhss = new Dictionary<int, string>()
        {
            { (int)eGioiTinhssss.tatCa, "Tất cả" },
            { (int)eGioiTinhssss.Nam, "Nam" },
            { (int)eGioiTinhssss.Nu, "Nữ"}
        };
        public readonly static Dictionary<int, string> eBaoCaoEmptys = new Dictionary<int, string>()
        {
            { (int)eBaoCaoEmpty.bvDeNghi, "Bệnh viện đề nghị" },
            { (int)eBaoCaoEmpty.giamDinhDuyet, "Giám định duyệt" },
        };
        public readonly static Dictionary<int, string> tuyenKCBs = new Dictionary<int, string>()
        {
            { (int)Number.one, "Đúng tuyến" },
               { (int)Number.two, "Trái tuyến" },
               { (int)Number.three , "Cấp cứu"},
            { (int)Number.four, "Thông tuyến"}
        };
        public readonly static Dictionary<int, string> loaiBaoCaos = new Dictionary<int, string>()
        {
            { (int)eLoaiBC.ngoaiTru, "Ngoại trú (79)" },
            { (int)eLoaiBC.noiTru, "Nội trú (80)" },
        };

        public readonly static Dictionary<int, string> luaChonIns = new Dictionary<int, string>()
        {
            { (int)eLuaChonIn.tatCa, "Tất cả" },
            { (int)eLuaChonIn.noiTinhBD, "Bệnh nhân nội tỉnh KCBBĐ" },
            { (int)eLuaChonIn.noiTinhDen, "Bệnh nhân nội tỉnh đến" },
            { (int)eLuaChonIn.ngoaiTinhDen, "Bệnh nhân ngoại tỉnh đến" },
        };
        public readonly static Dictionary<int, string> loaiBaoCao_BVs = new Dictionary<int, string>()
        {
            { (int)eLoaiBC.ngoaiTru, "Ngoại trú (79)" },
            { (int)eLoaiBC.noiTru, "Nội trú (80)" },
            { (int)eLoaiBC.noiTruCoCongKham, "Nội trú có công khám (80)" },
        };
        public readonly static Dictionary<int, string> kieuBaoCaos = new Dictionary<int, string>()
        {
            { (int)eKieuBC.chiTiet, "Chi tiết" },
            { (int)eKieuBC.theoBenhVien, "TH theo bệnh viện" },
        };
        public readonly static Dictionary<int, string> kieuBaoCao_BVs = new Dictionary<int, string>()
        {
            { (int)eKieuBC.theoBenhVien, "TH theo bệnh viện" },
            { (int)eKieuBC.theoTinh, "TH theo Đơn vị BHXH" },
        };
        public readonly static Dictionary<int, string> kieuBaoCao_Tinhs = new Dictionary<int, string>()
        {
            { (int)eKieuBC.chiTiet, "Chi tiết" },
            { (int)eKieuBC.theoBenhVien, "Tổng hợp theo bệnh viện" },
            { (int)eKieuBC.theoTinh, "Tổng hợp theo Tỉnh" },
        };
        public readonly static Dictionary<int, string> mauTTs = new Dictionary<int, string>()
        {
            { (int)eMauTT.mauA, "Đề nghị thanh toán (Mẫu A)" },
            { (int)eMauTT.mauB, "Duyệt thanh toán (Mẫu B)" },
        };
        public readonly static Dictionary<int, string> mau192021 = new Dictionary<int, string>()
        {
            { (int)emau192021.mauA, "Đề nghị thanh toán (Biểu A)" },
            { (int)emau192021.mauB, "Duyệt thanh toán (Biểu B)" },
        };
        public readonly static Dictionary<int, string> mauTT_BVs = new Dictionary<int, string>()
        {
            { (int)eMauTT.mauA, "Đề nghị thanh toán (Mẫu A)" },
            { (int)eMauTT.mauB, "Duyệt thanh toán (Mẫu B)" },
             { (int)eMauTT.lan2, "Duyệt thanh toán lần 2" },
        };
        public readonly static Dictionary<int, string> mauTT_TINHs = new Dictionary<int, string>()
        {
            { (int)eMauTT.mauA, "Đề nghị thanh toán" },
            { (int)eMauTT.mauB, "Duyệt thanh toán" },
        };
        public readonly static Dictionary<int, string> hinhThucs = new Dictionary<int, string>()
        {
            { (int)eHinhThuc.noiTru, "Nội trú" },
            { (int)eHinhThuc.ngoaiTru, "Ngoại trú" },
        };

        public static readonly Dictionary<int, string> DM_THUOC_KK_LOAIKEKHAI = new Dictionary<int, string>()
        {
            {(int)DM_THUOC_KK_LoaiKeKhai.KK_TrongNuoc,"Kê khai trong nước"},
            {(int)DM_THUOC_KK_LoaiKeKhai.KK_NgoaiNuoc,"Kê khai ngoài nước"},
            {(int)DM_THUOC_KK_LoaiKeKhai.KKL_TrongNuoc,"Kê khai lại trong nước"},
            {(int)DM_THUOC_KK_LoaiKeKhai.KKL_NgoaiNuoc,"Kê khai lại ngoài nước"}
        };
        public readonly static Dictionary<int, string> lyDoVaoViens = new Dictionary<int, string>()
        {
            { (int)eLyDoVaoVienShow.kcbbd, "KCB BĐ" },
            { (int)eLyDoVaoVienShow.chuyenDen, "Chuyển đến" },
            { (int)eLyDoVaoVienShow.capCuu, "Cấp cứu" },
            { (int)eLyDoVaoVienShow.traiTuyen, "Trái tuyến"}
            //{ (int)eLyDoVaoVienShow.capCuu, "Cấp cứu" },
          //  { (int)eLyDoVaoVienShow.traiTuyen, "Không Cấp Cứu"}
        };

        public readonly static Dictionary<int, string> tinhTrangNhapVien = new Dictionary<int, string>()
        {
            { (int)eLyDoVaoVienShow.capCuu, "Cấp cứu" },
            { (int)eLyDoVaoVienShow.traiTuyen, "Không cấp Cứu"},
            { (int)eLyDoVaoVienShow.chuyenDen, "Chuyển đến" },
        };

        public static Dictionary<int, string> tinhTrangRaViens = new Dictionary<int, string>()
        {
            { (int)eTinhTrangRaVien.raVien, "Ra viện" },
            { (int)eTinhTrangRaVien.chuyenVien, "Chuyển viện" },
            { (int)eTinhTrangRaVien.tronVien, "Trốn viện" },
            { (int)eTinhTrangRaVien.xinRaVien, "Xin ra viện" },
        };

        public static Dictionary<int, string> loaiKCBs = new Dictionary<int, string>()
        {
            { (int)eLoaiKCB.ngoaiTru, "Ngoại trú (Khám bệnh + Điều trị ngoại trú)" },
            { (int)eLoaiKCB.noiTru, "Nội trú"}
        };
        public static Dictionary<int, string> loaiKCBTL = new Dictionary<int, string>()
        {
            { (int)eLoaiKCBTL.ngoaiTru, "Ngoại trú (Khám bệnh + Điều trị ngoại trú)" },
             { (int)eLoaiKCBTL.kcb, "Ngoại trú (Khám bệnh + Điều trị ngoại trú)" },
            { (int)eLoaiKCBTL.noitru, "Nội trú"}
        };
        public static Dictionary<int, string> loaiKCBTL192021 = new Dictionary<int, string>()
        {
            { (int)eLoaiKCBTL.ngoaiTru, "Ngoại trú" },
             { (int)eLoaiKCBTL.kcb, "Khám bệnh" },
            { (int)eLoaiKCBTL.noitru, "Nội trú"}
        };
        public static Dictionary<int, string> loaiKCBs_Filter = new Dictionary<int, string>()
        {
            { (int)eLoaiKCB.all, "Tất cả" },
            { (int)eLoaiKCB.ngoaiTru, "Ngoại trú (Khám bệnh + Điều trị ngoại trú)" },
            { (int)eLoaiKCB.noiTru, "Nội trú"}
        };

        public static Dictionary<int, string> Ds_HangBenhVien = new Dictionary<int, string>()
        {
          { (int)Number.night, "Hạng đặc biệt" },
          { (int)Number.one, "Hạng 1" },
          { (int)Number.two, "Hạng 2" },
          { (int)Number.three, "Hạng 3" },
          { (int)Number.four, "Hạng 4"},
          { (int)Number.zero, "Chưa hoặc không xếp hạng" },
        };

        public static Dictionary<int, string> Ds_Tuyen = new Dictionary<int, string>()
        {
             { (int)Number.one, "Tuyến trung ương" },
            { (int)Number.two, "Tuyến tỉnh" },
            { (int)Number.three, "Tuyến huyện" },
           { (int)Number.four, "Tuyến xã"}
        };
        public static Dictionary<int, string> gioiTinh_TQT = new Dictionary<int, string>()
        {
            {1, "Nam" },
            {0, "Nữ" },
        };
        public static Dictionary<int, string> gioiTinhs = new Dictionary<int, string>()
        {
            {1, "Nam" },
            {2, "Nữ" },
        };

        public static Dictionary<int, string> LoaiCanhBaos = new Dictionary<int, string>()
        {
            { (int)eLoaiCanhBao.canhBao, "Cảnh báo" },
            { (int)eLoaiCanhBao.xuatToan, "Xuất toán"}
        };

        public static Dictionary<string, string> GoiThaus = new Dictionary<string, string>()
        {
            { "G1", "Generic" },
            { "G2", "Biệt dược gốc hoặc tương đương điều trị"},
            { "G3", "Gói thầu thuốc cổ truyền" },
            { "G4", "Gói thầu dược liệu" },
            { "G5", "Gói thầu vị thuốc cổ truyền" }
        };

        public static Dictionary<string, string> NhomThaus = new Dictionary<string, string>()
        {
            { "N1", "Nhóm 1" },
            { "N2", "Nhóm 2" },
            { "N3", "Nhóm 3" },
            { "N4", "Nhóm 4" },
            { "N5", "Nhóm 5" },
        };
        public static Dictionary<int, string> TrangThaiBCGD = new Dictionary<int, string>()
        {
             { (int)eTrangThaiBC.moi, "Mới" },
             { (int)eTrangThaiBC.daGui, "Đã gửi phòng KHTC"},
             { (int)eTrangThaiBC.biTraLai, "Bị trả lại"},
             { (int)eTrangThaiBC.daDuyet, "Đã duyệt"},
             { (int)eTrangThaiBC.daGuiTW, "Đã gửi Trung ương"},
        };
        public static Dictionary<int, string> TrangThaiCbbGD = new Dictionary<int, string>()
        {
             { -1, "Tất cả" },
             { (int)eTrangThaiBC.moi, "Mới" },
             { (int)eTrangThaiBC.daGui, "Đã gửi phòng KHTC"},
             { (int)eTrangThaiBC.biTraLai, "Bị trả lại"},
             { (int)eTrangThaiBC.daDuyet, "Đã duyệt"},

        };
        public static Dictionary<int, string> TrangThaiBCTCKT = new Dictionary<int, string>()
        {
             { -1, "Tất cả" },
             { (int)eTrangThaiBC.daGui, "Đã gửi phòng KHTC"},
             { (int)eTrangThaiBC.biTraLai, "Trả lại"},
             { (int)eTrangThaiBC.daDuyet, "Đã duyệt"},

        };
        public static Dictionary<int, string> TrangThaiBHYT12 = new Dictionary<int, string>()
        {
             { -1, "Tất cả" },
             { (int)eTrangThaiBCBHYT12.MoiGuiKHTC, "Đã gửi phòng KHTC"},
             { (int)eTrangThaiBCBHYT12.KHTCTraLai, "Phòng KHTC Trả lại"},
             { (int)eTrangThaiBCBHYT12.KHTCDaDuyet, "Phòng KHTC Đã duyệt"},
             { (int)eTrangThaiBCBHYT12.TWTraLai, "Trung Ương Trả Lại"},
             { (int)eTrangThaiBCBHYT12.TWDaDuyet, "Trung Ương Đã duyệt"}

        };
        public static Dictionary<int, string> TrangThaiBHYT12_TW = new Dictionary<int, string>()
        {
             { -1, "Tất cả" },
             { (int)eTrangThaiBCBHYT12.KHTCDaDuyet, "Phòng KHTC Đã duyệt"},
             { (int)eTrangThaiBCBHYT12.TWTraLai, "Trung Ương Trả Lại"},
             { (int)eTrangThaiBCBHYT12.TWDaDuyet, "Trung Ương Đã duyệt"}

        };
        public static Dictionary<int, string> TRANG_THAI_CHOTDL = new Dictionary<int, string>()
        {
            {1, "Đã Chốt số liệu" },
            {2, "Đang mở lại" }
        };
        public static Dictionary<int, string> TrangThaiBCTW = new Dictionary<int, string>()
        {
             { (int)eTrangThaiBC.daGuiTW, "Đã gửi Trung ương"},
        };
        #region Bộ lọc thời gian

        public enum eThoiGian
        {
            [Description("0: Năm")]
            nam = 0,
            [Description("1: Quý")]
            quy = 1,
            [Description("2: Tháng")]
            thang = 2,
            [Description("3: Ngày")]
            ngay = 3
        }
        public enum eThoiGianTQ
        {            
            [Description("1: Quý")]
            quy = 1,
        }
        public enum eThoiGianTKSLTCTT
        {
            nam = 0,
            quy = 1,
            thang = 2,
            ngay = 4
        }
        public enum eThoiGianTKKCBNL
        {
            thang = 1,
            quy = 2,
            nam = 3,
        }
        public enum eThoiGianNew
        {
            thang = 0,
            quy = 1,
            nam = 2,
            ngay = 3
        }

        public enum eThoiGian2
        {
            nam = 0,
            quy = 1,
            thang = 2,
            ngayra = 3,
            ngaythanhtoan = 4,
        }
        public enum eThoiGianQuyNam
        {
            quy = 2,
            nam = 1,
        }
        public enum eThoiGianTracuu
        {
            ngayrv = 0,
            ngaynhs = 1,
            ngaydhs = 2,
            thangqt = 3,
        }
        public enum eThoiGianTracuuNgoaimau
        {
            ngayrv = 3,
            //  ngaynhs = 1,
            //  ngaydhs = 2,
            thangqt = 2,
            quyqt = 1,
            namqt = 0,
        }
        public enum tieuchi
        {
            caoN = 1,
            cao = 2,
        }
        public static Dictionary<int, string> thoigian_TKKCBNL = new Dictionary<int, string>()
        {
            { (int)eThoiGianTKKCBNL.nam, "Năm" },
            { (int)eThoiGianTKKCBNL.quy, "Quý" },
            { (int)eThoiGianTKKCBNL.thang, "Tháng" },
        };
        public static Dictionary<int, string> Tieuchi = new Dictionary<int, string>()
        {
            { (int) tieuchi.caoN, "HS có chi phi cao nhất" },
                { (int)tieuchi.cao,  "HS có chi phi cao"},

        };
        public static Dictionary<int, string> thoiGianTracuuNgoaimau = new Dictionary<int, string>()
        {
            { (int) eThoiGianTracuuNgoaimau.ngayrv, "Giai đoạn" },
//{ (int)eThoiGianTracuuNgoaimau.ngaynhs, "Ngày nhận hồ sơ" },
           //   { (int)eThoiGianTracuuNgoaimau.ngaydhs, "Ngày duyệt hồ sơ" },
                { (int)eThoiGianTracuuNgoaimau.thangqt, "Tháng" },
                  { (int)eThoiGianTracuuNgoaimau.quyqt, "Quý" },
                    { (int)eThoiGianTracuuNgoaimau.namqt, "Năm" },

        };

        public static Dictionary<int, string> ThoiGianSession = new Dictionary<int, string>()
        {
            { (int) eThoiGian.quy, "Quý" },
            { (int) eThoiGian.thang, "Tháng" },

        };
        public static Dictionary<int, string> ThoiGianTK_HOSO_DENGHITHANHTOAN = new Dictionary<int, string>()
        {
            { (int) eThoiGian.thang, "Tháng" }
        };
        public static Dictionary<int, string> thoigian_TKSLTCTT = new Dictionary<int, string>()
        {
            { (int)eThoiGianTKSLTCTT.nam, "Năm" },
            { (int)eThoiGianTKSLTCTT.quy, "Quý" },
            { (int)eThoiGianTKSLTCTT.thang, "Tháng" },
        };
        public static Dictionary<int, string> thoiGians = new Dictionary<int, string>()
        {
            { (int)eThoiGian.nam, "Năm" },
            { (int)eThoiGian.quy, "Quý" },
            { (int)eThoiGian.thang, "Tháng" },
            { (int)eThoiGian.ngay, "Giai đoạn" },
        };
        public static Dictionary<int, string> thoiGianTK_VIPHAM = new Dictionary<int, string>()
        {
            { (int)eThoiGian.quy, "Quý" },
            { (int)eThoiGian.thang, "Tháng" }
        };
        public static Dictionary<int, string> thoiGianDLTRUNGLAPTQ = new Dictionary<int, string>()
        {
            { (int)eThoiGianTQ.quy, "Quý" },
        };
        public static Dictionary<int, string> thoiGianTracuu = new Dictionary<int, string>()
        {
            { (int)eThoiGianTracuu.ngayrv, "Ngày ra viện" },
            { (int)eThoiGianTracuu.ngaynhs, "Ngày nhận hồ sơ" },
            { (int)eThoiGianTracuu.ngaydhs, "Ngày duyệt hồ sơ" },
            { (int)eThoiGianTracuu.thangqt, "Tháng quyết toán" },
        };
        public static Dictionary<int, string> thoiGianNews = new Dictionary<int, string>()
        {
            { (int)eThoiGianNew.thang, "Tháng" },
            { (int)eThoiGianNew.quy, "Quý" },
            { (int)eThoiGianNew.nam, "Năm" },
            { (int)eThoiGianNew.ngay, "Giai đoạn" },
        };
        public static Dictionary<int, string> thoiGianNews2 = new Dictionary<int, string>()
        {
            { (int)eThoiGian.thang, "Tháng" },
            { (int)eThoiGian.quy, "Quý" },
            { (int)eThoiGian.nam, "Năm" },
        };
        public static Dictionary<int, string> thoiGian_TK = new Dictionary<int, string>()
        {
            { (int)eThoiGian.thang, "Tháng" },
            { (int)eThoiGian.quy, "Quý" },
            { (int)eThoiGian.nam, "Năm" },
             { (int)eThoiGian.ngay, "Giai đoạn" },
        };

        public static Dictionary<int, string> ThoiGian2 = new Dictionary<int, string>()
        {
            { (int) eThoiGian2.nam, "Năm" },
            { (int) eThoiGian2.quy, "Quý" },
            { (int) eThoiGian2.thang, "Tháng" },
            { (int) eThoiGian2.ngayra, "Ngày giờ ra" },
            { (int) eThoiGian2.ngaythanhtoan, "Ngày thanh toán" },
        };
        public static Dictionary<int, string> thoiGianNews3 = new Dictionary<int, string>()
        {
            { (int)eThoiGian.ngay, "Giai đoạn" },
            { (int)eThoiGian.thang, "Tháng" },
            { (int)eThoiGian.quy, "Quý" },
            { (int)eThoiGian.nam, "Năm" },
        };
        public static Dictionary<int, string> ThoiGianShort = new Dictionary<int, string>()
        {
            { (int) eThoiGian2.nam, "Năm" },
            { (int) eThoiGian2.quy, "Quý" },

        };

        public static Dictionary<int, string> ThoiGian192021 = new Dictionary<int, string>()
        {
            { (int) eThoiGianQuyNam.nam, "Năm" },
            { (int) eThoiGianQuyNam.quy, "Quý" },

        };

        public static Dictionary<int, string> ThoiGianC88 = new Dictionary<int, string>()
        {
            { (int) eThoiGian2.quy, "Quý" },
            { (int) eThoiGian2.nam, "Năm" },


        };
        public readonly static Dictionary<int, string> layMaus = new Dictionary<int, string>()
        {
            { (int)eLayMau.khoa, "Khoa" },
            { (int)eLayMau.ngay, "Ngày" },
            { (int)eLayMau.hoSo, "Hồ sơ" },
            { (int)eLayMau.khoaNgay, "Khoa - Ngày"}
        };
        public readonly static Dictionary<int, string> layMauTYTs = new Dictionary<int, string>()
        {
            { (int)eLayMau.khoa, "Khoa" },
            { (int)eLayMau.khoaNgay, "Khoa - Ngày"}
        };

        public static Dictionary<int, string> KyBaoCao = new Dictionary<int, string>()
        {
            { (int)eThoiGian.ngay, "Giai đoạn" },
            { (int)eThoiGian.thang, "Tháng" },
            { (int)eThoiGian.quy, "Quý" },
            { (int)eThoiGian.nam, "Năm" },
        };
        public enum eQuy
        {
            I = 1,
            II = 2,
            III = 3,
            IV = 4
        }
        public static Dictionary<int, string> quys = new Dictionary<int, string>()
        {
            { (int)eQuy.I, "I" },
            { (int)eQuy.II, "II" },
            { (int)eQuy.III, "III" },
            { (int)eQuy.IV, "IV" },
        };
        public static List<int> thangs = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        private static List<int> getNams()
        {
            List<int> nams = new List<int>();
            int now = DateTime.Now.Year;
            for (int i = now; i >= now - 50; i--)
                nams.Add(i);
            return nams;
        }
        public static List<int> nams = getNams();
        #endregion

        #region Bộ lọc tracuu form tổng hợp
        #region bộ lọc tiêu chí form tra cứu nhiều lần
        public enum etieuchi_nhieulan
        {
            nhieulan = 1,
            motthenhieuten = 2
        }
        public static Dictionary<int, string> tieuchi_nhieulan = new Dictionary<int, string>()
        {
        {(int)etieuchi_nhieulan.nhieulan, "Hồ sơ điều trị nhiều lần"},
        {(int)etieuchi_nhieulan.motthenhieuten, "Một thẻ nhiều tên"}
        };
        #endregion

        #region Bộ lọc tiêu chí tổng hợp cao và cao nhất chung
        public enum etieuchi
        {
            caonhat = 1,
            cao = 0
        }
        public static Dictionary<int, string> tieuchis = new Dictionary<int, string>()
        {
        {(int)etieuchi.caonhat, "Số hồ sơ có chi phí cao nhất"},
        {(int)etieuchi.cao, "Hồ sơ có chi phí cao"}
        };
        #endregion


        #region Bộ lọc tracuu form tổng hợp
        public readonly static Dictionary<Boolean, string> eTheoDois = new Dictionary<bool, string>()
        {

            {false, "Không theo dõi" },
            {true, "Theo dõi" },
        };
        #endregion

        #region Bộ lọc tracuu cho combo form tổng hợp
        //lọc cho các combo trong form tra cứu tổng hợp
        public enum eLkcbdanhdau
        {
            tatca = 0,
            ngoaitru = 1,
            noitru = 2,
        }
        public static Dictionary<int, string> loaikcbdanhdaus = new Dictionary<int, string>()
        {
            { (int)eLkcbdanhdau.tatca, "Tất cả" },
            { (int)eLkcbdanhdau.ngoaitru, "Ngoại trú" },
            { (int)eLkcbdanhdau.noitru, "Nội trú" },
        };

        public enum eketquadanhdau
        {
            tatca = 0,
            thanhtoannhudenghi = 1,
            canhbao = 2,
            xuattoanmotphan = 3,
            xuattoantoanbo = 4,
            trunglap = 5,
        }
        public static Dictionary<int, string> ketquadanhdaus = new Dictionary<int, string>()
        {
            { (int)eketquadanhdau.tatca, "Tất cả" },
            { (int)eketquadanhdau.thanhtoannhudenghi, "Thanh toán như đề nghị" },
             { (int)eketquadanhdau.canhbao, "Cảnh báo" },
            { (int)eketquadanhdau.xuattoanmotphan, "Xuất toán một phần" },
            { (int)eketquadanhdau.xuattoantoanbo, "Xuất toán toàn bộ" },
             { (int)eketquadanhdau.trunglap, "Trùng lặp" },
        };
        public enum etuyenkcbdanhdau
        {
            tatca = 0,
            benhnhannoitinhkcbbd = 1,
            benhnhannoitinhden = 2,
            benhnhanngoaitinhden = 3,
            //benhnhannoingoaitinhden = 4
        }
        public static Dictionary<int, string> etuyenkcbdanhdaus = new Dictionary<int, string>()
        {
            { (int)etuyenkcbdanhdau.tatca, "Tất cả" },
            { (int)etuyenkcbdanhdau.benhnhannoitinhkcbbd, "BN nội tỉnh KCBBĐ" },
            { (int)etuyenkcbdanhdau.benhnhannoitinhden, "BN nội tỉnh đến" },
            { (int)etuyenkcbdanhdau.benhnhanngoaitinhden, "BN ngoại tỉnh đến" },
            //{ (int)etuyenkcbdanhdau.benhnhannoingoaitinhden, "BN nội ngoại tỉnh đến" },
        };
        public enum etrangthaidanhdau
        {
            tatca = 0,
            chapnhan = 1,
            canhbao = 2,
            xuattoanmotphan = 3,
            xuattoantoanbo = 4,
            //trunglap = 5,
        }
        public static Dictionary<int, string> etrangthaidanhdaus = new Dictionary<int, string>()
        {
            { (int)etrangthaidanhdau.tatca, "Tất cả" },
            { (int)etrangthaidanhdau.chapnhan, "Chấp nhận" },
            { (int)etrangthaidanhdau.canhbao, "Cảnh báo" },
            { (int)etrangthaidanhdau.xuattoanmotphan, "Xuất toán một phần" },
            { (int)etrangthaidanhdau.xuattoantoanbo, "Xuất toán toàn bộ" },
            //{ (int)etrangthaidanhdau.trunglap, "Trùng lặp" },
        };


        public enum etieuchithongkesscpkcb
        {
            phantichchiphikcb = 0,
            //tocdogiatangchiphi = 1,
            //sosanhtheodichvukithuat = 2,
            sosanhchiphitheonamnhomdoituong = 3,
            //sosanhchiphicacbenhvientrencungdiaban = 4,
            sosanhchiphitheonhombonbadoituong = 5,
        }
        public static Dictionary<int, string> etieuchithongkesscpkcbs = new Dictionary<int, string>()
        {
            { (int)etieuchithongkesscpkcb.phantichchiphikcb, "Phân tích chi phí khám chữa bệnh" },
            //{ (int)etieuchithongkesscpkcb.tocdogiatangchiphi, "Tốc độ gia tăng chi phí" },
            { (int)etieuchithongkesscpkcb.sosanhchiphitheonamnhomdoituong, "So sánh chi phí theo 5 nhóm đối tượng"},
            //{ (int)etieuchithongkesscpkcb.sosanhchiphicacbenhvientrencungdiaban, "So sánh chi phí các bệnh viện trên cùng địa bàn"},
            { (int)etieuchithongkesscpkcb.sosanhchiphitheonhombonbadoituong, "So sánh chi phí theo 43 nhóm đối tượng"},
        };
        #endregion

        #region Bộ lọc tiêu chí form tra cứu
        //lọc cho combo tiêu chí form tổng hợp
        public enum eTieuChi
        {
            doituong = 0,
            nhieulan = 1,
            caonhat = 2,
            hienthicacgiatri = 3,
            mathevahoten = 4,
            khamchunhat = 5,
            HSnhieulan = 6,
        }
        public static Dictionary<int, string> tieuChiNL = new Dictionary<int, string>()
        {
            { (int)eTieuChi.HSnhieulan, "HS điều trị nhiều lần" },
            { (int)eTieuChi.mathevahoten, "Một thẻ nhiều tên" },
          //  { (int)eTieuChi.khamchunhat, "Hồ sơ khám ngày nghỉ,ngày lễ" },
        };
        public static Dictionary<int, string> tieuChis = new Dictionary<int, string>()
        {
            { (int)eTieuChi.doituong, "Đối tượng BHYT" },
        //    { (int)eTieuChi.mathevahoten, "Một thẻ nhiều tên" },
            { (int)eTieuChi.khamchunhat, "Hồ sơ khám ngày nghỉ, ngày lễ" },
        };
        public static Dictionary<int, string> thu7chunhats = new Dictionary<int, string>()
        {
            { (int)thu7chunhat.thu7, "Thứ 7" },
         //   { (int)eTieuChi.nhieulan, "Điều trị nhiều lần" },
         //   { (int)eTieuChi.caonhat, "Bệnh nhân có chi phí cao nhất" },
           // { (int)eTieuChi.hienthicacgiatri, "Tuyến KCB" },
            { (int)thu7chunhat.chunhat, "Chủ nhật" },
           // { (int)eTieuChi.khamchunhat, "Hồ sơ khám ngày nghỉ,ngày lễ" },
        };
        public enum eTuyenKCBBB
        {
            TT = 0,
            NTBD = 1,
            NTD = 2,
            NTINHDEN = 3,
        }
        public static Dictionary<int, string> tuyenkcbbds = new Dictionary<int, string>()
        {
            { (int)eTuyenKCBBB.TT, "Tất cả" },
            { (int)eTuyenKCBBB.NTBD, "Nội tỉnh ban đầu" },
            { (int)eTuyenKCBBB.NTD, "Nội tỉnh đến" },
            { (int)eTuyenKCBBB.NTINHDEN, "Ngoại tỉnh đến" },
        };
        public enum eTBCN
        {
            tatCa = -1,
            chon = 0,
            khongchon = 1
        }
        public static Dictionary<Boolean, string> tbcns = new Dictionary<bool, string>()
        {
            {true, "Checkchon" },
            {false, "KhongCheckchon" },
        };
        #endregion

        #region Bộ lọc thoi gian cho form tổng hợp
        //lọc thời gian cho form tổng hợp
        public enum etracuutonghop
        {
            thang = 0,
            quy = 1,
            nam = 2,
        }
        public static Dictionary<int, string> thoiGiantracuutonghop = new Dictionary<int, string>()
        {
                { (int)etracuutonghop.nam, "Tháng" },
                  { (int)etracuutonghop.quy, "Quý" },
                    { (int)etracuutonghop.thang, "Năm" },
        };
        #endregion


        #region Bộ lọc thoi gian cho form thống kê
        //lọc thời gian cho form tổng hợp
        public enum etracuuthongke
        {
            thanga = 0,
            quya = 1,
            nama = 2,
        }
        public static Dictionary<int, string> thoiGiantracuuthongke = new Dictionary<int, string>()
        {
                { (int)etracuuthongke.nama, "Tháng" },
                  { (int)etracuuthongke.quya, "Quý" },
                    { (int)etracuuthongke.thanga, "Năm" },
        };
        #endregion

        #region Bộ lọc thoi gian cho form giám định viên hướng dẫn
        //lọc thời gian cho form giám định viên dướng dẫn
        public enum eThoiGianTracuuGDVHD
        {
            ngayra = 0,
            //ngaydhs = 1,
            quyqt = 1,
            thangqt = 2,
            namqt = 3,
        }
        public static Dictionary<int, string> thoiGianTracuugdvhd = new Dictionary<int, string>()
        {
            { (int) eThoiGianTracuuGDVHD.ngayra, "Ngày ra viện" },
              //{ (int)eThoiGianTracuuGDVHD.ngaydhs, "Ngày duyệt hồ sơ" },
                { (int)eThoiGianTracuuGDVHD.thangqt, "Tháng" },
                  { (int)eThoiGianTracuuGDVHD.quyqt, "Quý" },
                    { (int)eThoiGianTracuuGDVHD.namqt, "Năm" },

        };
        #endregion

        #region Bộ lọc tracuu form tổng hợp
        //lọc cho lí lo vào viện form tra cứu tổng hợp
        public enum lydovaovien
        {
            Dungtuyen = 1,
            Capcuu = 2,
            Traituyen = 3
        }
        public static Dictionary<int, string> lydovaovientracuutonghop = new Dictionary<int, string>()
        {
           {(int)lydovaovien.Dungtuyen, "Đúng tuyến"},
           {(int)lydovaovien.Capcuu, "Cấp cứu"},
           {(int)lydovaovien.Traituyen, "Trái tuyến"},
        };

        public enum eTuyeKCB
        {
            tatCa = 0,
            Dungtuyen = 1,
            Capcuu = 2,
            Traituyen = 3
        }
        public static Dictionary<int, string> TuyenKCB = new Dictionary<int, string>()
        {
           {(int)eTuyeKCB.tatCa, "Tất cả"},
           {(int)eTuyeKCB.Dungtuyen, "Đúng tuyến"},
           {(int)eTuyeKCB.Capcuu, "Cấp cứu"},
           {(int)eTuyeKCB.Traituyen, "Trái tuyến"},
        };

        // lọc trạng thái form tra cứu tổng hợp
        public enum eTrangThai
        {
            Moi = 0,
            /// Đã giám đinh tự động: 1
            DaGDTuDong = 1,
            /// Đã giám định chủ động: 2
            DaGDChuDong = 2,
            /// Hủy: 3
            Huy = 3,
        }
        public static Dictionary<int, string> trangthaitracuutonghop = new Dictionary<int, string>()
        {
            {(int)eTrangThai.Moi, "Mới"},
            {(int)eTrangThai.DaGDTuDong, "Đã GĐ tự động"},
            {(int)eTrangThai.DaGDChuDong, "Đã GĐ chủ động"},
            {(int)eTrangThai.Huy, "Hủy"},
        };

        // lọc loại khám chữa bệnh, mã loại khám chữa bệnh
        public enum LoaiKCB
        {
            KhamBenh = 1,
            DTNgoaiTru = 2,
            DTNoiTru = 3
        }
        public static Dictionary<int, string> loaikcbtracuu = new Dictionary<int, string>()
        {
            {(int)LoaiKCB.KhamBenh, "Khám bệnh"},
            {(int)LoaiKCB.DTNgoaiTru, "ĐT ngoại trú"},
            {(int)LoaiKCB.DTNoiTru, "ĐT nội trú"},
        };
        #endregion
        #endregion

        public static Dictionary<int, string> dictTenLoaiKhamBenh = new Dictionary<int, string>()
        {
            {1, "Khám Bệnh"},
            {2, "Điệu Trị Ngoại Trú"},
            {3, "Thận Nhân Tạo"},
            {4, "Ngoại Trú"},
            {5, "Nội Trú"},
        };
        public static Dictionary<string, string> DMCSKCB = new Dictionary<string, string>()
        {
            {"DM_THUOC_BV", "Danh mục Thuốc sử dụng tại CSKCB"},// OK
            {"DM_VATTU_BV", "Danh mục VTYT tại CSKCB" },// NOT OK
            {"DM_DICHVU_BV", "Danh mục DVKT tại CSKCB" },// ok
            {"DM_GIADICHVU_BV", "Danh mục giá DVKT tại CSKCB" },// OK
            {"DM_BACSI", "Danh mục bác sĩ"},// OK
            {"DM_KHOA","Danh mục khoa phòng"}, // OK
            {"DM_NHANVIENYTE","Danh mục nhân viên y tế"},// OK
            {"DM_HOADON_CSKCB","Danh mục hóa đơn mua thuốc, vật tư y tế"},// NOT OK
            {"DM_DVYT_CSKCB","Danh mục dịch vụ y tế cơ sở khám chữa bệnh"}// OK
            //{"DM_DVYT_CSKCB_KHAC","Danh mục dịch vụ y tế khác"}

        };

        public static Dictionary<string, string> DMTINH = new Dictionary<string, string>()
        {
            {"DM_GOITHAU", "Danh mục kết quả trúng thầu thuốc tỉnh"}, // OK
            {"DM_GIADICHVU_TINH", "Danh mục giá DVKT tỉnh" },// NOT OK
           
        };
        public static Dictionary<string, string> DMDUNGCHUNG = new Dictionary<string, string>()
        {
            { "DM_HOATCHAT", "Thuốc tân dược, phóng xạ và hợp chất đánh dấu"},
            { "DM_NHOMHOATCHAT", "Nhóm Thuốc tân dược, phóng xạ và hợp chất đánh dấu" },
            { "DM_NHOMTHUOCYHCT", "Nhóm chế phẩm vị thuốc YHCT" },
            { "DM_THUOCYHCT", "Chế phẩm vị thuốc YHCT"},
            { "DM_HUHAOYHCT", "Tỷ lệ hư hao đối với vị thuốc YHCT TT49"},
            { "DM_LOAIMAU","Loại máu"},
            { "DM_MAU","Máu và chế phẩm của máu"},
            { "DM_THUOC","Thuốc được cấp số đăng ký hoặc GPLH"},
            { "DM_THUOC_KK","Thuốc công bố giá kê khai, kê khai lại"},
            { "DM_TENDICHVU","DVKT QĐ 5084"},
            {"DM_CHUONGDICHVU43","Nhóm DVKT TT 43"},
            {"DM_DICHVU43","DVKT TT 43"},
            {"DM_CHUONGDICHVU50","Nhóm DVKT TT 50"},
            {"DM_DICHVU50","DVKT TT 50"},
            {"DM_CHISOXETNGHIEM","Xét nghiệm, hóa sinh, vi sinh QĐ 4069"},
            {"DM_CHANDOANHINHANH","Chẩn đoán hình ảnh và nội soi QĐ 4069"},
            {"DM_NHOMDICHVU","Danh mục nhóm DVKT"},
            //{"DVKT TT04" ,"DM_DICHVU04Index04" },
            //{"DVKT TT03" ,"DM_DICHVU03Index03" },
            {"DM_NHOMVATTU","Nhóm Vật tư y tế"},
            {"DM_VATTU","Vật tư y tế"},//OK
            {"DM_GIATOIDAVTYT","Khung giá tối đa VTYT"},
            {"DM_ICDCHUONG","Danh mục Chương bệnh"},
            {"DM_ICDNHOM" , "Danh mục Nhóm bệnh"},
            {"DM_ICD" , "Danh mục Bệnh"},
            // {"DM_ICDMANTINH","Danh mục bệnh mãn tính"},
            {"DM_COSOKCB","Danh mục CSKCB"},
            {"DM_DONVIHANHCHINH","Danh mục Đơn vị hành chính"},
            {"DM_QUOCGIA","Danh mục Quốc gia"},
            {"DM_TINHTHANH","Danh mục Tỉnh thành"},
            {"DM_QUANHUYEN","Danh mục Quận huyện"},
            {"DM_XAPHUONG","Danh mục Xã phường"},
            {"DM_MAHOAKHOA","Danh mục mã hoá khoa"},
            {"DM_NHASX","Danh mục nhà sản xuất"},
            {"DM_CONGTYCUNGUNG","Danh mục công ty cung ứng"},
            {"DM_DUONGDUNG","Danh mục đường dùng"},
            {"DM_DONVITINH","Danh mục đơn vị tính"}
        };

        public static Dictionary<string, string> DMKHAC = new Dictionary<string, string>()
        {
            { "DM_NHOMDTBHYT", "Danh mục đối tượng BHYT" },//OK
            { "DM_DONVI", "Danh mục cơ quan BHXH" },
            { "DM_LUONGTOITHIEU", "Danh mục bậc lương tối thiểu" },// OK
            { "DM_MUCHUONGBHYT", "Danh mục mức quyền lợi hưởng" }, // NOT OK
            { "DM_TYLETTTRAITUYEN", "Tỷ lệ thanh toán trái tuyến" },// NOT OK
            { "DM_MUCHUONGTOIDATTTT", "Mức tối đa thanh toán trực tiếp" },// OK
            { "DM_MUCDONGBQ", "Khai báo mức đóng bình quân" },// OK
            { "DM_SUATPHIBQ", "Suất phí bình quân" },//ok
            { "DM_CHUNGTUKT", "Danh mục chứng từ đính kèm" },// OK
            { "DM_DULIEUDANSO", "Dữ liệu dân số theo năm" },// OK
            { "DM_QUYBHXH", "Danh mục quỹ BHXH" },//OK
            { "DM_NGAYNGHILE", "Danh mục ngày nghỉ lễ" },// OK
            { "DM_QUYTRINH", "Danh mục Quy trình DVKT" },//OK
            { "DM_VANBANQUYETDINH", "Danh mục Văn bản tài liệu" },//ok
            // { "DM_TUONGTACTHUOC", "Tương tác thuốc và chú ý khi chỉ định" },
            { "DM_TRACUU", "Tra cứu quy trình" },// NOT OK
            { "DM_GIAXANG", "Danh mục giá xăng" }, // OK
            { "DM_NHOMQUYTRINH", "Danh mục nhóm quy trình" }//NOT OK
        };

        public static Dictionary<int, string> PHIENTD = new Dictionary<int, string>()
        {
            { 0, "Không phiên" },
            { 1, "Đã phiên" },
            { 2, "Chưa phiên" }
        };

        public enum TypeRole
        {
            CapQuanTri = 0,
            CapTrungUong = 1,
            CapTinh = 2,
            CapKhac = 3,
        };

        public enum PermissionItems
        {
            Them = 0,
            Xem = 1,
            Sua = 2,
            Xoa = 3,
            Duyet = 4,
            Khac = 5
        };
        public static Dictionary<int, string> SelectListPermissionItems = new Dictionary<int, string>()
        {
            //thanhpt phân quyền
            { (int)PermissionItems.Them,"Thêm"},
            { (int)PermissionItems.Xem, "Xem"},
            { (int)PermissionItems.Sua, "Sửa"},
            { (int)PermissionItems.Xoa, "Xóa"},
            { (int)PermissionItems.Duyet, "Duyệt"},
            { (int)PermissionItems.Khac, "Khác"},
        };
        public enum eRole
        {
            GiamDinhVien = 0,
            TruongNhom = 1,
            LanhDao = 2,
        }

        /// <summary>
        /// list item cho combobox loại ánh xạ khi chọn loại thuốc là tất cả
        /// </summary>
        public static Dictionary<int, string> dictforAll = new Dictionary<int, string>()
        {
            { 1, "Danh mục thầu" },
            { 2, "Thông tư 40" },
            { 3, "Thông tư 05" },
            { 4, "Thuốc có SĐK" },
            { 5, "Thông tư 31" },
            { 6, "Thông tư 12" },
        };
        /// <summary>
        /// list item cho combobox loại ánh xạ khi chọn loại thuốc là tân dược hoặc phóng xạ
        /// </summary>
        public static Dictionary<int, string> dictfor40 = new Dictionary<int, string>()
        {
            { 1, "Danh mục thầu" },
            { 2, "Thông tư 40" },
            //{ 3, "Thông tư 05" },
            { 4, "Thuốc có SĐK" },
            { 5, "Thông tư 31" },
            //{ 6, "Thông tư 12" },
        };
        /// <summary>
        /// list item cho combobox loại ánh xạ khi chọn loại thuốc là chế phẩm hay vị thuốc
        /// </summary>
        public static Dictionary<int, string> dictfor05 = new Dictionary<int, string>()
        {
            { 1, "Danh mục thầu" },
            //{ 2, "Thông tư 40" },
            { 3, "Thông tư 05" },
            { 4, "Thuốc có SĐK" },
            //{ 5, "Thông tư 31" },
            { 6, "Thông tư 12" },
        };
        /// <summary>
        /// list item cho combobox loại ánh xạ khi chọn loại thuốc là tất cả
        /// </summary>
        public static Dictionary<int, string> dictforAll_Thau = new Dictionary<int, string>()
        {
            //{ 1, "Danh mục thầu" },
            { 2, "Thông tư 40" },
            { 3, "Thông tư 05" },
            { 4, "Thuốc có SĐK" },
            { 5, "Thông tư 31" },
            { 6, "Thông tư 12" },
        };
        /// <summary>
        /// list item cho combobox loại ánh xạ khi chọn loại thuốc là tân dược hoặc phóng xạ
        /// </summary>
        public static Dictionary<int, string> dictfor40_Thau = new Dictionary<int, string>()
        {
            //{ 1, "Danh mục thầu" },
            { 2, "Thông tư 40" },
            //{ 3, "Thông tư 05" },
            { 4, "Thuốc có SĐK" },
            { 5, "Thông tư 31" },
            //{ 6, "Thông tư 12" },
        };
        /// <summary>
        /// list item cho combobox loại ánh xạ khi chọn loại thuốc là chế phẩm hay vị thuốc
        /// </summary>
        public static Dictionary<int, string> dictfor05_Thau = new Dictionary<int, string>()
        {
            //{ 1, "Danh mục thầu" },
            //{ 2, "Thông tư 40" },
            { 3, "Thông tư 05" },
            { 4, "Thuốc có SĐK" },
            //{ 5, "Thông tư 31" },
            { 6, "Thông tư 12" },
        };
        /// <summary>
        /// list item cho combobox loại thuốc 
        /// </summary>
        public static Dictionary<int, string> LoaiThuocAXs = new Dictionary<int, string>()
        {
            { 0, "loại không xác định" },
            { 1, "Tân dược" },
            { 2, "Chế phẩm" },
            { 3, "Vị thuốc" },
            { 4, "Phóng xạ" },
            { 5, "Tân dược tự bào chế" },
            { 6, "Chế phẩm tự bào chế" },
        };

        public const String MA_KHOA_CUA_CAC_KHOA_CON_LAI = "CKCL";

        public static Dictionary<int, string> KY_BAO_CAO = new Dictionary<int, string>()
        {
            { 1, "Theo tháng"},
            { 2, "Theo quý"},
            { 3, "Theo năm"}
        };

        public static List<int> lstSelectTop = new List<int>() { 100, 200, 500, 1000 };

        public enum eLoaiThau
        {
            ThauTapTrung = 1,//
            ThauRieng,//
            NgoaiThau,//
        }
        public static Dictionary<int, string> LoaiThaus = new Dictionary<int, string>()
        {
            {(int)eLoaiThau.ThauTapTrung, "Thầu tập trung"},
            {(int)eLoaiThau.ThauRieng, "Thầu riêng"},
            {(int)eLoaiThau.NgoaiThau, "Ngoài thầu"},
        };
        public enum ePhongban
        {
            phonggiamdinh = 1
        }
        public static Dictionary<int, string> Loaiphongban = new Dictionary<int, string>()
        {
            {(int)ePhongban.phonggiamdinh, "Phòng giám định"},

        };
        public enum eTrangThaiAX
        {
            ChuaAnhXa = 0,
            DaAnhXaTD = 1,
            DaAnhXaCD = 2,
        }
        public static Dictionary<int, string> TrangThaiAXs = new Dictionary<int, string>()
        {
            { (int)eTrangThaiAX.ChuaAnhXa, "Chưa ánh xạ" },
            { (int)eTrangThaiAX.DaAnhXaTD, "Đã ánh xạ tự động" },
            { (int)eTrangThaiAX.DaAnhXaCD, "Đã ánh xạ chủ động" },
        };
        public enum eLoaiAX
        {
            /// <summary>
            /// dm thầu = 1
            /// </summary>
            Thau = 1,
            /// <summary>
            /// dm hoạt chất = 2
            /// </summary>
            HoatChat = 2,
            /// <summary>
            /// dm yhct = 3
            /// </summary>
            YHCT = 3,
            /// <summary>
            /// dm dung chung = 4
            /// </summary>
            DungChung = 4,
            /// <summary>
            /// tan duoc tu bao che = 5
            /// </summary>
            DanhMuc31 = 5,
            /// <summary>
            /// che pham tu bao che = 6
            /// </summary>
            DanhMuc12 = 6,
        }

        public static Dictionary<int, string> LoaiAnhXa = new Dictionary<int, string>()
        {
            { (int)eLoaiAX.Thau, "Danh mục thầu" },
            { (int)eLoaiAX.HoatChat, "Thông tư 40" },
            { (int)eLoaiAX.YHCT, "Thông tư 05" },
            { (int)eLoaiAX.DungChung, "Thuốc có SĐK" },
            { (int)eLoaiAX.DanhMuc31, "Thông tư 31" },
            { (int)eLoaiAX.DanhMuc12, "Thông tư 12" },
        };
        public enum eLoaiThuocThau
        {
            TatCa = 0,
            TanDuoc = 1,
            ChePham = 2,
            ViThuoc = 3,
            PhongXa = 4,
        }
        public static Dictionary<int, string> LoaiThuocThaus = new Dictionary<int, string>()
        {
             { (int)eLoaiThuocThau.TatCa, "Tất cả" },
             { (int)eLoaiThuocThau.TanDuoc, "Tân dược" },
             { (int)eLoaiThuocThau.ChePham, "Chế phẩm" },
             { (int)eLoaiThuocThau.ViThuoc, "Vị thuốc" },
             { (int)eLoaiThuocThau.PhongXa, "Phóng xạ" },
        };
        public static string ConfigPermission = "BanKhongCoQuyenThucHienChucNangNay";

        public static long ConvertNgayThangToLong()
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            var hh = DateTime.Now.Hour;
            var mm = DateTime.Now.Minute;

            string m = month < 10 ? "0" + month.ToString() : month.ToString();
            string d = day < 10 ? "0" + day.ToString() : day.ToString();
            string h = hh < 10 ? "0" + hh.ToString() : hh.ToString();
            string minute = mm < 10 ? "0" + mm.ToString() : mm.ToString();

            string res = year.ToString() + m + d + h + minute;
            long r = 0;
            long.TryParse(res, out r);
            return r;
        }

        public enum ELoaiTrangThaiRequest_log
        {
            DANG_XU_LY = 0,
            DA_XU_LY = 1,
            TU_CHOI_NHAN = 2,
            TAT_CA = 3
        }

        public static Dictionary<int, string> CacLoaiTrangThaiRequest_log = new Dictionary<int, string>()
        {
            { (int)ELoaiTrangThaiRequest_log.DANG_XU_LY, "Đang xử lý"},
            { (int)ELoaiTrangThaiRequest_log.DA_XU_LY, "Đã xử lý"},
            { (int)ELoaiTrangThaiRequest_log.TU_CHOI_NHAN, "Từ chối"},
            { (int)ELoaiTrangThaiRequest_log.TAT_CA, "Tất cả"},
        };
        public enum eRequestType
        {
            doiSoat = 1,
            hsTrungLap = 2
        }
        public enum LOAITRANGTHAIDMCSKCB
        {
            KHONG_HL = 0,
            CO_HL = 1,
            DE_NGHI_TAM_NGUNG_HD = 2,
            DE_NGHI_KH_HD_NHO_HON_6 = 3,
            DE_NGHI_KH_HD_LON_HON_6 = 4
        }

        public enum TRANGTHAIBHYT_13
        {
            TAO_MOI = 1,
            CHO_DUYET = 2,
            TU_CHOI = 3,
            DUYET = 4,
            HUY_DUYET = 5
        }
        public enum TRANGTHAIBHYT_192021
        {
            MOI = 0,
            DANG_XULY = 1,
            HOAN_THANH = 2,
            LOI = 3,
        }
        public enum etrangthaiDK
        {
            tatca = 0,
            TaiCSKCB = 1,
            noitinhden = 2,
            ngoaitinhden = 3,
            Nhomtheodkbd = 4,
            //trunglap = 5,
        }
        public enum ebieumau192021
        {
            bieumau19 = 1,
            bieumau20 = 2,
            bieumau21 = 3,
            
        }
        public enum eTichbieumau192021
        {
            tich = 1,
            khongtich = 0,
        }
        public enum QuyTacLoi
        {
            tatca=5,
            danhmuc = 0,
            thuoc =1,
            dichvu=2,
            vattu=3
        }
        public static Dictionary<int, string> QuyTacLoiForXoaHoso = new Dictionary<int, string>()
        {
            { (int)QuyTacLoi.tatca, "Tất cả"},
            { (int)QuyTacLoi.danhmuc, "Các danh mục (Thuốc, DVKT, VTYT) không nằm trong DM được BHYT phê duyệt"},
            { (int)QuyTacLoi.thuoc, "Thuốc không nằm trong DM được BHYT phê duyệt"},
            { (int)QuyTacLoi.dichvu, "DVKT không nằm trong DM được BHYT phê duyệt"},
            { (int)QuyTacLoi.vattu, "VTYT không nằm trong DM được BHYT phê duyệt"},
        };
        public static Dictionary<int, string> CacTrangThaiBHYT_13 = new Dictionary<int, string>()
        {
            { (int)TRANGTHAIBHYT_13.TAO_MOI, "Tạo mới"},
            { (int)TRANGTHAIBHYT_13.CHO_DUYET, "Chờ duyệt"},
            { (int)TRANGTHAIBHYT_13.TU_CHOI, "Từ chối duyệt"},
            { (int)TRANGTHAIBHYT_13.DUYET, "Duyệt"},
            { (int)TRANGTHAIBHYT_13.HUY_DUYET, "Hủy duyệt"},
        };
        public static Dictionary<int, string> CacTrangThaiBHYT_192021 = new Dictionary<int, string>()
        {
            { (int)TRANGTHAIBHYT_192021.MOI, "Mới"},
            { (int)TRANGTHAIBHYT_192021.DANG_XULY, "Đang xử lý"},
            { (int)TRANGTHAIBHYT_192021.HOAN_THANH, "Hoàn thành"},
            { (int)TRANGTHAIBHYT_192021.LOI, "Lỗi"},
        };
        public static Dictionary<int, string> etrangthaiDKBD = new Dictionary<int, string>()
        {
            { (int)etrangthaiDK.tatca, "Tất cả" },
            { (int)etrangthaiDK.TaiCSKCB, "Tại CSKCB" },
            { (int)etrangthaiDK.noitinhden, "Nội tỉnh đến" },
            { (int)etrangthaiDK.ngoaitinhden, "Ngoại tỉnh đến" },
            { (int)etrangthaiDK.Nhomtheodkbd, "Nhóm theo ĐK KCBBĐ" },
        };
        public static Dictionary<int, string> eBMau = new Dictionary<int, string>()
        {
            { (int)ebieumau192021.bieumau19, "19/BHYT" },
            { (int)ebieumau192021.bieumau20, "20/BHYT" },
            { (int)ebieumau192021.bieumau21, "21/BHYT" },
           
        };
        public static Dictionary<int, string> eCon = new Dictionary<int, string>()
        {
            { (int)eTichbieumau192021.tich, "Cả CS cha và CS con" },
            { (int)eTichbieumau192021.khongtich, "không xét CS con" },
            

        };

        public readonly static Dictionary<int, string> eLoaiBaoCaos = new Dictionary<int, string>()
        {
            { (int)eLoaiBaoCao.mau01, "Mẫu 01/TE" },
            { (int)eLoaiBaoCao.mauTonghop, "Mẫu tổng hợp" },
            { (int)eLoaiBaoCao.mauChiTiet, "Mẫu chi tiết"}
        };

        public static readonly Dictionary<bool, string> DTrangThaiChotSoLieu = new Dictionary<bool, string>()
        {
            { true, "Đã chốt số liệu" },
            { false, "Chưa chốt số liệu" }
        };
        public enum eKetQua
        {
            /// <summary>
            /// Chấp nhận: 1
            /// </summary>
            ChapNhan = 1,
            /// <summary>
            /// Cảnh báo: 2
            /// </summary>
            CanhBao = 2,
            /// <summary>
            /// Xuất toán 1 phần: 3
            /// </summary>
            XuatToan1Phan = 3,
            /// <summary>
            /// Xuất toán toàn bộ: 4
            /// </summary>
            XuatToanToanBo = 4, // xuat toan toan bo
            /// <summary>
            /// Trùng lặp: 5
            /// </summary>
            TrungLap = 5
        }
        public enum eKetQua_TD
        {
            /// <summary>
            /// Tất cả: 5
            /// </summary>
            TatCa = 5,
            /// <summary>
            /// Chấp nhận: 1
            /// </summary>
            ChapNhan = 1,
            /// <summary>
            /// Cảnh báo: 2
            /// </summary>
            CanhBao = 2,
            /// <summary>
            /// Xuất toán 1 phần: 3
            /// </summary>
            XuatToan1Phan = 3,
            /// <summary>
            /// Xuất toán toàn bộ: 4
            /// </summary>
            XuatToanToanBo = 4, // xuat toan toan bo
            Tongxuattoan = 7,
        }

        public enum GetDuLieuBc_14
        {
            Th_Excel = 1,
            Pdf
        }
        public enum eLoaiThongKe
        {
            vuotThau = 1,
            trongHanMuc = 2,
            Tatca = 0,

        }
        public readonly static Dictionary<int, string> LoaiThongKe = new Dictionary<int, string>()
        {
            { (int)eLoaiThongKe.Tatca, "Tất cả" },
            { (int)eLoaiThongKe.vuotThau, "Vượt thầu" },
            { (int)eLoaiThongKe.trongHanMuc, "Trong hạn mức"}
        };

        public static Dictionary<int, string> thoigian_DaTuyenDi = new Dictionary<int, string>()
        {
            { (int) eThoiGian.quy, "Quý" },
            { (int) eThoiGian.thang, "Tháng" },
        };


        public readonly static Dictionary<int, string> maLyDoVaoVienDa_TuyenDi = new Dictionary<int, string>()  
        {
            { (int)eLyDoVaoVien.dungTuyen, DaTuyenDi.LyDoVaoVien01 },
            { (int)eLyDoVaoVien.capCuu, DaTuyenDi.LyDoVaoVien02 },
            { (int)eLyDoVaoVien.traiTuyen, DaTuyenDi.LyDoVaoVien03 }
        };

        public readonly static Dictionary<int, string> ketquaDtDa_TuyenDi = new Dictionary<int, string>()
        {
            { (int)ketquaDt.khoi, DaTuyenDi.KqDieuTri01 },
            { (int)ketquaDt.Do, DaTuyenDi.KqDieuTri02 },
            { (int)ketquaDt.khongthaydoi, DaTuyenDi.KqDieuTri03 },
            { (int)ketquaDt.nanghon, DaTuyenDi.KqDieuTri04 },
            { (int)ketquaDt.tuvong, DaTuyenDi.KqDieuTri05 },
        };

        public readonly static Dictionary<int, string> maloaiKcbDa_TuyenDi = new Dictionary<int, string>()  
        {
            { (int)maloaiKcb.khambenh, DaTuyenDi.LoaiKcb01 },
            { (int)maloaiKcb.dieutrinoitru, DaTuyenDi.LoaiKcb02 },
            { (int)maloaiKcb.dieutringoaitru, DaTuyenDi.LoaiKcb03 }
        };
        public readonly static Dictionary<int, string> CcLoai_thuoc = new Dictionary<int, string>()
        {
            { (int)Thuoc_Loai.TanDuoc, "Tân dược"},
             {(int)Thuoc_Loai.ChePham, "Chế phẩm YHCT"},
             {(int)Thuoc_Loai.PhongXa, "Phóng xạ"},
             {(int)Thuoc_Loai.ViThuoc, "Vị thuốc YHCT"},
        };
    }
}