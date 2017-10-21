using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FX.Core;
using IdentityManagement.Domain;
using IdentityManagement.Service;

namespace EIS.FEW.Helper
{
    public class RbacUser
    {
        public user User {get; set; }

        public RbacUser(string username)
        {
            GetDatabaseUserRolesPermissions(username);
        }

        private void GetDatabaseUserRolesPermissions(string userName)
        {
            var userService = IoC.Resolve<IuserService>();
            User = !string.IsNullOrEmpty(userName) ? userService.GetByName(userName) : new user();
        }

        public bool HasPermission(string requiredPermission)
        {
            var bFound = false;
            foreach (var role in User.Roles)
            {
                bFound = (role.Permissions.Where(p => p.name == requiredPermission).ToList().Count > 0);
                if (bFound)
                    break;
            }
            return bFound;
        }

        public bool HasRole(string role)
        {
            return (User.Roles.Where(p => p.name == role).ToList().Count > 0);
        }

        public bool HasRoles(string roles)
        {
            var split = roles.ToLower().Split(';');
            foreach (var role in User.Roles)
            {
                try
                {
                    if (split.Contains(role.name.ToLower()))
                        return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
 
}