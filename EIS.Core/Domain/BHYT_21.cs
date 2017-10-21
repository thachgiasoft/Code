using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class BHYT_21
    {
        public virtual long ID { set; get; }
        public virtual long STT { set; get; }
        public virtual string TEN { set; get; }
        public virtual string MADVKT { set; get; }
        public virtual string MABV { set; get; }
        public virtual int LOAIKCB { set; get; }
        public virtual DM_COSOKCB COSOKCB_ID { set; get; }
        public virtual long TINHTHANH_ID { set; get; }
        public virtual int TRANGTHAI { set; get; }
        public virtual long HOSO_ID { set; get; }
        public virtual long KYGIAMDINH_ID { set; get; }
        public virtual string MIEUTA { set; get; }
        public virtual int STATUS { set; get; }
        public virtual long NAM_QT { set; get; }
        public virtual long THANG_QT { set; get; }
        public virtual long DONGIA { set; get; }
        public virtual long THANHTIEN { set; get; }
        public virtual long SOLUONG { set; get; }
        public virtual long NOITRU { set; get; }
        public virtual long NGOAITRU { set; get; }
        public virtual long KY_QT { set; get; }
    }
}
