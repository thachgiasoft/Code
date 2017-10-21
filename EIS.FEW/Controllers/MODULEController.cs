using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EIS.Core.Domain;
using FX.Core;
using EIS.Core.IService;
using IdentityManagement.Authorization;

namespace EIS.FEW.Controllers
{
    [RBACAuthorize(Roles = "Root")]
    public class MODULEController : Controller
    {
        private readonly IMODULEService _IMODULEService;
        private readonly INHOMCHUCNANGService _INHOMCHUCNANGService;
        private readonly ILogSystemService _iLogSystemService;

        public MODULEController()
        {
            _IMODULEService = IoC.Resolve<IMODULEService>();
            _INHOMCHUCNANGService = IoC.Resolve<INHOMCHUCNANGService>();
            _iLogSystemService = IoC.Resolve<ILogSystemService>();
        }
        public ActionResult Index()
        {
            return View(GetAllMODULE());
        }
        public ActionResult MODULEPartial()
        {
            return PartialView("MODULEPartial", GetAllMODULE());
        }
        public ActionResult AddMODULE(MODULE obj)
        {
            if (ModelState.IsValid)
            {
                obj.DESCRIPTION = obj.DESCRIPTION.Trim();
                var checkname = _IMODULEService.Query.FirstOrDefault(x => x.DESCRIPTION.ToUpper() == obj.DESCRIPTION.ToUpper());
                if (checkname == null)
                {
                    try
                    {
                        _IMODULEService.CreateNew(obj);
                        _IMODULEService.CommitChanges();
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Thêm mới Nhóm Module ", "Thực hiện chức năng thêm mới Module", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
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
            return PartialView("MODULEPartial", GetAllMODULE());
        }
        public ActionResult UpdateMODULE(MODULE obj)
        {
            if (ModelState.IsValid)
            {
                obj.DESCRIPTION = obj.DESCRIPTION.Trim();
                var checkname = _IMODULEService.Query.FirstOrDefault(x => x.DESCRIPTION.ToUpper() == obj.DESCRIPTION.ToUpper());
                if (checkname == null)
                {
                    try
                    {
                        _IMODULEService.Update(obj);
                        _IMODULEService.CommitChanges();
                        _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Cập nhật Nhóm Module ", "Thực hiện chức năng cập nhật Module", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
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
            return PartialView("MODULEPartial", GetAllMODULE());
        }
        public ActionResult DeleteMODULE(MODULE obj)
        {
            if (CheckForDeleteMODULE(obj.MODULEID))
            {
                _IMODULEService.Delete(obj);
                _IMODULEService.CommitChanges();
                _iLogSystemService.CreateNew(HttpContext.User.Identity.Name, "Xóa Nhóm Module ", "Thực hiện chức năng xóa Module", Helper.GetIPAddress.GetVisitorIPAddress(), HttpContext.Request.Browser.Browser);
            }
            else
            {
                ViewData["EditError"] = "Không thể xóa vì có nhóm chức năng trong Module này!";
            }
            return PartialView("MODULEPartial", GetAllMODULE());
        }
        public List<MODULE> GetAllMODULE()
        {
            var _lstNHOMMODULE = _IMODULEService.Query.OrderBy(m => m.MODULEID).ToList();
            return _lstNHOMMODULE;
        }
        public bool CheckForDeleteMODULE(int MODULEID)
        {
            var _NHOMMODULE = _INHOMCHUCNANGService.Query.FirstOrDefault(m => m.MODULEID == MODULEID);
            if (_NHOMMODULE != null)
                return false;
            return true;
        }
    }
}
