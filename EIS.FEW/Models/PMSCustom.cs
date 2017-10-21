using IdentityManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.FEW.Models
{
    class PMSCustom : permission
    {
        public virtual int LOAI_PERMISSION { get; set; }
    }
}
