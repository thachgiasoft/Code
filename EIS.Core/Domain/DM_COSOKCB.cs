using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EIS.Core.Domain
{
    public class DM_COSOKCB
    {
        public virtual long ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên !")]
        [StringLength(255, ErrorMessage = "Tên không được quá 255 kí tự!")]
        public virtual string TEN { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mã !")]
        [StringLength(50, ErrorMessage = "Mã không được quá 50 kí tự!")]
        public virtual string MA { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mã BHYT !")]
        [StringLength(50, ErrorMessage = "Mã BYT không được quá 50 kí tự!")]
        public virtual string MABHYT { get; set; }
        public virtual long? DONVIHANHCHINH_ID { get; set; }
        public virtual string DIACHI { get; set; }
        [RegularExpression(@"^\d{0,3}$", ErrorMessage = "Vui lòng nhập số không quá 3 chữ số")]
        public virtual int? HANGBENHVIEN { get; set; }
        [RegularExpression(@"^\d{0,3}$", ErrorMessage = "Vui lòng nhập số không quá 3 chữ số")]
        public virtual int? TUYENCMKT { get; set; }
        public virtual bool HIEULUC { get; set; }
        [RegularExpression(@"^\d{0,8}$", ErrorMessage = "Vui lòng nhập số không quá 8 chữ số")]
        public virtual int? STT { get; set; }
        public virtual string MIEUTA { get; set; }
        public virtual long? TINHTHANH_ID { get; set; }
        public virtual long? DONVI_ID { get; set; }
        public virtual long? QUANHUYEN_ID { get; set; }
        public virtual string MAQUANHUYEN { get; set; }
        public virtual string MATINHTHANH { get; set; }
        public virtual string MADONVI { get; set; }
        public virtual DM_DONVI DM_DONVI { get; set; }
        public virtual string MACOSOKCBCHA { get; set; }
        public virtual long? COSOKCBCHA_ID { get; set; }
        public virtual bool? THANNHANTAO { get; set; }
        public virtual bool? THAIGHEP { get; set; }
        public virtual int? LOAIHOPDONG { get; set; }
        public virtual int? DKKCBBD { get; set; }
        public virtual int? HINHTHUCTT { get; set; }
        public virtual int? LOAIBENHVIEN { get; set; }
        public virtual bool? KHAMTREEM { get; set; }
        public virtual DateTime? NGAYNGUNGHD { get; set; }
        public virtual string MATAICHINH { get; set; }
        public virtual string DIENTHOAI { get; set; }
        public virtual string MASOTHUE { get; set; }
        public virtual string EMAIL { get; set; }
        public virtual string FAX { get; set; }
        public virtual bool? PKDAKHOA { get; set; }
        public virtual bool? UNGTHU { get; set; }
        public virtual bool? VIEMGAN { get; set; }
        public virtual bool? TEBAOMAUTD { get; set; }
        public virtual bool? KHAMT7 { get; set; }
        public virtual bool? KHAMCN { get; set; }
        public virtual bool? KHAMNGAYLE { get; set; }
        //1: BV - Benh vien; 2: PK - Phong kham; 3: BBVCSSK - Ban bao ve cham soc suc khoe; 4: TTYTCK - Trung tam y te chuyen khoa; 5: YTCQ - Y te co quan; 6: TTYT - Trung tam y te; 7: TYT - Tram y te; 8: BX - Benh xa; 9: NHS - Nha ho sinh
        public virtual int? KIEUBV { get; set; }
        public virtual int? CAPCSKCB_MIN { get; set; }
        public virtual string SOHOPDONG { get; set; }
        public virtual int? LOAI_DONVICHUQUAN { get; set; }
        public virtual string COQUANCHUQUAN { get; set; }
        public virtual int? TTPHEDUYET { get; set; }
        public virtual string LYDO { get; set; }
        public virtual int? TRANGTHAI { get; set; }
        public virtual DateTime? NGAYKYHOPDONG { get; set; }
        public virtual DateTime? NGAYKYHOPDONGLANDAU { get; set; }
        public virtual int TUCHU { get; set; }
        //[RegularExpression(@"^\d{0,3}$", ErrorMessage = "Vui lòng nhập số không quá 3 chữ số")]
        public virtual int? HANGDICHVU_TD { get; set; }
        //[RegularExpression(@"^\d{0,3}$", ErrorMessage = "Vui lòng nhập số không quá 3 chữ số")]
        public virtual int? HANGTHUOC_TD { get; set; }
        //[RegularExpression(@"^\d{0,3}$", ErrorMessage = "Vui lòng nhập số không quá 3 chữ số")]
        public virtual int? HANGVATTU_TD { get; set; }
        public virtual long? SL_THE_BH_DKBD { get; set; }
        public virtual long? SL_THE_BH_DA_CAP { get; set; }

        public virtual bool? BYT { get; set; }
        public virtual int? KCB { get; set; }
        public virtual string SO_GPHD { get; set; }
        public virtual DateTime? NGAYCAPMA { get; set; }
        public virtual DateTime? NGAYDIEUCHINH { get; set; }
        public virtual bool? CHUA_PD43 { get; set; }
        public virtual string GHICHU_TINHTHAYDOI { get; set; }
        public virtual DM_COSOKCB COSOKCBCHA { get; set; }
       // public virtual DM_DONVIHANHCHINH DM_DONVIHANHCHINH { get; set; }
        public virtual DM_TINHTHANH DM_TINHTHANH { get; set; }
        //public virtual DM_QUANHUYEN DM_QUANHUYEN { get; set; }

        public virtual string MA_TEN
        {
            get { return string.Format("{0} - {1}", MA, TEN); }
        }

        public override string ToString()
        {
            return TEN;
        }

      //  public virtual List<BHYT_3360_TINH> BHYT_3360_TINH { get; set; }

        //public virtual List<DLTRUNGLAP_TQ> DLTRUNGLAP_TQ { get; set; }
    }
}
