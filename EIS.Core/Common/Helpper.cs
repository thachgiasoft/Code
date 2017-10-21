using System;

namespace EIS.Core.Common
{
    public static class Helpper
    {
        public const string LOAI_DIEU_TRI_KHAM_BENH = "KHB";

        public const string LOAI_CAC_KHOA_CON_LAI = "CKCL";

        public const string LOAI_DIEU_TRI_NGOAI_TRU = "NGT";

        public const string LOAI_DIEU_TRI_THAN_NHAN_TAO = "TNT";

        public const string TEN_LOAI_DIEU_TRI_KHAM_BENH = "Khám Bệnh";

        public const string TEN_CAC_KHOA_CON_LAI = "Các khoa còn lại";

        public const string TEN_LOAI_DIEU_TRI_NGOAI_TRU = "Điều Trị Ngoại Trú";

        public const string TEN_LOAI_DIEU_TRI_THAN_NHAN_TAO = "Thận Nhân Tạo";

        public const string TEN_LOAI_NOI_TRU = "Nội Trú";

        public const string TEN_LOAI_NGOAI_TRU = "Ngoại Trú";

        public const string MA_BHXHVN = "BHXHVN";

        public const string ERROR_SYSTEM = "Xẩy ra lỗi hệ thống, hãy thử lại sau.";
    }

    public static class DMCSKCBTextHelper
    {
        public const string TRANG_THAI__KHONG_CO_HIEU_LUC = "không có hiệu lực";

        public const string TRANG_THAI_CO_HIEU_LUC = "Có hiệu lực";

        public const string TRANG_THAI_DE_NGHI_TAM_NGUNG_HD = "Đề nghị tạm ngưng hợp đồng";

        public const string TRANG_THAI_DE_NGHI_KICH_HOAT_HD_NHO_HON_6_THANG = "Đề nghị kích hoạt hợp đồng <= 6 tháng";

        public const string TRANG_THAI_DE_NGHI_KICH_HOAT_HD_LON_HON_6_THANG = "Đề nghị kích hoạt hợp đồng > 6 tháng";
    }

    public static class SuccessCosoKcbHelper
    {
        public const string THANHCONG = "OK!";

        public const string success = "OK!|THÀNH CÔNG!";

        public const string successDeleteCsvc = "OK!|Xóa cơ sở vật chất nhân lực thành công!";

        public const string successDeleteNvyt = "OK!|Xóa nhân viên y tế thành công!";

        public const string successThemMoi = "OK!|Lưu thêm mới cơ sở THÀNH CÔNG!";
                        
        public const string successCdCapMoi = "OK!|Chuyển duyệt cấp mới cơ sở THÀNH CÔNG!";
             
        public const string successTcCapMoi = "OK!|Từ chối cấp mới cơ sở THÀNH CÔNG!";
                      
        public const string successDyCapMoi = "OK!|Đồng ý cấp mới cơ sở THÀNH CÔNG!";
                     
        public const string successLuuActivate = "OK!|Lưu active cơ sở THÀNH CÔNG!";
                           
        public const string successTcDieuChinhThongTin = "OK!|Từ chối điều chỉnh thông tin cơ sở THÀNH CÔNG!";
                         
        public const string successDyDieuChinhThongTin = "OK!|Đồng ý điều chỉnh thông tin cơ sở THÀNH CÔNG!";
                           
        public const string successLuuDieuChinhThongTin = "OK!|Lưu điều chỉnh thông tin cơ sở THÀNH CÔNG!";
                          
        public const string successLuuGiaHanHopDong = "OK!|Gia hạn hợp đồng cơ sở THÀNH CÔNG!";
                          
        public const string successCdDieuChinhThongTin = "OK!|Chuyển duyệt điều chỉnh thông tin cơ sở THÀNH CÔNG!";
                       
        public const string successLuuNgungHopDong = "OK!|Lưu ngưng hợp đồng cơ sở THÀNH CÔNG!";
                           
        public const string successCdActivatebe = "OK!|Chuyển duyệt kích hoạt HĐ <= 6 tháng THÀNH CÔNG!";
                           
        public const string successCdActivatelon = "OK!|Chuyển duyệt kích hoạt HĐ > 6 tháng THÀNH CÔNG!";
                           
        public const string successTcActivatebe = "OK!|Từ chối kích hoạt HĐ <= 6 tháng THÀNH CÔNG!";
                            
        public const string successDyActivatebe = "OK!|Đồng ý kích hoạt HĐ <= 6 tháng THÀNH CÔNG!";
                        
        public const string successTcActivatelon = "OK!|Từ chối kích hoạt HĐ > 6 tháng THÀNH CÔNG!";
                          
        public const string successDyActivatelon = "OK!|Đồng ý kích hoạt HĐ > 6 tháng THÀNH CÔNG!";
                          
        public const string successKhongHienThiMa = "OK!|KHONGHIENTHI!";
    }

    public static class ErrorCosoKcbHelper
    {
        //public const string errorTrungMa01 = "TRUNGMA!";

        public static readonly string errorMaQua1000Store = "-99999";

        public const string errorDeleteCsvc = "NO!|Xóa cơ sở vật chất nhân lực không thành công!";     

        public const string errorFtp = "NO!|Đã xảy ra lỗi khi đẩy lên FTP, vui lòng thử lại sau!";

        public const string errorTrungMa = "TRUNGMA!|Mã đã trùng, đã tạo ra mã mới, vui lòng sử dụng mã mới hoặc điền lại thông tin!";

        public const string errorChuyenDuyet = "NO!|Chuyển duyệt thất bại, vui lòng kiểm tra lại dữ liệu của cơ sở, hoặc liên hệ với ban quản trị!";

        public const string errorDongY = "NO!|Đồng ý thất bại, vui lòng kiểm tra lại dữ liệu của cơ sở, hoặc liên hệ với ban quản trị!";

        public const string errorTuChoi = "NO!|Từ chối thất bại, vui lòng kiểm tra lại dữ liệu của cơ sở, hoặc liên hệ với ban quản trị!";

        public const string errorLoiTryCatch = "NO!|Đã xảy ra lỗi, vui lòng thử lại sau, hoặc liên hệ với ban quản trị!";

        public const string errorLoiInportCsVc = "NO!|Đã xảy ra lỗi trong quá trình inport file cơ sở vật chất nhân lực, vui lòng thử lại sau!";

        public const string errorLoiInportNvyt = "NO!|Đã xảy ra lỗi trong quá trình inport file nhân viên y tế, vui lòng thử lại sau!";

        public const string errorLoiAddCsvcLoaiGia = "NO!|Đã xảy ra lỗi trong quá trình lưu sang cơ sở khám chữa bệnh loại giá, vui lòng thử lại sau!";

        public const string errorHetPhienLamViec = "NO!|Hết phiên làm việc, mời bạn đăng nhập lại, hoặc thử lại sau!";      

        public const string errorGhiChuQuaGioiHan = "NO!|Trường ghi chú đã vượt quá giới hạn cho phép, thông báo cho trung ương để thực hiện điều chỉnh, hoặc liên hệ ban quản trị!";

        public const string errorMaqua1000 = "NO!|Mã cơ sở KCB đã vượt quá 1000, hiện tại không thể thêm mới hay sửa đổi thông tin cho cơ sở này!";

        public const string errorKhongTonTaiCoSo = "NO!|Cơ sở không tồn tại!";

        public const string errorKhongCoBanGhi = "NO!|Không có bản ghi nào có thể xóa!";

        public const string errorDeleteNvyt = "NO!|Xóa nhân viên y tế không thành công!";

    }


    public static class TrangThaiPheDuyet
    {

        public static readonly string ThemMoi = "Thêm mới";

        public static readonly string DuyetDangKyGrid = "Duyệt đăng ký";

        public const string ChoDuyetCapMoi = "Chờ duyệt cấp mới";

        public const string DuyetCapMoi = "Duyệt cấp mới";

        public const string TuChoiDuyetCapMoi = "Từ chối duyệt cấp mới";

        public const string DaDuyetActive = "Đã duyệt active";

        public const string NgungHopDong = "Ngừng hợp đồng";

        public const string ChoDuyetDieuChinhThongTin = "Chờ duyệt điều chỉnh thông tin";

        public const string DuyetDieuChinhThongTin = "Duyệt điều chỉnh thông tin";

        public const string TuChoiDuyetDieuChinhThongTin = "Từ chối duyệt điều chỉnh thông tin";

        public const string ChoDuyetActiveMin = "Chờ duyệt Active HĐ <= 6 tháng";

        public const string HuyDuyetActiveMin = "Hủy đề nghị active <= 6 tháng";

        public const string DuyetActiveMin = "Phê duyệt hợp đồng <= 6 tháng";

        public const string ChoDuyetActiveMax = "Chờ duyệt Active HĐ > 6 tháng";

        public const string HuyDuyetActiveMax = "Hủy đề nghị active > 6 tháng";

        public const string DuyetActiveMax = "Phê duyệt hợp đồng > 6 tháng";

    }

    public static class ValidateNameInportDkCosoKcb
    {
        public const string CsvcnlName = "CSVC";

        public const string NvytName = "NVYT";
    }

    public static class NameMotaFileDinhKem
    {
        public const string Mau02 = "02/BHYT";

        public const string Gphd = "Mẫu giấy phép hoạt động";

        public const string Cvdn = "Công văn đề nghị của tỉnh";

        public const string Dmdvkt = "Quyết định phê duyệt danh mục dịch vụ kĩ thuật thực hiện tại cơ sở";

        public const string HopDong = "Hợp đồng";

        public const string CacloaiHsKhac = "Các loại hồ sơ khác";
    }

    public static class ValidateNameDkFileDkCosoKcb
    {
        public const string Mau02 = "02";

        public const string Gphd = "GPHD";

        public const string Cvdn = "CV";

        public const string Dmdvkt = "QD";
    }


    public static class LoiCallStore
    {
        public const int ErrorStore = -99;
    }


    public static class ErrorGeneral
    {
        public const int EndOfSession = -101;
    }


    public static class DmCsKcbTextHelper
    {
        public const string TuyenCmkt01 = "Trung ương";

        public const string TuyenCmkt02 = "Tỉnh";

        public const string TuyenCmkt03 = "Huyện";

        public const string TuyenCmkt04 = "Xã";


        public const string HangBenhVien = "Hạng ";

        public const string HangBenhVien01 = "Hạng 1";

        public const string HangBenhVien02 = "Hạng 2";

        public const string HangBenhVien03 = "Hạng 3";

        public const string HangBenhVien04 = "Hạng 4";

        public const string HangBenhVien0 = "Hạng đặc biệt";

        public const string HangBenhVien05 = "Chưa xếp hạng";


        public const string LoaiBenhVien01 = "Công lập";

        public const string LoaiBenhVien02 = "Ngoài công lập";


        public const string IsHieuLucCo = "Có";

        public const string IsHieuLucKhong = "Không";


        public const string ThongTuPheDuyet0 = "Chờ duyệt cấp mới";

        public const string ThongTuPheDuyet01 = "Duyệt đăng ký";

        public const string ThongTuPheDuyet02 = "Từ chối duyệt cấp mới";

        public const string ThongTuPheDuyet04 = "Thêm mới";

        public const string ThongTuPheDuyet06 = "Ngừng hợp đồng";

        public const string ThongTuPheDuyet08 = "Chờ duyệt điều chỉnh thông tin";

        public const string ThongTuPheDuyet09 = "Duyệt điều chỉnh thông tin";

        public const string ThongTuPheDuyet10 = "Từ chối duyệt điều chỉnh thông tin";

        public const string ThongTuPheDuyet11 = "Chờ duyệt Active HĐ <= 6 tháng";

        public const string ThongTuPheDuyet12 = "Hủy đề nghị active <= 6 tháng";

        public const string ThongTuPheDuyet13 = "Chờ duyệt Active HĐ > 6 tháng";

        public const string ThongTuPheDuyet14 = "Hủy đề nghị active > 6 tháng";


        public const string TuChu01 = "Tự chủ";

        public const string TuChu02 = "Không tự chủ";


        public const string TrangThai0 = "Cấp mới";

        public const string TrangThai01 = "Điều chỉnh nội dung";

        public const string TrangThai02 = "Ngừng hợp đồng";

        public const string TrangThai03 = "Đề nghị kích hoạt hợp HĐ <= 6 tháng";

        public const string TrangThai04 = "Đề nghị kích hoạt HĐ > 6 tháng";


        public const string DangKyKcbBanDau0 = "Không nhận đăng ký KCBBĐ";

        public const string DangKyKcbBanDau01 = "Nhận đăng ký KCBBĐNT";

        public const string DangKyKcbBanDau02 = "Nhận đăng ký KCBBĐ nội, ngoại tỉnh (ngoại tỉnh có điều kiện)";

        //kiểu bệnh viên

        public const string KieuBenhVien01 = "BV - Bệnh viện đa khoa";

        public const string KieuBenhVien02 = "PK - Phòng khám đa khoa";

        public const string KieuBenhVien03 = "BBVCSSK - Ban bảo vệ chăm sóc sức khỏe";

        public const string KieuBenhVien04 = "TTYTCK - Trung tâm y tế chuyên khoa";

        public const string KieuBenhVien05 = "YTCQ - Y tế cơ quan";

        public const string KieuBenhVien06 = "TTYT - Trung tâm y tế";

        public const string KieuBenhVien07 = "TYT - Trạm y tế";

        public const string KieuBenhVien08 = "BX - Bệnh xá";

        public const string KieuBenhVien09 = "NHS - Nhà hộ sinh";

        public const string KieuBenhVien10 = "BV - Bệnh viện YHCT";

        public const string KieuBenhVien11 = "BV - Bệnh viện chuyên khoa";

        public const string KieuBenhVien12 = "PK - Phòng khám chuyên khoa";


        // khám chữa bệnh
        public const string KhamChuaBenh01 = "Không KCB";

        public const string KhamChuaBenh02 = "Có khám chữa bệnh";


        public const string HinhThucThanhToan01 = "Theo phí dịch vụ";

        public const string HinhThucThanhToan02 = "Theo định suất";


        public const string LoaiDonViChuQuan01 = "BYT";

        public const string LoaiDonViChuQuan02 = "SYT";

        public const string LoaiDonViChuQuan03 = "Y tế bộ/ngành";

        public const string LoaiDonViChuQuan04 = "Khác";



        public const string LoaiHopDong01 = "KCB BHYT ngoại trú";

        public const string LoaiHopDong02 = "KCB BHYT nội trú";

        public const string LoaiHopDong03 = "KCB BHYT nội, ngoại trú";

        public const string LoaiHopDong04 = "Thanh toán trực tiếp";
    }


    public static class DaTuyenDi
    {
        public const string LyDoVaoVien01 = "Đúng tuyến";

        public const string LyDoVaoVien02 = "Cấp cứu";

        public const string LyDoVaoVien03 = "Trái tuyến";



        public const string LoaiKcb01 = "Khám bệnh";

        public const string LoaiKcb02 = "Điều trị nội trú";

        public const string LoaiKcb03 = "Điều trị ngoại trú";


        public const string KqDieuTri01 = "Khỏi";

        public const string KqDieuTri02 = "Đỡ";

        public const string KqDieuTri03 = "Không thay đổi";

        public const string KqDieuTri04 = "Nặng hơn";

        public const string KqDieuTri05 = "Tử vong";
    }


    public static class DsGdhosokcb192021New
    {
        public const string LyDoVaoVien04 = "KCB BĐ";

        public const string LyDoVaoVien05 = "Chuyển đến";



        public const string MaLoaiKcb01 = "Ngoại trú";

        public const string MaLoaiKcb02 = "Khám bệnh";

        public const string MaLoaiKcb03 = "Nội trú";
    }
}