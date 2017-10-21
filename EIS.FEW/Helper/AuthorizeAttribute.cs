using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace EIS.FEW.Helper
{
    public class RbacAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var requiredPermission = String.Format("{0}_{1}",
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, Roles);

            var requestingUser = new RbacUser(filterContext.RequestContext
                                                   .HttpContext.User.Identity.Name);
            if (String.IsNullOrEmpty(filterContext.RequestContext.HttpContext.User.Identity.Name))
            {
                filterContext.Result = new RedirectToRouteResult(
                                               new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "Account" } });
            }
            if (!requestingUser.HasPermission(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName) &&
                !requestingUser.HasPermission(requiredPermission))// & !requestingUser.User.ISADMIN)
            {
                filterContext.Result = new RedirectToRouteResult(
                                               new RouteValueDictionary { 
                                                { "action", "Index" }, 
                                                { "controller", "Unauthorised" } });
            }
        }
    }
}