using EIS.Core.Domain;
using IdentityManagement.Domain;
using System.Collections.Generic;

namespace EIS.Core.CustomView
{
    public class UserGroupModelDetail
    {
        public IList<NHOM_USER> Groups { get; set; }

        public IList<role> Roles { get; set; }

        public IList<user> usersOfGroup { get; set; }
    }

    public class UserNonGroupViewModel
    {
        public List<user> users { get; set; }

        public List<int> userIdOfGroup { get; set; }
        
        public UserNonGroupViewModel()
        {
            userIdOfGroup = new List<int>();
        }
    }
}