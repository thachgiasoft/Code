using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.WebUtils;
using DevExpress.XtraSpreadsheet;
using EIS.Core.Domain;
using EIS.FEW.Helper;
using FX.Core;
using FX.Data;
using EIS.Core.IService;
using EIS.Core.ServiceImp;
using IdentityManagement.Service;
using IdentityManagement.Domain;
using EIS.Core.CustomView;
using EIS.FEW.Models;
using iTextSharp.text.pdf;
using IdentityManagement.Authorization;

namespace EIS.FEW.Controllers
{
    [RBACAuthorize(Roles = "Root")]
    public class RolePermissionController : Controller
    {
        private readonly IroleService _iroleService;
        private readonly IobjectService _iobjService;
        private readonly IpermissionService _ipmsService;
        private readonly ITYPE_ROLEService _iTypeRoleService;
        private readonly ILogSystemService _iLogSystemService;

        public RoleObjectPms RoleObjPms = new RoleObjectPms();
        public RoleObjectPms RoleObjPmsReal = new RoleObjectPms();
        List<permission> Listpermission = new List<permission>();
        permission _permission = new permission();
        TYPE_ROLE typeRole = new TYPE_ROLE();

        public RolePermissionController()
        {
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
            _iroleService = IoC.Resolve<IroleService>();
            _iobjService = IoC.Resolve<IobjectService>();
            _ipmsService = IoC.Resolve<IpermissionService>();
            _iTypeRoleService = IoC.Resolve<ITYPE_ROLEService>();
        }

        public ActionResult Index()
        {
            //ThaiPV : chỉnh sửa ngày 10/12/2016, loại bỏ role tra cứu giám định, role này dành riêng cho hỗ trợ, ko hiển thị cho người dùng.
            return View(_iroleService.GetAll().Where(x => x.name != "Tiện ích tra cứu hệ thống giám định").OrderBy(r => r.roleid));
        }

        public ActionResult RolePartial()
        {
            return PartialView("RolePartial", _iroleService.GetAll().OrderBy(r => r.roleid));
        }

        public ActionResult EditRole(string roleId)
        {
            Session["ArrayPms"] = null;
            Session["ChangeCheckBoxPms"] = null;

            int roleid = int.Parse(roleId);
            Session["roleIdTemp"] = roleId;

            ViewBag.rolename = _iroleService.Query.Where(m => m.roleid == roleid).Select(n => n.name).FirstOrDefault();
            ViewBag.TypeRoleId = _iTypeRoleService.Query.Where(m => m.ROLE_ID == roleid).Select(n => n.TYPE).FirstOrDefault();

            ViewBag.hdRoleId = roleId;

            if (roleid == 76 || roleid == 77 || roleid == 79 || roleid == 183 || roleid == 184 || roleid == 203 || roleid == 204 || roleid == 205 || roleid == 206 || roleid == 207 || roleid == 208)
                ViewBag.RoleEnable = false;
            else
                ViewBag.RoleEnable = true;

            RoleObjPmsReal = GetListObPmsByRoleId(roleid);
            return View(RoleObjPmsReal);
        }

        public ActionResult PermissionForAddRolePartial()
        {
            if (Session["ArrayPms"] == null)
            {
                var results = GetListObPmsByObjId();
                return PartialView("PermissionForAddRolePartial", results);
            }
            else
            {
                var results = GetListObPmsByTickAdd();
                return PartialView("PermissionForAddRolePartial", results.lstObjPmsView);
            }
        }

        public ActionResult PermissionForEditRolePartial()
        {
            int roleid = int.Parse(Session["roleIdTemp"].ToString());
            RoleObjPmsReal = GetListObPmsByRoleId(roleid);
            return PartialView("PermissionForEditRolePartial", RoleObjPmsReal.lstObjPmsView);
        }

        public ActionResult AddNewRole()
        {
            Session["ArrayPms"] = null;
            var results = GetListObPmsByObjId();
            return View(results);
        }
        public ActionResult DoAddNewRole(string name, string RoleTypeId)
        {
            string results = "OK";
            if (ModelState.IsValid && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(RoleTypeId))
            {
                var checkExist = _iroleService.Query.FirstOrDefault(x => x.name.ToUpper() == name.ToUpper());
                if (checkExist == null)
                {
                    try
                    {
                        role p = new role();
                        p.AppID = 1;
                        p.name = name.Trim();
                        List<string> lstPmsChange = new List<string>();

                        if (Session["ArrayPms"] != null)
                            lstPmsChange = (List<string>)Session["ArrayPms"];

                        foreach (string word in lstPmsChange)
                        {
                            _permission = _ipmsService.GetByName(word, 1);
                            Listpermission.Add(_permission);
                        }
                        p.Permissions = Listpermission;

                        _iroleService.BeginTran();
                        _iroleService.CreateNew(p);

                        typeRole = new TYPE_ROLE { ROLE_ID = p.roleid, TYPE = int.Parse(RoleTypeId) };

                        _iTypeRoleService.CreateNew(typeRole);
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Thêm mới Phân quyền ", "Thực hiện chức năng thêm mới Phân quyền", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                        _iroleService.CommitTran();
                    }
                    catch (Exception e)
                    {
                        _iroleService.RolbackTran();
                        results = e.Message;
                    }
                }
                else
                {
                    results = "ExistName";
                }
            }
            else
            {
                results = "NotOK";
            }
            Session["ArrayPms"] = null;
            Session["ChangeCheckBoxPms"] = null;
            return Content(results, "text/html");
        }

        public ActionResult UpdateRole(string roleId, string name, string RoleTypeId)
        {
            string results = "OK";
            if (ModelState.IsValid && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(RoleTypeId) && !string.IsNullOrEmpty(roleId))
            {

                var checkExist = _iroleService.Query.FirstOrDefault(x => x.name.ToUpper() == name.ToUpper());
                if (checkExist == null || checkExist.roleid == int.Parse(roleId))
                {
                    try
                    {
                        int roleid = int.Parse(roleId);
                        role p = new role();
                        p = _iroleService.Query.FirstOrDefault(m => m.roleid == roleid);

                        p.AppID = 1;
                        p.name = name.Trim();

                        List<string> lstPmsChange = new List<string>();
                        if (Session["ArrayPms"] != null)
                            lstPmsChange = (List<string>)Session["ArrayPms"];

                        foreach (string word in lstPmsChange)
                        {
                            _permission = _ipmsService.GetByName(word, 1);
                            Listpermission.Add(_permission);
                        }
                        p.Permissions = Listpermission;

                        var type = _iTypeRoleService.Query.FirstOrDefault(x => x.ROLE_ID == roleid);

                        _iroleService.BeginTran();

                        _iroleService.Update(p);

                        if (type != null) type.TYPE = Int32.Parse(RoleTypeId);
                        _iTypeRoleService.Update(type);

                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Cập nhật Phân quyền ", "Thực hiện chức năng cập nhật Phân quyền", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                        _iroleService.CommitTran();
                    }
                    catch (Exception e)
                    {
                        _iroleService.RolbackTran();
                        results = e.Message;
                    }
                }
                else
                {
                    results = "ExistName";
                }
            }
            else
            {
                results = "NotOK";
            }
            Session["ArrayPms"] = null;
            Session["ChangeCheckBoxPms"] = null;
            return Content(results, "text/html");
        }

        public EmptyResult ChangeCheckBoxPms(string pms, bool checkReal)
        {
            List<string> lstPmsReal = new List<string>();
            if (Session["ArrayPms"] != null)
                lstPmsReal = (List<string>)Session["ArrayPms"];

            if (checkReal)
                lstPmsReal.Add(pms);
            else
                lstPmsReal.Remove(pms);
            Session["ArrayPms"] = lstPmsReal;

            if (Session["ChangeCheckBoxPms"] == null)
                Session["ChangeCheckBoxPms"] = true;

            return new EmptyResult();
        }

        public ActionResult DeleteRole(string roleId)
        {
            try
            {
                int roleid = int.Parse(roleId);
                string Results = "OK";
                string CheckDelete = CheckForDeleteRole(roleid);
                if (CheckDelete == "Success")
                {
                    var model = _iroleService.Getbykey(roleid);
                    var modelTypeRole = _iTypeRoleService.Query.FirstOrDefault(m => m.ROLE_ID == roleid);
                    _iroleService.BeginTran();
                    _iTypeRoleService.Delete(modelTypeRole);
                    _iroleService.Delete(model);
                    _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa Phân quyền ", "Thực hiện chức năng xóa Phân quyền", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                    _iroleService.CommitTran();
                }
                else if (CheckDelete == "ExistPms")
                    Results = "NotOKPms";
                else
                    Results = "NotOKUser";

                return Content(Results, "text/html");
            }
            catch (Exception)
            {
                _iroleService.RolbackTran();
                return null;
            }
        }

        public string CheckForDeleteRole(int roleid)
        {
            string Results = "Success";
            var pmsbyroleid = _iroleService.Query.Where(m => m.roleid == roleid).FirstOrDefault().Permissions;
            if (pmsbyroleid.Any())
                Results = "ExistPms";
            else
            {
                var userbyroleid = _iroleService.Query.Where(m => m.roleid == roleid).FirstOrDefault().Users;
                if (userbyroleid.Any())
                    Results = "ExistUser";
            }
            return Results;
        }

        public ObPmsView GetObjPmsByObjId(int objid)
        {
            List<string> _lstName = new List<string>();
            List<permission> dataPms = _ipmsService.Query.Where(m => m.ObjectRBAC.objectid == objid).OrderBy(p => p.permissionid).Take(8).ToList();
            int n = dataPms.Count();
            ObPmsView _objpms = new ObPmsView();
            if (n > 0)
            {
                if (n == 1)
                {
                    _objpms.pms1 = dataPms[0].name.Trim();
                }
                else if (n == 2)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                    }
                    _lstName.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];

                }
                else if (n == 3)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                    }
                    _lstName.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                }
                else if (n == 4)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                    }
                    _lstName.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                }
                else if (n == 5)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                    }
                    _lstName.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                }
                else if (n == 6)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                    }
                    _lstName.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                }
                else if (n == 7)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                    }
                    _lstName.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                }
                else if (n == 8)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                    }
                    _lstName.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                    _objpms.pms8 = _lstName[7];
                }
            }
            _objpms.objectid = objid;
            _objpms.name = _iobjService.Query.Where(o => o.objectid == objid).Select(m => m.name).FirstOrDefault();
            return _objpms;
        }


        public List<ObPmsView> GetListObPmsByObjId()
        {
            var _objList = _iobjService.GetAll().OrderBy(o => o.objectid);
            List<ObPmsView> ObPmsViewList = new List<ObPmsView>();

            foreach (var item in _objList)
            {
                ObPmsView _objpms = new ObPmsView();
                _objpms = GetObjPmsByObjId(item.objectid);
                ObPmsViewList.Add(_objpms);
            }
            return ObPmsViewList;
        }


        //Lấy ListpmsName by RoleId
        public List<string> GetPmsStringByRoleId(int RoleId)
        {
            List<string> _lstResults = new List<string>();
            var _lstpms = _iroleService.Query.Where(m => m.roleid == RoleId).FirstOrDefault().Permissions.ToList();
            if (_lstpms != null)
            {
                foreach (var item in _lstpms)
                {
                    _lstResults.Add(item.name.Trim());
                }
            }
            return _lstResults;
        }

        public List<objectRbac> GetObjectByRoleId(int RoleId)
        {
            var _lstPermision = _iroleService.Query.Where(r => r.roleid == RoleId).FirstOrDefault().Permissions.ToList();
            var _lstObjectIE = _lstPermision.Select(o => o.ObjectRBAC);
            var tempObject = _lstObjectIE.GroupBy(x => x.objectid).Select(x => x.FirstOrDefault()).ToList();
            return tempObject;
        }

        //Lấy permission thực theo mỗi role
        public RoleObjectPms GetListObPmsByRoleId(int roleid)
        {
            var _lstRole = new List<role>();
            var _alllstObPmsView = GetListObPmsByObjId();
            RoleObjPms.lstRole = _lstRole;
            RoleObjPms.lstObjPmsView = _alllstObPmsView;


            var lstObPmsViewAll = _alllstObPmsView;

            //Lưu trữ lại pms cho việc cập nhật Checkbox
            List<string> lstpmsViewData = new List<string>();

            if (Session["ChangeCheckBoxPms"] != null)
            {
                lstpmsViewData = (List<string>)Session["ArrayPms"];
            }
            else
            {
                lstpmsViewData = GetPmsStringByRoleId(roleid);
                Session["ArrayPms"] = lstpmsViewData;
            }

            //Check  permission của Role có thuộc Tất cả Permission
            int n = lstObPmsViewAll.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms1))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms1))
                        {
                            RoleObjPms.lstObjPmsView[i].pms1 = " " + RoleObjPms.lstObjPmsView[i].pms1.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms2))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms2))
                        {
                            RoleObjPms.lstObjPmsView[i].pms2 = " " + RoleObjPms.lstObjPmsView[i].pms2.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms3))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms3))
                        {
                            RoleObjPms.lstObjPmsView[i].pms3 = " " + RoleObjPms.lstObjPmsView[i].pms3.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms4))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms4))
                        {
                            RoleObjPms.lstObjPmsView[i].pms4 = " " + RoleObjPms.lstObjPmsView[i].pms4.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms5))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms5))
                        {
                            RoleObjPms.lstObjPmsView[i].pms5 = " " + RoleObjPms.lstObjPmsView[i].pms5.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms6))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms6))
                        {
                            RoleObjPms.lstObjPmsView[i].pms6 = " " + RoleObjPms.lstObjPmsView[i].pms6.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms7))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms7))
                        {
                            RoleObjPms.lstObjPmsView[i].pms7 = " " + RoleObjPms.lstObjPmsView[i].pms7.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms8))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms8))
                        {
                            RoleObjPms.lstObjPmsView[i].pms8 = " " + RoleObjPms.lstObjPmsView[i].pms8.Trim();
                        }
                    }
                }
            }
            return RoleObjPms;
        }


        //Lấy permission thực theo Add tick
        public RoleObjectPms GetListObPmsByTickAdd()
        {
            var _lstRole = new List<role>();
            var _alllstObPmsView = GetListObPmsByObjId();
            RoleObjPms.lstRole = _lstRole;
            RoleObjPms.lstObjPmsView = _alllstObPmsView;

            var lstObPmsViewAll = _alllstObPmsView;

            //Lưu trữ lại pms cho việc cập nhật Checkbox
            List<string> lstpmsViewData = new List<string>();

            lstpmsViewData = (List<string>)Session["ArrayPms"];

            //Check  permission của Role có thuộc Tất cả Permission
            int n = lstObPmsViewAll.Count;
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms1))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms1))
                        {
                            RoleObjPms.lstObjPmsView[i].pms1 = " " + RoleObjPms.lstObjPmsView[i].pms1.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms2))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms2))
                        {
                            RoleObjPms.lstObjPmsView[i].pms2 = " " + RoleObjPms.lstObjPmsView[i].pms2.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms3))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms3))
                        {
                            RoleObjPms.lstObjPmsView[i].pms3 = " " + RoleObjPms.lstObjPmsView[i].pms3.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms4))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms4))
                        {
                            RoleObjPms.lstObjPmsView[i].pms4 = " " + RoleObjPms.lstObjPmsView[i].pms4.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms5))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms5))
                        {
                            RoleObjPms.lstObjPmsView[i].pms5 = " " + RoleObjPms.lstObjPmsView[i].pms5.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms6))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms6))
                        {
                            RoleObjPms.lstObjPmsView[i].pms6 = " " + RoleObjPms.lstObjPmsView[i].pms6.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms7))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms7))
                        {
                            RoleObjPms.lstObjPmsView[i].pms7 = " " + RoleObjPms.lstObjPmsView[i].pms7.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms8))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms8))
                        {
                            RoleObjPms.lstObjPmsView[i].pms8 = " " + RoleObjPms.lstObjPmsView[i].pms8.Trim();
                        }
                    }
                }
            }
            return RoleObjPms;
        }

    }
}
