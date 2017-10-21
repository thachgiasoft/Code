using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIS.FEW.Models
{
    public class UserModel
    {
        public UserRecord currentUserRecord { get; set; }

        public List<UserModelView> lstUserModelView { get; set; }
    }
}