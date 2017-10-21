using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EIS.Core;
using EIS.Core.Domain;
using FX.Core;
using EIS.Core.IService;
using IdentityManagement.Service;
using IdentityManagement.Domain;
using EIS.FEW.Models;
using FX.Context;

namespace EIS.FEW.Controllers
{
    [Authorize]
    public class RolePmsController : Controller
    {
        private readonly IuserService _IuserService;
        private readonly IroleService _iroleService;
        private readonly IobjectService _iobjService;
        private readonly IpermissionService _ipmsService;
        private readonly ITYPE_ROLEService _iTypeRoleService;
        private readonly ILogSystemService _iLogSystemService;
        private readonly IMODULEService _IMODULEService;
        private readonly INHOMCHUCNANGService _INHOMCHUCNANGService;
        private readonly ITYPE_PERMISSIONService _ITYPE_PERMISSIONService;
        private readonly INHOMCHUCNANG_OBJECTService _INHOMCHUCNANG_OBJECTService;
        private readonly NGUOIDUNG CurrentNguoidung;
        RoleObjectPms RoleObjPms = new RoleObjectPms();
        RoleObjectPms RoleObjPmsReal = new RoleObjectPms();
        List<permission> Listpermission = new List<permission>();
        permission _permission = new permission();
        TYPE_ROLE typeRole = new TYPE_ROLE();
        int sttUser;
        int MODULEID;
        List<MODULE> lstMODULE;
        public RolePmsController()
        {
            _IuserService = IoC.Resolve<IuserService>();
            CurrentNguoidung = ((EISContext)FXContext.Current).CurrentNguoidung;
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
            _iroleService = IoC.Resolve<IroleService>();
            _iobjService = IoC.Resolve<IobjectService>();
            _ipmsService = IoC.Resolve<IpermissionService>();

            _iTypeRoleService = IoC.Resolve<ITYPE_ROLEService>();
            _ITYPE_PERMISSIONService = IoC.Resolve<ITYPE_PERMISSIONService>();
            _INHOMCHUCNANG_OBJECTService = IoC.Resolve<INHOMCHUCNANG_OBJECTService>();
            _INHOMCHUCNANGService = IoC.Resolve<INHOMCHUCNANGService>();
            _IMODULEService = IoC.Resolve<IMODULEService>();
            sttUser = CheckStatusTinhOrTrungUongOrRoot();
        }
        public ActionResult Index()
        {
            Session["CheckStatusAdminRoot"] = CheckStatusAdminRoot();
            Session["_lstGetTypeRole"] = GetTypeRole();
            var results = new List<role>();
            if (CheckRedirect())
                results = GetListRoles();
            else
                return RedirectToAction("Index", "NonAuthorize");
            return View(results);
        }
        public ActionResult RolePartial()
        {
            return PartialView("RolePartial", GetListRoles());
        }
        //Add
        public ActionResult AddNewRole()
        {
            ViewData["SttUser"] = sttUser;

            MODULEID = 0;
            lstMODULE = GetMODULE();
            if (lstMODULE.Any())
                MODULEID = lstMODULE.FirstOrDefault().MODULEID;
            Session["stLstNhomchucnangID"] = null;
            Session["MODULEID"] = MODULEID;
            ViewBag.vbSttUser = sttUser;
            ViewData["lstMODULE"] = lstMODULE;
            ViewData["FirstNHOMCHUCNANG"] = GetNHOMCHUCNANGByMODULE(MODULEID);
            Session["ArrayPms"] = null;
            Session["TypeRoleCombobox"] = sttUser;
            var results = GetListObPmsByObjIdByTypeRole(sttUser.ToString());
            return View(results);
        }
        public ActionResult PermissionForAddRolePartial()
        {
            if (Session["TypeRoleCombobox"] != null && Session["stLstNhomchucnangID"] == null)
            {
                string ComboTypeRole = Session["TypeRoleCombobox"].ToString();
                if (Session["ArrayPms"] == null)
                {
                    var results = GetListObPmsByObjIdByTypeRole(ComboTypeRole);
                    return PartialView("PermissionForAddRolePartial", results);
                }
                else
                {
                    var results = GetListObPmsByTickAddByTypeRole(ComboTypeRole);
                    return PartialView("PermissionForAddRolePartial", results.lstObjPmsView);
                }
            }

            if (Session["TypeRoleCombobox"] != null && Session["stLstNhomchucnangID"] != null)
            {
                string ComboTypeRole_2 = Session["TypeRoleCombobox"].ToString();
                string stLstNhomchucnangID_2 = Session["stLstNhomchucnangID"].ToString();

                if (Session["ArrayPms"] == null)
                {
                    return PermissionForAddRolePartialForSearch(stLstNhomchucnangID_2, ComboTypeRole_2);
                }

                var _lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangID_2);
                var results = GetListObPmsByTickAddByTypeRoleAndNhomchucnangId(_lstRealNhomchucnangInt, ComboTypeRole_2);
                return PartialView("PermissionForAddRolePartial", results.lstObjPmsView);
            }

            string stLstNhomchucnangID = (string)Session["stLstNhomchucnangID"];
            return PermissionForAddRolePartialForSearch(stLstNhomchucnangID, sttUser.ToString());
        }
        public ActionResult PermissionForAddRolePartialForSearch(string stLstNhomchucnangID, string ComboTypeRole)
        {
            Session["ArrayPms"] = null;
            if (stLstNhomchucnangID == "")
            {
                if (Session["ArrayPms"] == null)
                {
                    var results = GetListObPmsByObjIdByTypeRole(ComboTypeRole);
                    return PartialView("PermissionForAddRolePartial", results);
                }
                else
                {
                    var results = GetListObPmsByTickAddByTypeRole(ComboTypeRole);
                    return PartialView("PermissionForAddRolePartial", results.lstObjPmsView);
                }
            }

            if (stLstNhomchucnangID.Trim() != "")
            {
                Session["stLstNhomchucnangID"] = stLstNhomchucnangID;

                List<int> _lstRealNhomchucnangInt = new List<int>();
                _lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangID);

                if (Session["ArrayPms"] == null)
                {
                    var results = GetPmsByListNHOMCHUCNANG(_lstRealNhomchucnangInt, ComboTypeRole);
                    return PartialView("PermissionForAddRolePartial", results);
                }
                else
                {
                    string ComboTypeRole_2 = Session["TypeRoleCombobox"].ToString();
                    string stLstNhomchucnangID_2 = Session["stLstNhomchucnangID"].ToString();
                    _lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangID_2);
                    var results = GetListObPmsByTickAddByTypeRoleAndNhomchucnangId(_lstRealNhomchucnangInt, ComboTypeRole_2);
                    return PartialView("PermissionForAddRolePartial", results.lstObjPmsView);
                }
            }
            Session["TypeRoleCombobox"] = null;
            Session["stLstNhomchucnangID"] = null;
            return PermissionForAddRolePartial();
        }
        //End Add
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
                        p.Permissions = Listpermission.GroupBy(x => x.permissionid).Select(x => x.First()).ToList();

                        _iroleService.BeginTran();
                        _iroleService.CreateNew(p);

                        if (CheckStatusAdminRoot())
                            typeRole = new TYPE_ROLE { ROLE_ID = p.roleid, TYPE = int.Parse(RoleTypeId) };
                        else
                        {
                            int userid = _IuserService.GetByName(CurrentNguoidung.TENDANGNHAP).userid;
                            typeRole = new TYPE_ROLE { ROLE_ID = p.roleid, TYPE = int.Parse(RoleTypeId), USERID = userid };
                        }

                        _iTypeRoleService.CreateNew(typeRole);
                        _iroleService.CommitTran();

                        Session["ArrayPms"] = null;
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Thêm mới Phân quyền ", "Thực hiện chức năng thêm mới Phân quyền", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
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

            Session["stLstNhomchucnangID"] = null;
            Session["ChangeCheckBoxPms"] = null;
            Session["TypeRoleCombobox"] = null;
            return Content(results, "text/html");
        }

        #region tác vụ sửa

        public ActionResult EditRole(string roleId)
        {
            Session["RolePms_CheckChuyen"] = null;
            sttUser = CheckStatusTinhOrTrungUongOrRoot();
            ViewData["SttUser"] = sttUser;
            MODULEID = 0;
            lstMODULE = GetMODULE();
            if (lstMODULE.Any())
                MODULEID = lstMODULE.FirstOrDefault().MODULEID;
            Session["MODULEID"] = MODULEID;
            Session["ArrayPms"] = null;
            Session["ChangeCheckBoxPms"] = null;
            ViewData["lstMODULE"] = lstMODULE;
            ViewData["FirstNHOMCHUCNANG"] = GetNHOMCHUCNANGByMODULE(MODULEID);
            Session["stLstNhomchucnangIDEdit"] = null;
            int roleid = int.Parse(roleId);
            Session["roleIdTemp"] = roleId;

            ViewBag.rolename = _iroleService.Getbykey(roleid).name;
            int typeroleid = _iTypeRoleService.Getbykey(roleid).TYPE;
            ViewBag.TypeRoleId = typeroleid;
            Session["TypeRoleCombobox"] = typeroleid.ToString();
            @ViewBag.vbSttUserEdit = typeroleid;
            @ViewBag.hdRoleIdEdit = roleId;
            ViewBag.hdRoleId = roleId;
            Session["statusEnableCheckBox"] = CheckStatusUpdateRole(roleid);

            if (roleid == 76 || roleid == 77 || roleid == 79 || roleid == 183 || roleid == 184 || roleid == 203 || roleid == 204 || roleid == 205 || roleid == 206 || roleid == 207 || roleid == 208)
                ViewBag.RoleEnable = false;
            else
                ViewBag.RoleEnable = true;

            RoleObjPmsReal = GetListObPmsByRoleId(roleid);
            Session["EndCallbackCheckBox"] = RoleObjPmsReal;
            return View(RoleObjPmsReal);
        }

        public ActionResult PermissionForEditRolePartial()
        {
            if (Session["TypeRoleCombobox"] != null && Session["stLstNhomchucnangID"] == null)
            {
                string ComboTypeRole = Session["TypeRoleCombobox"].ToString();
                if (Session["ArrayPms"] == null)
                {
                    var results = GetListObPmsByObjIdByTypeRole(ComboTypeRole);
                    return PartialView("PermissionForAddRolePartial", results);
                }
                else
                {
                    var results = GetListObPmsByTickAddByTypeRole(ComboTypeRole);
                    return PartialView("PermissionForEditRolePartial", results.lstObjPmsView);
                }
            }

            if (Session["TypeRoleCombobox"] != null && Session["stLstNhomchucnangID"] != null)
            {
                string ComboTypeRole_2 = Session["TypeRoleCombobox"].ToString();
                string stLstNhomchucnangID_2 = Session["stLstNhomchucnangID"].ToString();

                if (Session["ArrayPms"] == null)
                {
                    return PermissionForEditRolePartialForSearch(stLstNhomchucnangID_2, ComboTypeRole_2, 2);
                }

                var _lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangID_2);
                var results = GetListObPmsByTickAddByTypeRoleAndNhomchucnangId(_lstRealNhomchucnangInt, ComboTypeRole_2);
                return PartialView("PermissionForEditRolePartial", results.lstObjPmsView);
            }

            string stLstNhomchucnangID = (string)Session["stLstNhomchucnangID"];
            if (stLstNhomchucnangID == null)
            {
                return RedirectToAction("Index", "RolePms");
            }
            //string stLstNhomchucnangID = Session["stLstNhomchucnangID"] == null ? "" : (string)Session["stLstNhomchucnangID"];
            return PermissionForEditRolePartialForSearch(stLstNhomchucnangID, sttUser.ToString(), 2);
            //if (Session["stLstNhomchucnangIDEdit"] == null && Session["TypeRoleCombobox"] != null)
            //{
            //    string TypeRole = Session["TypeRoleCombobox"].ToString();
            //    int roleid = int.Parse(Session["roleIdTemp"].ToString());
            //    RoleObjPmsReal = GetListObPmsByTypeID(roleid, TypeRole);
            //    return PartialView("PermissionForEditRolePartial", RoleObjPmsReal.lstObjPmsView);
            //}
            //string ComboTypeRole = Session["TypeRoleCombobox"].ToString();
            //string stLstNhomchucnangID = (string)Session["stLstNhomchucnangIDEdit"];
            //return PermissionForEditRolePartialForSearch(stLstNhomchucnangID, ComboTypeRole,2);
        }

        public ActionResult PermissionForEditRolePartialForSearch(string stLstNhomchucnangID, string ComboTypeRole, int? CheckChuyen)
        {
            if (CheckChuyen == 1)
            {
                Session["RolePms_CheckChuyen"] = 1;
            }
            if (CheckChuyen == 2 && (int?)Session["RolePms_CheckChuyen"] == 1)
            {
                CheckChuyen = 1;
            }
            if (CheckChuyen == 1)
            {
                Session["ArrayPms"] = null;
                if (stLstNhomchucnangID == "")
                {
                    if (Session["ArrayPms"] == null)
                    {
                        var results = GetListObPmsByObjIdByTypeRole(ComboTypeRole);
                        return PartialView("PermissionForEditRolePartial", results);
                    }
                    else
                    {
                        var results = GetListObPmsByTickAddByTypeRole(ComboTypeRole);
                        return PartialView("PermissionForEditRolePartial", results.lstObjPmsView);
                    }
                }

                if (stLstNhomchucnangID.Trim() != "")
                {
                    Session["stLstNhomchucnangID"] = stLstNhomchucnangID;

                    List<int> _lstRealNhomchucnangInt = new List<int>();
                    _lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangID);

                    if (Session["ArrayPms"] == null)
                    {
                        var results = GetPmsByListNHOMCHUCNANG(_lstRealNhomchucnangInt, ComboTypeRole);
                        return PartialView("PermissionForEditRolePartial", results);
                    }
                    else
                    {
                        string ComboTypeRole_2 = Session["TypeRoleCombobox"].ToString();
                        string stLstNhomchucnangID_2 = Session["stLstNhomchucnangID"].ToString();
                        _lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangID_2);
                        var results = GetListObPmsByTickAddByTypeRoleAndNhomchucnangId(_lstRealNhomchucnangInt, ComboTypeRole_2);
                        return PartialView("PermissionForEditRolePartial", results.lstObjPmsView);
                    }
                }
                Session["TypeRoleCombobox"] = null;
                Session["stLstNhomchucnangID"] = null;
                return PermissionForEditRolePartial();
            }
            else
            {
                if (stLstNhomchucnangID.Trim() != "")
                {
                    Session["stLstNhomchucnangIDEdit"] = stLstNhomchucnangID;
                    Session["stLstNhomchucnangID"] = stLstNhomchucnangID;
                    List<int> _lstRealNhomchucnangInt = new List<int>();
                    _lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangID);
                    int roleid = int.Parse(Session["roleIdTemp"].ToString());
                    RoleObjPmsReal = GetListObPmsByRoleIdTypeForEditSearch(roleid, ComboTypeRole, _lstRealNhomchucnangInt);
                    return PartialView("PermissionForEditRolePartial", RoleObjPmsReal.lstObjPmsView);
                }
                else
                {
                    int roleid = int.Parse(Session["roleIdTemp"].ToString());
                    RoleObjPmsReal = GetListObPmsByTypeID(roleid, ComboTypeRole);
                    return PartialView("PermissionForEditRolePartial", RoleObjPmsReal.lstObjPmsView);
                }
            }


        }


        //THANHNC thêm tác vụ check all theo dòng
        public EmptyResult OnSelectAllFunctionPms(string pmsListInput)
        {
            var lstPmsReal = new List<string>();
            if (Session["ArrayPms"] != null)
                lstPmsReal = (List<string>)Session["ArrayPms"];

            //chuyển chuỗi truyền vào thành một list các permission và loại bỏ phần tử trống
            var listInputPermisson = pmsListInput.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //lấy index của các permission chưa được chọn
            var notCheckedIndexList = (from pms in listInputPermisson
                                             where !lstPmsReal.Contains(pms)
                                             select listInputPermisson.IndexOf(pms)).ToList();
            if (!notCheckedIndexList.Any())
            {
                //Nếu tất cả các permission truyền vào đều đã được check thì uncheck tất cả
                foreach (var pms in listInputPermisson)
                {
                    lstPmsReal.Remove(pms);
                }
            }
            else
            {
                //Nếu có permission chưa được check thì thực hiện check chọn
                lstPmsReal.AddRange(notCheckedIndexList.Select(index => listInputPermisson[index]));
            }
            Session["ArrayPms"] = lstPmsReal;

            if (Session["ChangeCheckBoxPms"] == null)
                Session["ChangeCheckBoxPms"] = true;

            return new EmptyResult();
        }

        #endregion

        public ActionResult UpdateRole(string roleId, string name, string RoleTypeId)
        {
            string results = "OK";
            var userid = _iTypeRoleService.Query.FirstOrDefault(m => m.ROLE_ID == long.Parse(roleId)).USERID;
            var userhientai = _IuserService.Query.FirstOrDefault(m => m.username == CurrentNguoidung.TENDANGNHAP).userid;
            if (userid == userhientai || userhientai == 1)
            {
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

                            p.Permissions = Listpermission.GroupBy(x => x.permissionid).Select(x => x.First()).ToList();

                            var type = _iTypeRoleService.Getbykey(roleid);

                            _iroleService.BeginTran();
                            _iroleService.Update(p);

                            if (type != null) type.TYPE = Int32.Parse(RoleTypeId);
                            _iTypeRoleService.Update(type);
                            _iroleService.CommitTran();
                            _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Cập nhật Phân quyền ", "Thực hiện chức năng cập nhật Phân quyền", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
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
            }
            else
            {
                results = "KoCoQuyen";
            }

            Session["stLstNhomchucnangIDEdit"] = null;
            Session["stLstNhomchucnangID"] = null;
            Session["ArrayPms"] = null;
            Session["ChangeCheckBoxPms"] = null;
            Session["TypeRoleCombobox"] = null;
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
        // Tích all Add
        public ActionResult CheckAllBoxPms(bool CheckAllPer)
        {
            List<string> lstPmsReal = new List<string>();
            var lsttypeper = _ITYPE_PERMISSIONService.Query.Where(o => o.LOAI_PERMISSION == 0).ToList();
            var _lstPms = _ipmsService.Query.Where(x => x.name != "TRA_CUU_GD").ToList();
            var lstP = (from p in _lstPms
                        join q in lsttypeper on p.permissionid equals q.PERMISSIONID
                        select p.name).ToList();
            if (Session["ArrayPms"] != null)
                lstPmsReal = (List<string>)Session["ArrayPms"];

            if (CheckAllPer)
            {
                lstPmsReal.AddRange(lstP);
            }
            else
            {
                lstPmsReal = lstPmsReal.Where(o => !lstP.Contains(o)).ToList();
            }
            if (Session["ChangeCheckBoxPms"] == null)
                Session["ChangeCheckBoxPms"] = true;
            Session["ArrayPms"] = lstPmsReal;
            return new EmptyResult();
        }
        // Tích all View
        public ActionResult CheckAllViewPms(bool CheckAllPer)
        {
            List<string> lstPmsReal = new List<string>();
            var lsttypeper = _ITYPE_PERMISSIONService.Query.Where(o => o.LOAI_PERMISSION == 1).ToList();
            var _lstPms = _ipmsService.Query.Where(x => x.name != "TRA_CUU_GD").ToList();
            var lstP = (from p in _lstPms
                        join q in lsttypeper on p.permissionid equals q.PERMISSIONID
                        select p.name).ToList();
            if (Session["ArrayPms"] != null)
                lstPmsReal = (List<string>)Session["ArrayPms"];

            if (CheckAllPer)
            {
                lstPmsReal.AddRange(lstP);
            }
            else
            {
                lstPmsReal = lstPmsReal.Where(o => !lstP.Contains(o)).ToList();
            }
            if (Session["ChangeCheckBoxPms"] == null)
                Session["ChangeCheckBoxPms"] = true;
            Session["ArrayPms"] = lstPmsReal;
            return new EmptyResult();
        }
        // Tích all Edit
        public ActionResult CheckAllEditPms(bool CheckAllPer)
        {
            List<string> lstPmsReal = new List<string>();
            var lsttypeper = _ITYPE_PERMISSIONService.Query.Where(o => o.LOAI_PERMISSION == 2).ToList();
            var _lstPms = _ipmsService.Query.Where(x => x.name != "TRA_CUU_GD").ToList();
            var lstP = (from p in _lstPms
                        join q in lsttypeper on p.permissionid equals q.PERMISSIONID
                        select p.name).ToList();
            if (Session["ArrayPms"] != null)
                lstPmsReal = (List<string>)Session["ArrayPms"];

            if (CheckAllPer)
            {
                lstPmsReal.AddRange(lstP);
            }
            else
            {
                lstPmsReal = lstPmsReal.Where(o => !lstP.Contains(o)).ToList();
            }
            if (Session["ChangeCheckBoxPms"] == null)
                Session["ChangeCheckBoxPms"] = true;
            Session["ArrayPms"] = lstPmsReal;
            return new EmptyResult();
        }
        // Tích all Delete
        public ActionResult CheckAllDeletePms(bool CheckAllPer)
        {
            List<string> lstPmsReal = new List<string>();
            var lsttypeper = _ITYPE_PERMISSIONService.Query.Where(o => o.LOAI_PERMISSION == 3).ToList();
            var _lstPms = _ipmsService.Query.Where(x => x.name != "TRA_CUU_GD").ToList();
            var lstP = (from p in _lstPms
                        join q in lsttypeper on p.permissionid equals q.PERMISSIONID
                        select p.name).ToList();
            if (Session["ArrayPms"] != null)
                lstPmsReal = (List<string>)Session["ArrayPms"];

            if (CheckAllPer)
            {
                lstPmsReal.AddRange(lstP);
            }
            else
            {
                lstPmsReal = lstPmsReal.Where(o => !lstP.Contains(o)).ToList();
            }
            if (Session["ChangeCheckBoxPms"] == null)
                Session["ChangeCheckBoxPms"] = true;
            Session["ArrayPms"] = lstPmsReal;
            return new EmptyResult();
        }
        // Tích all Duyệt
        public ActionResult CheckAllDuyetPms(bool CheckAllPer)
        {
            List<string> lstPmsReal = new List<string>();
            var lsttypeper = _ITYPE_PERMISSIONService.Query.Where(o => o.LOAI_PERMISSION == 4).ToList();
            var _lstPms = _ipmsService.Query.Where(x => x.name != "TRA_CUU_GD").ToList();
            var lstP = (from p in _lstPms
                        join q in lsttypeper on p.permissionid equals q.PERMISSIONID
                        select p.name).ToList();
            if (Session["ArrayPms"] != null)
                lstPmsReal = (List<string>)Session["ArrayPms"];

            if (CheckAllPer)
            {
                lstPmsReal.AddRange(lstP);
            }
            else
            {
                lstPmsReal = lstPmsReal.Where(o => !lstP.Contains(o)).ToList();
            }
            if (Session["ChangeCheckBoxPms"] == null)
                Session["ChangeCheckBoxPms"] = true;
            Session["ArrayPms"] = lstPmsReal;
            return new EmptyResult();
        }

        public EmptyResult BackPage()
        {
            Session["stLstNhomchucnangID"] = null;
            Session["stLstNhomchucnangIDEdit"] = null;
            Session["MODULEID"] = null;
            Session["ArrayPms"] = null;
            Session["roleIdTemp"] = null;
            Session["ChangeCheckBoxPms"] = null;
            Session["statusEnableCheckBox"] = null;
            Session["statusEnableCheckBoxCopy"] = null;
            Session["TypeRoleCombobox"] = null;
            return new EmptyResult();
        }
        public ActionResult DeleteRole(string roleId)
        {
            try
            {
                int roleid = int.Parse(roleId);
                string Results = "OK";
                bool checkUpdateUserRole = CheckStatusUpdateRole(roleid);
                string CheckDelete = CheckForDeleteRole(roleid);
                if (CheckDelete == "Success" && checkUpdateUserRole)
                {
                    var model = _iroleService.Getbykey(roleid);
                    var modelTypeRole = _iTypeRoleService.Query.FirstOrDefault(m => m.ROLE_ID == roleid);
                    _iroleService.BeginTran();
                    _iTypeRoleService.Delete(modelTypeRole);
                    _iroleService.Delete(model);
                    _iroleService.CommitTran();
                    _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa Phân quyền ", "Thực hiện chức năng xóa Phân quyền", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                }
                else if (CheckDelete == "ExistPms")
                    Results = "NotOKPms";
                else
                    Results = "NotOKUser";

                return Content(Results, "text/html");
            }
            catch (Exception ex)
            {
                _iroleService.RolbackTran();
                return null;
            }
        }
        public ActionResult NHOMCHUCNANGPartial()
        {
            int MODULEID = (int)Session["MODULEID"];
            return PartialView("NHOMCHUCNANGPartial", GetNHOMCHUCNANGByMODULE(MODULEID));
        }
        public EmptyResult ChangeMODULEIDCBB(int MODULEID)
        {
            Session["MODULEID"] = MODULEID;
            return new EmptyResult();
        }
        public EmptyResult ChangeTypeRoleCombox(string TypeRole)
        {
            Session["TypeRoleCombobox"] = TypeRole;
            return new EmptyResult();
        }
        private string CheckForDeleteRole(int roleid)
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
        private ObPmsView GetObjPmsByObjId(int objid, List<PMSCustom> _lstpms)
        {
            List<string> _lstName = new List<string>();
            List<string> _lstNameDes = new List<string>();

            List<PMSCustom> dataPms = _lstpms.Where(m => m.ObjectRBAC.objectid == objid).OrderBy(p => p.LOAI_PERMISSION).Take(10).ToList();
            List<PMSCustom> pmstemp = new List<PMSCustom>();

            for (int i = 0; i <= 5; i++)
            {
                var loai = dataPms.Where(c => c.LOAI_PERMISSION == i).ToList();
                if (loai.Any())
                    pmstemp.AddRange(loai);
                else
                {
                    PMSCustom pms = new PMSCustom();
                    pms.AppID = 1;
                    pms.Description = string.Empty;
                    pms.name = string.Empty;
                    pms.ObjectRBAC = null;
                    pms.Operation = null;
                    pms.permissionid = -1;
                    pms.Roles = null;
                    pms.LOAI_PERMISSION = i;
                    pmstemp.Add(pms);
                }
            }


            dataPms = pmstemp.OrderBy(o => o.LOAI_PERMISSION).Take(10).ToList();


            int n = dataPms.Count();
            ObPmsView _objpms = new ObPmsView();
            if (n > 0)
            {
                if (n == 1)
                {
                    _objpms.pms1 = dataPms[0].name.Trim();
                    _objpms.pms1Des = dataPms[0].Description.Trim();
                }
                else if (n == 2)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());

                    }
                    //_lstName.Sort();
                    //_lstNameDes.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                }
                else if (n == 3)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    //_lstName.Sort();
                    //_lstNameDes.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                }
                else if (n == 4)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    //_lstName.Sort();
                    //_lstNameDes.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                }
                else if (n == 5)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    //_lstName.Sort();
                    //_lstNameDes.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                }
                else if (n == 6)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    //_lstName.Sort();
                    //_lstNameDes.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                }
                else if (n == 7)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    //_lstName.Sort();
                    //_lstNameDes.Sort();

                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                    _objpms.pms7Des = _lstNameDes[6];
                }
                else if (n == 8)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                    _objpms.pms8 = _lstName[7];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                    _objpms.pms7Des = _lstNameDes[6];
                    _objpms.pms8Des = _lstNameDes[7];
                }
                else if (n == 9)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                    _objpms.pms8 = _lstName[7];
                    _objpms.pms9 = _lstName[8];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                    _objpms.pms7Des = _lstNameDes[6];
                    _objpms.pms8Des = _lstNameDes[7];
                    _objpms.pms9Des = _lstNameDes[8];
                }
                else if (n == 10)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                    _objpms.pms8 = _lstName[7];
                    _objpms.pms9 = _lstName[8];
                    _objpms.pms10 = _lstName[9];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                    _objpms.pms7Des = _lstNameDes[6];
                    _objpms.pms8Des = _lstNameDes[7];
                    _objpms.pms9Des = _lstNameDes[8];
                    _objpms.pms10Des = _lstNameDes[9];
                }
                else if (n == 11)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                    _objpms.pms8 = _lstName[7];
                    _objpms.pms9 = _lstName[8];
                    _objpms.pms10 = _lstName[9];
                    _objpms.pms11 = _lstName[10];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                    _objpms.pms7Des = _lstNameDes[6];
                    _objpms.pms8Des = _lstNameDes[7];
                    _objpms.pms9Des = _lstNameDes[8];
                    _objpms.pms10Des = _lstNameDes[9];
                    _objpms.pms11Des = _lstNameDes[10];
                }
                else if (n == 12)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                    _objpms.pms8 = _lstName[7];
                    _objpms.pms9 = _lstName[8];
                    _objpms.pms10 = _lstName[9];
                    _objpms.pms11 = _lstName[10];
                    _objpms.pms12 = _lstName[11];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                    _objpms.pms7Des = _lstNameDes[6];
                    _objpms.pms8Des = _lstNameDes[7];
                    _objpms.pms9Des = _lstNameDes[8];
                    _objpms.pms10Des = _lstNameDes[9];
                    _objpms.pms11Des = _lstNameDes[10];
                    _objpms.pms12Des = _lstNameDes[11];
                }
                else if (n == 11)
                {
                    for (int i = 0; i < n; i++)
                    {
                        _lstName.Add(dataPms[i].name.Trim());
                        _lstNameDes.Add(dataPms[i].Description.Trim());
                    }
                    _objpms.pms1 = _lstName[0];
                    _objpms.pms2 = _lstName[1];
                    _objpms.pms3 = _lstName[2];
                    _objpms.pms4 = _lstName[3];
                    _objpms.pms5 = _lstName[4];
                    _objpms.pms6 = _lstName[5];
                    _objpms.pms7 = _lstName[6];
                    _objpms.pms8 = _lstName[7];
                    _objpms.pms9 = _lstName[8];
                    _objpms.pms10 = _lstName[9];
                    _objpms.pms11 = _lstName[10];
                    _objpms.pms12 = _lstName[11];
                    _objpms.pms13 = _lstName[12];

                    _objpms.pms1Des = _lstNameDes[0];
                    _objpms.pms2Des = _lstNameDes[1];
                    _objpms.pms3Des = _lstNameDes[2];
                    _objpms.pms4Des = _lstNameDes[3];
                    _objpms.pms5Des = _lstNameDes[4];
                    _objpms.pms6Des = _lstNameDes[5];
                    _objpms.pms7Des = _lstNameDes[6];
                    _objpms.pms8Des = _lstNameDes[7];
                    _objpms.pms9Des = _lstNameDes[8];
                    _objpms.pms10Des = _lstNameDes[9];
                    _objpms.pms11Des = _lstNameDes[10];
                    _objpms.pms12Des = _lstNameDes[11];
                    _objpms.pms13Des = _lstNameDes[12];
                }
            }
            _objpms.objectid = objid;
            _objpms.name = _iobjService.Query.Where(o => o.objectid == objid).Select(m => m.name).FirstOrDefault();
            if (_objpms.name == "Danh mục Loại chuẩn bị")
            {
                var a = "Danh mục Loại chuẩn bị";
            }
            return _objpms;
        }
        private List<ObPmsView> GetListObPmsByObjIdByTypeRole(string TypePms)
        {
            List<int> _objList = new List<int>();
            List<ObPmsView> ObPmsViewList = new List<ObPmsView>();
            //ThaiPV : chỉnh sửa ngày 10/12/2016, loại bỏ permission tra cứu giám định, permission này dành riêng cho hỗ trợ, ko hiển thị cho người dùng.
            var _lstPms = _ipmsService.Query.Where(x => x.name != "TRA_CUU_GD").ToList();
            List<PMSCustom> _realPms = new List<PMSCustom>();
            //PMSCustom
            var _lstTypePms = _ITYPE_PERMISSIONService.Query.ToList();
            _realPms = (from p in _lstPms
                        join q in _lstTypePms on p.permissionid equals q.PERMISSIONID
                        where q.TYPE_PERMISSIONMULTI.Contains(TypePms)
                        orderby q.LOAI_PERMISSION
                        select new PMSCustom
                        {
                            AppID = p.AppID,
                            Description = p.Description,
                            name = p.name,
                            ObjectRBAC = p.ObjectRBAC,
                            Operation = p.Operation,
                            permissionid = p.permissionid,
                            Roles = p.Roles,
                            LOAI_PERMISSION = q.LOAI_PERMISSION,

                        }).ToList();
            _objList = _realPms.Select(m => m.ObjectRBAC.objectid).Distinct().ToList();
            _objList.Sort();

            foreach (var item in _objList)
            {
                ObPmsView _objpms = new ObPmsView();
                _objpms = GetObjPmsByObjId(item, _realPms);
                ObPmsViewList.Add(_objpms);
            }
            return ObPmsViewList;
        }
        //Lấy ListpmsName by RoleId
        private List<string> GetPmsStringByRoleId(int RoleId)
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
        //Lấy permission thực theo mỗi role
        private RoleObjectPms GetListObPmsByTypeID(int roleid, string TypeID)
        {
            var _lstRole = new List<role>();
            // var _alllstObPmsView = GetListObPmsByObjId();
            var _alllstObPmsView = GetListObPmsByObjIdByTypeRole(TypeID);
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
                            RoleObjPms.lstObjPmsView[i].pms1Des = " " + RoleObjPms.lstObjPmsView[i].pms1Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms2))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms2))
                        {
                            RoleObjPms.lstObjPmsView[i].pms2Des = " " + RoleObjPms.lstObjPmsView[i].pms2Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms3))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms3))
                        {
                            RoleObjPms.lstObjPmsView[i].pms3Des = " " + RoleObjPms.lstObjPmsView[i].pms3Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms4))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms4))
                        {
                            RoleObjPms.lstObjPmsView[i].pms4Des = " " + RoleObjPms.lstObjPmsView[i].pms4Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms5))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms5))
                        {
                            RoleObjPms.lstObjPmsView[i].pms5Des = " " + RoleObjPms.lstObjPmsView[i].pms5Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms6))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms6))
                        {
                            RoleObjPms.lstObjPmsView[i].pms6Des = " " + RoleObjPms.lstObjPmsView[i].pms6Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms7))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms7))
                        {
                            RoleObjPms.lstObjPmsView[i].pms7Des = " " + RoleObjPms.lstObjPmsView[i].pms7Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms8))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms8))
                        {
                            RoleObjPms.lstObjPmsView[i].pms8Des = " " + RoleObjPms.lstObjPmsView[i].pms8Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms9))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms9))
                        {
                            RoleObjPms.lstObjPmsView[i].pms9Des = " " + RoleObjPms.lstObjPmsView[i].pms9Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms10))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms10))
                        {
                            RoleObjPms.lstObjPmsView[i].pms10Des = " " + RoleObjPms.lstObjPmsView[i].pms10Des.Trim();
                        }
                    }
                }
            }
            return RoleObjPms;
        }
        private RoleObjectPms GetListObPmsByRoleId(int roleid)
        {
            int _typeRole = GetTypeRoleByRoleId(roleid);
            var _lstRole = new List<role>();
            // var _alllstObPmsView = GetListObPmsByObjId();
            var _alllstObPmsView = GetListObPmsByObjIdByTypeRole(_typeRole.ToString());
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
                            RoleObjPms.lstObjPmsView[i].pms1Des = " " + RoleObjPms.lstObjPmsView[i].pms1Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms2))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms2))
                        {
                            RoleObjPms.lstObjPmsView[i].pms2Des = " " + RoleObjPms.lstObjPmsView[i].pms2Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms3))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms3))
                        {
                            RoleObjPms.lstObjPmsView[i].pms3Des = " " + RoleObjPms.lstObjPmsView[i].pms3Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms4))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms4))
                        {
                            RoleObjPms.lstObjPmsView[i].pms4Des = " " + RoleObjPms.lstObjPmsView[i].pms4Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms5))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms5))
                        {
                            RoleObjPms.lstObjPmsView[i].pms5Des = " " + RoleObjPms.lstObjPmsView[i].pms5Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms6))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms6))
                        {
                            RoleObjPms.lstObjPmsView[i].pms6Des = " " + RoleObjPms.lstObjPmsView[i].pms6Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms7))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms7))
                        {
                            RoleObjPms.lstObjPmsView[i].pms7Des = " " + RoleObjPms.lstObjPmsView[i].pms7Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms8))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms8))
                        {
                            RoleObjPms.lstObjPmsView[i].pms8Des = " " + RoleObjPms.lstObjPmsView[i].pms8Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms9))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms9))
                        {
                            RoleObjPms.lstObjPmsView[i].pms9Des = " " + RoleObjPms.lstObjPmsView[i].pms9Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms10))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms10))
                        {
                            RoleObjPms.lstObjPmsView[i].pms10Des = " " + RoleObjPms.lstObjPmsView[i].pms10Des.Trim();
                        }
                    }
                }
            }
            return RoleObjPms;
        }
        private RoleObjectPms GetListObPmsByRoleIdTypeForEditSearch(int roleid, string TypeRole, List<int> nhomchucnangList)
        {
            var _lstRole = new List<role>();
            var _alllstObPmsView = GetPmsByListNHOMCHUCNANG(nhomchucnangList, TypeRole);
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
                            RoleObjPms.lstObjPmsView[i].pms1Des = " " + RoleObjPms.lstObjPmsView[i].pms1Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms2))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms2))
                        {
                            RoleObjPms.lstObjPmsView[i].pms2Des = " " + RoleObjPms.lstObjPmsView[i].pms2Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms3))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms3))
                        {
                            RoleObjPms.lstObjPmsView[i].pms3Des = " " + RoleObjPms.lstObjPmsView[i].pms3Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms4))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms4))
                        {
                            RoleObjPms.lstObjPmsView[i].pms4Des = " " + RoleObjPms.lstObjPmsView[i].pms4Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms5))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms5))
                        {
                            RoleObjPms.lstObjPmsView[i].pms5Des = " " + RoleObjPms.lstObjPmsView[i].pms5Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms6))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms6))
                        {
                            RoleObjPms.lstObjPmsView[i].pms6Des = " " + RoleObjPms.lstObjPmsView[i].pms6Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms7))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms7))
                        {
                            RoleObjPms.lstObjPmsView[i].pms7Des = " " + RoleObjPms.lstObjPmsView[i].pms7Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms8))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms8))
                        {
                            RoleObjPms.lstObjPmsView[i].pms8Des = " " + RoleObjPms.lstObjPmsView[i].pms8Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms9))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms9))
                        {
                            RoleObjPms.lstObjPmsView[i].pms9Des = " " + RoleObjPms.lstObjPmsView[i].pms9Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms10))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms10))
                        {
                            RoleObjPms.lstObjPmsView[i].pms10Des = " " + RoleObjPms.lstObjPmsView[i].pms10Des.Trim();
                        }
                    }
                }
            }
            return RoleObjPms;
        }
        //Lấy permission thực theo mỗi role For EditSearch
        private RoleObjectPms GetListObPmsByTickAddByTypeRoleAndNhomchucnangId(List<int> nhomchucnangList, string TypePms)
        {
            var _lstRole = new List<role>();
            var _alllstObPmsView = GetPmsByListNHOMCHUCNANG(nhomchucnangList, TypePms);
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
                            RoleObjPms.lstObjPmsView[i].pms1Des = " " + RoleObjPms.lstObjPmsView[i].pms1Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms2))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms2))
                        {
                            RoleObjPms.lstObjPmsView[i].pms2Des = " " + RoleObjPms.lstObjPmsView[i].pms2Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms3))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms3))
                        {
                            RoleObjPms.lstObjPmsView[i].pms3Des = " " + RoleObjPms.lstObjPmsView[i].pms3Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms4))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms4))
                        {
                            RoleObjPms.lstObjPmsView[i].pms4Des = " " + RoleObjPms.lstObjPmsView[i].pms4Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms5))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms5))
                        {
                            RoleObjPms.lstObjPmsView[i].pms5Des = " " + RoleObjPms.lstObjPmsView[i].pms5Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms6))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms6))
                        {
                            RoleObjPms.lstObjPmsView[i].pms6Des = " " + RoleObjPms.lstObjPmsView[i].pms6Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms7))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms7))
                        {
                            RoleObjPms.lstObjPmsView[i].pms7Des = " " + RoleObjPms.lstObjPmsView[i].pms7Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms8))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms8))
                        {
                            RoleObjPms.lstObjPmsView[i].pms8Des = " " + RoleObjPms.lstObjPmsView[i].pms8Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms9))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms9))
                        {
                            RoleObjPms.lstObjPmsView[i].pms9Des = " " + RoleObjPms.lstObjPmsView[i].pms9Des.Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms10))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms10))
                        {
                            RoleObjPms.lstObjPmsView[i].pms10Des = " " + RoleObjPms.lstObjPmsView[i].pms10Des.Trim();
                        }
                    }
                }
            }
            return RoleObjPms;
        }
        private RoleObjectPms GetListObPmsByTickAddByTypeRole(string TypePms)
        {
            var _lstRole = new List<role>();
            var _alllstObPmsView = GetListObPmsByObjIdByTypeRole(TypePms);
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
                            RoleObjPms.lstObjPmsView[i].pms1Des = " " + RoleObjPms.lstObjPmsView[i].pms1Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms2))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms2))
                        {
                            RoleObjPms.lstObjPmsView[i].pms2Des = " " + RoleObjPms.lstObjPmsView[i].pms2Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms3))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms3))
                        {
                            RoleObjPms.lstObjPmsView[i].pms3Des = " " + RoleObjPms.lstObjPmsView[i].pms3Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms4))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms4))
                        {
                            RoleObjPms.lstObjPmsView[i].pms4Des = " " + RoleObjPms.lstObjPmsView[i].pms4Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms5))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms5))
                        {
                            RoleObjPms.lstObjPmsView[i].pms5Des = " " + RoleObjPms.lstObjPmsView[i].pms5Des.Trim();
                        }
                    }


                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms6))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms6))
                        {
                            RoleObjPms.lstObjPmsView[i].pms6Des = " " + RoleObjPms.lstObjPmsView[i].pms6Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms7))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms7))
                        {
                            RoleObjPms.lstObjPmsView[i].pms7Des = " " + RoleObjPms.lstObjPmsView[i].pms7Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms8))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms8))
                        {
                            RoleObjPms.lstObjPmsView[i].pms8Des = " " + RoleObjPms.lstObjPmsView[i].pms8Des.Trim();
                        }
                    }
                    //thanhpt
                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms9))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms9))
                        {
                            RoleObjPms.lstObjPmsView[i].pms9Des = " " + RoleObjPms.lstObjPmsView[i].pms9Des.Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(lstObPmsViewAll[i].pms10))
                    {
                        if (lstpmsViewData.Contains(lstObPmsViewAll[i].pms10))
                        {
                            RoleObjPms.lstObjPmsView[i].pms10Des = " " + RoleObjPms.lstObjPmsView[i].pms10Des.Trim();
                        }
                    }
                }
            }
            RoleObjPms.lstObjPmsView.OrderBy(o => o.objectid);
            return RoleObjPms;
        }
        //Lấy permission thực theo Add tick Search
        // New function
        private bool CheckRedirect()
        {
            bool results = false;
            if (CurrentNguoidung.ISPQ == true || CheckStatusAdminRoot())
                results = true;
            return results;
        }
        private List<role> GetListRoles()
        {
            string username = CurrentNguoidung.TENDANGNHAP;
            var _userRecord = _IuserService.Query.FirstOrDefault(m => m.username == username);
            List<role> _results = new List<role>();

            if (_userRecord != null)
            {
                var _lstrole = _userRecord.Roles;
                if (_lstrole.Any())
                {
                    var _userOfRoleRecord = _lstrole.FirstOrDefault(m => m.name == "Root");
                    if (_userOfRoleRecord != null)
                    {
                        var _lst = _iroleService.Query.OrderBy(m => m.roleid);
                        _results = _lst.OrderBy(m => m.roleid).ToList();
                        return _results;
                    }
                }
                int userid = _userRecord.userid;
                int LevelUser = CheckStatusTinhOrTrungUong();
                var _lstTyperoles = _iTypeRoleService.Query.Where(m => m.USERID == 0 || m.USERID == userid || m.TYPE == LevelUser).Select(n => n.ROLE_ID).ToList();
                var _lstNotRoot = _iroleService.Query.Where(m => _lstTyperoles.Contains(m.roleid));
                //ThaiPV : chỉnh sửa ngày 10/12/2016, loại bỏ role tra cứu giám định, role này dành riêng cho hỗ trợ, ko hiển thị cho người dùng.
                _results = _lstNotRoot.Where(x => x.name != "Tiện ích tra cứu hệ thống giám định").OrderBy(m => m.roleid).ToList();
            }
            return _results;
        }
        private List<long> GetTypeRole()
        {
            string username = CurrentNguoidung.TENDANGNHAP;
            var _userRecord = _IuserService.Query.FirstOrDefault(m => m.username == username);
            int userid = 0;
            if (_userRecord != null)
            {
                userid = _userRecord.userid;
            }
            return _iTypeRoleService.Query.Where(m => m.USERID == userid).Select(n => n.ROLE_ID).ToList();
        }
        private int CheckStatusTinhOrTrungUong()
        {
            var results = 2;
            var groupgame_1 = _IuserService.Query.FirstOrDefault(m => m.username == CurrentNguoidung.TENDANGNHAP).GroupName;
            var groupName_2 = _IuserService.Query.FirstOrDefault(m => m.username == groupgame_1);
            if (groupName_2 != null)
            {
                if (groupName_2.GroupName == "admin")
                    results = 1;
            }
          
            return results;
        }
        private int CheckStatusTinhOrTrungUongOrRoot()
        {
            var results = 2;
            string username = CurrentNguoidung.TENDANGNHAP;
            var _userRecord = _IuserService.Query.FirstOrDefault(m => m.username == username);
            if (_userRecord != null)
            {
                var _lstrole = _userRecord.Roles;
                if (_lstrole.Any())
                {
                    var _userOfRoleRecord = _lstrole.FirstOrDefault(m => m.name == "Root");
                    if (_userOfRoleRecord != null)
                    {
                        results = 0;
                        return results;
                    }
                }

                var groupgame_1 = _IuserService.Query.FirstOrDefault(m => m.username == username).GroupName;
                var groupName_2 = _IuserService.Query.FirstOrDefault(m => m.username == groupgame_1);
                if (groupName_2 != null)
                {
                if (groupName_2.GroupName == "admin")
                        results = 1;
                }
            }
            return results;
        }
        private bool CheckStatusUpdateRole(int roleid)
        {
            var results = false;
            string username = CurrentNguoidung.TENDANGNHAP;
            var _userRecord = _IuserService.Query.FirstOrDefault(m => m.username == username);

            if (_userRecord != null)
            {
                var _lstrole = _userRecord.Roles;
                if (_lstrole.Any())
                {
                    var _userOfRoleRecord = _lstrole.FirstOrDefault(m => m.name == "Root");
                    if (_userOfRoleRecord != null)
                    {
                        results = true;
                        return results;
                    }
                }
                int userid = _userRecord.userid;
                var _userOfRoleRecordTg = _iTypeRoleService.Query.FirstOrDefault(m => m.USERID == userid && m.ROLE_ID == roleid);
                if (_userOfRoleRecordTg != null)
                {
                    results = true;
                }
            }
            return results;
        }
        private bool CheckStatusAdminRoot()
        {
            var results = false;
            string username = CurrentNguoidung.TENDANGNHAP;
            var _userRecord = _IuserService.Query.FirstOrDefault(m => m.username == username);

            if (_userRecord != null)
            {
                var _lstrole = _userRecord.Roles;
                if (_lstrole.Any())
                {
                    var _userOfRoleRecord = _lstrole.FirstOrDefault(m => m.name == "Root");
                    if (_userOfRoleRecord != null)
                    {
                        results = true;
                    }
                }
            }
            return results;
        }
        private List<MODULE> GetMODULE()
        {
            return _IMODULEService.Query.OrderBy(m => m.MODULEID).ToList();
        }
        private List<NHOMCHUCNANG> GetNHOMCHUCNANGByMODULE(int MODULEID)
        {
            return _INHOMCHUCNANGService.Query.Where(m => m.MODULEID == MODULEID).OrderBy(n => n.NHOMCHUCNANGID).ToList();
        }
        private List<ObPmsView> GetPmsByListNHOMCHUCNANG(List<int> nhomchucnangList, string TypeRole)
        {
            List<ObPmsView> ObPmsViewList = new List<ObPmsView>();

            var _lstPms = _ipmsService.Query.ToList();

            List<PMSCustom> _realPms = new List<PMSCustom>();

            List<NHOMCHUCNANG_OBJECT> _lstRealNHOMCN_OBJECT = new List<NHOMCHUCNANG_OBJECT>();

            var _lstNHOMCN_OBJECT = _INHOMCHUCNANG_OBJECTService.GetAll();

            _lstRealNHOMCN_OBJECT = (from p in _lstNHOMCN_OBJECT
                                     join q in nhomchucnangList
                                         on p.NHOMCHUCNANGID equals q
                                     select p).ToList();

            var _lstRealPermisison = (from p in _lstRealNHOMCN_OBJECT
                                      join q in _lstPms
                                          on p.OBJECTID equals q.ObjectRBAC.objectid
                                      select q);

            var _lstTypePms = _ITYPE_PERMISSIONService.Query.ToList();
            _realPms = (from p in _lstRealPermisison
                        join q in _lstTypePms
                            on p.permissionid equals q.PERMISSIONID
                        where q.TYPE_PERMISSIONMULTI.Contains(TypeRole)
                        select new PMSCustom
                        {
                            AppID = p.AppID,
                            Description = p.Description,
                            name = p.name,
                            ObjectRBAC = p.ObjectRBAC,
                            Operation = p.Operation,
                            permissionid = p.permissionid,
                            Roles = p.Roles,
                            LOAI_PERMISSION = q.LOAI_PERMISSION,

                        }).ToList();

            var _objList = _realPms.Select(m => new { m.ObjectRBAC.objectid }).Distinct().OrderBy(n => n.objectid);
            foreach (var item in _objList)
            {
                ObPmsView _objpms = new ObPmsView();
                _objpms = GetObjPmsByObjId(item.objectid, _realPms);
                ObPmsViewList.Add(_objpms);
            }
            return ObPmsViewList;
        }
        private int GetTypeRoleByRoleId(int roleid)
        {
            return _iTypeRoleService.Query.FirstOrDefault(m => m.ROLE_ID == roleid).TYPE;
        }
        private List<int> GetlstNhomchucnangId(string stLstNhomchucnangID)
        {
            string[] strDestination = stLstNhomchucnangID.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var _lstRealNhomchucnangString = strDestination.ToList();
            List<int> _lstRealNhomchucnangInt = new List<int>();

            if (_lstRealNhomchucnangString.Any())
            {
                foreach (var item in _lstRealNhomchucnangString)
                {
                    int itemInt = int.Parse(item);
                    _lstRealNhomchucnangInt.Add(itemInt);
                }
            }
            return _lstRealNhomchucnangInt;
        }


        #region Edit Coppy New 
        public ActionResult EditRoleCoppy(string roleId)
        {
            Session["RolePms_CheckChuyen"] = null;
            sttUser = CheckStatusTinhOrTrungUongOrRoot();
            ViewData["SttUser"] = sttUser;
            MODULEID = 0;
            lstMODULE = GetMODULE();
            if (lstMODULE.Any())
                MODULEID = lstMODULE.FirstOrDefault().MODULEID;
            Session["MODULEID"] = MODULEID;
            Session["ArrayPms"] = null;
            Session["ChangeCheckBoxPms"] = null;
            ViewData["lstMODULE"] = lstMODULE;
            ViewData["FirstNHOMCHUCNANG"] = GetNHOMCHUCNANGByMODULE(MODULEID);
            Session["stLstNhomchucnangIDEdit"] = null;
            var roleid = int.Parse(roleId);
            Session["roleIdTemp"] = roleId;
            var item = _iroleService.Getbykey(roleid);
            var flag = item.name.Contains("Copy") || item.name.Contains("Coppy");
            var nameRole = flag
                ? item.name
                : string.Format("{0} {1}", _iroleService.Getbykey(roleid).name, "Copy");


            ViewBag.rolename = nameRole;
            var typeroleid = _iTypeRoleService.Getbykey(roleid).TYPE;
            ViewBag.TypeRoleId = typeroleid;
            Session["TypeRoleCombobox"] = typeroleid.ToString();
            @ViewBag.vbSttUserEdit = typeroleid;
            @ViewBag.hdRoleIdEdit = roleId;
            ViewBag.hdRoleId = roleId;
            Session["statusEnableCheckBoxCopy"] = _INHOMCHUCNANGService.UserPhanQuyen(roleid);

            if (roleid == 76 || roleid == 77 || roleid == 79 || roleid == 183 || roleid == 184 || roleid == 203 || roleid == 204 || roleid == 205 || roleid == 206 || roleid == 207 || roleid == 208)
                ViewBag.RoleEnable = false;
            else
                ViewBag.RoleEnable = true;

            RoleObjPmsReal = GetListObPmsByRoleId(roleid);
            Session["EndCallbackCheckBox"] = RoleObjPmsReal;
            return View(RoleObjPmsReal);
        }

        public ActionResult PermissionForEditRoleCoppyPartial()
        {
            if (Session["TypeRoleCombobox"] != null && Session["stLstNhomchucnangID"] == null)
            {
                var comboTypeRole = Session["TypeRoleCombobox"].ToString();
                if (Session["ArrayPms"] == null)
                {
                    var results = GetListObPmsByObjIdByTypeRole(comboTypeRole);
                    return PartialView("PermissionForAddRolePartial", results);
                }
                else
                {
                    var results = GetListObPmsByTickAddByTypeRole(comboTypeRole);
                    return PartialView("PermissionForEditRoleCoppyPartial", results.lstObjPmsView);
                }
            }

            if (Session["TypeRoleCombobox"] != null && Session["stLstNhomchucnangID"] != null)
            {
                var comboTypeRole2 = Session["TypeRoleCombobox"].ToString();
                var stLstNhomchucnangId2 = Session["stLstNhomchucnangID"].ToString();

                if (Session["ArrayPms"] == null)
                {
                    return PermissionForEditRoleCoppyPartialForSearch(stLstNhomchucnangId2, comboTypeRole2, 2);
                }

                var lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangId2);
                var results = GetListObPmsByTickAddByTypeRoleAndNhomchucnangId(lstRealNhomchucnangInt, comboTypeRole2);
                return PartialView("PermissionForEditRoleCoppyPartial", results.lstObjPmsView);
            }

            var stLstNhomchucnangId = (string)Session["stLstNhomchucnangID"];
            return stLstNhomchucnangId == null ? RedirectToAction("Index", "RolePms") : PermissionForEditRoleCoppyPartialForSearch(stLstNhomchucnangId, sttUser.ToString(), 2);
        }

        public ActionResult PermissionForEditRoleCoppyPartialForSearch(string stLstNhomchucnangId, string comboTypeRole, int? checkChuyen)
        {
            if (checkChuyen == 1)
            {
                Session["RolePms_CheckChuyen"] = 1;
            }
            if (checkChuyen == 2 && (int?)Session["RolePms_CheckChuyen"] == 1)
            {
                checkChuyen = 1;
            }
            if (checkChuyen == 1)
            {
                Session["ArrayPms"] = null;
                if (stLstNhomchucnangId == "")
                {
                    if (Session["ArrayPms"] == null)
                    {
                        var results = GetListObPmsByObjIdByTypeRole(comboTypeRole);
                        return PartialView("PermissionForEditRoleCoppyPartial", results);
                    }
                    else
                    {
                        var results = GetListObPmsByTickAddByTypeRole(comboTypeRole);
                        return PartialView("PermissionForEditRoleCoppyPartial", results.lstObjPmsView);
                    }
                }

                if (stLstNhomchucnangId.Trim() != "")
                {
                    Session["stLstNhomchucnangID"] = stLstNhomchucnangId;

                    var lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangId);

                    if (Session["ArrayPms"] == null)
                    {
                        var results = GetPmsByListNHOMCHUCNANG(lstRealNhomchucnangInt, comboTypeRole);
                        return PartialView("PermissionForEditRoleCoppyPartial", results);
                    }
                    else
                    {
                        var comboTypeRole2 = Session["TypeRoleCombobox"].ToString();
                        var stLstNhomchucnangId2 = Session["stLstNhomchucnangID"].ToString();
                        lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangId2);
                        var results = GetListObPmsByTickAddByTypeRoleAndNhomchucnangId(lstRealNhomchucnangInt, comboTypeRole2);
                        return PartialView("PermissionForEditRoleCoppyPartial", results.lstObjPmsView);
                    }
                }
                Session["TypeRoleCombobox"] = null;
                Session["stLstNhomchucnangID"] = null;
                return PermissionForEditRoleCoppyPartial();
            }

            if (stLstNhomchucnangId.Trim() != "")
            {
                Session["stLstNhomchucnangIDEdit"] = stLstNhomchucnangId;
                Session["stLstNhomchucnangID"] = stLstNhomchucnangId;
                var lstRealNhomchucnangInt = GetlstNhomchucnangId(stLstNhomchucnangId);
                var roleid = int.Parse(Session["roleIdTemp"].ToString());
                RoleObjPmsReal = GetListObPmsByRoleIdTypeForEditSearch(roleid, comboTypeRole, lstRealNhomchucnangInt);
                return PartialView("PermissionForEditRoleCoppyPartial", RoleObjPmsReal.lstObjPmsView);
            }
            else
            {
                var roleid = int.Parse(Session["roleIdTemp"].ToString());
                RoleObjPmsReal = GetListObPmsByTypeID(roleid, comboTypeRole);
                return PartialView("PermissionForEditRoleCoppyPartial", RoleObjPmsReal.lstObjPmsView);
            }
        }


        public ActionResult UpdateRoleCoppy(string name, string RoleTypeId)
        {
            var results = "OK";
            if (ModelState.IsValid && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(RoleTypeId))
            {
                var checkExist = _iroleService.Query.FirstOrDefault(x => x.name.ToUpper() == name.ToUpper());
                if (checkExist == null)
                {
                    try
                    {
                        var p = new role {AppID = 1, name = name.Trim()};
                        var lstPmsChange = new List<string>();

                        if (Session["ArrayPms"] != null)
                            lstPmsChange = (List<string>)Session["ArrayPms"];

                        foreach (var word in lstPmsChange)
                        {
                            _permission = _ipmsService.GetByName(word, 1);
                            Listpermission.Add(_permission);
                        }
                        p.Permissions = Listpermission.GroupBy(x => x.permissionid).Select(x => x.First()).ToList();


                        _iroleService.BeginTran();
                        _iroleService.CreateNew(p);

                        if (CheckStatusAdminRoot())
                            typeRole = new TYPE_ROLE { ROLE_ID = p.roleid, TYPE = int.Parse(RoleTypeId) };
                        else
                        {
                            var userid = _IuserService.GetByName(CurrentNguoidung.TENDANGNHAP).userid;
                            typeRole = new TYPE_ROLE { ROLE_ID = p.roleid, TYPE = int.Parse(RoleTypeId), USERID = userid };
                        }

                        _iTypeRoleService.CreateNew(typeRole);
                        _iroleService.CommitTran();

                        Session["stLstNhomchucnangID"] = null;
                        Session["ArrayPms"] = null;
                        Session["ChangeCheckBoxPms"] = null;
                        Session["TypeRoleCombobox"] = null;
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Thêm mới Phân quyền ", "Thực hiện chức năng thêm mới Phân quyền", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
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
          
            return Content(results, "text/html");
        }


        [HttpPost]
        public JsonResult EncalBackCheckBox()
        {
            var mess = "";
            var model = Session["EndCallbackCheckBox"] as RoleObjectPms;

            if (model == null) return Json(new { flag = true });
            var pms1 = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms1));
            var pms1Des = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms1Des) && o.pms1Des.Substring(0,1) == " ");
            mess += pms1 == pms1Des ? "1|" : "0|";

            var pms2 = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms2));
            var pms2Des = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms2Des) && o.pms2Des.Substring(0, 1) == " ");
            mess += pms2 == pms2Des ? "1|" : "0|";


            var pms3 = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms3));
            var pms3Des = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms3Des) && o.pms3Des.Substring(0, 1) == " ");
            mess += pms3 == pms3Des ? "1|" : "0|";


            var pms4 = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms4));
            var pms4Des = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms4Des) && o.pms4Des.Substring(0, 1) == " ");
            mess += pms4 == pms4Des ? "1|" : "0|";


            var pms5 = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms5));
            var pms5Des = model.lstObjPmsView.Count(o => !string.IsNullOrEmpty(o.pms5Des) && o.pms5Des.Substring(0, 1) == " ");
            mess += pms5 == pms5Des ? "1|" : "0|";

            return Json(new { flag = false, mess });
        }


        #endregion


        //End new function
    }
}