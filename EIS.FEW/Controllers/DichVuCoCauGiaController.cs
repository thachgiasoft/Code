using EIS.FEW.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIS.FEW.Controllers
{
    public class DichVuCoCauGiaController : Controller
    {
        //
        // GET: /DichVuCoCauGia/
        private static readonly ILog log = LogManager.GetLogger(typeof(DichVuCoCauGiaController));

        public ActionResult Index()
        {
            var model = new COCAUGIA_DICHVU();
            model.Filter = new COCAUGIA_DICHVU_FILTER();
            try
            {
            }
            catch (Exception e)
            {
                log.ErrorFormat("Index - message: {0}{1} - {2}", e.Message, Environment.NewLine, e.StackTrace);
            }
            return View(model);
        }
    }
}
