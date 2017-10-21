using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class DM_DONVI
    {
        public virtual long ID {get;set;}
        [Required(ErrorMessage = "Bạn chưa nhập tên !")]
        [StringLength(255, ErrorMessage = "Tên không được quá 255 kí tự!")]
        public virtual string TEN { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mã !")]
        [StringLength(50, ErrorMessage = "Mã không được quá 50 kí tự!")]
        public virtual string MA { get; set; }
        [StringLength(2000, ErrorMessage = "Mô tả không được quá 2000 kí tự!")]
        public virtual string MIEUTA { get; set; }
        public virtual bool? HIEULUC { get; set; }
        public virtual long? DONVICHA_ID { get; set; }
        [RegularExpression(@"^\d{0,8}$", ErrorMessage = "Vui lòng nhập số ít hơn 8 chữ số")]
        public virtual int? STT { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên BHXH cấp trên!")]
        [StringLength(50, ErrorMessage = "Mã cha không được quá 50 kí tự!")]
        
        public virtual string MACHA { get; set; }
        [StringLength(50, ErrorMessage = "Mã tỉnh thành không được quá 50 kí tự!")]
        public virtual string MATINHTHANH { get; set; }
          [Required(ErrorMessage = "Bạn chưa nhập tỉnh thành!")]
        public virtual long? TINHTHANH_ID { get; set; }
        public virtual DM_DONVI DONVICHA { get; set; }
        public virtual DM_TINHTHANH TINHTHANH { get; set; }
        public virtual string TENTAT
        {
            get
            {
                return TEN.Replace("BHXH ","");
            }
        }

        //public virtual List<BHYT_04> LstBHYT_04 { get; set; }

        //public virtual List<BHYT_3360_TINH> BHYT_3360_TINH { get; set; }
        //public virtual List<C88HDCHITIET> C88HDCHITIET { get; set; }
    }
}
