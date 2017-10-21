using EIS.Core;
using EIS.Core.IService;
using FX.Context;
using FX.Core;
using log4net;
using System;
using System.Globalization;
using System.Web.Mvc;
using EIS.Core.Common;

namespace EIS.FEW.Controllers
{
    /// <summary>
    /// Base of all controller when user login system
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// return role of user
        /// </summary>
        /// <returns>0 - user null, 1 - TW, 2 - Tỉnh, 3 - Trưởng nhóm, 4 - Trưởng nhóm, giám định viên cấp cơ sở</returns>
        protected int GetUserRole()
        {
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            return Constants.CheckVaiTro(nguoidung);
        }

        protected bool TryLogSystem(string yourEvent, string yourComment)
        {
            try
            {
                ILogSystemService _iLogSystemService = IoC.Resolve<ILogSystemService>();
                var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
                _iLogSystemService.CreateNew(nguoidung.TENDANGNHAP.Trim(), yourEvent.Trim(), yourComment.Trim(),
                Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        protected bool TryLog(ILog log, Exception ex)
        {
            try
            {
                log.ErrorFormat("message: {0}{1} stacktrace: {2}", ex.Message, Environment.NewLine, ex.StackTrace);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected bool TryMeasureTime(ILog log, string numberOrder)
        {
            try
            {
                log.Warn(numberOrder + ":" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                         CultureInfo.InvariantCulture));
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void AlertSuccess(string message)
        {
            Session["ALERT_MESSAGE"] = message;
            TempData["ALERT_TYPE"] = "success";
        }

        protected void AlertWarning(string message)
        {
            Session["ALERT_MESSAGE"] = message;
            TempData["ALERT_TYPE"] = "warning";
        }

        protected void AlertError(string message)
        {
            Session["ALERT_MESSAGE"] = message;
            TempData["ALERT_TYPE"] = "error";
        }

        [HttpPost]
        public JsonResult ShowMessage()
        {
            var message = Session["ALERT_MESSAGE"] as string;
            Session["ALERT_MESSAGE"] = null;
            TempData["ALERT_TYPE"] = null;
            return Json(new
            {
                data = message
            });
        }
    }
}