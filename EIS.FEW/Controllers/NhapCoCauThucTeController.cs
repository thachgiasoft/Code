using EIS.Core.IService;
using EIS.Core.Common;
using FX.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIS.Core.Domain;
using DevExpress.Web.Mvc;
using EIS.FEW.Providers;
using EIS.FEW.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EIS.FEW.Controllers
{
    #region NhapCoCauThucTeController class definition
    public class NhapCoCauThucTeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NhapCoCauThucTeController));
        private NhapCoCauGiaThucTeProvider _NhapCoCauGiaThucTeProvider;

        public NhapCoCauThucTeController()
        {
            _NhapCoCauGiaThucTeProvider = new NhapCoCauGiaThucTeProvider();
        }
        public ActionResult Index()
        {
            //Session["idThucTe"] = 13;
            if (Session["idThucTe"] != null)
            {
                COCAUGIA_THUCTE thucte = _NhapCoCauGiaThucTeProvider._CoCauGiaThucTe.GetCoCauThucTeByID(Convert.ToInt32(Session["idThucTe"]));
                ViewData["thucte"] = thucte;
            }

            return View();
        }

        #region ViewPartials
        public ActionResult GridViewThucTeChiTietPartial()
        {

            try
            {
                if (Session["idThucTe"] != null)
                {
                    var thucte_tc = _NhapCoCauGiaThucTeProvider
                        ._CoCauGiaThucTeChiTiet
                        .GetThucTeChiTietByIDThucTe(Convert.ToInt64(Session["idThucTe"]))
                        .Select(x => new CoCauThucTeChiTietItem
                        {
                            ID = x.ID,
                            ID_ThucTe = x.THUCTE.ID,
                            IDLoai = x.LOAI.ID,
                            LoaiName = x.LOAI.TEN,
                            MaCoCau = x.MA_COCAU,
                            TenCoCauOld = x.TEN_COCAU_OLD,
                            DonGiaOld = x.DONGIA_OLD,
                            SoLuongOld = x.SO_LUONG_OLD,
                            DonGiaNew = x.DONGIA_NEW,
                            SoLuongNew = x.SO_LUONG_NEW
                        });
                    return PartialView("GridViewThucTeChiTietPartial", thucte_tc);
                }

            }
            catch (Exception e)
            {
                log.ErrorFormat("Index - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }

            return PartialView("GridViewThucTeChiTietPartial", new List<CoCauThucTeChiTietItem>().AsQueryable());
        }
        public ActionResult ComboBoxCoSoKCBPartial()
        {
            return PartialView();
        }
        public ActionResult EditFormPartial(long id)
        {
            COCAUGIA_THUCTE_CT item = _NhapCoCauGiaThucTeProvider._CoCauGiaThucTeChiTiet.GetThucTeChiTietByID(id);
            CoCauThucTeChiTietItem thucte_ct = new CoCauThucTeChiTietItem
            {
                ID = item.ID,
                IDLoai = item.LOAI.ID,
                LoaiName = item.LOAI.TEN,
                MaCoCau = item.MA_COCAU,
                TenCoCauOld = item.TEN_COCAU_OLD,
                DonGiaOld = item.DONGIA_OLD,
                SoLuongOld = item.SO_LUONG_OLD,
                TenCoCauNew = item.TEN_COCAU_NEW,
                DonGiaNew = item.DONGIA_NEW,
                SoLuongNew = item.SO_LUONG_NEW
            };
            //CoCauThucTeChiTietItem thucte_ct = _NhapCoCauGiaThucTeProvider._CoCauGiaThucTeChiTiet.GetThucTeChiTietByID(id);
            return PartialView("EditFormPartial", thucte_ct);
        }
        public ActionResult CreateFormPartial()
        {
            CoCauThucTeChiTietItem thucte_ct = new CoCauThucTeChiTietItem();
            return PartialView("EditFormPartial", thucte_ct);
        }

        #endregion

        #region ThucTe
        public ActionResult AddThucTe(string maCskcb, string maThe, string ngayVao, string ngayRa, string maDichVu)
        {
            try
            {
                maThe = maThe.Trim();
                maDichVu = maDichVu.Trim();

                if (string.IsNullOrEmpty(maThe))
                    throw new ArgumentNullException(nameof(maThe));

                if (string.IsNullOrEmpty(maDichVu))
                    throw new ArgumentNullException(nameof(maDichVu));

                var thucte = _NhapCoCauGiaThucTeProvider._CoCauGiaThucTe.AddCoCauThucTe(maCskcb, maThe, DateTime.Parse(ngayVao), DateTime.Parse(ngayRa), maDichVu);

                if (thucte != null)
                {
                    Session["idThucTe"] = thucte.ID;
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("AddThucTe - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }

            return RedirectToAction("Index", "NhapCoCauThucTe");
        }
        public ActionResult GridViewCoCauThucTeFilter(string maCskcb, string maThe, string ngayVao, string ngayRa, string maDichVu)
        {
            try
            {
                maThe = maThe.Trim();
                maDichVu = maDichVu.Trim();

                if (string.IsNullOrEmpty(maDichVu))
                {
                    throw new ArgumentNullException(nameof(maDichVu));
                }

                if (string.IsNullOrEmpty(maThe))
                {
                    throw new ArgumentNullException(nameof(maThe));
                }

                var thucte = _NhapCoCauGiaThucTeProvider._CoCauGiaThucTe.GetCoCauThucTe(maCskcb, maThe, DateTime.Parse(ngayVao), DateTime.Parse(ngayRa), maDichVu);

                if (thucte != null)
                {
                    Session["idThucTe"] = thucte.ID;
                    //var thucte_ct = thucte.THUCTE_CTs;
                    return GridViewThucTeChiTietByIDThucTe(Convert.ToInt64(Session["idThucTe"]));
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("GridViewThucTeChiTietPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }

            return PartialView("GridViewThucTeChiTietPartial", new List<CoCauThucTeChiTietItem>().AsQueryable());

        }
        #endregion

        #region ThucTeChiTiet
        public ActionResult GridViewThucTeChiTietByIDThucTe(long id)
        {
            var thucte_cts = _NhapCoCauGiaThucTeProvider
                ._CoCauGiaThucTeChiTiet.
                GetThucTeChiTietByIDThucTe(id)
                .Select(x => new CoCauThucTeChiTietItem
                {
                    ID = x.ID,
                    ID_ThucTe = x.THUCTE.ID,
                    IDLoai = x.LOAI.ID,
                    LoaiName = x.LOAI.TEN,
                    MaCoCau = x.MA_COCAU,
                    TenCoCauOld = x.TEN_COCAU_OLD,
                    DonGiaOld = x.DONGIA_OLD,
                    SoLuongOld = x.SO_LUONG_OLD,
                    DonGiaNew = x.DONGIA_NEW,
                    SoLuongNew = x.SO_LUONG_NEW
                });
            return PartialView("GridViewThucTeChiTietPartial", thucte_cts);
        }
        public ActionResult AddThucTeChiTiet(CoCauThucTeChiTietItem item)
        {

            COCAUGIA_THUCTE thucte = _NhapCoCauGiaThucTeProvider._CoCauGiaThucTe.GetCoCauThucTeByID(Convert.ToInt64(Session["idThucTe"]));
            COCAUGIA_LOAI loai = _NhapCoCauGiaThucTeProvider._CoCauGiaLoai.GetCoCauGiaLoaiByID(item.IDLoai);
            COCAUGIA_THUCTE_CT thucte_ct = new COCAUGIA_THUCTE_CT
            {
                THUCTE = thucte,
                LOAI = loai,
                MA_COCAU = item.MaCoCau,
                TEN_COCAU_OLD = item.TenCoCauOld,
                DONGIA_OLD = item.DonGiaOld,
                SO_LUONG_OLD = item.SoLuongOld,
                TEN_COCAU_NEW = item.TenCoCauNew,
                DONGIA_NEW = item.DonGiaNew,
                SO_LUONG_NEW = item.SoLuongNew
            };
            _NhapCoCauGiaThucTeProvider._CoCauGiaThucTeChiTiet.AddCoCauThucTeChiTiet(thucte_ct);
            return GridViewThucTeChiTietByIDThucTe(Convert.ToInt64(Session["idThucTe"]));
        }
        public ActionResult UpdateThucTeChiTiet(CoCauThucTeChiTietItem item)
        {
            COCAUGIA_THUCTE_CT thucte_ct = _NhapCoCauGiaThucTeProvider._CoCauGiaThucTeChiTiet.GetThucTeChiTietByID(item.ID);

            thucte_ct.MA_COCAU = item.MaCoCau;
            thucte_ct.DONGIA_OLD = item.DonGiaOld;
            thucte_ct.SO_LUONG_OLD = item.SoLuongOld;

            thucte_ct.TEN_COCAU_NEW = item.TenCoCauNew;
            thucte_ct.DONGIA_NEW = item.DonGiaNew;
            thucte_ct.SO_LUONG_NEW = item.SoLuongNew;

            _NhapCoCauGiaThucTeProvider._CoCauGiaThucTeChiTiet.UpdateCoCauThucTeChiTiet(thucte_ct);

            return GridViewThucTeChiTietByIDThucTe(Convert.ToInt64(Session["idThucTe"]));
        }
        #endregion
    }
    #endregion
}