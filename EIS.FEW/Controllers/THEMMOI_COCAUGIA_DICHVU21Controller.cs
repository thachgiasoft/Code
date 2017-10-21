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
    public class THEMMOI_COCAUGIA_DICHVU21Controller : Controller
    {
        //
        // GET: /THEMMOI_COCAUGIA_DICHVU21/
        private static readonly ILog log = LogManager.GetLogger(typeof(THEMMOI_COCAUGIA_DICHVU21Controller));
        private readonly ICOCAUGIA_CHITIETService _ICOCAUGIA_CHITIETService;
        private readonly ICOCAUGIA_DICHVU21Service _ICOCAUGIA_DICHVU21Service;
        private readonly ICOCAUGIA_LOAIService _ICOCAUGIA_LOAIService;
        private readonly IDM_GIADICHVUService _IDM_GIADICHVUService;
        private readonly IDM_DICHVU43Service _IDM_DICHVU43Service;
        private readonly IDM_DICHVU50Service _IDM_DICHVU50Service;
        //private readonly string nameGridDanhSach = "gridviewDanhSach";
        //private readonly string nameGridChiTiet = "gridviewChiTiet";
        private GridViewCustomBindingHandlers gridViewHanders;

        public THEMMOI_COCAUGIA_DICHVU21Controller()
        {
            _ICOCAUGIA_DICHVU21Service = IoC.Resolve<ICOCAUGIA_DICHVU21Service>();
            _ICOCAUGIA_CHITIETService = IoC.Resolve<ICOCAUGIA_CHITIETService>();
            _ICOCAUGIA_LOAIService = IoC.Resolve<ICOCAUGIA_LOAIService>();
            _IDM_GIADICHVUService = IoC.Resolve<IDM_GIADICHVUService>();
            _IDM_DICHVU43Service = IoC.Resolve<IDM_DICHVU43Service>();
            _IDM_DICHVU50Service = IoC.Resolve<IDM_DICHVU50Service>();
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

        private static GridViewModel CreateGridViewModelWithSummary()
        {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "ID";
            //viewModel.Columns.Add("MA_DICHVU21");
            return viewModel;
        }

        private PartialViewResult AdvancedCustomBindingCore(string viewName, GridViewModel viewModel)
        {
            viewModel.ProcessCustomBinding(
                gridViewHanders.GetDataRowCountAdvanced,
                gridViewHanders.GetDataAdvanced,
                gridViewHanders.GetSummaryValuesAdvanced,
                gridViewHanders.GetGroupingInfoAdvanced,
                gridViewHanders.GetUniqueHeaderFilterValuesAdvanced
            );
            return PartialView(viewName, viewModel);
        }

        public ActionResult _GridViewDanhSachDichVuPartial()
        {
            try
            {
                //var viewModel = GridViewExtension.GetViewModel(nameGridDanhSach);
                //if (viewModel == null)
                //{
                //    viewModel = CreateGridViewModelWithSummary();
                //}
                //viewModel.Pager.PageSize = 15;
                //gridViewHanders.Model = _ICOCAUGIA_DICHVU21Service.Query;
                ////gridViewHanders.Model = new List<COCAUGIA_DICHVU21>().AsQueryable();
                //return AdvancedCustomBindingCore("_GridViewDanhSachDichVuPartial", viewModel);
                if (Session["_GridViewDanhSachDichVuPartial"] != null)
                    return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDanhSachDichVuPartial"]);
                else
                {
                    Session["_GridViewDanhSachDichVuPartial"] = _IDM_GIADICHVUService.Query.Where(item => item.MA_DICHVU21.StartsWith("37"));
                    return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDanhSachDichVuPartial"]);
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewDanhSachDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewDanhSachDichVuPartial", new List<DM_GIADICHVU>().AsQueryable());
        }

        public ActionResult _GridViewDanhSachDichVuFilter(string nguonDuLieu)
        {
            try
            {
                //var viewModel = GridViewExtension.GetViewModel(nameGridDanhSach);
                //if (viewModel == null)
                //{
                //    viewModel = CreateGridViewModelWithSummary();
                //}
                //viewModel.Pager.PageSize = 15;
                ////gridViewHanders.Model = _ICOCAUGIA_DICHVU21Service.Query;
                //gridViewHanders.Model = new List<COCAUGIA_DICHVU21>().AsQueryable();
                //return AdvancedCustomBindingCore("_GridViewDanhSachDichVuPartial", viewModel);
                if (nguonDuLieu.Equals("Thông tư 37"))
                    Session["_GridViewDanhSachDichVuPartial"] = _IDM_GIADICHVUService.Query.Where(item => item.MA_DICHVU21.StartsWith("37"));
                if (nguonDuLieu.Equals("Thông tư 50"))
                    Session["_GridViewDanhSachDichVuPartial"] = _IDM_DICHVU50Service.Query;
                if (nguonDuLieu.Equals("Thông tư 42"))
                    Session["_GridViewDanhSachDichVuPartial"] = _IDM_DICHVU43Service.Query;
                if (nguonDuLieu.Equals("Ngoài thông tư"))
                    Session["_GridViewDanhSachDichVuPartial"] = _ICOCAUGIA_DICHVU21Service.Query.Where(item=>!item.MA_DICHVU21.StartsWith("37.") && !item.MA_DICHVU21.StartsWith("43."));
                if (Session["_GridViewDanhSachDichVuPartial"] != null)
                    return PartialView("_GridViewDanhSachDichVuPartial", Session["_GridViewDanhSachDichVuPartial"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewDanhSachDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewDanhSachDichVuPartial", new List<DM_GIADICHVU>().AsQueryable());
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

        public ActionResult _GridViewChiTietDichVuFilter(string maNhomdv, string tenNhomdv)
        {
            try
            {
                var data = _ICOCAUGIA_DICHVU21Service.Query.Where(item => item.MA_DICHVU21 == maNhomdv);

                if (data.Count<COCAUGIA_DICHVU21>() == 0)
                {
                    var newData = new COCAUGIA_DICHVU21();

                    newData.MA_DICHVU21 = maNhomdv;
                    newData.TEN_DICHVU21 = tenNhomdv;
                    newData.TRANGTHAI = 0;

                    _ICOCAUGIA_DICHVU21Service.Save(newData);
                    _ICOCAUGIA_DICHVU21Service.CommitChanges();
                }

                Session["_GridViewChiTietDichVuPartial"] = _ICOCAUGIA_CHITIETService.getByFilter(maNhomdv);
                return PartialView("_GridViewChiTietDichVuPartial", Session["_GridViewChiTietDichVuPartial"]);
            }
            catch (Exception e)
            {
                log.ErrorFormat("_GridViewChiTietDichVuPartial - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return PartialView("_GridViewChiTietDichVuPartial", new List<COCAUGIA_CHITIET>().AsQueryable());
        }

        public dynamic Delete(int[] ids)
        {
            try
            {
                //foreach (int id in ids) _ICOCAUGIA_DICHVU21Service.Delete(id);

                //_ICOCAUGIA_DICHVU21Service.CommitChanges();

                return new { status = "success" };
            }
            catch (Exception e)
            {
                return new { status = "error", mess = e.ToString() };
            }
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

                        return _GridViewDanhSachDichVuPartial();
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

        [HttpPost]
        public JsonResult AddNewChiTiet(COCAUGIA_CHITIET model, string MA_DICHVU21, string TEN_DICHVU21, long LOAI_CC_ID)
        {
            string mess = "1";
            try
            {
                var data = new COCAUGIA_CHITIET();

                var loaicc = _ICOCAUGIA_LOAIService.getById(LOAI_CC_ID).FirstOrDefault();
                model.DICHVU21_ID = _ICOCAUGIA_DICHVU21Service.Query.Where(item => item.MA_DICHVU21 == MA_DICHVU21).FirstOrDefault();

                if (model.DICHVU21_ID == null)
                {
                    model.DICHVU21_ID = new COCAUGIA_DICHVU21();

                    model.DICHVU21_ID.MA_DICHVU21 = MA_DICHVU21;
                    model.DICHVU21_ID.TEN_DICHVU21 = TEN_DICHVU21;
                    model.DICHVU21_ID.TRANGTHAI = 0;

                    _ICOCAUGIA_DICHVU21Service.Save(model.DICHVU21_ID);
                    mess = "2";
                }

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
                        data.TEN_CC = model.TEN_CC;

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

        public ActionResult _ComboBoxChitietCCPartial()
        {
            return PartialView();
        }
    }
}
