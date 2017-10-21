using EIS.FEW;
using FX.Context;
using FX.Utils;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
namespace EIS.FEW
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //AuthConfig.RegisterAuth();
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/") + "config/logging.config"));

            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();
            Bootstrapper.InitializeContainer();
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;

            XmlConfigurator.Configure();
            DevExpress.Data.Helpers.ServerModeCore.DefaultForceCaseInsensitiveForAnySource = true; 

        }
  
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private void GetCurrentSite(HttpContext context)
        {
            IFXContext _FXcontext = FXContext.Current;
            try
            {
                string siteUrl = UrlUtil.GetSiteUrl();
                _FXcontext.PhysicalSiteDataDirectory = Context.Server.MapPath("SiteData");
                string sitepath = FXContext.Current.PhysicalSiteDataDirectory;
            }
            catch (Exception ex)
            {
                log.Error("An unexpected error occured while setting the current site context.", ex);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            GetCurrentSite(HttpContext.Current);
        }
        protected void Application_Error(object sender, EventArgs e) 
        {
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
            //TODO: Handle Exception
            //if (exception is HttpRequestValidationException || exception is ArgumentException)
            //{
            //    Response.Redirect("/Home/PotentiallyError");
            //}
        }
    }
}