using EIS.Core.Domain;
using System.Globalization;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using IdentityManagement.Domain;
using IdentityManagement.Service;
using NPOI.OpenXmlFormats.Vml.Office;

namespace EIS.Core.ServiceImp
{
    public class NGUOIDUNGService : BaseService<NGUOIDUNG, long>, INGUOIDUNGService
    {
        private IDM_DONVIService _donviSvc;
        private IuserService _iuserService = IoC.Resolve<IuserService>();

        public NGUOIDUNGService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        {
            _donviSvc = IoC.Resolve<IDM_DONVIService>();
            _iuserService = IoC.Resolve<IuserService>();
        }
        public List<NGUOIDUNG> getAll()
        {
            return Query.ToList();
        }
        public IQueryable<NGUOIDUNG> getByNguoidung(NGUOIDUNG nguoidung)
        {
            var donvinguoidung = _donviSvc.getByDonviID((long)nguoidung.DONVI_ID).Select(i => i.ID).ToList();
            return Query.Where(o => o.DONVI_ID == nguoidung.DONVI_ID || donvinguoidung.Contains(o.DONVI_ID ?? 0));
        }
        public bool CheckPermission(string username, string pmsname)
        {
            //pmsname = pmsname + "1";
            var firstOrDefault = _iuserService.Query.FirstOrDefault(m => m.username == username);
            if (firstOrDefault == null) return false;
            var _lstRoles = firstOrDefault.Roles;
            return _lstRoles != null && _lstRoles.Select(item => item.Permissions).Where(_lstpms => _lstpms != null).SelectMany(_lstpms => _lstpms).Any(itempms => itempms.name == pmsname);
        }

        public string CheckquyenPermission(string lstTacVu, NGUOIDUNG nguoidung) // dung cho check quyen cac from danh muc
        {
            string[] tacvustrs = lstTacVu.Split(',');
            string tacvustrsADD = lstTacVu.Split(',')[0];
            string tacvustrsEDIT = lstTacVu.Split(',')[1];
            string tacvustrsDELETE = lstTacVu.Split(',')[2];
            string result = "";
            bool ketqua = false;
            for (int i = 0; i < tacvustrs.Length; i++)
            {
                if (i == 0) ketqua = (DoCheckPermission(nguoidung.TENDANGNHAP, tacvustrsADD));
                if (i == 1) ketqua = (DoCheckPermission(nguoidung.TENDANGNHAP, tacvustrsEDIT));
                if (i == 2) ketqua = (DoCheckPermission(nguoidung.TENDANGNHAP, tacvustrsDELETE));
                result += (ketqua ? 1 : 0) + "|";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        // dung cho check quyen lãnh đạo tỉnh from danh muc đơn vị
        public string CheckquyenPermission_tinh(string lstTacVu, NGUOIDUNG nguoidung)
        {
            string[] tacvustrs = lstTacVu.Split(',');
            string tacvustrsADD = lstTacVu.Split(',')[0];
            string tacvustrsEDIT = lstTacVu.Split(',')[1];
            string tacvustrsDELETE = lstTacVu.Split(',')[2];
            string result = "";
            bool ketqua = false;
            for (int i = 0; i < tacvustrs.Length; i++)
            {
                if (i == 0) ketqua = (DoCheckPermission(nguoidung.TENDANGNHAP, tacvustrsADD));
                if (i == 1) ketqua = (DoCheckPermission(nguoidung.TENDANGNHAP, tacvustrsEDIT) && ((nguoidung.DONVI.DONVICHA_ID != null && nguoidung.VAITRO == 2) || nguoidung.DONVI.DONVICHA_ID == null));
                if (i == 2) ketqua = (DoCheckPermission(nguoidung.TENDANGNHAP, tacvustrsDELETE));
                result += (ketqua ? 1 : 0) + "|";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
        public bool DoCheckPermission(string username, string pmsname)
        {
            var _iuserService = IoC.Resolve<IuserService>();
            var firstOrDefault = _iuserService.Query.FirstOrDefault(m => m.username == username);
            if (firstOrDefault == null) return false;
            var lstRoles = firstOrDefault.Roles;
            if (lstRoles == null) return false;

            return lstRoles.Select(item => item.Permissions).
                            Where(lstpms => lstpms != null).
                            Select(lstpms => lstpms.Any(x => x.name.ToUpper() == pmsname.ToUpper())).
                            Any(results => results);
        }
    }
}