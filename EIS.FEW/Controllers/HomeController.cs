using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIS.FEW.Models;
using DevExpress.Web.Mvc;
using DevExpress.DashboardWeb.Mvc;
using System.IO;
using FX.Context;
using EIS.Core;
using EIS.Core.IService;
using FX.Core;

namespace EIS.FEW.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            if (nguoidung != null)
            {
                // DXCOMMENT: Pass a data model for GridView
                ViewBag.Title = "CƠ CẤU GIÁ BẢO HIỂM Y TẾ";
                //return RedirectToAction("Index", "SuDungQuyKCB");
                return View();
            }
            return RedirectToAction("Index", "Account");
        }
        public ActionResult TaskRun()
        {
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            if (nguoidung != null)
            {
                ViewBag.NotificationTimesDelay = DateTime.Now;
                return PartialView();
            }
            return new EmptyResult();
        }

        [ValidateInput(false)]
        public ActionResult DashboardViewerPartial()
        {
            return PartialView("_DashboardViewerPartial", DashboardViewerSettings.Model);
        }
        public ActionResult PotentiallyError()
        {
            // DXCOMMENT: Pass a data model for GridView
            ViewBag.Title = "HỆ THỐNG GIÁM SÁT BẢO HIỂM Y TẾ";
            return View();
        }

        public ActionResult ErrorPage()
        {
            // DXCOMMENT: Pass a data model for GridView
            ViewBag.Title = "HỆ THỐNG GIÁM SÁT BẢO HIỂM Y TẾ";
            return View();
        }

        public ActionResult ErrDefault()
        {
            // DXCOMMENT: Pass a data model for GridView
            ViewBag.Title = "HỆ THỐNG GIÁM SÁT BẢO HIỂM Y TẾ";
            return View();
        }

        public ActionResult ErrUnauthorized()
        {
            // DXCOMMENT: Pass a data model for GridView
            ViewBag.Title = "HỆ THỐNG GIÁM SÁT BẢO HIỂM Y TẾ";
            return View();
        }

        public ActionResult ErrPageNotFound()
        {
            // DXCOMMENT: Pass a data model for GridView
            ViewBag.Title = "HỆ THỐNG GIÁM SÁT BẢO HIỂM Y TẾ";
            return View();
        }

        public ActionResult ErrServer()
        {
            // DXCOMMENT: Pass a data model for GridView
            ViewBag.Title = "HỆ THỐNG GIÁM SÁT BẢO HIỂM Y TẾ";
            return View();
        }
        public FileStreamResult DashboardViewerPartialExport()
        {
            return DevExpress.DashboardWeb.Mvc.DashboardViewerExtension.Export("DashboardViewer", DashboardViewerSettings.Model);
        }
    }
    class DashboardViewerSettings
    {
        public static DevExpress.DashboardWeb.Mvc.DashboardSourceModel Model
        {
            get
            {
                DashboardSourceModel model = new DashboardSourceModel();
                model.DashboardId = "EIS";
                model.DashboardLoading = (sender, e) =>
                {
                    if (e.DashboardId == "EIS")
                    {
                        string dashboardDefinition = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~\App_Data\EIS.xml"));
                        var nguoiDung = ((EISContext)FXContext.Current).CurrentNguoidung;
                        if (nguoiDung != null)
                        {
                            //if (nguoiDung.DF_COSOKCB_ID != null)
                            //{
                                //var cskcb = IoC.Resolve<IDMCOSOKCBService>().Getbykey(nguoiDung.DF_COSOKCB_ID.Value);
                                // Luu y check lai db de cskcb cua nguoi dung ko bi null
                                //if (cskcb != null)
                                //{
                                //    //var tencskcb = IoC.Resolve<IDMCOSOKCBService>().Getbykey(nguoiDung.DF_COSOKCB_ID.Value).TEN.ToUpper();
                                //    var tuthang = 0; var denthang = 0; var nam = 0;
                                //    if (nguoiDung.DF_LOAITG != null)
                                //    {
                                //        if (nguoiDung.DF_LOAITG == 2)
                                //        {
                                //            tuthang = nguoiDung.DF_THANG.Value;
                                //            denthang = nguoiDung.DF_THANG.Value;
                                //        }
                                //        else if (nguoiDung.DF_LOAITG == 1)
                                //        {
                                //            switch (nguoiDung.DF_QUY)
                                //            {
                                //                case 1:
                                //                    {
                                //                        tuthang = 1;
                                //                        denthang = 3;
                                //                        break;
                                //                    }
                                //                case 2:
                                //                    {
                                //                        tuthang = 4;
                                //                        denthang = 6;
                                //                        break;
                                //                    }
                                //                case 3:
                                //                    {
                                //                        tuthang = 7;
                                //                        denthang = 9;
                                //                        break;
                                //                    }
                                //                case 4:
                                //                    {
                                //                        tuthang = 10;
                                //                        denthang = 12;
                                //                        break;
                                //                    }
                                //            }

                                //        }
                                //    }
                                //    if (nguoiDung.DF_NAM != null) nam = nguoiDung.DF_NAM.Value;
                                //    e.DashboardXml = string.Format(dashboardDefinition, tencskcb, nguoiDung.DF_COSOKCB_ID, nam, tuthang, denthang);
                                //}
                                //else
                                //{
                                //    e.DashboardXml = string.Format(dashboardDefinition, "BỆNH VIỆN", 0, 0, 0, 0);
                                //}
                            //    e.DashboardXml = string.Format(dashboardDefinition, "BỆNH VIỆN", 0, 0, 0, 0);
                            //}
                            //else
                            //{
                            //    e.DashboardXml = string.Format(dashboardDefinition, "BỆNH VIỆN", 0, 0, 0, 0);
                            //}
                        }
                        else
                        {
                            e.DashboardXml = string.Format(dashboardDefinition, "BỆNH VIỆN", 0, 0, 0, 0);
                        }
                    }
                };
                return model;
            }
        }
    }

}

public enum HeaderViewRenderMode { Full, Title }