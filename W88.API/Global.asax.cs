using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using W88.BusinessLogic;

namespace W88.API
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(System.Web.Routing.RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            routes.RouteExistingFiles = true;

            Route rtError400 = new Route("400", new PageRouteHandler("~/_Static/Pages/400.aspx"));
            Route rtError403 = new Route("403", new PageRouteHandler("~/_Static/Pages/403.aspx"));
            Route rtError404 = new Route("404", new PageRouteHandler("~/_Static/Pages/404.aspx"));
            Route rtError408 = new Route("408", new PageRouteHandler("~/_Static/Pages/408.aspx"));
            Route rtError500 = new Route("500", new PageRouteHandler("~/_Static/Pages/500.aspx"));
            Route rtError502 = new Route("502", new PageRouteHandler("~/_Static/Pages/502.aspx"));

            routes.Add(rtError400);
            routes.Add(rtError403);
            routes.Add(rtError404);
            routes.Add(rtError408);
            routes.Add(rtError500);
            routes.Add(rtError502);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}