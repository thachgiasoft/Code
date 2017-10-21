using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using EIS.Core.Common;

namespace EIS.FEW.Providers
{
    public class CoCauGiaThucTe : ICoCauGiaThucTe
    {
        private readonly ICOCAUGIA_THUCTEService _ICOCAUGIA_THUCTEService;
        private readonly ICOCAUGIA_DICHVU21Service _ICOCAUGIA_DICHVU21Service;
        private readonly IDMCOSOKCBService _IDMCOSOKCBService;
        public CoCauGiaThucTe()
        {
            _ICOCAUGIA_THUCTEService = IoC.Resolve<ICOCAUGIA_THUCTEService>();
            _ICOCAUGIA_DICHVU21Service = IoC.Resolve<ICOCAUGIA_DICHVU21Service>();
            _IDMCOSOKCBService = IoC.Resolve<IDMCOSOKCBService>();
        }
        public COCAUGIA_THUCTE AddCoCauThucTe(string maCskcb, string maThe, DateTime ngayVao, DateTime ngayRa, string maDichVu)
        {
            
            var entity = GetCoCauThucTe(maCskcb, maThe, ngayVao, ngayRa, maDichVu);
            if (entity == null)
            {
                COCAUGIA_DICHVU21 dichvu21 = _ICOCAUGIA_DICHVU21Service.Query.Where(x => x.MA_DICHVU21.EndsWith(maDichVu.Right(4))).FirstOrDefault();
                DM_COSOKCB cskcb = _IDMCOSOKCBService.Query.Where(x => x.MA.Equals(maCskcb)).FirstOrDefault();

                COCAUGIA_THUCTE thucte = new COCAUGIA_THUCTE
                {
                    DICHVU21 = dichvu21,
                    COSOKCB = cskcb,
                    MA_THE = maThe,
                    NGAY_VAO = ngayVao,
                    NGAY_RA = ngayRa
                };
                var result = _ICOCAUGIA_THUCTEService.Save(thucte);
                _ICOCAUGIA_THUCTEService.CommitChanges();
                return result;
            }
            else
            {
                return null;
            }
        }

        public COCAUGIA_THUCTE GetCoCauThucTe(string maCskcb, string maThe, DateTime ngayVao, DateTime ngayRa, string maDichVu)
        {
            var thucte = _ICOCAUGIA_THUCTEService.Query.Where(x =>
           x.COSOKCB.MA.Equals(maCskcb) && x.MA_THE.Equals(maThe) &&
           x.DICHVU21.MA_DICHVU21.EndsWith(maDichVu.Right(4)) &&
           x.NGAY_VAO == ngayVao && x.NGAY_RA == ngayRa).FirstOrDefault();
            return thucte;
        }

        public COCAUGIA_THUCTE GetCoCauThucTeByID(long id)
        {
            var thucte = _ICOCAUGIA_THUCTEService.Query.Where(x => x.ID == id).FirstOrDefault();
            return thucte;
        }
    }
}