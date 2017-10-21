using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class COCAUGIA_THUCTE_CT
    {
        public virtual long ID { set; get; }

        public virtual string MA_COCAU { set; get; }
        public virtual string TEN_COCAU_OLD { set; get; }
        public virtual double SO_LUONG_OLD { set; get; }
        public virtual double DONGIA_OLD { set; get; }
        public virtual string TEN_COCAU_NEW { set; get; }
        public virtual double SO_LUONG_NEW { set; get; }
        public virtual double DONGIA_NEW { set; get; }

        public virtual COCAUGIA_LOAI LOAI { set; get; }
        public virtual COCAUGIA_THUCTE THUCTE { set; get; }
    }
}
