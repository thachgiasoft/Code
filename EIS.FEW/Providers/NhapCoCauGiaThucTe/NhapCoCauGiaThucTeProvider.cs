using EIS.Core.IService;
using FX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIS.Core.Domain;
using EIS.Core.Common;

namespace EIS.FEW.Providers
{
    public class NhapCoCauGiaThucTeProvider
    {
        public ICoCauGiaThucTe _CoCauGiaThucTe { set; get; }
        public ICoCauGiaThucTeChiTiet _CoCauGiaThucTeChiTiet { set; get; }
        public ICoCauGiaLoai _CoCauGiaLoai { set; get; }

        public NhapCoCauGiaThucTeProvider()
        {
            _CoCauGiaLoai = new CoCauGiaLoai();
            _CoCauGiaThucTe = new CoCauGiaThucTe();
            _CoCauGiaThucTeChiTiet = new CoCauGiaThucTeChiTiet();
        }
        public NhapCoCauGiaThucTeProvider(ICoCauGiaThucTe thucte, ICoCauGiaThucTeChiTiet thucte_ct, ICoCauGiaLoai loai)
        {
            _CoCauGiaThucTe = thucte;
            _CoCauGiaThucTeChiTiet = thucte_ct;
            _CoCauGiaLoai = loai;
        }
        
    }
}