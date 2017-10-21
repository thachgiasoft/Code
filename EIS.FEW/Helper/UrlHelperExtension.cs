using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DevExpress.Utils.OAuth.Provider;

//using DevExpress.Utils.OAuth.Provider;

namespace EIS.FEW.HtmlHelpers
{
     public static class UrlHelperExtension
     {
         /// <summary>
         /// Description: Modules the URL.
         /// </summary>
         /// <param name="urlHelper">The URL helper.</param>
         /// <param name="url">The URL.</param>
         /// <returns>System.String.</returns>
        public static string ModuleUrl(this UrlHelper urlHelper,string url)
        {
            return ResolveUrl(url);
        }

        public static string ResolveUrl(this UrlHelper urlHelper, string url, bool isExternalUrl = false)
        {
            if (isExternalUrl)
            {
                return url;
            }
            return ResolveUrl(url);
        }

        public static string ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            url = url.TrimStart('/');
            return BaseUrl() + url;
        }

        /// <summary>
        /// Description: Bases the URL.
        /// </summary>
        /// <returns>System.String.</returns>
         public static string BaseUrl()
         {
             string resolveUrl;
             var applicationPath = HttpContext.Current.Request.ApplicationPath + "";
             if (applicationPath == "/")
             {
                 resolveUrl = applicationPath;
             }
             else
             {
                 if (!applicationPath.EndsWith("/"))
                 {
                     applicationPath += "/";
                 }
                 resolveUrl = applicationPath;
             }
             return resolveUrl;
         }
         public static string LogOn(this UrlHelper urlHelper, string returnUrl)
         {
             if (!string.IsNullOrEmpty(returnUrl))
                 return urlHelper.Action("Login", "Account", new { ReturnUrl = returnUrl });
             return urlHelper.Action("Login", "Account");
         }

         public static string LogOff(this UrlHelper urlHelper, string returnUrl)
         {
             if (!string.IsNullOrEmpty(returnUrl))
                 return urlHelper.Action("LogOff", "Account", new { ReturnUrl = returnUrl });
             return urlHelper.Action("LogOff", "Account");
         }
      

    }
}
