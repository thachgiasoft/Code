using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FX.Core;
using EIS.Core.IService;
using IdentityManagement.Service;
using IdentityManagement.Domain;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.FEW.Models;
using IdentityManagement.Authorization;

namespace EIS.FEW.Controllers
{
    [RBACAuthorize(Roles = "Root")]
    public class PermissionController : Controller
    {
        private readonly IpermissionService _iPMSService;
        private readonly IobjectService _iServiceObj;
        private readonly IroleService _iroleService;
        private readonly ITYPE_ROLEService _iTypeRoleService;
        private readonly ILogSystemService _iLogSystemService;
        private readonly ITYPE_PERMISSIONService _ITYPE_PERMISSIONService;
        public PermissionController()
        {
            _iPMSService = IoC.Resolve<IpermissionService>();
            _iServiceObj = IoC.Resolve<IobjectService>();
            _iTypeRoleService = IoC.Resolve<ITYPE_ROLEService>();
            _iroleService = IoC.Resolve<IroleService>();
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
            _ITYPE_PERMISSIONService = IoC.Resolve<ITYPE_PERMISSIONService>();

        }
        public ActionResult Index()
        {
            Session["MultiType"] = null;
            Session["MultiTypeCheckEdit"] = null;
            Session["ObjectList"] = GetAllObjectRbac();
            var result = GetAllPms();
            return View(result);
        }
        public ActionResult PERMISSIONPartial()
        {
            return PartialView("PERMISSIONPartial", GetAllPms());
        }
        public ActionResult AddNewPms(ObjectPermission obpms)
        {
            if (ModelState.IsValid && Session["MultiType"] != null)
            {
                var checkPermissionInObject =
                    _iPMSService.Query.Where(m => m.ObjectRBAC.objectid == obpms.ObjectId)
                        .Select(n => n.permissionid)
                        .ToList();
                int count = checkPermissionInObject.Count;
                if (count < 8)
                {
                    var checkname = _iPMSService.Query.FirstOrDefault(x => x.name.ToUpper() == obpms.PmsName.ToUpper());
                    if (checkname == null)
                    {

                        try
                        {
                            objectRbac objRb = new objectRbac();
                            objRb.objectid = obpms.ObjectId;

                            permission p = new permission();
                            p.AppID = 1;
                            p.name = obpms.PmsName.Trim();
                            p.Description = obpms.DesPms.Trim();
                            p.ObjectRBAC = objRb;
                            var Type_pms = new TYPE_PERMISSION();
                            string TYPE_PERMISSIONMULTI = Session["MultiType"].ToString();

                            _iPMSService.BeginTran();

                            _iPMSService.CreateNew(p);
                            Type_pms.PERMISSIONID = p.permissionid;
                            Type_pms.TYPE_PERMISSIONMULTI = TYPE_PERMISSIONMULTI;
                            Type_pms.LOAI_PERMISSION = obpms.LoaiPermission;
                            _ITYPE_PERMISSIONService.CreateNew(Type_pms);

                            _iPMSService.CommitTran();
                            _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Thêm mới tác vụ ", "Thực hiện chức năng thêm mới tác vụ", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                        }
                        catch (Exception e)
                        {
                            _iPMSService.RolbackTran();
                            ViewData["EditError"] = e.Message;

                        }
                    }
                    else
                    {
                        ViewData["EditError"] = "Tên đã tồn tại, xin chọn tên khác!";
                    }
                }
                else
                {
                    ViewData["EditError"] = "Một chức năng chỉ chứa tối đa 8 tác vụ, nhóm hiện đã có 8 tác vụ, vui lòng chọn chức năng khác!";
                }
            }
            else
            {
                ViewData["EditError"] = "Bạn phải nhập đầy đủ thông tin";
            }
            Session["MultiType"] = null;
            Session["MultiTypeCheckEdit"] = null;
            return PartialView("PERMISSIONPartial", GetAllPms());
        }
        public ActionResult UpdatePms(ObjectPermission obpms)
        {
            if ((ModelState.IsValid && Session["MultiTypeCheckEdit"] == null) || (ModelState.IsValid && Session["MultiType"] != null))
            {
                var checkPermissionInObject =
                  _iPMSService.Query.Where(m => m.ObjectRBAC.objectid == obpms.ObjectId)
                      .Select(n => n.permissionid)
                      .ToList();
                int count = checkPermissionInObject.Count;
                if (count <= 8)
                {
                    var checkname = _iPMSService.Query.FirstOrDefault(x => x.name.ToUpper() == obpms.PmsName.ToUpper());
                    if (checkname == null || checkname.permissionid == obpms.PermissionId)
                    {
                        try
                        {
                            objectRbac objRecord = new objectRbac();
                            objRecord.objectid = obpms.ObjectId;
                            permission p = new permission();
                            p = _iPMSService.Getbykey(obpms.PermissionId);
                            p.name = obpms.PmsName.Trim();
                            p.Description = obpms.DesPms.Trim();
                            p.ObjectRBAC = objRecord;


                            _iPMSService.BeginTran();


                            if (Session["MultiType"] != null)
                            {
                                var Type_pms = _ITYPE_PERMISSIONService.Getbykey(obpms.PermissionId);
                                string TYPE_PERMISSIONMULTI = Session["MultiType"].ToString();
                                Type_pms.TYPE_PERMISSIONMULTI = TYPE_PERMISSIONMULTI;
                                var listrole = _iroleService.Query.Where(m => m.Permissions.Any(n => n.permissionid == obpms.PermissionId));
                                foreach (var item in listrole)
                                {
                                    var typerole = _iTypeRoleService.Query.FirstOrDefault(m => m.ROLE_ID == item.roleid) != null ? _iTypeRoleService.Query.FirstOrDefault(m => m.ROLE_ID == item.roleid).TYPE : -1;
                                    if (!(TYPE_PERMISSIONMULTI.Contains(typerole + "")))
                                    {
                                        IList<permission> listtam = item.Permissions;
                                        var bientam_PMS = listtam.FirstOrDefault(m => m.permissionid == obpms.PermissionId);
                                        listtam.Remove(bientam_PMS);
                                        item.Permissions = listtam;
                                        _iroleService.Update(item);
                                    }
                                }
                                _ITYPE_PERMISSIONService.Update(Type_pms);
                            }
                            _iPMSService.Update(p);
                            _iPMSService.CommitTran();
                            _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Cập nhật tác vụ ", "Thực hiện chức năng cập nhật tác vụ", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                        }
                        catch (Exception e)
                        {
                            _iPMSService.RolbackTran();
                            ViewData["EditError"] = e.Message;
                        }
                    }
                    else
                    {
                        ViewData["EditError"] = "Tên đã tồn tại, xin chọn tên khác!";
                    }
                }
                else
                {
                    ViewData["EditError"] = "Một chức năng chỉ chứa tối đa 8 tác vụ, nhóm hiện đã có 8 tác vụ, vui lòng chọn chức năng khác!";
                }
            }
            else
            {
                ViewData["EditError"] = "Bạn phải nhập đầy đủ thông tin";
            }
            Session["MultiType"] = null;
            Session["MultiTypeCheckEdit"] = null;
            return PartialView("PERMISSIONPartial", GetAllPms());
        }
        public EmptyResult ChangeMultiType(string lstMultiType)
        {
            if (lstMultiType.Trim() != "")
                Session["MultiType"] = lstMultiType.Trim();
            else
                Session["MultiType"] = null;
            Session["MultiTypeCheckEdit"] = true;

            return new EmptyResult();
        }
        public ActionResult DeletePms(ObjectPermission obpms)
        {
            if (CheckForDeletePms(obpms.PermissionId))
            {
                permission model = _iPMSService.Getbykey(obpms.PermissionId);
                var Type_pms = _ITYPE_PERMISSIONService.Getbykey(obpms.PermissionId);

                _iPMSService.BeginTran();
                _iPMSService.Delete(model);
                _ITYPE_PERMISSIONService.Delete(Type_pms);
                _iPMSService.CommitTran();
                _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa tác vụ ", "Thực hiện chức năng Xóa tác vụ", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
            }
            else
            {
                _iPMSService.RolbackTran();
                ViewData["EditErrorDelete"] = "Không thể xóa vì có phân quyền chứa tác vụ này!";
            }
            return PartialView("PERMISSIONPartial", GetAllPms());
        }
        private bool CheckForDeletePms(int pmsid)
        {
            var pms = _iroleService.GetAll();
            int n = pms.Count();
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    var pmsrole = pms[i].Permissions.Select(m => m.permissionid).ToList();
                    if (pmsrole.Any())
                    {
                        if (pmsrole.Contains(pmsid))
                            return false;
                    }
                }
            }
            return true;
        }
        private string GetPmsMultiTypeName(string stMultiId)
        {
            string results = "";
            stMultiId = stMultiId.Trim();
            var multiid = stMultiId.Split(',').ToList();
            foreach (var item in multiid)
            {
                if (item == "0")
                    results += "Quản trị,";
                else if (item == "1")
                    results += "Trung ương,";
                else if (item == "2")
                    results += "Tỉnh,";
                else if (item == "3")
                    results += "Huyện,";
            }

            int lengthstMulti = results.Length;
            results = results.Substring(0, lengthstMulti - 1);
            return results;
        }
        public static List<SelectListItem> GetRealSelectListItems(string MutiTypeName)
        {
            List<SelectListItem> _lstFull = new List<SelectListItem>();
            _lstFull.Add(new SelectListItem { Value = "0", Text = "Quản trị" });
            _lstFull.Add(new SelectListItem { Value = "1", Text = "Trung ương" });
            _lstFull.Add(new SelectListItem { Value = "2", Text = "Tỉnh" });
            _lstFull.Add(new SelectListItem { Value = "3", Text = "Huyện" });

            var _lstRealMultiName = MutiTypeName.Split(',');
            foreach (var item in _lstFull)
            {
                if (_lstRealMultiName.Contains(item.Text))
                {
                    item.Text = " " + item.Text;
                }
            }
            return _lstFull;
        }
        private List<ObjectPermission> GetAllPms()
        {
            List<TYPE_PERMISSION> _lstType_Pms = new List<TYPE_PERMISSION>();
            _lstType_Pms = _ITYPE_PERMISSIONService.GetAll();

            List<Type_PmsMulti> _lstPmsMulti = new List<Type_PmsMulti>();
            foreach (var item in _lstType_Pms)
            {
                Type_PmsMulti _typeRecord = new Type_PmsMulti();
                _typeRecord.PERMISSIONID = item.PERMISSIONID;
                _typeRecord.TYPE_PERMISSIONMULTIID = item.TYPE_PERMISSIONMULTI;
                _typeRecord.TYPE_PERMISSIONMULTINAME = GetPmsMultiTypeName(item.TYPE_PERMISSIONMULTI);
                _typeRecord.Loai_Permission = item.LOAI_PERMISSION;
                _lstPmsMulti.Add(_typeRecord);
            }

            List<permission> _lstAllPms = new List<permission>();
            //ThaiPV : chỉnh sửa ngày 10/12/2016, loại bỏ permission tra cứu giám định, permission này dành riêng cho hỗ trợ, ko hiển thị cho người dùng.
            _lstAllPms = _iPMSService.GetAll().Where(x => x.name != "TRA_CUU_GD").ToList();


            var _lstPmsFull = (from p in _lstPmsMulti
                               join q in _lstAllPms on p.PERMISSIONID equals q.permissionid
                               select new
                               {
                                   permissionid = p.PERMISSIONID,
                                   name = q.name,
                                   Description = q.Description,
                                   MultiPmsTypeID = p.TYPE_PERMISSIONMULTIID,
                                   TYPE_PERMISSIONMULTINAME = p.TYPE_PERMISSIONMULTINAME,
                                   objid = q.ObjectRBAC.objectid,
                                   objname = q.ObjectRBAC.name,
                                   loaiPermission = p.Loai_Permission

                               });

            List<ObjectPermission> lstObjPmsResults = new List<ObjectPermission>();

            foreach (var item in _lstPmsFull)
            {
                ObjectPermission objP = new ObjectPermission();
                objP.AppID = 1;
                objP.PermissionId = item.permissionid;
                objP.PmsName = item.name;
                objP.ObjName = item.objname;
                objP.ObjectId = item.objid;
                objP.DesPms = item.Description;
                objP.MultiTypeID = item.MultiPmsTypeID;
                objP.MultiTypeName = item.TYPE_PERMISSIONMULTINAME;
                objP.LoaiPermission = item.loaiPermission;
                lstObjPmsResults.Add(objP);
            }
            return lstObjPmsResults.OrderBy(m => m.PermissionId).ToList();
        }
        public List<objectRbac> GetAllObjectRbac()
        {
            return _iServiceObj.Query.OrderBy(m => m.objectid).ToList();
        }
    }
}
