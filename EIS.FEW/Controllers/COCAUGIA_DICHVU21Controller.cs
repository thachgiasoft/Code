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
    public class COCAUGIA_DICHVU21Controller : Controller
    {
        //
        // GET: /COCAUGIA_DICHVU21/
        private static readonly ILog log = LogManager.GetLogger(typeof(COCAUGIA_DICHVU21Controller));
        private readonly ICOCAUGIA_CHITIETService _ICOCAUGIA_CHITIETService;
        private readonly ICOCAUGIA_DICHVU21Service _ICOCAUGIA_DICHVU21Service;
        private readonly ICOCAUGIA_LOAIService _ICOCAUGIA_LOAIService;
        private GridViewCustomBindingHandlers gridViewHanders;

        public COCAUGIA_DICHVU21Controller()
        {
            _ICOCAUGIA_DICHVU21Service = IoC.Resolve<ICOCAUGIA_DICHVU21Service>();
            _ICOCAUGIA_CHITIETService = IoC.Resolve<ICOCAUGIA_CHITIETService>();
            _ICOCAUGIA_LOAIService = IoC.Resolve<ICOCAUGIA_LOAIService>();
            gridViewHanders = new GridViewCustomBindingHandlers();
        }

        public ActionResult Index()
        {
            var model = new COCAUGIA_DICHVU();
            model.Filter = new COCAUGIA_DICHVU_FILTER();
            Session["_GridViewDanhSachDichVuPartial"] = null;
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

        public ActionResult _GridViewDanhSachDichVuPartial()
        {
            try
            {
                if (Session["_GridViewDanhSachDichVuPartial"] != null)
                    return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDanhSachDichVuPartial"]);
                else
                {
                    Session["_GridViewDanhSachDichVuPartial"] = _ICOCAUGIA_DICHVU21Service.Query;
                    return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDanhSachDichVuPartial"]);
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewDanhSachDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewDanhSachDichVuPartial", new List<COCAUGIA_DICHVU21>().AsQueryable());
        }

        public ActionResult _GridViewDanhSachDichVuFilter(string maTinh, string trangThai)
        {
            try
            {
                Session["maTinh"] = maTinh;
                Session["trangThai"] = trangThai;
                Session["_GridViewDanhSachDichVuPartial"] = _ICOCAUGIA_DICHVU21Service.getByFilter(maTinh, trangThai);
                return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDanhSachDichVuPartial"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewDanhSachDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewDanhSachDichVuPartial", new List<COCAUGIA_DICHVU21>().AsQueryable());
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

        public ActionResult _GridViewChiTietDichVuFilter(int nhomdvId)
        {
            try
            {
                ViewData["DichVuID"] = nhomdvId;
                Session["nhomdvId"] = nhomdvId;
                Session["_GridViewChiTietDichVuPartial"] = _ICOCAUGIA_CHITIETService.getByFilter(nhomdvId);
                return PartialView("_GridViewChiTietDichVuPartial", Session["_GridViewChiTietDichVuPartial"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewChiTietDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewChiTietDichVuPartial", new List<COCAUGIA_CHITIET>().AsQueryable());
        }

        public JsonResult Duyet(int[] ids)
        {
            try
            {
                foreach (int id in ids)
                {
                    var data = _ICOCAUGIA_DICHVU21Service.Getbykey(id);

                    if (data != null)
                    {
                        data.TRANGTHAI = 1;

                        _ICOCAUGIA_DICHVU21Service.Save(data);
                    }
                }

                _ICOCAUGIA_DICHVU21Service.CommitChanges();

                return Json(new { status = "success" });
            }
            catch (Exception e)
            {
                return Json(new { status = "error", mess = e.ToString() });
            }
        }

        public JsonResult DuyetAll(int[] ids)
        {
            try
            {
                _ICOCAUGIA_DICHVU21Service.UpdateAll();

                return Json(new { status = "success" });
            }
            catch (Exception e)
            {
                return Json(new { status = "error", mess = e.ToString() });
            }
        }

        public JsonResult HuyDuyet(int[] ids)
        {
            try
            {
                foreach (int id in ids)
                {
                    var data = _ICOCAUGIA_DICHVU21Service.Getbykey(id);

                    if (data != null)
                    {
                        data.TRANGTHAI = 0;

                        _ICOCAUGIA_DICHVU21Service.Save(data);
                    }
                }

                _ICOCAUGIA_DICHVU21Service.CommitChanges();

                return Json(new { status = "success" });
            }
            catch (Exception e)
            {
                return Json(new { status = "error", mess = e.ToString() });
            }
        }

        public JsonResult Xoa(int[] ids)
        {
            try
            {
                foreach (int id in ids)
                {
                    var data = _ICOCAUGIA_DICHVU21Service.Getbykey(id);

                    if (data != null && data.TRANGTHAI == 0) _ICOCAUGIA_DICHVU21Service.Delete(data);
                }

                _ICOCAUGIA_DICHVU21Service.CommitChanges();

                return Json(new { status = "success" });
            }
            catch (Exception e)
            {
                return Json(new { status = "error", mess = e.ToString() });
            }
        }

        public JsonResult TuChoi(int[] ids)
        {
            return HuyDuyet(ids);
        }
        public ActionResult AddNewChiTiet(COCAUGIA_CHITIET model, int DichVuID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = new COCAUGIA_CHITIET();

                    var loaicc = _ICOCAUGIA_LOAIService.getById(model.LOAI_CC.ID).FirstOrDefault();
                    var dichvu = _ICOCAUGIA_DICHVU21Service.getById(DichVuID).FirstOrDefault();

                    data.MA_CC = model.MA_CC;
                    data.TEN_CC = model.TEN_CC;
                    if (loaicc != null) data.LOAI_CC = loaicc;
                    data.SOLUONG_CC = model.SOLUONG_CC;
                    data.DONGIA_CC = model.DONGIA_CC;
                    data.THANHTIEN_CC = model.THANHTIEN_CC;
                    data.DICHVU21_ID = dichvu;
                    data.NGAYTAO = DateTime.Now;
                    data.NGAYCAPNHAT = DateTime.Now;

                    _ICOCAUGIA_CHITIETService.Save(data);
                    _ICOCAUGIA_CHITIETService.CommitChanges();
                }
                else
                    ViewData["EditError"] = "Dữ liệu bạn vừa nhập không chính xác.";
            }
            catch (Exception e)
            {
                ViewData["EditError"] = "Đã xảy ra lỗi vui lòng thử lại sau.";
                log.ErrorFormat("Add New - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return _GridViewChiTietDichVuPartial();
        }
        public ActionResult UpdateDichVu(COCAUGIA_DICHVU21 model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _ICOCAUGIA_DICHVU21Service.Getbykey(model.ID);
                    if (data != null)
                    {
                        data.TEN_DICHVU21 = model.TEN_DICHVU21;

                        _ICOCAUGIA_DICHVU21Service.Save(data);
                        _ICOCAUGIA_DICHVU21Service.CommitChanges();

                        return _GridViewDanhSachDichVuFilter(Convert.ToString(Session["maTinh"]), Convert.ToString(Session["trangThai"]));
                    }
                    else
                        ViewData["EditError"] = "Nhóm dịch vụ không tồn tại hoặc đã bị xoá.";
                }
                else
                    ViewData["EditError"] = "Dữ liệu bạn vừa nhập không chính xác.";
            }
            catch (Exception e)
            {
                ViewData["EditError"] = "Đã xảy ra lỗi vui lòng thử lại sau.";
                log.ErrorFormat("Update - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return _GridViewDanhSachDichVuPartial();
        }

        public ActionResult DeleteDichVu(int id)
        {
            if (id > 0)
            {
                try
                {
                    var data = _ICOCAUGIA_DICHVU21Service.Getbykey(id);

                    if (data != null && data.TRANGTHAI == 0)
                    {
                        _ICOCAUGIA_DICHVU21Service.Delete(data);
                        _ICOCAUGIA_DICHVU21Service.CommitChanges();
                    }

                    return _GridViewDanhSachDichVuFilter(Convert.ToString(Session["maTinh"]), Convert.ToString(Session["trangThai"]));
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Đã xảy ra lỗi vui lòng thử lại sau.";
                    log.ErrorFormat("Update - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
                }
            }
            return _GridViewDanhSachDichVuPartial();
        }

        public ActionResult UpdateChiTiet(COCAUGIA_CHITIET model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _ICOCAUGIA_CHITIETService.Getbykey(model.ID);
                    if (data != null)
                    {
                        var loaicc = _ICOCAUGIA_LOAIService.getById(model.LOAI_CC.ID).FirstOrDefault();

                        data.MA_CC = model.MA_CC;
                        data.TEN_CC = model.TEN_CC;
                        if (loaicc != null) data.LOAI_CC = loaicc;
                        data.SOLUONG_CC = model.SOLUONG_CC;
                        data.DONGIA_CC = model.DONGIA_CC;
                        data.THANHTIEN_CC = model.THANHTIEN_CC;

                        _ICOCAUGIA_CHITIETService.Save(data);
                        _ICOCAUGIA_CHITIETService.CommitChanges();

                        return _GridViewChiTietDichVuFilter(Convert.ToInt32(Session["nhomdvId"]));
                    }
                    else
                        ViewData["EditError"] = "Chi tiết dịch vụ không tồn tại hoặc đã bị xoá.";
                }
                else
                    ViewData["EditError"] = "Dữ liệu bạn vừa nhập không chính xác.";
            }
            catch (Exception e)
            {
                ViewData["EditError"] = "Đã xảy ra lỗi vui lòng thử lại sau.";
                log.ErrorFormat("Update - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return _GridViewChiTietDichVuPartial();
        }

        public ActionResult DeleteChiTiet(int id)
        {
            if (id > 0)
            {
                try
                {
                    _ICOCAUGIA_CHITIETService.Delete(id);
                    _ICOCAUGIA_CHITIETService.CommitChanges();

                    return _GridViewChiTietDichVuFilter(Convert.ToInt32(Session["nhomdvId"]));
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Đã xảy ra lỗi vui lòng thử lại sau.";
                    log.ErrorFormat("Update - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
                }
            }
            return _GridViewChiTietDichVuPartial();
        }
    }
}
