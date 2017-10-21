using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class COCAUGIA_THUCTE_CT_QUERY
    {
        public virtual long ID { set; get; }
        //public virtual COCAUGIA_THUCTE ID_THUCTE { set; get; }
        public virtual long ID_THUCTE { set; get; }

        public virtual string MA_COCAU { set; get; }
        public virtual string TEN_COCAU_OLD { set; get; }
        public virtual decimal SO_LUONG_OLD { set; get; }
        public virtual decimal DONGIA_OLD { set; get; }
        public virtual string TEN_COCAU_NEW { set; get; }
        public virtual decimal SO_LUONG_NEW { set; get; }
        public virtual decimal DONGIA_NEW { set; get; }

        //public virtual COCAUGIA_LOAI ID_LOAI { set; get; }
        public virtual long ID_LOAI { set; get; }
        public virtual string TEN_LOAI { set; get; }
    }
}
