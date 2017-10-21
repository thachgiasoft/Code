using EIS.Core.Domain;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.IService
{
    public interface ICOCAUGIA_THUCTEService : IBaseService<COCAUGIA_THUCTE, long>
    {
        COCAUGIA_THUCTE GetByFilter(string maCskcb, string maThe, DateTime? ngayVao, DateTime? ngayRa, string maDichVu);
    }
}
