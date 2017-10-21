using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIS.FEW.Models;
using EIS.Core.Domain2;

namespace EIS.FEW.Controllers
{
    public class SoSanhCoCau21Controller : Controller
    {
        private const string sessionMonthKey = "ss21_month";
        private const string sessionYearKey = "ss21_year";
        private const string sessionMaCskcbKey = "ss21_maCskcb";
        private const string sessionMaDvKey = "ss21_maDv";

        private static readonly ILog log = LogManager.GetLogger(typeof(COCAUGIA_DICHVU21Controller));
        private readonly ICOCAUGIA_THUCTE_CTService _COCAUGIA_THUCTE_CTService;
        private readonly ISoSanhCoCau21 _SoSanhCoCau21Service;

        public SoSanhCoCau21Controller()
        {
            _COCAUGIA_THUCTE_CTService = IoC.Resolve<ICOCAUGIA_THUCTE_CTService>();
            _SoSanhCoCau21Service = IoC.Resolve<ISoSanhCoCau21>();
        }
        public ActionResult Index()
        {
            Session["_GridViewChiTietDichVuPartial"] = null;

            Session[sessionMonthKey] = null;
            Session[sessionYearKey] = null;
            Session[sessionMaCskcbKey] = null;
            Session[sessionMaDvKey] = null;

            return View();
        }

        public ActionResult _GridViewDanhSachDichVuPartial()
        {
            try
            {
                if (Session["_GridViewDichVu"] != null)
                    return PartialView("_GridViewDanhSachDichVuPartial", new List<DichVu21DonGia>().AsQueryable());
                else
                {
                    return PartialView("_GridViewDanhSachDichVuPartial", new List<DichVu21DonGia>().AsQueryable());
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewDanhSachDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewDanhSachDichVuPartial", new List<DichVu21DonGia>().AsQueryable());
        }

        /*public ActionResult _GridViewDanhSachDichVuFilter(int month, int year, long? cskcbId)
        {

            try
            {
                var queryResult = _SoSanhCoCau21Service.GetDichVu21(month, year, cskcbId);
                Session["_GridViewDichVu"] = XuLyDataDichVu(queryResult);
                return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDichVu"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewDanhSachDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            _SoSanhCoCau21Service.GetDichVu21(month, year, cskcbId);
            return PartialView("_GridViewDanhSachDichVuPartial", new List<DichVu21DonGia>().AsQueryable());
        }*/

        public ActionResult _GridViewDanhSachDichVuFilter(int month, int year, string maCskcb)
        {

            try
            {
                Session[sessionMonthKey] = month;
                Session[sessionYearKey] = year;
                Session[sessionMaCskcbKey] = maCskcb;

                var queryResult = _SoSanhCoCau21Service.GetDichVu21(month, year, maCskcb);
                Session["_GridViewDichVu"] = queryResult;
                return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDichVu"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewDanhSachDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewDanhSachDichVuPartial", new List<DichVu21DonGia>().AsQueryable());
        }

        public ActionResult _GridViewChiTietDichVuFilter(string maDichVu)
        {
            try
            {
                int month = (int)Session[sessionMonthKey];
                int year = (int)Session[sessionYearKey];
                string maCskcb = Session[sessionMaCskcbKey] as string;
                var queryResult = _SoSanhCoCau21Service.GetCocauGiaThucTe(month, year, maCskcb, maDichVu);
                Session["_GridViewChiTietDichVuPartial"] = XuLyDuLieu(queryResult);
                return PartialView("_GridViewChiTietDichVuPartial", Session["_GridViewChiTietDichVuPartial"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewCompareFilter - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewChiTietDichVuPartial", new List<TongHopCoCauChiTietItem>().AsQueryable());
        }

        public ActionResult _GridViewChiTietDichVuPartial()
        {
            try
            {
                if (Session["_GridViewChiTietDichVuPartial"] != null)
                    return PartialView("_GridViewChiTietDichVuPartial", Session["_GridViewChiTietDichVuPartial"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewChiTietDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewChiTietDichVuPartial", new List<COCAUGIA_CHITIET>().AsQueryable());
        }

        public IQueryable<DichVu21DonGia> XuLyDataDichVu(IEnumerable<DichVu21DonGia> queryResult)
        {
            Dictionary<string, DichVu21DonGia> result = new Dictionary<string, DichVu21DonGia>();
            foreach (var record in queryResult)
            {
                string MA_DICHVU = record.MADICHVU;
                if (string.IsNullOrEmpty(MA_DICHVU))
                    continue;

                DichVu21DonGia item = null;
                if (result.ContainsKey(MA_DICHVU))
                {
                    item = result[MA_DICHVU];
                    item.SOLUONG += record.SOLUONG;
                    item.THANHTIEN += record.THANHTIEN;
                }
                else
                {
                    item = new DichVu21DonGia
                    {
                        ID = record.ID,
                        MADICHVU = record.MADICHVU,
                        TENDICHVU = record.TENDICHVU,
                        DONGIA = record.DONGIA,
                        SOLUONG = record.SOLUONG,
                        THANHTIEN = record.THANHTIEN
                    };

                    result.Add(MA_DICHVU, item);
                }

                ////Tinh toan
                //item.ThanhTienOld = item.SoLuongOld * item.DonGiaOld;
                //item.ThanhTienNew = item.SoLuongNew * item.DonGiaNew;
                //item.ChenhLech = item.ThanhTienNew - item.ThanhTienOld;
            }

            return result.Values.AsQueryable();
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


                //Tinh toan
                item.ThanhTienOld = item.SoLuongOld * item.DonGiaOld;
                item.ThanhTienNew = item.SoLuongNew * item.DonGiaNew;
                item.ChenhLech = item.ThanhTienNew - item.ThanhTienOld;

                result.Add(item);
            }

            return result.AsQueryable();
        }

        private string XuLyTenItem(COCAUGIA_THUCTE_CT_QUERY item)
        {
            if (string.IsNullOrEmpty(item.TEN_COCAU_OLD))
                return item.TEN_COCAU_NEW;

            return string.Format("{0}({1})", item.TEN_COCAU_NEW, item.TEN_COCAU_OLD);
        }
    }
}