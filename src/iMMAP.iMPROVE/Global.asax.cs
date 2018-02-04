using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace iMMAP.iMPROVE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (!string.IsNullOrEmpty(authTicket.UserData))
                {
                    string[] parts = authTicket.UserData.Split('=');
                    if (parts.Length == 2 && parts[0] == "Provider")
                    {
                        RolePrincipal newRolePrincipal = new RolePrincipal(parts[1], HttpContext.Current.User.Identity);
                        HttpContext.Current.User = newRolePrincipal;
                        System.Threading.Thread.CurrentPrincipal = newRolePrincipal;
                    }
                }
            }
        }
    }
}
