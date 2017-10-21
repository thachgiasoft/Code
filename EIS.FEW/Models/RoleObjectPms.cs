using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityManagement.Domain;
namespace EIS.FEW.Models
{
    public class RoleObjectPms
    {
        public List<role> lstRole { get; set; }
        public List<ObPmsView> lstObjPmsView { get; set; }
    }
}