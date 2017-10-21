using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class DM_GIADICHVU
    {
        public virtual long ID { get; set; }
        public virtual string MA_DICHVU21 { get; set; }
        public virtual string TEN_DICHVU21 { get; set; }

        public virtual string STT { get; set; }
    }
}
