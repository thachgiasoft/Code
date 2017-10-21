using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class COCAUGIA_CHITIET
    {
        public virtual long ID { get; set; }
        public virtual string MA_CC { get; set; }
        public virtual string TEN_CC { get; set; }
        public virtual string DONVITINH_CC { get; set; }
        public virtual double DONGIA_CC { get; set; }
        public virtual double SOLUONG_CC { get; set; }
        public virtual double THANHTIEN_CC { get; set; }
        public virtual COCAUGIA_LOAI LOAI_CC { get; set; }
        public virtual COCAUGIA_DICHVU21 DICHVU21_ID { get; set; }
        public virtual string NGUOITAO { get; set; }
        public virtual DateTime NGAYTAO { get; set; }
        public virtual string NGUOICAPNHAT { get; set; }
        public virtual DateTime NGAYCAPNHAT { get; set; }
        public virtual string MATINH { get; set; }
        public virtual string TENTINH { get; set; }
    }
}
