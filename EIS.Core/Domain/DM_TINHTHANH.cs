using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EIS.Core.Domain
{
    public class DM_TINHTHANH
    {
        public virtual long ID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên !")]
        [StringLength(255, ErrorMessage = "Tên không được quá 255 kí tự!")]
        public virtual string TEN { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mã !")]
        [StringLength(50, ErrorMessage = "Mã không được quá 50 kí tự!")]
        public virtual string MA { get; set; }
        public virtual long? QUOCGIA_ID { get; set; }
        public virtual bool? HIEULUC { get; set; }
        [RegularExpression(@"^\d{0,8}$", ErrorMessage = "Vui lòng nhập số ít hơn 8 chữ số")]
        public virtual int? STT { get; set; }
        [StringLength(2000, ErrorMessage = "Mô tả không được quá 2000 kí tự!")]
        public virtual string MIEUTA { get; set; }
       // public virtual DM_QUOCGIA DM_QUOCGIA { get; set; }
        //public virtual IList<DM_QUANHUYEN> DM_QUANHUYENs { get; set; }

        public override string ToString()
        {
            return TEN;
        }

        public virtual long? KHUVUC_ID { get; set; }

       // public virtual List<BHYT15> LstBHYT15 { get; set; }
    }
}
