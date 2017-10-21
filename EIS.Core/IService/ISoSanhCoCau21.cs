using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EIS.Core.Domain;
using EIS.Core.Domain2;
using FX.Data;

namespace EIS.Core.IService
{
    public interface ISoSanhCoCau21 : IBaseService<DichVu21DonGia, int>
    {
        IEnumerable<DichVu21DonGia> GetDichVu21(int month, int year, long? cskcbId);
        IEnumerable<DichVu21DonGia> GetDichVu21(int month, int year, string maCskcb);

        IEnumerable<COCAUGIA_THUCTE_CT_QUERY> GetCoCauGiaThucTe(long IdDichVu, long? cskcbId);
        IEnumerable<COCAUGIA_THUCTE_CT_QUERY> GetCocauGiaThucTe(int month, int year, string msCskcb, string maDichVu);
    }
}
