using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIS.Core.Domain;
using EIS.FEW.Helper;
using FX.Core;
using FX.Data;
using EIS.Core.IService;
using EIS.Core.ServiceImp;
using IdentityManagement.Domain;
using IdentityManagement.Service;
using IdentityManagement.Authorization;
using EIS.FEW.Models;

namespace EIS.FEW.Controllers
{
    [RBACAuthorize(Roles = "Root")]
    public class ObjectController : Controller
    {
        private readonly IobjectService _iService;
        private readonly IpermissionService _ipmsService;
        private readonly ILogSystemService _iLogSystemService;
        private readonly INHOMCHUCNANGService _INHOMCHUCNANGService;
        private readonly INHOMCHUCNANG_OBJECTService _INHOMCHUCNANG_OBJECTService;
        public ObjectController()
        {
            _iService = IoC.Resolve<IobjectService>();
            _ipmsService = IoC.Resolve<IpermissionService>();
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
            _INHOMCHUCNANGService = IoC.Resolve<INHOMCHUCNANGService>();
            _INHOMCHUCNANG_OBJECTService = IoC.Resolve<INHOMCHUCNANG_OBJECTService>();
        }
        public ActionResult Index()
        {
            Session["NHOMCHUCNANGListView"] = GetAllNHOMCHUCNANG();
            return View(GetAllObjectView());
        }
        public ActionResult ObjectPartial()
        {
            return PartialView("ObjectPartial", GetAllObjectView());
        }
        public ActionResult AddNewObj(ObjectView obj)
        {

            if (ModelState.IsValid)
            {
                var checkname = _iService.Query.FirstOrDefault(x => x.name.ToUpper() == obj.name.ToUpper());
                if (checkname == null)
                {
                    try
                    {
                        objectRbac p = new objectRbac();
                        p.AppID = 1;
                        p.locked = false;
                        p.name = obj.name.Trim();
                        _iService.BeginTran();
                        _iService.CreateNew(p);
                        var _nhomchucnangobject = new NHOMCHUCNANG_OBJECT { NHOMCHUCNANGID = obj.nhomchucnangid, OBJECTID = p.objectid };
                        _INHOMCHUCNANG_OBJECTService.CreateNew(_nhomchucnangobject);
                        _iService.CommitTran();
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Thêm mới chức năng", "Thực hiện chức năng thêm mới chức năng", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                    }
                    catch (Exception e)
                    {
                        _iService.RolbackTran();
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
                ViewData["EditError"] = "Bạn phải nhập đầy đủ thông tin!";
            }
            return PartialView("ObjectPartial", GetAllObjectView());
        }
        public ActionResult UpdateObj(ObjectView obj)
        {
            if (ModelState.IsValid)
            {
                var checkname = _iService.Query.FirstOrDefault(x => x.name.ToUpper() == obj.name.ToUpper());
                if (checkname == null || checkname.objectid == obj.objectid)
                {
                    try
                    {
                        objectRbac p = new objectRbac();
                        p = _iService.Getbykey(obj.objectid);
                        p.name = obj.name.Trim();

                        var _nhomcn_object = _INHOMCHUCNANG_OBJECTService.Getbykey(obj.objectid);
                        _nhomcn_object.NHOMCHUCNANGID = obj.nhomchucnangid;

                        _iService.BeginTran();
                        _INHOMCHUCNANG_OBJECTService.Update(_nhomcn_object);
                        _iService.Update(p);
                        _iService.CommitTran();
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Cập nhật chức năng", "Thực hiện chức năng cập nhật chức năng", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                    }
                    catch (Exception e)
                    {
                        _iService.RolbackTran();
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
                ViewData["EditError"] = "Bạn phải nhập đầy đủ thông tin!";
            }
            return PartialView("ObjectPartial", GetAllObjectView());
        }
        public ActionResult DeleteObj(ObjectView obj)
        {
            if (CheckForDeleteObject(obj.objectid))
            {
                _iService.BeginTran();
                _INHOMCHUCNANG_OBJECTService.Delete(obj.objectid);
                _iService.Delete(obj.objectid);
                _iService.CommitTran();
                _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa chức năng", "Thực hiện chức năng xóa chức năng", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
            }
            else
            {
                _iService.RolbackTran();
                ViewData["EditError"] = "Không thể xóa vì có tác vụ trong chức năng này!";
            }
            return PartialView("ObjectPartial", GetAllObjectView());
        }
        public List<ObjectView> GetAllObjectView()
        {
            var _nhomchucnang = _INHOMCHUCNANGService.GetAll();
            var _nhomchucnang_objectRelate = _INHOMCHUCNANG_OBJECTService.GetAll();
            var _nhomchucnang_objectid = (from p in _nhomchucnang
                                          join q in _nhomchucnang_objectRelate on p.NHOMCHUCNANGID equals q.NHOMCHUCNANGID
                                          select new
                                          {
                                              NHOMCHUCNANGID = p.NHOMCHUCNANGID,
                                              NHOMCHUCNANGDES = p.DESCRIPTION,
                                              OBJECTID = q.OBJECTID
                                          });

            var _objectmain = _iService.GetAll();

            var _nhomchucnang_objectfull = (from p in _nhomchucnang_objectid
                                            join q in _objectmain on p.OBJECTID equals q.objectid
                                            select new
                                            {
                                                NHOMCHUCNANGID = p.NHOMCHUCNANGID,
                                                NHOMCHUCNANGDES = p.NHOMCHUCNANGDES,
                                                objectid = q.objectid,
                                                objname = q.name
                                            });


            List<ObjectView> _lstObjectView = new List<ObjectView>();
            if (_nhomchucnang_objectfull.Any())
            {
                foreach (var item in _nhomchucnang_objectfull)
                {
                    ObjectView _objectView = new ObjectView();
                    _objectView.objectid = item.objectid;
                    _objectView.name = item.objname;
                    _objectView.DESCRIPTION = item.NHOMCHUCNANGDES;
                    _objectView.nhomchucnangid = item.NHOMCHUCNANGID;
                    _lstObjectView.Add(_objectView);
                }
            }
            return _lstObjectView.OrderBy(m => m.objectid).ToList();
        }
        public bool CheckForDeleteObject(int objid)
        {
            var pmsbyobjid = _ipmsService.Query.Where(o => o.ObjectRBAC.objectid == objid).Select(m => m.permissionid).Take(1).ToList();
            if (pmsbyobjid.Any())
                return false;
            return true;
        }
        public List<NHOMCHUCNANG> GetAllNHOMCHUCNANG()
        {
            return _INHOMCHUCNANGService.Query.OrderBy(m => m.NHOMCHUCNANGID).ToList();
        }
    }
}
