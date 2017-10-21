using EIS.Core;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.Core.IService;
using EIS.FEW.Models;
using FX.Context;
using FX.Core;
using IdentityManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IdentityManagement.Domain;

namespace EIS.FEW.Controllers
{
    [Authorize]
    public class ThongTinTaiKhoanController : Controller
    {
        private NGUOIDUNG nguoidung;
        private static readonly ILog log = LogManager.GetLogger(typeof(ThongTinTaiKhoanController));
        private static readonly char[] SpecialChars = "!@#$%^&*()".ToCharArray();
        private static readonly char[] UPPERCHAR = "QWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();
        private static readonly char[] NUMBER = "123456789".ToCharArray();
        public IuserService UserDataService;
        public user usercurent = new user();
        public UserModel userModel = new UserModel();

        public ThongTinTaiKhoanController()
        {
            try
            {
                UserDataService = IoC.Resolve<IuserService>();
                nguoidung = ((EIS.Core.EISContext)FX.Context.FXContext.Current).CurrentNguoidung;
                usercurent = UserDataService.GetByName(nguoidung.TENDANGNHAP);
                userModel = GetUserModel();
                ViewBag.Username = nguoidung.TENDANGNHAP;
                ViewBag.Ten = nguoidung.TEN;
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace + e.Message);
                ViewData["EditError"] = e.Message;
            }
        }

        public ActionResult Index()
        {
            if (usercurent.ISADMIN == true)
                ViewBag.IsAdmin = "1";
            else
                ViewBag.IsAdmin = "0";
            return View(userModel);
        }
         //if (models.Password.IndexOfAny(UPPERCHAR) == -1)
         //       {
         //           check = true;
         //           error = "Password phải chứa ký tự hoa!";
         //       }
         //       if (models.Password.IndexOfAny(NUMBER) == -1)
         //       {
         //           check = true;
         //           error = "Password phải chứa số!";
         //       }
         //       if (models.ID != 0)
         //       {
         //           models.VaiTro_ID = currentNd.VAITRO;
         //       }
        public JsonResult UpdateUser(string _password, string _newpasswordRe, string _newpassword)
        {
            string returnedData = "NotOK";

            if (_password.Trim() != "" && _newpassword.Trim().Length >= 8/* && _newpasswordRe.Trim().Length >= 8 && _password.IndexOfAny(SpecialChars) != -1 && _password.IndexOfAny(UPPERCHAR) != -1 && _password.IndexOfAny(NUMBER) != -1*/)
            {
                var user = UserDataService.Getbykey(usercurent.userid);
                var pass_word = FormsAuthentication.HashPasswordForStoringInConfigFile(_password, "MD5");

                if (user.password == pass_word)
                {
                    var newpw = FormsAuthentication.HashPasswordForStoringInConfigFile(_newpassword, "MD5");
                    user.password = newpw;
                    UserDataService.Update(user);
                    UserDataService.CommitChanges();
                    returnedData = "ok";
                    Session["MustChangePW"] = false;
                }
            }

            return Json(returnedData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DS_NguoiDungPartial()
        {
            return PartialView("Form_ThongTinTaiKhoanCapDuoi", GetUserModelViews());
        }

        [HttpPost]
        public ActionResult ChangeCapDuoi(UserModelView _user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    user user_save = new user();
                    user_save = UserDataService.Getbykey(_user.userid);
                    user_save.password = FormsAuthentication.HashPasswordForStoringInConfigFile(_user.NewPassword, "MD5");
                    user_save.LastPasswordChangedDate = DateTime.Now;
                    UserDataService.Update(user_save);
                    UserDataService.CommitChanges();
                }
                catch (Exception e)
                {
                    log.Error(e.StackTrace + e.Message);
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("Form_ThongTinTaiKhoanCapDuoi", GetUserModelViews());
        }
        public JsonResult CheckPW(string pw)
        {
            string a = FormsAuthentication.HashPasswordForStoringInConfigFile(pw, "MD5");
            string b = usercurent.password;
            string returnedData = "";
            if (a == b)
            {
                returnedData = "ok";
            }
            else
            {
                returnedData = "fail";
            }
            return Json(returnedData, JsonRequestBehavior.AllowGet);
        }

        public bool CheckExactPasswordBelowLevel(int UserIdRecord, string putPassword)
        {
            bool ifpasswordExist = false;
            try
            {
                string PasswordRecord = UserDataService.Getbykey(UserIdRecord).password;
                putPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(putPassword, "MD5");
                ifpasswordExist = putPassword.Equals(PasswordRecord) ? true : false;
                return ifpasswordExist;
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace + e.Message);
                return ifpasswordExist;
            }
        }

        public UserModel GetUserModel()
        {
            UserModel _userModel = new UserModel();
            _userModel.currentUserRecord = GetCurrentUser();
            _userModel.lstUserModelView = GetUserModelViews();
            return _userModel;
        }

        public UserRecord GetCurrentUser()
        {
            UserRecord urResults = new UserRecord();
            if (usercurent != null)
            {
                urResults.userid = usercurent.userid;
                urResults.username = usercurent.username;
                urResults.FullName = nguoidung.TEN;
            }
            return urResults;
        }

        public List<UserModelView> GetUserModelViews()
        {
            var _lstUserModelViewResults = new List<UserModelView>();
            var user = nguoidung.TENDANGNHAP.ToUpper();
            try
            {
                var _listuser = new List<user>();

                if (user == "ADMIN")
                    _listuser = UserDataService.GetAll().ToList();
                else
                    _listuser = UserDataService.Query.Where(w => w.GroupName == nguoidung.TENDANGNHAP).ToList();

                if (_listuser != null)
                {
                    foreach (var item in _listuser)
                    {
                        UserModelView umViewRecord = new UserModelView();
                        umViewRecord.userid = item.userid;
                        umViewRecord.username = item.username;
                        umViewRecord.LastPasswordChangedDate = item.LastPasswordChangedDate;
                        _lstUserModelViewResults.Add(umViewRecord);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace + e.Message);
            }
           
            return _lstUserModelViewResults;
        }
    }
}
