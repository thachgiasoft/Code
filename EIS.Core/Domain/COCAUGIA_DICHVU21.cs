using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class COCAUGIA_DICHVU21
    {
        public virtual long ID { get; set; }
        public virtual string MA_DICHVU21 { get; set; }
        public virtual string TEN_DICHVU21 { get; set; }
        public virtual string MATINH { get; set; }
        public virtual DateTime NGAYTAO { get; set; }
        public virtual string NGUOITAO { get; set; }
        public virtual DateTime NGAYCAPNHAT { get; set; }
        public virtual string NGUOICAPNHAT { get; set; }
        public virtual string TENTINH { get; set; }
        public virtual int? TRANGTHAI { get; set; }

        public virtual int? STT { get; set; }
    }
}
