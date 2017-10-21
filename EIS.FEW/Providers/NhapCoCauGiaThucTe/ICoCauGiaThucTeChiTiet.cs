using EIS.Core.Domain;
using EIS.FEW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.FEW.Providers
{
    public interface ICoCauGiaThucTeChiTiet
    {
        COCAUGIA_THUCTE_CT AddCoCauThucTeChiTiet(COCAUGIA_THUCTE_CT entity);
        COCAUGIA_THUCTE_CT UpdateCoCauThucTeChiTiet(COCAUGIA_THUCTE_CT entity);
        IQueryable<COCAUGIA_THUCTE_CT> GetThucTeChiTietByIDThucTe(long idThucTe);
        COCAUGIA_THUCTE_CT GetThucTeChiTietByID(long id);
    }
}
