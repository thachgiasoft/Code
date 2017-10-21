using DevExpress.Web.Mvc;
using EIS.Core.Domain;
using EIS.Core.IService;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FX.Core;
using EIS.FEW.Utils;
using EIS.FEW.Models;

namespace EIS.FEW.Controllers
{
    public class TongHopCoCauChiTietController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TongHopCoCauChiTietController));
        private readonly ICOCAUGIA_THUCTE_CTService _COCAUGIA_THUCTE_CTService;

        public TongHopCoCauChiTietController()
        {
            _COCAUGIA_THUCTE_CTService = IoC.Resolve<ICOCAUGIA_THUCTE_CTService>();  
        }

        public ActionResult Index()
        {
            //var result = _COCAUGIA_THUCTE_CTService.TongHopCoCauChiTiet(8, 2017, 1).ToList();
            Session["_GridViewCompare"] = null;
            return View();
        }

        public ActionResult GridViewFilterServices()
        {
            try
            {
                if (Session["_GridViewCompare"] != null)
                    return PartialView("GridViewCompare", Session["_GridViewCompare"]);
            }
            catch(Exception e)
            {
                log.ErrorFormat("GridViewFilterServices - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);

            }
            return PartialView("GridViewCompare", new List<COCAUGIA_CHITIET>().AsQueryable());
        }


        public ActionResult _GridViewCompareFilter(int month, int year, string maCskcb)
        {
            try
            {
                var queryResult = _COCAUGIA_THUCTE_CTService.TongHopCoCauChiTiet(month, year, maCskcb);
                Session["_GridViewCompare"] = XuLyDuLieu(queryResult);

                return PartialView("GridViewCompare", Session["_GridViewCompare"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewCompareFilter - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("GridViewCompare", new List<TongHopCoCauChiTietItem>().AsQueryable());

        }

        private IQueryable<TongHopCoCauChiTietItem> XuLyDuLieu(IEnumerable<COCAUGIA_THUCTE_CT_QUERY> queryResult)
        {
            List<TongHopCoCauChiTietItem> result = new List<TongHopCoCauChiTietItem>();
            int generator = 0;
            foreach (var record in queryResult)
            {
                var item = new TongHopCoCauChiTietItem
                {
                    Id = generator++,
                    Ma = record.MA_COCAU,
                    NoiDung = XuLyTenItem(record),
                    SoLuongOld = record.SO_LUONG_OLD,
                    DonGiaOld = record.DONGIA_OLD,
                    SoLuongNew = record.SO_LUONG_NEW,
                    DonGiaNew = record.DONGIA_NEW,
                    TenLoai = record.TEN_LOAI
                };

                result.Add(item);

                //Tinh toan
                item.ThanhTienOld = item.SoLuongOld * item.DonGiaOld;
                item.ThanhTienNew = item.SoLuongNew * item.DonGiaNew;
                item.ChenhLech = item.ThanhTienNew - item.ThanhTienOld;
            }

            return result.AsQueryable();
        }

        private string XuLyTenItem(COCAUGIA_THUCTE_CT_QUERY item)
        {
            if (string.IsNullOrEmpty(item.TEN_COCAU_OLD))
                return item.TEN_COCAU_NEW;

            return string.Format("{0}({1})", item.TEN_COCAU_NEW, item.TEN_COCAU_OLD);
        }

        public ActionResult GridViewListSerives()
        {
            return View();
        }

        public ActionResult GridViewCompare()
        {
            try
            {
                if (Session["_GridViewCompare"] != null)
                    return PartialView("GridViewCompare", Session["_GridViewCompare"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewChiTietDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("GridViewCompare", new List<COCAUGIA_CHITIET>().AsQueryable());
        }
    }
}