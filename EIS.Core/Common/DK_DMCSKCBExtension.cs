using EIS.Core.Domain;

namespace EIS.Core.Common
{
    public static class DK_DMCSKCBExtension
    {
        public static void ThemMoi(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 0;
            model.TTPHEDUYET = 4;
        }

        public static void ChoDuyetDangKy(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 0;
            model.TTPHEDUYET = 0;
        }

        public static void DuyetDangKyChoActive(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 1;
            model.TTPHEDUYET = 1;
        }

        public static void ActiveCSKCB(this DM_COSOKCB model)
        {
            model.HIEULUC = true;
            model.TRANGTHAI = 1;
            model.TTPHEDUYET = 1;
        }

        public static void DeNghiTamNgung(this DM_COSOKCB model)
        {
            model.HIEULUC = true;
            model.TRANGTHAI = 2;
            model.TTPHEDUYET = 5;
        }

        public static void DuyetTamNgung(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 2;
            model.TTPHEDUYET = 6;
        }

        public static void DenNghiActiveHopDongNhoHon6Thang(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 3;
            model.TTPHEDUYET = 11;
        }

        public static void PheDuyetHopDongNhoHon6Thang(this DM_COSOKCB model)
        {
            model.HIEULUC = true;
            model.TRANGTHAI = 3;
            model.TTPHEDUYET = 1;
        }

        public static void DeNghiActiveHopDongLonHon6Thang(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 4;
            model.TTPHEDUYET = 13;
        }

        public static void PheDuyetHopDongLonHon6Thang(this DM_COSOKCB model)
        {
            model.HIEULUC = true;
            model.TRANGTHAI = 4;
            model.TTPHEDUYET = 1;
        }

        public static void HuyDuyetDangKy(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 0;
            model.TTPHEDUYET = 2;
        }

        public static void HuyDeNghiTamNgung(this DM_COSOKCB model)
        {
            model.HIEULUC = true;
            model.TRANGTHAI = 2;
            model.TTPHEDUYET = 7;
        }

        public static void HuyDeNghiActiveNhoHon6Thang(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 3;
            model.TTPHEDUYET = 12;
        }

        public static void HuyDeNghiActiveLonHon6Thang(this DM_COSOKCB model)
        {
            model.HIEULUC = false;
            model.TRANGTHAI = 4;
            model.TTPHEDUYET = 14;
        }

        public static bool TrangThaiChoDuyetDieuChinhThongTin(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 1 && model.TTPHEDUYET == 8;
        }
        public static bool TrangThaiTuChoiDuyetDieuChinhThongTin(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 0 && model.TTPHEDUYET == 10;
        }

        public static bool TrangThaiDuyetDeNghiTamNgung(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 2 && model.TTPHEDUYET == 6 && model.HIEULUC == false;
        }

        public static bool TaoMoi(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 0 && model.TTPHEDUYET == 4 && model.HIEULUC == false;
        }

        public static bool ChoDuyetDangKyCapMoi(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 0 && model.TTPHEDUYET == 0 && model.HIEULUC == false;
        }

        public static bool ChoKichHoat(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 1 && model.TTPHEDUYET == 1 && model.HIEULUC == false;
        }

        public static bool TuChoiDuyetCapMoi(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 0 && model.TTPHEDUYET == 2 && model.HIEULUC == false;
        }

        public static bool CdActiveHopDongNhoHon6Thang(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 3 && model.TTPHEDUYET == 11 && model.HIEULUC == false;
        }

        public static bool CdActiveHopDongLonHon6Thang(this DM_COSOKCB model)
        {
            return model.TRANGTHAI == 4 && model.TTPHEDUYET == 13 && model.HIEULUC == false;
        }
    }
}