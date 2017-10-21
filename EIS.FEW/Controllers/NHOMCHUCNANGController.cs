using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevExpress.XtraSpreadsheet;
using EIS.Core.Domain;
using FX.Core;
using EIS.Core.IService;
using IdentityManagement.Authorization;
using EIS.FEW.Models;

namespace EIS.FEW.Controllers
{
    [RBACAuthorize(Roles = "Root")]
    public class NHOMCHUCNANGController : Controller
    {
        private readonly IMODULEService _IMODULEService;
        private readonly INHOMCHUCNANGService _INHOMCHUCNANGService;
        private readonly ILogSystemService _iLogSystemService;
        private readonly INHOMCHUCNANG_OBJECTService _INHOMCHUCNANG_OBJECTService;

        public NHOMCHUCNANGController()
        {
            _IMODULEService = IoC.Resolve<IMODULEService>();
            _INHOMCHUCNANGService = IoC.Resolve<INHOMCHUCNANGService>();
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
            _INHOMCHUCNANG_OBJECTService = IoC.Resolve<INHOMCHUCNANG_OBJECTService>();

            
        }
        public ActionResult Index()
        {
            Session["MODULEList"] = GetAllMODULE();
            return View(GetAllModuleChucnang());
        }
        public ActionResult NHOMCHUCNANGPartial()
        {
            return PartialView("NHOMCHUCNANGPartial", GetAllModuleChucnang());
        }
        public ActionResult AddNHOMCHUCNANG(ModuleChucnang obj)
        {
            if (ModelState.IsValid)
            {
                obj.ChucnangDESCRIPTION = obj.ChucnangDESCRIPTION.Trim();
                var checkname = _INHOMCHUCNANGService.Query.FirstOrDefault(x => x.DESCRIPTION.ToUpper() == obj.ChucnangDESCRIPTION.ToUpper());
                if (checkname == null)
                {
                    try
                    {
                        NHOMCHUCNANG item = new NHOMCHUCNANG();
                        item.MODULEID = obj.MODULEID;
                        item.DESCRIPTION = obj.ChucnangDESCRIPTION;

                        _INHOMCHUCNANGService.CreateNew(item);
                        _INHOMCHUCNANGService.CommitChanges();
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Thêm mới Nhóm chức năng ", "Thực hiện chức năng thêm mới Nhóm chức năng", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                    }
                    catch (Exception e)
                    {
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
            return PartialView("NHOMCHUCNANGPartial", GetAllModuleChucnang());
        }
        public ActionResult UpdateNHOMCHUCNANG(ModuleChucnang obj)
        {
            if (ModelState.IsValid)
            {
                obj.ChucnangDESCRIPTION = obj.ChucnangDESCRIPTION.Trim();
                var checkname = _INHOMCHUCNANGService.Query.FirstOrDefault(x => x.DESCRIPTION.ToUpper() == obj.ChucnangDESCRIPTION.ToUpper());
                if (checkname == null)
                {
                    try
                    {
                        NHOMCHUCNANG item = new NHOMCHUCNANG();
                        item = _INHOMCHUCNANGService.Getbykey(obj.NHOMCHUCNANGID);

                        item.MODULEID = obj.MODULEID;
                        item.DESCRIPTION = obj.ChucnangDESCRIPTION;

                        _INHOMCHUCNANGService.Update(item);
                        _INHOMCHUCNANGService.CommitChanges();
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Cập nhật Nhóm chức năng ", "Thực hiện chức năng cập nhật Nhóm chức năng", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
                    }
                    catch (Exception e)
                    {
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
            return PartialView("NHOMCHUCNANGPartial", GetAllModuleChucnang());
        }
        public ActionResult DeleteNHOMCHUCNANG(ModuleChucnang obj)
        {
            if (CheckForDeleteNHOMCHUCNANG(obj.NHOMCHUCNANGID))
            {
                _INHOMCHUCNANGService.Delete(obj.NHOMCHUCNANGID);
                _INHOMCHUCNANGService.CommitChanges();
                _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa Nhóm chức năng ", "Thực hiện chức năng xóa Nhóm chức năng", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
            }
            else
            {
                ViewData["EditErrorDelete"] = "Không thể xóa vì tồn tại chức năng trong nhóm chức năng này!";
            }
            return PartialView("NHOMCHUCNANGPartial", GetAllModuleChucnang());
        }
        public List<ModuleChucnang> GetAllModuleChucnang()
        {
            List<ModuleChucnang> lstModuleChucnang = new List<ModuleChucnang>();
            var _nhomChucnang = _INHOMCHUCNANGService.Query;
            var _nhomModule = _IMODULEService.Query;

            var _lstNHOMCHUCNANG = (from p in _nhomChucnang
                                    join q in _nhomModule on p.MODULEID equals q.MODULEID
                                    orderby p.NHOMCHUCNANGID ascending
                                    select new
                                    {
                                        NHOMCHUCNANGID = p.NHOMCHUCNANGID,
                                        MODULEID = q.MODULEID,
                                        ChucnangDESCRIPTION = p.DESCRIPTION,
                                        ModuleDESCRIPTION = q.DESCRIPTION
                                    });

            if (_lstNHOMCHUCNANG.Any())
            {
                foreach (var item in _lstNHOMCHUCNANG)
                {
                    ModuleChucnang _item = new ModuleChucnang();
                    _item.NHOMCHUCNANGID = item.NHOMCHUCNANGID;
                    _item.MODULEID = item.MODULEID;
                    _item.ChucnangDESCRIPTION = item.ChucnangDESCRIPTION;
                    _item.ModuleDESCRIPTION = item.ModuleDESCRIPTION;
                    lstModuleChucnang.Add(_item);
                }
            }
            return lstModuleChucnang;
        }
        public bool CheckForDeleteNHOMCHUCNANG(int NHOMCHUCNANGID)
        {
            var _NHOMCHUCNANG = _INHOMCHUCNANG_OBJECTService.Query.FirstOrDefault(m => m.NHOMCHUCNANGID == NHOMCHUCNANGID);
            return (_NHOMCHUCNANG == null);
        }
        public List<MODULE> GetAllMODULE()
        {
            var _lstNHOMMODULE = _IMODULEService.Query.OrderBy(m => m.MODULEID).ToList();
            return _lstNHOMMODULE;
        }
    }
}
