using EIS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIS.FEW.Providers
{
    public interface ICoCauGiaThucTe
    {
        COCAUGIA_THUCTE GetCoCauThucTe(string maCskcb, string maThe, DateTime ngayVao, DateTime ngayRa, string maDichVu);
        COCAUGIA_THUCTE GetCoCauThucTeByID(long id);
        COCAUGIA_THUCTE AddCoCauThucTe(string maCskcb, string maThe, DateTime ngayVao, DateTime ngayRa, string maDichVu);
    }
}