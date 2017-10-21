using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIS.FEW.Models
{
    public class Type_PmsMulti
    {
        public virtual int PERMISSIONID { get; set; }
        public virtual string TYPE_PERMISSIONMULTIID { get; set; }
        public virtual string TYPE_PERMISSIONMULTINAME { get; set; }
        public virtual int Loai_Permission { get; set; }
    }
}