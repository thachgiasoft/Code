using EIS.Core.Domain;
using FX.Core;
using IdentityManagement.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace EIS.Core.Common
{
    public class Constants
    {
        public static int CheckVaiTro(NGUOIDUNG nguoiDung)
        {
            if (nguoiDung.DONVI != null)
            {

                if (nguoiDung.DONVI.DONVICHA_ID == null)// && nguoiDung.VAITRO == 2)
                {
                    return (int)(Common.VaiTroND.TrungUong); //Trung ương
                }
                if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.VAITRO == 2)
                {
                    return (int)(Common.VaiTroND.Tinh); //lãnh đạo tỉnh
                }
                if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.DONVI.DONVICHA.DONVICHA_ID != null && nguoiDung.VAITRO != 2)
                {
                    return (int)(Common.VaiTroND.GDVCoSo); //Trưởng nhóm, giám định viên cấp cơ sở
                }
                if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.VAITRO != 2)
                {
                    return (int)(Common.VaiTroND.TruongNhom); //Trưởng nhóm, giám định viên cấp tỉnh
                }
            }
            return 0;
        }
        // check th có vai trò = 0 và = 1 trong trưởng nhóm riêng
        // thanhpt
        public static int CheckVaiTroBaoCao12NghiepVu(NGUOIDUNG nguoiDung)
        {
            if (nguoiDung.DONVI != null)
            {

                if (nguoiDung.DONVI.DONVICHA_ID == null)// && nguoiDung.VAITRO == 2)
                {
                    return (int)(Common.VaiTroND.TrungUong); //Trung ương
                }
                if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.VAITRO == 2)
                {
                    return (int)(Common.VaiTroND.Tinh); //lãnh đạo tỉnh
                }
                if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.DONVI.DONVICHA.DONVICHA_ID != null && nguoiDung.VAITRO != 2)
                {
                    //return (int)(Common.VaiTroND.GDVCoSo); //Trưởng nhóm, giám định viên cấp cơ sở
                    if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.VAITRO == 0)
                    {
                        return (int)(Common.VaiTroND.GDVCoSo); 
                    }
                    else
                    {
                        return (int)(Common.VaiTroND.TruongNhom);
                    }
                }
                if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.VAITRO != 2)
                {
                    if (nguoiDung.DONVI.DONVICHA_ID != null && nguoiDung.VAITRO == 0)
                    {
                        return (int)(Common.VaiTroND.GDVCoSo); //Trưởng nhóm, giám định viên cấp cơ sở
                    }
                    else
                    {
                        return (int)(Common.VaiTroND.TruongNhom); //Trưởng nhóm, giám định viên cấp tỉnh
                    }
                }
            }
            return 0;
        }
        // Chỉ check 3 vai trò để lấy dữ liệu đơn vị hoặc cskcb
        public static int CheckVaiTro_GetDL(NGUOIDUNG nguoiDung)
        {

            var vaiTro = CheckVaiTro(nguoiDung);
            if (vaiTro == (int)(Common.VaiTroND.TrungUong))
            {
                return (int)(Common.VaiTroND.TrungUong);
            }
            else if (vaiTro == (int)(Common.VaiTroND.Tinh))
            {
                return (int)(Common.VaiTroND.Tinh);
            }
            else
            {
                return (int)(Common.VaiTroND.TruongNhom);
            }
        }

        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string DoCheckPermission(string username, string pmsname)
        {
            IuserService UserDataService = IoC.Resolve<IuserService>();
            string results = "NotOK";
            var _lstRoles = UserDataService.Query.Where(m => m.username == username).FirstOrDefault().Roles;
            if (_lstRoles.Any())
            {
                foreach (var item in _lstRoles)
                {
                    var _lstpms = item.Permissions.Select(m => m.name).ToList();
                    if (_lstpms.Any())
                    {
                        bool _checkLstPms = _lstpms.Contains(pmsname);
                        if (_checkLstPms)
                        {
                            results = "OK";
                            return results;
                        }
                    }
                }
            }
            return results;
        }

    }
}
