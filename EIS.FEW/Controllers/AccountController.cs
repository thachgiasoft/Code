using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using EIS.Core;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Context;
using FX.Core;
using IdentityManagement.Authorization;
using System.Collections.Generic;
using IdentityManagement.Domain;
using IdentityManagement.Service;
using log4net;
using EIS.Core.Common;
using EIS.Core.CustomView;

namespace EIS.FEW.Controllers
{
    public class AccountController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountController));
        public INGUOIDUNGService NguoidungService;
        private readonly ILogSystemService _iLogSystemService;
        private readonly FanxiAuthenticationBase authenticationService;
        public IuserService UserDataService;

        public AccountController()
        {
            NguoidungService = IoC.Resolve<INGUOIDUNGService>();
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
            UserDataService = IoC.Resolve<IuserService>();
            authenticationService = IoC.Resolve<FanxiAuthenticationBase>();

        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return RedirectToAction("Index", "NonAuthorize");
        }

        public ActionResult Index()
        {

            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            if (nguoidung != null)
            {
               // return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "EIS v" + RouteConfig.Version + " | Đăng nhập";
            Session.Clear();
            return View(new NGUOIDUNG
            {
                Time = new KyFilterSession(),
                CapImage = "data:image/png;base64," + Convert.ToBase64String(new Utility().VerificationTextGenerator()),
                CapImageText = Convert.ToString(Session["Captcha"])
            });
        }
      

        [HttpPost]
        public ActionResult Login(string username, string password, string captcha)
        {
            string CapImaText = Convert.ToString(Session["Captcha"]);
            if (captcha != CapImaText)
            {
                return Json(new { data = 3 }, JsonRequestBehavior.AllowGet);
            }
            Session["MustChangePW"] = false;
            _iLogSystemService.CreateNew(username.Trim(), "Đăng nhập hệ thống ", "Thực hiện đăng nhập hệ thống",
                Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
            try
            {
                Session["password"] = password;
                if (authenticationService.Logon(username.Trim(), password.Trim()))
                {
                    //isert log system_iLogSystemService.CreateNew(username.Trim(), "Đăng nhập hệ thống ", "Đăng nhập thành công",Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);

                    //CheckChangePassword cc_password = new CheckChangePassword();
                    //bool isChange = cc_password.CheckChange(password);

                    //var currentUser = UserDataService.Query.FirstOrDefault(x => x.username.ToUpper() == username.ToUpper()) ?? new user();
                    //var nguoidungId = NguoidungService.Query.FirstOrDefault(x => x.TENDANGNHAP.ToUpper() == username.Trim().ToUpper()) ?? new NGUOIDUNG();

                    //if (currentUser.ISADMIN != true && nguoidungId.ISPQ != true && nguoidungId.DF_LOAITG == null)
                    //{
                    //    if (!isChange)
                    //    {
                    //        Session["MustChangePW"] = true;
                    //    }

                    //    System.Web.HttpContext.Current.Session["COSOKCB_Session"] = nguoidungId.COSOKCBS;
                    //    return Json(new { data = 5 }, JsonRequestBehavior.AllowGet);
                    //}

                    //if (!isChange)
                    //{
                    //    Session["MustChangePW"] = true;
                    //    return Json(new { data = 4 }, JsonRequestBehavior.AllowGet);
                    //}


                    #region tungns - lấy danh sách permission của Người dùng
                    var currentuser =
                        UserDataService.Query.FirstOrDefault(x => x.username.ToUpper() == username.ToUpper());
                    if (currentuser == null)
                    {
                        return Json(new { data = 2 }, JsonRequestBehavior.AllowGet);
                    }
                    var currentuserRoles = currentuser.Roles;
                    var listPer = new List<permission>();
                    foreach (var role in currentuserRoles)
                    {
                        listPer.AddRange(role.Permissions.ToList());
                    }
                    var lstPer = new HashSet<String>(listPer.Select(item => item.name)).ToList();

                    Session["LIST_PERMISSION"] = lstPer;
                    #endregion

                    return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { data = 2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return Json(new { data = 6, message = e.Message, stack = e.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Account");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
     
        public ActionResult CskcbComboboxPatial()
        {
            return PartialView();
        }

        public ActionResult PartialPhienLamViec()
        {
            return View(new NGUOIDUNG
            {
                Time = new KyFilterSession()
            });
        }

        public ActionResult PartialImgCaptcha()
        {
            Session["Captcha"] = null;
            NGUOIDUNG objNguoidung = new NGUOIDUNG();
            objNguoidung.CapImage = "data:image/png;base64," + Convert.ToBase64String(new Utility().VerificationTextGenerator());
            objNguoidung.CapImageText = Convert.ToString(Session["Captcha"]);
            return PartialView(objNguoidung);
        }
    }
}