using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EIS.Core.Domain;


namespace EIS.FEW.Providers
{
    public interface INhapCoCauGiaThucTeProvider
    {
        COCAUGIA_THUCTE GetCoCauThucTe(string maCskcb, string maThe, DateTime ngayVao, DateTime ngayRa, string maDichVu);
        COCAUGIA_THUCTE AddCoCauThucTe(string maCskcb, string maThe, DateTime ngayVao, DateTime ngayRa, string maDichVu);
        COCAUGIA_THUCTE_CT AddCoCauThucTeChiTiet(COCAUGIA_THUCTE_CT entity);
        IQueryable<COCAUGIA_THUCTE_CT> GetThucTeChiTietByID(long idThucTe);
    }
}
