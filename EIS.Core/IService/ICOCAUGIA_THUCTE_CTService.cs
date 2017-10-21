using EIS.Core.Domain;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.IService
{
    public interface ICOCAUGIA_THUCTE_CTService : IBaseService<COCAUGIA_THUCTE_CT, long>
    {
        IEnumerable<COCAUGIA_THUCTE_CT_QUERY> TongHopCoCauChiTiet(int month, int year, string maCskcb);
    }
}
