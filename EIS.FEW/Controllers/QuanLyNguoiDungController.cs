using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DevExpress.Office.Utils;
using EIS.Core;
using EIS.Core.Common;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.Core.IService;
using EIS.Core.ServiceImp;
using FX.Context;
using FX.Core;
using IdentityManagement.Domain;
using IdentityManagement.Service;
using DevExpress.Web;
using IdentityManagement.Authorization;
namespace EIS.FEW.Controllers
{
    [Authorize]
    public class QuanLyNguoiDungController : Controller
    {
        private readonly ILogSystemService _iLogSystemService;
        public IuserService UserDataService;
        public IroleService RoleService;
        public IDMCOSOKCBService DmcosokcbService;
        public INGUOIDUNGService Nguoidung;
        public IDM_DONVIService DmDonviService;
        public IApplicationsService ApplicationsService;
        public ITYPE_ROLEService TypeRoleService;
        private readonly NGUOIDUNG CurrentNguoidung;
        private static readonly char[] SpecialChars = "!@#$%^&*()".ToCharArray();
        private static readonly char[] UPPERCHAR = "QWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();
        private static readonly char[] NUMBER = "123456789".ToCharArray();
        public QuanLyNguoiDungController()
        {
            CurrentNguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            UserDataService = IoC.Resolve<IuserService>();
            RoleService = IoC.Resolve<IroleService>();
            DmcosokcbService = IoC.Resolve<IDMCOSOKCBService>();
            Nguoidung = IoC.Resolve<INGUOIDUNGService>();
            DmDonviService = IoC.Resolve<IDM_DONVIService>();
            ApplicationsService = IoC.Resolve<IApplicationsService>();
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
            TypeRoleService = IoC.Resolve<ITYPE_ROLEService>();
            GetValueCommon();
        }

        public ActionResult Index()
        {
            
            var insertLog1 = _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Truy cập chức năng QLND", "Truy cập chức năng QLND",
                       Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
           
            Session.Remove("Role_QLPQ");
            return GetValue("Index", string.Empty);
        }

        public ActionResult DS_NguoiDungPartial()
        {
            var strTimKiem = System.Web.HttpContext.Current.Session["str_TimKiem_QLPQ"] == null ? string.Empty
                : System.Web.HttpContext.Current.Session["str_TimKiem_QLPQ"].ToString();

            return GetValue("DS_NguoiDungPartial", strTimKiem);
            //return PartialView(UserDataService.Query);

        }

        public ActionResult ft_TimKiem_DSND(string strTimKiem)
        {
            System.Web.HttpContext.Current.Session["str_TimKiem_QLPQ"] = strTimKiem;
            return GetValue("DS_NguoiDungPartial", strTimKiem);
        }

        public static object GetPersonsRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            return (from person in IoC.Resolve<IuserService>().Query
                    where (person.username).StartsWith(args.Filter)
                    orderby person.username
                    select person
                    ).Skip(skip).Take(take);
        }

        public static object GetPersonByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            return IoC.Resolve<IuserService>().Query.Where(p => p.userid == id).Take(1);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(QuanLyNguoiDungModels models)
        {
            var error = string.Empty;
            var currentUser = UserDataService.Getbykey(models.ID) ?? new user();
            var currentNd = Nguoidung.Query.FirstOrDefault(x => x.TENDANGNHAP.ToUpper() == models.UserName.ToUpper()) ?? new NGUOIDUNG();
            var tempNgD = ((EISContext)FXContext.Current).CurrentNguoidung;
            var check = false;
            if (tempNgD.ISPQ == true)
            {
                if (models.VaiTro_ID == null)
                {
                    check = true;
                    error = error + "Vui lòng nhập vai trò người dùng!";
                }
                if (models.DonVi_ID == null)
                {
                    models.DonVi_ID = currentNd.DONVI_ID;
                }
                models.UserName = currentUser.username;
                models.Password = currentUser.password;
                models.IsApproved = currentUser.IsApproved;
                models.IsLockedOut = currentUser.IsLockedOut;
                models.IsAdmin = currentUser.ISADMIN;
                models.Email = currentUser.email;

                models.NguoiDungId = currentNd.ID;
                models.Ten = currentNd.TEN;
                models.SDT = currentNd.PHONE;
                models.SoCMND = currentNd.SOCMT;
                models.DiaChi = currentNd.ADDRESS;
                models.IsPQ = currentNd.ISPQ;
            }
            else
            {
                if (string.IsNullOrEmpty(models.Ten))
                {
                    check = true;
                    error = "Vui lòng nhập tên người dùng!";
                }
                else if (models.Email == null)
                {
                    check = true;
                    error = "Vui lòng nhập email!";
                }
                else if (models.DonVi_ID == null)
                {
                    check = true;
                    error = "Vui lòng nhập đơn vị!";
                }
                if (models.Password != null)
                {
                    if (models.Password.IndexOfAny(SpecialChars) == -1)
                    {
                        check = true;
                        error = "Password phải chứa ký tự đặc biệt!";
                    }
                    if (models.Password.IndexOfAny(UPPERCHAR) == -1)
                    {
                        check = true;
                        error = "Password phải chứa ký tự hoa!";
                    }
                    if (models.Password.IndexOfAny(NUMBER) == -1)
                    {
                        check = true;
                        error = "Password phải chứa số!";
                    }
                }
                else
                {
                    if(models.ID == 0)
                    {
                        check = true;
                        error = "Password không thể để trống!";
                    }
                }
                
                if (models.ID != 0)
                {
                    models.VaiTro_ID = currentNd.VAITRO;
                }
            }

            if (ModelState.IsValid && check == false)
            {
                var roles = models.Roles == null ? new List<long>() : models.Roles.Split(',').Where(x => !String.IsNullOrEmpty(x)).Select(x => Convert.ToInt64(x)).ToList();
                var cosokcb = models.COSO_KCBID == null ? new List<long>() : models.COSO_KCBID.Split(',').Where(x => !String.IsNullOrEmpty(x)).Select(x => Convert.ToInt64(x)).ToList();
                var tempDf = cosokcb.FirstOrDefault(x => x == currentNd.DF_COSOKCB_ID);
                var dfcskcb = tempDf != 0 ? currentNd.DF_COSOKCB_ID : null;
               // var redis = EIS.FEW.MvcApplication.redis;

                string password;
                if (tempNgD.ISPQ != true)
                {
                    password = !string.IsNullOrEmpty(models.Password)
                        ? FormsAuthentication.HashPasswordForStoringInConfigFile(models.Password, "MD5")
                        : currentUser.password;
                }
                else
                {
                    password = models.Password;
                }
                var user = new user
                {
                    userid = models.ID,
                    username = models.UserName,
                    password = password,
                    PasswordSalt = "MD5",
                    GroupName = tempNgD.ISPQ != true ? System.Web.HttpContext.Current.User.Identity.Name : currentUser.GroupName,
                    email = models.Email,
                    IsApproved = models.IsApproved != null && (bool)(models.IsApproved),
                    IsLockedOut = models.IsLockedOut != null && (bool)(models.IsLockedOut),
                    Roles = RoleService.Query.Where(t => roles.Contains(t.roleid)).ToList(),
                    CreateDate = DateTime.Now,
                    ISADMIN = models.IsAdmin != null && (bool)(models.IsAdmin),
                    ApplicationList = ApplicationsService.Query.Where(x => x.AppID == 1).ToList()
                };
                var nguoiDung = new NGUOIDUNG
                {
                    ID = models.NguoiDungId,
                    TEN = models.Ten,
                    TENDANGNHAP = models.UserName,
                    TRANGTHAI = 1,
                    PHONE = models.SDT,
                    SOCMT = models.SoCMND,
                    ADDRESS = models.DiaChi,
                    VAITRO = models.VaiTro_ID,
                    DONVI_ID = models.DonVi_ID == 0 ? null : models.DonVi_ID,
                    COSOKCBS = DmcosokcbService.Query.Where(t => cosokcb.Contains(t.ID)).ToList(),
                    ISPQ = models.IsPQ,
                    DF_COSOKCB_ID = dfcskcb,
                    DF_LOAITG = currentNd.DF_LOAITG,
                    DF_NAM = currentNd.DF_NAM,
                    DF_QUY = currentNd.DF_QUY,
                    DF_THANG = currentNd.DF_THANG
                };

                try
                {
                    UserDataService.BeginTran();
                    UserDataService.Clear();
                    UserDataService.Save(user);
                    // insert log
                    _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Tạo mới người dùng", "Tạo mới người dùng thành công:" + user.username,
                        Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);

                    Nguoidung.Save(nguoiDung);
                    UserDataService.CommitTran();
                   
                    if (user == null || nguoiDung == null )
                    {
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Tạo mới người dùng", "Tạo mới hoặc sửa người dùng thất bại do không thể kết nối redis",
                       Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                    }
                    else
                    {
                        //var bientam_nguoidung = new NGUOIDUNG();
                        //bientam_nguoidung.ADDRESS = nguoiDung.ADDRESS;
                        //bientam_nguoidung.CapImage = nguoiDung.CapImage;
                        //bientam_nguoidung.CapImageText = nguoiDung.CapImageText;
                        //bientam_nguoidung.CaptchaCodeText = nguoiDung.CaptchaCodeText;
                        //bientam_nguoidung.COSOKCB = nguoiDung.COSOKCB;
                        //bientam_nguoidung.COSOKCB.DM_DONVI.TINHTHANH = null;
                        //bientam_nguoidung.COSOKCB.DM_DONVI.DONVICHA = null;
                        //bientam_nguoidung.COSOKCBS = nguoiDung.COSOKCBS;
                        //foreach (var item in bientam_nguoidung.COSOKCBS)
                        //{
                        //    if (item.DM_DONVI != null)
                        //    {
                        //        item.DM_DONVI.DONVICHA = null;
                        //        item.DM_TINHTHANH = null;
                        //        item.DM_DONVIHANHCHINH = null;
                        //        item.DM_DONVI = null;
                        //        item.DM_QUANHUYEN = null;
                        //    }
                        //}
                   
                        //bientam_nguoidung.DF_COSOKCB_ID = nguoiDung.DF_COSOKCB_ID;
                        //bientam_nguoidung.DF_LOAITG = nguoiDung.DF_LOAITG;
                        //bientam_nguoidung.DF_NAM = nguoiDung.DF_NAM;
                        //bientam_nguoidung.DF_QUY = nguoiDung.DF_QUY;
                        //bientam_nguoidung.DF_THANG = nguoiDung.DF_THANG;
                        //bientam_nguoidung.DONVI = nguoiDung.DONVI;
                        //bientam_nguoidung.DONVI_ID = nguoiDung.DONVI_ID;
                        //bientam_nguoidung.ID = nguoiDung.ID;
                        //bientam_nguoidung.ISPQ = nguoiDung.ISPQ;
                        //bientam_nguoidung.PHONE = nguoiDung.PHONE;
                        //bientam_nguoidung.SOCMT = nguoiDung.SOCMT;
                        //bientam_nguoidung.TEN = nguoiDung.TEN;
                        //bientam_nguoidung.TENDANGNHAP = nguoiDung.TENDANGNHAP;
                        //bientam_nguoidung.TRANGTHAI = nguoiDung.TRANGTHAI;
                        //bientam_nguoidung.VAITRO = nguoiDung.VAITRO;
                        string keyUser = "USERDATA_" + user.username;
                        string keyNguoidung = "NGUOIDUNG_" + nguoiDung.TENDANGNHAP;
                       // redis.PushNguoiDung(keyNguoidung, nguoiDung);
                       // redis.PushRedis<user>(keyUser, user);
                    }
               //     redis.Close();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    UserDataService.RolbackTran();
                    ViewBag.IsAdd = models.ID == 0;
                    ViewBag.IsAdmin = tempNgD.ISPQ != true;
                //    redis.Close();
                    return View("TaoMoi_NguoiDungPartial", new QuanLyNguoiDungModels
                    {
                        ID = models.ID,
                        NguoiDungId = models.NguoiDungId,
                        UserName = models.UserName,
                        Password = models.Password,
                        PasswordRe = models.PasswordRe,
                        Email = models.Email,
                        IsApproved = models.IsApproved,
                        IsLockedOut = models.IsLockedOut,
                        IsAdmin = models.IsAdmin,
                        DonVi_ID = models.DonVi_ID,
                        VaiTro_ID = models.VaiTro_ID,
                        Ten = models.Ten,
                        COSO_KCBID = models.COSO_KCBID,
                        Roles = models.Roles,
                        IsPQ = models.IsPQ
                    });
                }
            }
            else
            {
                ViewBag.IsAdd = models.ID == 0;
                ViewBag.IsAdmin = tempNgD.ISPQ != true;
                ViewData["EditError"] = error == string.Empty ? Resources.Localizing.MessageCommon : error;
                return View("TaoMoi_NguoiDungPartial", new QuanLyNguoiDungModels
                {
                    ID = models.ID,
                    NguoiDungId = models.NguoiDungId,
                    UserName = models.UserName,
                    Password = models.Password,
                    PasswordRe = models.PasswordRe,
                    Email = models.Email,
                    IsApproved = models.IsApproved,
                    IsLockedOut = models.IsLockedOut,
                    IsAdmin = models.IsAdmin,
                    DonVi_ID = models.DonVi_ID,
                    VaiTro_ID = models.VaiTro_ID,
                    Ten = models.Ten,
                    COSO_KCBID = models.COSO_KCBID,
                    Roles = models.Roles,
                    IsPQ = models.IsPQ
                });
            }
            return RedirectToAction("Index", "QuanLyNguoiDung");
        }

        public ActionResult Delete(string UserId)
        {
          //  var redis = EIS.FEW.MvcApplication.redis;
            int userid = int.Parse(UserId);
            if (!CheckQuyenThucthi(userid))
            {
                return RedirectToAction("Index", "Home");
            }
            if (userid == 0 || UserDataService.Getbykey(userid) == null) return RedirectToAction("Index");
            try
            {
                var tempUser = UserDataService.Getbykey(userid);
                var firstOrDefault = Nguoidung.Query.FirstOrDefault(x => x.TENDANGNHAP == tempUser.username);
                var tempNd = firstOrDefault != null ? firstOrDefault.ID : 0;
                UserDataService.BeginTran();
                UserDataService.Delete(userid);
                Nguoidung.Delete(tempNd);
               
               
                // insert log
                _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa người dùng", "Xóa người dùng thành công:" + tempUser.username,
                    Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                UserDataService.CommitTran();
                if (tempUser == null || firstOrDefault == null )
                {
                    _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa người dùng", "Xóa người dùng thất bại do không kết nối được redis",
                  Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                }
                else
                {
                    string keyUser = "USERDATA_" + tempUser.username;
                    string keyNguoidung = "NGUOIDUNG_" + firstOrDefault.TENDANGNHAP;
                   // redis.RemoveRedisKey(keyUser);
                   // redis.RemoveRedisKey(keyNguoidung);
                }
             //   redis.Close();                
            }
            catch (Exception e)
            {
                //  ViewData["EditError"] = e.Message;
                ViewData["EditError"] = "Tồn tại chức năng sử dụng bởi người dùng này, bạn không thể xóa người dùng này!";
                UserDataService.RolbackTran();
                //   throw new Exception(Resources.Localizing.MessageDeleteError);
               // redis.Close();
                return Json("NotOK", JsonRequestBehavior.AllowGet);
            }
            var txtTimKiem = System.Web.HttpContext.Current.Session["str_TimKiem_QLPQ"] == null ?
                string.Empty : System.Web.HttpContext.Current.Session["str_TimKiem_QLPQ"].ToString();
            //   return GetValue("Index", txtTimKiem);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddDsnd(int userid)
        {
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var nguoidungdata = UserDataService.Query.FirstOrDefault(x => x.username.ToUpper() == nguoidung.TENDANGNHAP.ToUpper());
            if (!(nguoidung.ISPQ == true || nguoidungdata.ISADMIN == true))
            {
                return RedirectToAction("Index", "Home");
            }
            if(userid==0 && nguoidung.ISPQ==true)
            {
                return RedirectToAction("Index", "QuanLyNguoiDung");
            }
            if (!CheckQuyenThucthi(userid))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.IsAdd = userid == 0;
            ViewBag.IsAdmin = true;

            var tempNgD = ((EISContext)FXContext.Current).CurrentNguoidung;

            if (tempNgD.ISPQ == true)
            {
                ViewBag.IsAdmin = false;
            }

            System.Web.HttpContext.Current.Session["SelectCSKCB_QLPQ"] = null;
            //add
            if (userid == 0)
            {
                var temp = (user)Session["User_QLPQ"];
                var tempNd = ((EISContext)FXContext.Current).CurrentNguoidung;
                return View("TaoMoi_NguoiDungPartial", new QuanLyNguoiDungModels
                {
                    UserName = temp.userid + "_" + tempNd.DONVI_ID + "_"
                });
            }
            //update
            var usertemp = UserDataService.Getbykey(userid);

            var roles = string.Empty;
            if (usertemp != null)
            {
                var rolestemp = usertemp.Roles;
                roles = rolestemp.Aggregate("", (current, t) => current + "," + t.roleid);
            }

            var nguoidungtemp = Nguoidung.Query.FirstOrDefault(x => x.TENDANGNHAP.ToUpper() == usertemp.username.ToUpper());

            var cskcb = string.Empty;
            if (nguoidungtemp != null)
            {
                var nguoidungcskcb = nguoidungtemp.COSOKCBS;
                cskcb = nguoidungcskcb.Aggregate("", (current, t) => current + "," + t.ID);
                System.Web.HttpContext.Current.Session["SelectCSKCB_QLPQ"] = cskcb;
            }
            if (usertemp != null && nguoidungtemp != null)
                return View("TaoMoi_NguoiDungPartial", new QuanLyNguoiDungModels
                {
                    ID = usertemp.userid,
                    NguoiDungId = nguoidungtemp.ID,
                    UserName = usertemp.username,
                    Password = usertemp.password,
                    PasswordRe = usertemp.password,
                    Email = usertemp.email,
                    SDT = nguoidungtemp.PHONE,
                    DiaChi = nguoidungtemp.ADDRESS,
                    SoCMND = nguoidungtemp.SOCMT,
                    IsApproved = usertemp.IsApproved,
                    IsLockedOut = usertemp.IsLockedOut,
                    IsAdmin = usertemp.ISADMIN,
                    DonVi_ID = nguoidungtemp.DONVI_ID,
                    VaiTro_ID = nguoidungtemp.VAITRO,
                    Ten = nguoidungtemp.TEN,
                    COSO_KCBID = cskcb,
                    Roles = roles,
                    IsPQ = nguoidungtemp.ISPQ,
                });

            var tempUser = (user)Session["User_QLPQ"];
            return View("TaoMoi_NguoiDungPartial", new QuanLyNguoiDungModels
            {
                UserName = tempNgD.DONVI.MA + "_"
            });
        
        }

        public ActionResult CSKCBPartial()
        {
            return PartialView(Session["CSKCB_QLPQ"]);
        }

        [HttpPost]
        public JsonResult CheckUserName(int id, string userName)
        {
            if (id != 0) return Json(true);
            var user = UserDataService.Query.FirstOrDefault(x => x.username.ToUpper() == userName.ToUpper());
            return Json(user == null);
        }
        [HttpPost]
        public JsonResult CheckEmail(int id, string email)
        {
            if (id != 0) return Json(true);
            var user = UserDataService.Query.FirstOrDefault(x => x.email.ToUpper() == email.ToUpper());
            return Json(user == null);
        }


        #region Common
        private bool CheckQuyenThucthi(int userid)
        {
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var nguoidungdata = UserDataService.Query.FirstOrDefault(x => x.username.ToUpper() == nguoidung.TENDANGNHAP.ToUpper());
            var usertemp = UserDataService.Getbykey(userid);
            if (userid == 0 && (nguoidung.ISPQ==true || nguoidungdata.ISADMIN==true))
            {
                return true;
            }
            if (nguoidung.ISPQ == true)
            {
                if (nguoidungdata.GroupName == usertemp.GroupName)
                {
                    return true;
                }
            }
            if (nguoidungdata.ISADMIN == true)
            {
                if (nguoidungdata.username == usertemp.GroupName || nguoidungdata.username=="admin")
                {
                    return true;
                }
            }
            return false;
        }
        private ActionResult GetValue(string view, string txtTimkiem)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var firstUser = (user)Session["User_QLPQ"];
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            if (user.ToUpper() == "ADMIN")
            {
                return PartialView(view, UserDataService.Query.Where(x =>
                    string.IsNullOrEmpty(txtTimkiem)
                    || x.username.ToUpper().Contains(txtTimkiem.ToUpper())).OrderBy(x => x.userid));
            }
            if (firstRole != null)
            {
                return PartialView(view, UserDataService.Query.Where(x =>
                            x.username.ToUpper() != "ADMIN"
                            && (string.IsNullOrEmpty(txtTimkiem)
                            || x.username.ToUpper().Contains(txtTimkiem.ToUpper()))).OrderBy(x => x.userid));
            }
            if (nguoidung.ISPQ == true)
            {
                return PartialView(view, UserDataService.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper()
                    && x.ISADMIN != true
                    && (string.IsNullOrEmpty(txtTimkiem)
                    || x.username.ToUpper().Contains(txtTimkiem.ToUpper()))).OrderBy(x => x.userid));
            }

            return PartialView(view, UserDataService.Query.
                Where(x => x.username.ToUpper() == user.ToUpper() || x.GroupName.ToUpper() == user).
                Where(x => (string.IsNullOrEmpty(txtTimkiem) || x.username.ToUpper().Contains(txtTimkiem.ToUpper()))).OrderBy(x => x.userid));

        }

        private int CheckStatusTinhOrTrungUong()
        {
            var results = 2;
            var groupgame_1 = UserDataService.Query.FirstOrDefault(m => m.username == CurrentNguoidung.TENDANGNHAP).GroupName;
            var groupName_2 = UserDataService.Query.FirstOrDefault(m => m.username == groupgame_1).GroupName;
            if (groupName_2 == "admin")
                results = 1;
            return results;
        }
        private List<role> GetListRoles(int loaiuser)
        {
            string username = CurrentNguoidung.TENDANGNHAP;
            var _userRecord = UserDataService.Query.FirstOrDefault(m => m.username == username);
            List<role> _results = new List<role>();

            if (_userRecord != null)
            {
                var _lstrole = _userRecord.Roles;
                if (_lstrole.Any())
                {
                    var _userOfRoleRecord = _lstrole.FirstOrDefault(m => m.name == "Root");
                    if (_userOfRoleRecord != null)
                    {
                        var _lst = RoleService.Query.OrderBy(m => m.roleid);
                        _results = _lst.OrderBy(m => m.roleid).ToList();
                        return _results;
                    }
                }
             
                int userid = _userRecord.userid;
                var _lstTyperoles = TypeRoleService.Query.Where(m => (m.USERID == 0) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
                if (loaiuser == (int)EIS.Core.Common.Common.LoaiUser.Tinh)
                {
                    _lstTyperoles = TypeRoleService.Query.Where(m => (m.USERID == 0 && (m.TYPE == (int)EIS.Core.Common.Common.TypeRole.CapTinh || m.TYPE == (int)EIS.Core.Common.Common.TypeRole.CapKhac)) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
                }
                else
                {
                    _lstTyperoles = TypeRoleService.Query.Where(m => (m.USERID == 0 && m.TYPE==loaiuser) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
                }
                var _lstNotRoot = RoleService.Query.Where(m => _lstTyperoles.Contains(m.roleid));
                _results = _lstNotRoot.OrderBy(m => m.roleid).ToList();
            }
            return _results;
        }

        private void GetValueCommon()
        {
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            if (System.Web.HttpContext.Current.Session["User_QLPQ"] == null)
                System.Web.HttpContext.Current.Session["User_QLPQ"] =
                    UserDataService.Query.FirstOrDefault(x => x.username.ToUpper() == username.ToUpper());
            //get list don vi 
            var nguoidungId = ((EISContext)FXContext.Current).CurrentNguoidung;
            if (nguoidungId.ISPQ == true)
            {
                System.Web.HttpContext.Current.Session["IsPQ_QLPQ"] = true;
            }
            else
            {
                System.Web.HttpContext.Current.Session["IsPQ_QLPQ"] = false;
            }
            if (System.Web.HttpContext.Current.Session["DonVi_QLPQ"] == null)
                System.Web.HttpContext.Current.Session["DonVi_QLPQ"] =
                    DmDonviService.Query.Where(x => x.ID == nguoidungId.DONVI_ID || x.DONVICHA_ID == nguoidungId.DONVI_ID)
                        .OrderBy(x => x.ID);
            //get list role
            if (nguoidungId.DONVI.DONVICHA_ID != null)
            {
                if (System.Web.HttpContext.Current.Session["Role_QLPQ"] == null)
                {
                    if (nguoidungId.ISPQ == true)
                    {
                        //   var typeRole = TypeRoleService.Query.Where(x => x.TYPE == (int)Common.TypeRole.CapTinh).Select(x => x.ROLE_ID).ToList();
                        // System.Web.HttpContext.Current.Session["Role_QLPQ"] = RoleService.Query.Where(x => typeRole.Contains(x.roleid)).OrderBy(x => x.roleid);

                        System.Web.HttpContext.Current.Session["Role_QLPQ"] = GetListRoles((int)EIS.Core.Common.Common.LoaiUser.Tinh);
                    }
                    else
                    {
                        //System.Web.HttpContext.Current.Session["Role_QLPQ"] = new List<role>().AsQueryable();
                        System.Web.HttpContext.Current.Session["Role_QLPQ"] = GetListRoles((int)EIS.Core.Common.Common.LoaiUser.Tinh);
                    }
                }

                if (System.Web.HttpContext.Current.Session["CSKCB_QLPQ"] == null)
                {
                    //Thai Pv : chỉnh sửa ngày 02/03/2017 - sai do chưa lọc đơn vị đa tuyến, tất cả các đơn vị của 97 sẽ được chuyển đơn vị về Tỉnh
                    //if (nguoidungId.DONVI.TINHTHANH_ID != null)
                    //{
                    //    var bientam = nguoidungId.DONVI.TINHTHANH_ID;
                    //    System.Web.HttpContext.Current.Session["CSKCB_QLPQ"] =
                    //      DmcosokcbService.Query.Where(x => x.TINHTHANH_ID == bientam && x.HIEULUC == true);
                    //}
                    //else
                    {
                        var bientam = nguoidungId.DONVI.ID;
                        var listDv = DmDonviService.Query.Where(x => x.ID == bientam || x.DONVICHA_ID == bientam).Select(x=>x.ID).ToList();
                        System.Web.HttpContext.Current.Session["CSKCB_QLPQ"] =
                          DmcosokcbService.Query.Where(x=> x.HIEULUC==true
                            && (listDv.Contains(x.DONVI_ID??0))
                          );
                    }
                }
                ViewBag.QLNDCHECK = true;
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["Role_QLPQ"] == null)
                {
                    if (nguoidungId.ISPQ == true)
                    {
                        //var typeRole = TypeRoleService.Query.Where(x => x.TYPE == (int)Common.TypeRole.CapTrungUong).Select(x => x.ROLE_ID).ToList();
                        // System.Web.HttpContext.Current.Session["Role_QLPQ"] = RoleService.Query.Where(x => typeRole.Contains(x.roleid)).OrderBy(x => x.roleid); ;
                        System.Web.HttpContext.Current.Session["Role_QLPQ"] = GetListRoles((int)EIS.Core.Common.Common.LoaiUser.TW);
                    }
                    else
                    {
                        // System.Web.HttpContext.Current.Session["Role_QLPQ"] = new List<role>().AsQueryable();
                        System.Web.HttpContext.Current.Session["Role_QLPQ"] = GetListRoles((int)EIS.Core.Common.Common.LoaiUser.TW);
                    }
                }
                System.Web.HttpContext.Current.Session["CSKCB_QLPQ"] = new List<DM_DONVI>().AsQueryable();
            }
            var a = UserDataService.Query.FirstOrDefault(x => x.username.ToUpper() == nguoidungId.TENDANGNHAP.ToUpper());
            if (a != null) System.Web.HttpContext.Current.Session["ABC"] = a.userid;

        }

        #endregion
    }
}
