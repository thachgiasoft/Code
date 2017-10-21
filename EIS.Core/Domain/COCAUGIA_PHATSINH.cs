using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class COCAUGIA_PHATSINH
    {
        public virtual long ID { get; set; }
		public virtual int CHITIET_ID { get; set; }
        public virtual double SOLUONG_PS { get; set; }
        public virtual double DONGIA_PS { get; set; }
        public virtual double THANHTIEN_PS { get; set; }
		public virtual int COSOKCB_ID { get; set; }
		public virtual string SP_HSBA { get; set; }
		public virtual string NGUOITAO { get; set; }
		public virtual DateTime NGAYTAO { get; set; }
		public virtual string NGUOICAPNHAT { get; set; }
		public virtual DateTime NGAYCAPNHAT { get; set; }		
    }
}
