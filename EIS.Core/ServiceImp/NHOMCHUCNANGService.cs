using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EIS.Core.ServiceImp
{
    public class NHOMCHUCNANGService : BaseService<NHOMCHUCNANG, int>, INHOMCHUCNANGService
    {
        public NHOMCHUCNANGService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public bool UserPhanQuyen(int roleid)
        {
            var typerollsrv = IoC.Resolve<ITYPE_ROLEService>();
            var userSrv = IoC.Resolve<IdentityManagement.Service.IuserService>();
            var nguoidung = ((EISContext)FX.Context.FXContext.Current).CurrentNguoidung;       

            var results = false;
            var username = nguoidung.TENDANGNHAP;
            var userRecord = userSrv.Query.FirstOrDefault(m => m.username == username);
            if (userRecord == null) return false;

            var lstrole = userRecord.Roles;
            if (lstrole.Any())
            {
                var userOfRoleRecord = lstrole.FirstOrDefault(m => m.name == "Root");
                if (userOfRoleRecord != null)
                {
                    return true;
                }
            }

            var userid = userRecord.userid;
            if (nguoidung.DONVI.DONVICHA_ID == null)
            {
                var userOfRoleRecordTg = typerollsrv.Query.FirstOrDefault(o => o.ROLE_ID == roleid && o.TYPE != 0);// && (o.USERID == userid || o.USERID == 0));
                if (userOfRoleRecordTg != null)
                {
                    results = true;
                }
            }
            else
            {
                var userOfRoleRecordTg = typerollsrv.Query.FirstOrDefault(o => o.ROLE_ID == roleid && o.TYPE != 0 && o.TYPE != 1); //&& (o.USERID == userid || o.USERID == 0));
                if (userOfRoleRecordTg != null)
                {
                    results = true;
                }
            }
            return results;
        }
    }
}
