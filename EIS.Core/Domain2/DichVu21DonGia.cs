using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain2
{
    public class DichVu21DonGia
    {
        public virtual long ID { set; get; }
        public virtual string MADICHVU { set; get; }
        public virtual string TENDICHVU { set; get; }
        public virtual decimal SOLUONG { set; get; }
        public virtual decimal DONGIA { set; get; }
        public virtual decimal THANHTIEN { set; get; }
    }
}
