using EIS.Core;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Context;
using FX.Core;
using IdentityManagement.Domain;
using IdentityManagement.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EIS.FEW.Controllers
{
    [Authorize]
    public class GroupUserController : BaseController
    {
        private readonly IuserService UserDataService;
        private readonly INGUOIDUNGService _INGUOIDUNGService = IoC.Resolve<INGUOIDUNGService>();
        private NGUOIDUNG nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
        private static readonly ILog log = LogManager.GetLogger(typeof(GroupUserController));

        //
        // GET: /GroupUser/

        public ActionResult Index()
        {
            var groupSrv = IoC.Resolve<INHOM_USERService>();
            var model = new UserGroupModelDetail();
            int a = groupSrv.Query.Where(o => o.USER_ID == nguoidung.ID).Count() > 0 ? groupSrv.Query.Where(o => o.USER_ID == nguoidung.ID).OrderByDescending(o => o.ID).FirstOrDefault().ID : 0;//
            model.Roles = GetListRoles().ToList();
            model.Groups = groupSrv.Query.Where(o=>o.USER_ID == nguoidung.ID).OrderByDescending(o=>o.ID).ToList() ?? new List<NHOM_USER>();
            ViewBag.DSUSER = UserRoleCallbackPanelCustom(a);
            Session["Group_LstRole"] = model.Roles;
            ViewBag.Content = RenderRazorViewToString("checkBoxLstRole", model.Roles);
            model.usersOfGroup = LoadUserOfGroupModel(a);
            return View(model);
        }

        public ActionResult CbbGroupCallBack()
        {
            var groupSrv = IoC.Resolve<INHOM_USERService>();
            var selectGroupUS = groupSrv.Query.Where(o => o.USER_ID == nguoidung.ID).ToList().OrderByDescending(o=>o.ID);
            //ViewBag.DescenNhom = groupSrv.Query.Where(o => o.USER_ID == nguoidung.ID).OrderByDescending(o => o.GROUP_NAME).FirstOrDefault().ID;//.ToList();
            Session["group_user_cbbgroup"] = groupSrv.Query.Where(o => o.USER_ID == nguoidung.ID).OrderByDescending(o => o.ID).FirstOrDefault().ID;
            return PartialView("GroupPartial", selectGroupUS);
        }

        public ActionResult LoadUserNonOfGroup(int? groupId)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();

            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            //thanhpt check những thằng tồn tại ở nhóm khác thì không được hiển thị lên lưới sửa
            var idUsergroupFalse = userGroupSrv.Query.Where(o => o.GROUP_ID != groupId).Select(o => o.USER_ID).ToList();
            var nguoidungs = _INGUOIDUNGService.Query.Where(c => c.ISPQ == true).Select(c => c.TENDANGNHAP).ToList();

            var lstusers = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true && !idUsergroupFalse.Contains(x.userid) && !nguoidungs.Contains(x.username)).ToList();
            //end thanhpt
            //var lstusers = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true).ToList();
            var model = new UserNonGroupViewModel();
           
            if (groupId.HasValue)
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUserIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId.Value).Select(n => n.USER_ID).ToList();
                model.userIdOfGroup = lstUserIdOfGroup;
                lstUserIdOfGroups.RemoveAll(x => lstUserIdOfGroup.Contains(x));
            }
            else
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUsersOfGroups = lstusers.Where(n => lstUserIdOfGroups.Contains(n.userid)).ToList();

                lstusers.RemoveAll(x => lstUsersOfGroups.Contains(x));
            }
            model.users = lstusers;
            return View("UserNonOfGroupPartial", model);
        }

        public ActionResult LoadUserNonOfGroupCustom(int? groupId)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();

            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            //thanhpt check những thằng tồn tại ở nhóm khác thì không được hiển thị lên lưới sửa
            var idUsergroupFalse = userGroupSrv.Query.Where(o => o.GROUP_ID != groupId).Select(o => o.USER_ID).ToList();

            var nguoidungs = _INGUOIDUNGService.Query.Where(c => c.ISPQ ==true).Select(c => c.TENDANGNHAP).ToList();

            var lstusers = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true && !idUsergroupFalse.Contains(x.userid) &&  !nguoidungs.Contains(x.username)).ToList();
            
            //userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true && !idUsergroupFalse.Contains(x.userid) && x.userid != nguoidung.ID).ToList();
            //end thanhpt
            //var lstusers = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true).ToList();
            var model = new UserNonGroupViewModel();
            if (groupId.HasValue)
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUserIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId.Value).Select(n => n.USER_ID).ToList();
                model.userIdOfGroup = lstUserIdOfGroup;
                lstUserIdOfGroups.RemoveAll(x => lstUserIdOfGroup.Contains(x));
            }
            else
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUsersOfGroups = lstusers.Where(n => lstUserIdOfGroups.Contains(n.userid)).ToList();

                lstusers.RemoveAll(x => lstUsersOfGroups.Contains(x));
            }
            model.users = lstusers;
            return View("UserNonOfGroupPartial", model);
        }

        public ActionResult LoadUserNonOfGroupEdit(int? groupId)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();

            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            //thanhpt check những thằng tồn tại ở nhóm khác thì không được hiển thị lên lưới sửa
            var idUsergroupFalse = userGroupSrv.Query.Where(o => o.GROUP_ID != groupId).Select(o => o.USER_ID).ToList();
            var nguoidungs = _INGUOIDUNGService.Query.Where(c => c.ISPQ == true).Select(c => c.TENDANGNHAP).ToList();

            var lstusers = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true && !idUsergroupFalse.Contains(x.userid) && !nguoidungs.Contains(x.username)).ToList();
            //end thanhpt
            var model = new UserNonGroupViewModel();
            if (groupId.HasValue)
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUserIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId.Value).Select(n => n.USER_ID).ToList();
                model.userIdOfGroup = lstUserIdOfGroup;
                lstUserIdOfGroups.RemoveAll(x => lstUserIdOfGroup.Contains(x));
            }
            else
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUsersOfGroups = lstusers.Where(n => lstUserIdOfGroups.Contains(n.userid)).ToList();

                lstusers.RemoveAll(x => lstUsersOfGroups.Contains(x));
            }
            model.users = lstusers;
            return View("UserNonOfGroupEditPartial", model);
        }

        public ActionResult LoadUserNonOfGroupCustomEdit(int? groupId)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();

            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            //thanhpt check những thằng tồn tại ở nhóm khác thì không được hiển thị lên lưới sửa
            var idUsergroupFalse = userGroupSrv.Query.Where(o => o.GROUP_ID  != groupId).Select(o=>o.USER_ID).ToList();
            var nguoidungs = _INGUOIDUNGService.Query.Where(c => c.ISPQ == true).Select(c => c.TENDANGNHAP).ToList();

            var lstusers = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true && !idUsergroupFalse.Contains(x.userid) && !nguoidungs.Contains(x.username)).ToList();
            //end thanhpt
            var model = new UserNonGroupViewModel();
            if (groupId.HasValue)
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUserIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId.Value).Select(n => n.USER_ID).ToList();
                model.userIdOfGroup = lstUserIdOfGroup;
                lstUserIdOfGroups.RemoveAll(x => lstUserIdOfGroup.Contains(x));
            }
            else
            {
                var lstUserIdOfGroups = userGroupSrv.GetAll().Select(n => n.USER_ID).ToList();
                var lstUsersOfGroups = lstusers.Where(n => lstUserIdOfGroups.Contains(n.userid)).ToList();

                lstusers.RemoveAll(x => lstUsersOfGroups.Contains(x));
            }
            model.users = lstusers;
            return View("UserNonOfGroupEditPartial", model);
        }
        public List<user> LoadUserOfGroupModel(int groupId)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var groupSrv = IoC.Resolve<INHOM_USERService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            //if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            var model = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true).ToList();

            var userIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId).Select(n => n.USER_ID).ToList();
            var a = model.Where(n => userIdOfGroup.Contains(n.userid)).ToList();

            return a;
        }
        public ActionResult LoadUserOfGroup(int groupId)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var groupSrv = IoC.Resolve<INHOM_USERService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            var model = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true).ToList();

            var userIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId).Select(n => n.USER_ID).ToList();
            model = model.Where(n => userIdOfGroup.Contains(n.userid)).ToList();

            return PartialView("UserOfGroupPartial", model);
        }
        // load lần đầu
        public List<int> UserRoleCallbackPanelCustom(int groupId)
        {
            var UserDataService = IoC.Resolve<IuserService>();
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var groupSrv = IoC.Resolve<INHOM_USERService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;

            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return new List<int>();
            var firstRole = firstUser.Roles.FirstOrDefault(x => x.name.ToUpper() == "ROOT");
            var model = userSrv.Query.Where(x => x.GroupName.ToUpper() == firstUser.GroupName.ToUpper() && x.ISADMIN != true).ToList();

            var userIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId).Select(n => n.USER_ID).FirstOrDefault();
            if(userIdOfGroup != 0)
            {
                var currentuserRoles = UserDataService.Query.FirstOrDefault(x => x.userid == userIdOfGroup) != null ? UserDataService.Query.FirstOrDefault(x => x.userid == userIdOfGroup).Roles : null;
                return currentuserRoles.Select(o => o.roleid).ToList();
            }
            else
            {
                return new List<int>();
            }
            
            
            
        }
        //thànhpt đổ dữ liệu quyền truy cập vào lưới
        public ActionResult UserRoleCallbackPanel(int groupId)
        {
            var UserDataService = IoC.Resolve<IuserService>();
            var user = System.Web.HttpContext.Current.User.Identity.Name.ToUpper();
            var userSrv = IoC.Resolve<IuserService>();
            var groupSrv = IoC.Resolve<INHOM_USERService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
            var nguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            var firstUser = userSrv.Query.FirstOrDefault(x => x.username.ToUpper() == user.ToUpper());
            if (firstUser == null || (firstUser.ISADMIN == false && nguoidung.ISPQ != true)) return RedirectToAction("Index", "NonAuthorize");          
            ViewBag.DSUSER = UserRoleCallbackPanelCustom(groupId);
            //foreach(var item in userIdOfGroup)
            //{
            //    var currentuserRoles = UserDataService.Query.FirstOrDefault(x => x.userid == item).Roles;
            //}
            ViewBag.Content = RenderRazorViewToString("checkBoxLstRole", Session["Group_LstRole"]);
            return PartialView("RolesPartialView");
        }
        private List<role> GetListRoles()
        {
            //var loaiuser = (int?)System.Web.HttpContext.Current.Session["Role_QLPQ"];
            var userSrv = IoC.Resolve<IuserService>();
            var roleSrv = IoC.Resolve<IroleService>();
            var typeRoleSrv = IoC.Resolve<ITYPE_ROLEService>();
            var nguoidungId = ((EISContext)FXContext.Current).CurrentNguoidung;
            string username = nguoidung.TENDANGNHAP;
            var _userRecord = userSrv.Query.FirstOrDefault(m => m.username == username);
            List<role> _results = new List<role>();
            int userid = _userRecord.userid;
            var _lstTyperoles = typeRoleSrv.Query.Where(m => (m.USERID == 0) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
            if (_userRecord != null)
            {
                var _lstrole = _userRecord.Roles;
                if (_lstrole.Any())
                {
                    var _userOfRoleRecord = _lstrole.FirstOrDefault(m => m.name == "Root");
                    if (_userOfRoleRecord != null)
                    {
                        var _lst = roleSrv.Query.OrderBy(m => m.roleid);
                        _results = _lst.OrderBy(m => m.roleid).ToList();
                        return _results;
                    }
                }
                if (nguoidungId.DONVI.DONVICHA_ID != null)
                {
                    _lstTyperoles = typeRoleSrv.Query.Where(m => (m.USERID == 0 && (m.TYPE == (int)EIS.Core.Common.Common.TypeRole.CapTinh || m.TYPE == (int)EIS.Core.Common.Common.TypeRole.CapKhac)) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
                }
                else
                {
                    if (System.Web.HttpContext.Current.Session["Role_QLPQ"] == null)
                    {
                        if (nguoidungId.ISPQ == true)
                        {
                            _lstTyperoles = typeRoleSrv.Query.Where(m => (m.USERID == 0 && m.TYPE == (int)EIS.Core.Common.Common.LoaiUser.TW) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session["CSKCB_QLPQ"] = new List<DM_DONVI>().AsQueryable();
                        }
                    }
                }
                //if (loaiuser == (int)EIS.Core.Common.Common.LoaiUser.Tinh)
                //{
                //    _lstTyperoles = typeRoleSrv.Query.Where(m => (m.USERID == 0 && (m.TYPE == (int)EIS.Core.Common.Common.TypeRole.CapTinh || m.TYPE == (int)EIS.Core.Common.Common.TypeRole.CapKhac)) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
                //}
                //else
                //{
                //    _lstTyperoles = typeRoleSrv.Query.Where(m => (m.USERID == 0 && m.TYPE == loaiuser) || m.USERID == userid).Select(n => n.ROLE_ID).ToList();
                //}
                var _lstNotRoot = roleSrv.Query.Where(m => _lstTyperoles.Contains(m.roleid));
                _results = _lstNotRoot.OrderBy(m => m.roleid).ToList();
                
            }
            return _results;
        }

         
        [HttpPost]
        public JsonResult CreateNewGroup(string groupName, string ids)
        {
            string mess = string.Empty;
            try
            {
                var groupSrv = IoC.Resolve<INHOM_USERService>();
                
                var lstUserIds = new JavaScriptSerializer().Deserialize<List<int>>(ids);
                if (groupSrv.AddNewGroup(groupName, lstUserIds, out mess))
                {
                    var idNhom = groupSrv.Query.Where(o => o.GROUP_NAME == groupName).FirstOrDefault().ID;//.Select(o => o.ID);
                    return Json(new
                    {
                        status = true,
                        mess = mess,
                        ID = idNhom
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        mess = mess
                    });
                }
            }
            catch (Exception e)
            {
                TryLog(log, e);
                return Json(new
                {
                    status = false,
                    mess = "Lỗi hệ thống, hãy thử lại sau"
                });
            }
        }

        [HttpPost]
        public JsonResult UpdateGroupUser(string groupName, int groupId, string ids)
        {
            string mess = string.Empty;
            try
            {
                var groupSrv = IoC.Resolve<INHOM_USERService>();
                var lstUserIds = new JavaScriptSerializer().Deserialize<List<int>>(ids);
                if (groupSrv.UpdateGroup(groupId, groupName, lstUserIds, out mess))
                {
                    var idNhom = groupSrv.Query.Where(o => o.GROUP_NAME == groupName).Select(o => o.ID);
                    return Json(new
                    {
                        status = true,
                        mess = mess,
                        ID = idNhom
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        mess = mess
                    });
                }
            }
            catch (Exception e)
            {
                TryLog(log, e);
                return Json(new
                {
                    status = false,
                    mess = "Lỗi hệ thống, hãy thử lại sau"
                });
            }
        }

        [HttpPost]
        public JsonResult AddRolesToGroup(int groupId, string roles)
        {
            var userSrv = IoC.Resolve<IuserService>();
            var groupSrv = IoC.Resolve<INHOM_USERService>();
            var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
            var roleSrv = IoC.Resolve<IroleService>();
            try
            {
                var lstRolesId = new JavaScriptSerializer().Deserialize<List<int>>(roles);

                var userIdOfGroup = userGroupSrv.GetUserIdByGroupId(groupId).Select(n => n.USER_ID).ToList();
                userSrv.BeginTran();
                // thanhpt sửa save quyền truy cập 
                userSrv.Clear();
                foreach (var userid in userIdOfGroup)
                {
                    var user = userSrv.Getbykey(userid);
                    user.Roles = roleSrv.Query.Where(n => lstRolesId.Contains(n.roleid)).ToList();
                    userSrv.Save(user);
                }
                userSrv.CommitTran();
                return Json(new
                {
                    status = true,
                    mess = "Cập nhật thành công."
                });
            }
            catch (Exception e)
            {
                userSrv.RolbackTran();
                TryLog(log, e);
                return Json(new
                {
                    status = false,
                    mess = "Cập nhật thất bại, vui lòng thử lại sau."
                });
            }
        }

        [HttpPost]
        public JsonResult DeleteGroup(int groupId)
        {
            try
            {
                var nguoidungs = ((EISContext)FXContext.Current).CurrentNguoidung;
                var groupSrv = IoC.Resolve<INHOM_USERService>();
                string mess = string.Empty;
                if (groupSrv.DeleteGroup(groupId, out mess))
                {
                    var idNhom = groupSrv.Query.Where(o=>o.USER_ID == nguoidungs.ID).OrderByDescending(o=>o.ID).FirstOrDefault().ID;
                    return Json(new
                    {
                        status = true,
                        mess = mess,
                        ID = idNhom
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        mess = mess
                    });
                }
            }
            catch (Exception e)
            {
                TryLog(log, e);
                return Json(new
                {
                    status = false,
                    mess = "Lỗi hệ thống, vui lòng thử lại sau."
                });
            }
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
