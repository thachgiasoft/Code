using IdentityManagement.Domain;
using System.Collections.Generic;

namespace EIS.Core.CustomView
{
    public class UserGroupViewModel
    {
        public IList<user> Users { get; set; }

        public IList<role> Roles { get; set; }
    }
}