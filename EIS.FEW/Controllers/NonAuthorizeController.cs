using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIS.FEW.Controllers
{
  //  [Authorize]
    public class NonAuthorizeController : Controller
    {
        //
        // GET: /NonAuthorize/

        public ActionResult Index()
        {
            return View();
        }

    }
}
