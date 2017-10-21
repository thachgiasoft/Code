using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using IdentityManagement.Authorization;
using EIS.Core;
using EIS.Core.Common;
using DevExpress.Web.Mvc;
using log4net;

namespace EIS.FEW.Controllers
{
    [Authorize]
    public class LogSystemController : Controller
    {
       private static readonly ILog log = LogManager.GetLogger(typeof(DM_DONVI));
        public ILogSystemService LogService;
        private NGUOIDUNG nguoidung;
        private GridViewCustomBindingHandlers gridViewHanders = null;
        private readonly string nameGrid = "gvLogSystem";
        int? sobanghi_CSG;

        public LogSystemController()
        {
            LogService = IoC.Resolve<ILogSystemService>();
            nguoidung = ((EISContext)FX.Context.FXContext.Current).CurrentNguoidung;
            gridViewHanders = new GridViewCustomBindingHandlers();
        }
        //[RBACAuthorize(Permissions = "LogSystem_VIEW")]
        public ActionResult Index()
        {
            Session.Remove("strTimKiem_Log");
            Session.Remove("fromDate_Log");
            Session.Remove("toDate_Log");
            Session.Remove("ListLog");
            return View(new List<LogSystem>().AsQueryable());
        }

        [ValidateInput(false)]
        public ActionResult LogPartial()
        {
            var strTimKiem = Session["strTimKiem_Log"] == null ? string.Empty : Session["strTimKiem_Log"].ToString();
            var fromDate = Session["fromDate_Log"] == null ? null : (DateTime?)(Session["fromDate_Log"]);
            var toDate = Session["toDate_Log"] == null ? null : (DateTime?)Session["toDate_Log"];
            var IsAdmin = false;
            if (nguoidung.TENDANGNHAP == "admin")
            {
                IsAdmin = true;
            }
            int? sobanghi = null;
            var lstLog = LogService.GetByFilter(strTimKiem, fromDate, toDate, IsAdmin, nguoidung.TENDANGNHAP);
            var viewModel = GridViewExtension.GetViewModel(nameGrid);
            if (viewModel == null)
            {
                viewModel = CreateGridViewModelWithSummary();
            }
            viewModel.Pager.PageSize = (sobanghi ?? 15);
            sobanghi_CSG = (sobanghi ?? 15);
            List<LogSystem> list = new List<LogSystem>();
            if (Session["ListLog"] != null)
            {
                list = (List<LogSystem>)Session["ListLog"];
            }
            gridViewHanders.Model = list.AsQueryable();
            Session["ListLog"] = lstLog;
            return AdvancedCustomBindingCore(viewModel);
            //return PartialView("LogPartial", LogService.GetByFilter(strTimKiem, fromDate, toDate,IsAdmin,nguoidung.TENDANGNHAP));
        }
        //[RBACAuthorize(Permissions = "LogSystem_VIEW")]
        public ActionResult ft_TimKiem(string strTimKiem, string fromDate, string toDate, int ? sobanghi)
        {
            DateTime? Tungay = null;
            DateTime? Denngay = null;
            if (fromDate != null)
            {
                Tungay = new DateTime(int.Parse(fromDate.Split('/')[2]), int.Parse(fromDate.Split('/')[0]), int.Parse(fromDate.Split('/')[1]),00, 00, 00);
            }
            if (toDate != null)
            {
                Denngay = new DateTime(int.Parse(toDate.Split('/')[2]), int.Parse(toDate.Split('/')[0]), int.Parse(toDate.Split('/')[1]),23, 59,59);
            }
            //if (Tungay == Denngay)
            //{
            //    Tungay = 
            //}
            Session["strTimKiem_Log"] = strTimKiem;
            Session["fromDate_Log"] = Tungay;
            Session["toDate_Log"] = Denngay;
            var IsAdmin = false;
            if (nguoidung.TENDANGNHAP == "admin")
            {
                IsAdmin = true;
            }
            var lstLog = LogService.GetByFilter(strTimKiem, Tungay, Denngay, IsAdmin, nguoidung.TENDANGNHAP);
            Session["ListLog"] = lstLog;
            var viewModel = GridViewExtension.GetViewModel(nameGrid);
            viewModel.Pager.PageSize = (sobanghi ?? 15);
            sobanghi_CSG = (sobanghi ?? 15);
            gridViewHanders.Model = lstLog; // Tungay > Denngay ? null : lstLog;
            return AdvancedCustomBindingCore(viewModel);
            //return Json("");
            //return Tungay > Denngay ? null : PartialView("LogPartial", LogService.GetByFilter(strTimKiem, Tungay, Denngay,IsAdmin,nguoidung.TENDANGNHAP));
        }
        #region CustomBinding

        // Paging
        public ActionResult AdvancedCustomBindingPagingAction(GridViewPagerState pager)
        {
            try
            {
                var strTimKiem = Session["strTimKiem_Log"] == null ? string.Empty : Session["strTimKiem_Log"].ToString();
                var fromDate = Session["fromDate_Log"] == null ? null : (DateTime?)(Session["fromDate_Log"]);
                var toDate = Session["toDate_Log"] == null ? null : (DateTime?)Session["toDate_Log"];
                var IsAdmin = false;
                if (nguoidung.TENDANGNHAP == "admin")
                {
                    IsAdmin = true;
                }
                var lstLog = LogService.GetByFilter(strTimKiem, fromDate, toDate, IsAdmin, nguoidung.TENDANGNHAP);
                Session["ListLog"] = lstLog;
                gridViewHanders.Model = lstLog;
                var viewModel = GridViewExtension.GetViewModel(nameGrid);
                viewModel.ApplyPagingState(pager);
                return AdvancedCustomBindingCore(viewModel);
            }
            catch (Exception e)
            {
                //log.Error("Paging_DM_CHISOGIACatch:" + e.ToString());
                log.Error("getFilter-AdvancedCustomBindingPagingAction: " + e.Message);
                ViewData["EditError"] = "getFilter-AdvancedCustomBindingPagingAction: " + e.Message;
                return LogPartial();
            }
        }

        // Filtering
        public ActionResult AdvancedCustomBindingFilteringAction(GridViewFilteringState filteringState)
        {
            try
            {
                var strTimKiem = Session["strTimKiem_Log"] == null ? string.Empty : Session["strTimKiem_Log"].ToString();
                var fromDate = Session["fromDate_Log"] == null ? null : (DateTime?)(Session["fromDate_Log"]);
                var toDate = Session["toDate_Log"] == null ? null : (DateTime?)Session["toDate_Log"];
                var IsAdmin = false;
                if (nguoidung.TENDANGNHAP == "admin")
                {
                    IsAdmin = true;
                }
                var lstLog = LogService.GetByFilter(strTimKiem, fromDate, toDate, IsAdmin, nguoidung.TENDANGNHAP);
                Session["ListLog"] = lstLog;
                gridViewHanders.Model = lstLog;
                var viewModel = GridViewExtension.GetViewModel(nameGrid);
                viewModel.Pager.PageSize = (sobanghi_CSG ?? 15);
                viewModel.ApplyFilteringState(filteringState);
                return AdvancedCustomBindingCore(viewModel);
            }
            catch (Exception e)
            {
                log.Error("getFilter-AdvancedCustomBindingFilteringAction: " + e.Message);
                ViewData["EditError"] = "getFilter-AdvancedCustomBindingFilteringAction: " + e.Message;
                return LogPartial();
            }
        }

        // Sorting
        public ActionResult AdvancedCustomBindingSortingAction(GridViewColumnState column, bool reset)
        {
            try
            {
                var strTimKiem = Session["strTimKiem_Log"] == null ? string.Empty : Session["strTimKiem_Log"].ToString();
                var fromDate = Session["fromDate_Log"] == null ? null : (DateTime?)(Session["fromDate_Log"]);
                var toDate = Session["toDate_Log"] == null ? null : (DateTime?)Session["toDate_Log"];
                var IsAdmin = false;
                if (nguoidung.TENDANGNHAP == "admin")
                {
                    IsAdmin = true;
                }
                var lstLog = LogService.GetByFilter(strTimKiem, fromDate, toDate, IsAdmin, nguoidung.TENDANGNHAP);
                Session["ListLog"] = lstLog;
                gridViewHanders.Model = lstLog;
                var viewModel = GridViewExtension.GetViewModel(nameGrid);
                viewModel.Pager.PageSize = (sobanghi_CSG ?? 15);
                viewModel.ApplySortingState(column, reset);
                return AdvancedCustomBindingCore(viewModel);
            }
            catch (Exception e)
            {
                log.Error("getFilter-AdvancedCustomBindingSortingAction: " + e.Message);
                ViewData["EditError"] = "getFilter-AdvancedCustomBindingSortingAction: " + e.Message;
                return LogPartial();
            }
        }

        // Grouping
        public ActionResult AdvancedCustomBindingGroupingAction(GridViewColumnState column)
        {
            var viewModel = GridViewExtension.GetViewModel(nameGrid);
            viewModel.ApplyGroupingState(column);
            return AdvancedCustomBindingCore(viewModel);
        }

        private PartialViewResult AdvancedCustomBindingCore(GridViewModel viewModel)
        {
            viewModel.ProcessCustomBinding(
                gridViewHanders.GetDataRowCountAdvanced,
                gridViewHanders.GetDataAdvanced,
                gridViewHanders.GetSummaryValuesAdvanced,
                gridViewHanders.GetGroupingInfoAdvanced,
                gridViewHanders.GetUniqueHeaderFilterValuesAdvanced
            );
            var list = (IQueryable<LogSystem>)gridViewHanders.Model.ApplyFilter(viewModel.FilterExpression).AsQueryable();
            return PartialView("LogPartial", viewModel);
        }

        private static GridViewModel CreateGridViewModelWithSummary()
        {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "ID";
            viewModel.Columns.Add("ID");
            viewModel.Columns.Add("User.username");
            viewModel.Columns.Add("EVENT");
            viewModel.Columns.Add("COMMENT_LOG");
            viewModel.Columns.Add("IPADDRESS");
            viewModel.Columns.Add("DICHVU");
            viewModel.Columns.Add("BROWSER");
            viewModel.Columns.Add("CREATEDATE");
            return viewModel;
        }
        #endregion
    }
}
