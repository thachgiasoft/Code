using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class COCAUGIA_THUCTE
    {
        public virtual long ID { set; get; }
        public virtual string MA_THE { set; get; }
        public virtual DateTime? NGAY_RA { set; get; }
        public virtual DateTime? NGAY_VAO { set; get; }

        public virtual DM_COSOKCB COSOKCB { set; get; }
        public virtual IList<COCAUGIA_THUCTE_CT> THUCTE_CTs { set; get; }
        public virtual COCAUGIA_DICHVU21 DICHVU21 { set; get; }
    }
}
