using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EIS.FEW.Models
{
    public class RoleModel
    {
        public IEnumerable<SelectListItem> CheckboxList { get; set; }
        public List<IdentityManagement.Domain.role> RoleList { get; set; }
    }
}