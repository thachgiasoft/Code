using DevExpress.Web.Mvc;
using EIS.Core.Common;
using EIS.Core.Domain;
using EIS.Core.IService;
using EIS.Core.ServiceImp;
using EIS.FEW.Models;
using FX.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.Formula.Functions;

namespace EIS.FEW.Controllers
{
    public class COCAUGIA_PHATSINHController : Controller
    {
        //
        // GET: /COCAUGIA_DICHVU21/
        private static readonly ILog log = LogManager.GetLogger(typeof(COCAUGIA_DICHVU21Controller));
        private readonly ICOCAUGIA_CHITIETService _ICOCAUGIA_CHITIETService;
        private readonly ICOCAUGIA_DICHVU21Service _ICOCAUGIA_DICHVU21Service;
        private readonly ICOCAUGIA_LOAIService _ICOCAUGIA_LOAIService;
        private readonly ICOCAUGIA_PHATSINHService _ICOCAUGIA_PHATSINHService;
        private readonly ICOCAUGIA_THUCTE_CTService _ICOCAUGIA_THUCTE_CTService;
        private readonly ICOCAUGIA_THUCTEService _ICOCAUGIA_THUCTEService;
        private readonly IDMCOSOKCBService _IDMCOSOKCBService;
        private GridViewCustomBindingHandlers gridViewHanders;

        public COCAUGIA_PHATSINHController()
        {
            _ICOCAUGIA_DICHVU21Service = IoC.Resolve<ICOCAUGIA_DICHVU21Service>();
            _ICOCAUGIA_CHITIETService = IoC.Resolve<ICOCAUGIA_CHITIETService>();
            _ICOCAUGIA_LOAIService = IoC.Resolve<ICOCAUGIA_LOAIService>();
            _ICOCAUGIA_PHATSINHService = IoC.Resolve<ICOCAUGIA_PHATSINHService>();
            _IDMCOSOKCBService = IoC.Resolve<IDMCOSOKCBService>();
            _ICOCAUGIA_THUCTE_CTService = IoC.Resolve<ICOCAUGIA_THUCTE_CTService>();
            _ICOCAUGIA_THUCTEService = IoC.Resolve<ICOCAUGIA_THUCTEService>();
            gridViewHanders = new GridViewCustomBindingHandlers();
        }

        public ActionResult Index()
        {
            var model = new COCAUGIA_DICHVU();
            model.Filter = new COCAUGIA_DICHVU_FILTER();
            Session["_GridViewChiTietDichVuPartial"] = null;
            try
            {
            }
            catch (Exception e)
            {
                log.ErrorFormat("Index - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return View(model);
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

        public ActionResult _GridViewChiTietDichVuFilter(string maCskcb, string maThe, string ngayVao, string ngayRa, string maDichVu)
        {
            try
            {
                maThe = maThe.Trim();
                maDichVu = maDichVu.Trim();

                Session["maCskcb"] = maCskcb;
                Session["maThe"] = maThe;
                Session["ngayVao"] = ngayVao;
                Session["ngayRa"] = ngayRa;
                Session["maDichVu"] = maDichVu;
                Session["idThucTe"] = null;
                Session["idDichVu"] = null;

                var nv = DateTime.Parse(ngayVao);

                if (string.IsNullOrEmpty(maThe))
                    throw new ArgumentNullException(nameof(maThe));

                if (string.IsNullOrEmpty(maDichVu))
                    throw new ArgumentNullException(nameof(maDichVu));

                var thucTe = _ICOCAUGIA_THUCTEService.GetByFilter(maCskcb, maThe, DateTime.Parse(ngayVao), DateTime.Parse(ngayRa), maDichVu);

                List<COCAUGIA_THUCTE_CT> danhSachThucTeChiTiet = null;

                if (thucTe != null && thucTe.THUCTE_CTs != null)
                {
                    Session["idThucTe"] = thucTe.ID;
                    danhSachThucTeChiTiet = thucTe.THUCTE_CTs.ToList();
                }
                else
                {
                    danhSachThucTeChiTiet = new List<COCAUGIA_THUCTE_CT>();
                    var danhSachChiTietCuaDichVu = _ICOCAUGIA_CHITIETService.GetByMaTrung4So(maDichVu);
                    Session["idDichVu"] = danhSachChiTietCuaDichVu.FirstOrDefault().DICHVU21_ID.ID;

                    int index = 0;
                    foreach (var item in danhSachChiTietCuaDichVu)
                    {
                        danhSachThucTeChiTiet.Add(new COCAUGIA_THUCTE_CT
                        {
                            ID = index,
                            LOAI = item.LOAI_CC,
                            MA_COCAU = item.MA_CC,
                            DONGIA_OLD = item.DONGIA_CC,
                            SO_LUONG_OLD = item.SOLUONG_CC,
                            TEN_COCAU_OLD = item.TEN_CC,
                            DONGIA_NEW = item.DONGIA_CC,
                            TEN_COCAU_NEW = item.TEN_CC,
                            SO_LUONG_NEW = 0
                        });

                        index++;
                    }
                }

                Session["_GridViewChiTietDichVuPartial"] = danhSachThucTeChiTiet;
                return PartialView("_GridViewChiTietDichVuPartial", Session["_GridViewChiTietDichVuPartial"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewChiTietDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewChiTietDichVuPartial", new List<COCAUGIA_CHITIET>().AsQueryable());
        }

        public JsonResult LuuTamDuLieu(int cskcbId, string spHsba)
        {
            try
            {
                Session["cskcbId"] = cskcbId;
                Session["spHsba"] = spHsba;

                return Json(new { status = "success" });
            }
            catch (Exception e)
            {
                return Json(new { status = "error", mess = e.ToString() });
            }
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<COCAUGIA_THUCTE_CT, int> updateValues)
        {
            /*
            foreach (var item in updateValues.Update)
            {
                var data = new COCAUGIA_PHATSINH();

                data.CHITIET_ID = Convert.ToInt32(item.ID);
                data.COSOKCB_ID = Convert.ToInt32(Session["cskcbId"]);
                data.SP_HSBA = Convert.ToString(Session["spHsba"]);
                data.DONGIA_PS = item.DONGIA_CC;
                data.SOLUONG_PS = item.SOLUONG_CC;
                data.THANHTIEN_PS = data.SOLUONG_PS * data.DONGIA_PS;

                _ICOCAUGIA_PHATSINHService.Save(data);
            }*/

            long? idThucTe = Session["idThucTe"] as long?;

            // Lấy danh sách các COCAUGIA_THUCTE_CT trong session (được clone từ COCAUGIA_CHITIET)
            var thucTeCts = Session["_GridViewChiTietDichVuPartial"] as List<COCAUGIA_THUCTE_CT>;

            // Tạo dữ liệu mới
            if (idThucTe == null)
            {
                long? idDichVu = Session["idDichVu"] as long?;
                if (idDichVu != null)
                {
                    // Lấy dịch vụ
                    COCAUGIA_DICHVU21 dichvu21 = _ICOCAUGIA_DICHVU21Service.getById(idDichVu.Value).FirstOrDefault();

                    // Lấy cơ sở KCB
                    DM_COSOKCB cskcb = _IDMCOSOKCBService.GetByMa(Session["maCskcb"] as string).FirstOrDefault();

                    // Tạo một bản ghi vào COCAUGIA_THUCTE
                    COCAUGIA_THUCTE thucTe = new COCAUGIA_THUCTE
                    {
                        DICHVU21 = dichvu21,
                        COSOKCB = cskcb,
                        MA_THE = Session["maThe"] as string,
                        NGAY_VAO = DateTime.Parse(Session["ngayVao"] as string),
                        NGAY_RA = DateTime.Parse(Session["ngayRa"] as string),
                        THUCTE_CTs = new List<COCAUGIA_THUCTE_CT>()
                    };

                    // Ghi đè các phần tử đã được update trong form
                    foreach (var item in updateValues.Update)
                    {
                        int index = thucTeCts.FindIndex(x => x.ID == item.ID);
                        thucTeCts[index].SO_LUONG_NEW = item.SO_LUONG_NEW;
                        thucTeCts[index].DONGIA_NEW = item.DONGIA_NEW;
                        thucTeCts[index].TEN_COCAU_NEW = item.TEN_COCAU_NEW;
                    }

                    // Reset lại id và gán thực tế để insert vào DB
                    foreach (var item in thucTeCts)
                    {
                        item.ID = 0;
                        thucTe.THUCTE_CTs.Add(item);
                        item.THUCTE = thucTe;
                    }

                    _ICOCAUGIA_THUCTEService.Save(thucTe);
                    _ICOCAUGIA_THUCTEService.CommitChanges();
                }

            }
            else // Update lại dữ liệu
            {
                foreach (var item in updateValues.Update)
                {
                    int index = thucTeCts.FindIndex(x => x.ID == item.ID);
                    thucTeCts[index].SO_LUONG_NEW = item.SO_LUONG_NEW;
                    thucTeCts[index].DONGIA_NEW = item.DONGIA_NEW;
                    thucTeCts[index].TEN_COCAU_NEW = item.TEN_COCAU_NEW;

                    _ICOCAUGIA_THUCTE_CTService.Save(thucTeCts[index]);
                }

                _ICOCAUGIA_THUCTE_CTService.CommitChanges();

            }

            return _GridViewChiTietDichVuFilter(
                Session["maCskcb"] as string, 
                Session["maThe"] as string, 
                Session["ngayVao"] as string, 
                Session["ngayRa"] as string, 
                Session["maDichVu"] as string);
        }

        public ActionResult _ComboBoxCoSoKCBPartial()
        {
            return PartialView();
        }

        public ActionResult _ComboboxDichVu21Partial()
        {
            return PartialView();
        }
    }
}
