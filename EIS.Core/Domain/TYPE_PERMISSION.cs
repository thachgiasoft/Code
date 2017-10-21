using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.Domain
{
    public class TYPE_PERMISSION
    {
        public virtual int PERMISSIONID { get; set; }
        public virtual int LOAI_PERMISSION { get; set; }
        public virtual string TYPE_PERMISSIONMULTI { get; set; }
    }
}
