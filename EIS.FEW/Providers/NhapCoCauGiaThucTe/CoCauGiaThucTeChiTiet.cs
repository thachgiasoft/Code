using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIS.Core.Domain;
using EIS.Core.IService;
using EIS.FEW.Models;
using FX.Core;

namespace EIS.FEW.Providers
{
    public class CoCauGiaThucTeChiTiet : ICoCauGiaThucTeChiTiet
    {
        private readonly ICOCAUGIA_THUCTE_CTService _ICOCAUGIA_THUCTE_CTService;
        public CoCauGiaThucTeChiTiet()
        {
            _ICOCAUGIA_THUCTE_CTService = IoC.Resolve<ICOCAUGIA_THUCTE_CTService>();
        }
        public COCAUGIA_THUCTE_CT AddCoCauThucTeChiTiet(COCAUGIA_THUCTE_CT entity)
        {
            try
            {
                var thucte_ct = _ICOCAUGIA_THUCTE_CTService.Save(entity);
                _ICOCAUGIA_THUCTE_CTService.CommitChanges();
                return thucte_ct;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public COCAUGIA_THUCTE_CT GetThucTeChiTietByID(long id)
        {
            return _ICOCAUGIA_THUCTE_CTService.Query.Where(x => x.ID == id).FirstOrDefault();
        }
        public IQueryable<COCAUGIA_THUCTE_CT> GetThucTeChiTietByIDThucTe(long idThucTe)
        {
            var thucte_cts = _ICOCAUGIA_THUCTE_CTService.Query.Where(x => x.THUCTE.ID == idThucTe);
            return thucte_cts;
        }
        public COCAUGIA_THUCTE_CT UpdateCoCauThucTeChiTiet(COCAUGIA_THUCTE_CT entity)
        {
            COCAUGIA_THUCTE_CT thucte_ct = _ICOCAUGIA_THUCTE_CTService.Update(entity);
            _ICOCAUGIA_THUCTE_CTService.CommitChanges();
            return thucte_ct;
        }

    }
}