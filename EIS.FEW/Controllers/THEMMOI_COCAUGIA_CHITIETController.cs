using DevExpress.Web.Mvc;
using EIS.Core.Common;
using EIS.Core.Domain;
using EIS.Core.IService;
using EIS.FEW.Models;
using FX.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIS.FEW.Controllers
{
    public class THEMMOI_COCAUGIA_CHITIETController : Controller
    {
        //
        // GET: /THEMMOI_COCAUGIA_DICHVU21/
        private static readonly ILog log = LogManager.GetLogger(typeof(THEMMOI_COCAUGIA_DICHVU21Controller));
        private readonly ICOCAUGIA_CHITIETService _ICOCAUGIA_CHITIETService;
        private readonly ICOCAUGIA_DICHVU21Service _ICOCAUGIA_DICHVU21Service;
        private readonly ICOCAUGIA_LOAIService _ICOCAUGIA_LOAIService;
        //private readonly string nameGridDanhSach = "gridviewDanhSach";
        //private readonly string nameGridChiTiet = "gridviewChiTiet";
        private GridViewCustomBindingHandlers gridViewHanders;

        public THEMMOI_COCAUGIA_CHITIETController()
        {
            _ICOCAUGIA_DICHVU21Service = IoC.Resolve<ICOCAUGIA_DICHVU21Service>();
            _ICOCAUGIA_CHITIETService = IoC.Resolve<ICOCAUGIA_CHITIETService>();
            _ICOCAUGIA_LOAIService = IoC.Resolve<ICOCAUGIA_LOAIService>();
            gridViewHanders = new GridViewCustomBindingHandlers();
        }

        public ActionResult Index(int id)
        {
            var model = new COCAUGIA_DICHVU();
            model.Filter = new COCAUGIA_DICHVU_FILTER();
            Session["_GridViewDanhSachDichVuPartial"] = null;
            Session["_GridViewChiTietDichVuPartial"] = null;
            try
            {
                var data = _ICOCAUGIA_DICHVU21Service.Getbykey(id);
                Session["idDichVu"] = id;
                ViewData["TEN_DV"] = data.TEN_DICHVU21;
                ViewData["MA_DV"] = data.MA_DICHVU21;
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
                return PartialView("_GridViewChiTietDichVuPartial", _ICOCAUGIA_CHITIETService.getByFilter(Convert.ToInt32(Session["idDichVu"])));
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewChiTietDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewChiTietDichVuPartial", new List<COCAUGIA_CHITIET>().AsQueryable());
        }

        //public ActionResult _GridViewChiTietDichVuFilter(int nhomdvId)
        //{
        //    try
        //    {
        //        Session["_GridViewChiTietDichVuPartial"] = _ICOCAUGIA_CHITIETService.getByFilter(nhomdvId);
        //        return PartialView("_GridViewChiTietDichVuPartial", Session["_GridViewChiTietDichVuPartial"]);
        //    }
        //    catch (Exception e)
        //    {
        //        log.ErrorFormat("_GridViewChiTietDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
        //    }
        //    return PartialView("_GridViewChiTietDichVuPartial", new List<COCAUGIA_CHITIET>().AsQueryable());
        //}

        [HttpPost]
        public JsonResult AddNewChiTiet(COCAUGIA_CHITIET model, string MA_DICHVU21, string TEN_DICHVU21, long LOAI_CC_ID)
        {
            string mess = "1";
            try
            {
                var data = new COCAUGIA_CHITIET();

                var loaicc = _ICOCAUGIA_LOAIService.getById(LOAI_CC_ID).FirstOrDefault();
                model.DICHVU21_ID = _ICOCAUGIA_DICHVU21Service.Getbykey(Convert.ToInt32(Session["idDichVu"]));

                data.MA_CC = model.MA_CC;
                data.TEN_CC = model.TEN_CC;
                if (loaicc != null) data.LOAI_CC = loaicc;
                data.SOLUONG_CC = model.SOLUONG_CC;
                data.DONGIA_CC = model.DONGIA_CC;
                data.THANHTIEN_CC = model.THANHTIEN_CC;
                data.DICHVU21_ID = model.DICHVU21_ID;

                _ICOCAUGIA_CHITIETService.Save(data);
                _ICOCAUGIA_CHITIETService.CommitChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = "Đã xảy ra lỗi vui lòng thử lại sau.";
                log.ErrorFormat("Add New - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
                mess = "0";
            }
            return Json(new { status = "success", mess = mess });
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

                        return _GridViewChiTietDichVuPartial();
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

                    return _GridViewChiTietDichVuPartial();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Đã xảy ra lỗi vui lòng thử lại sau.";
                    log.ErrorFormat("Update - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
                }
            }
            return _GridViewChiTietDichVuPartial();
        }
        
        public ActionResult _ComboBoxChitietCCPartial()
        {
            return PartialView();
        }
    }
}
