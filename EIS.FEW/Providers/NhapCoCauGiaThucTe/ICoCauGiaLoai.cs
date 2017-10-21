using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EIS.Core.Domain;

namespace EIS.FEW.Providers
{
    public interface ICoCauGiaLoai
    {
        COCAUGIA_LOAI GetCoCauGiaLoaiByID(long id);
    }
}
