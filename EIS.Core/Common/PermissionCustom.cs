using FX.Core;
using IdentityManagement.Service;
using System.Linq;

namespace EIS.Core.Common
{
    public class PermissionCustom
    {
        private static readonly PermissionCustom _instance = new PermissionCustom();

        public static PermissionCustom Instance
        {
            get
            {
                return _instance;
            }
        }

        private PermissionCustom()
        {

        }

        public string DoCheckPermission(string username, string pmsname)
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

        public bool CheckPermissionForDM(string username, string pmsname)
        {
            var firstOrDefault = IoC.Resolve<IuserService>().Query.FirstOrDefault(m => m.username == username);
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