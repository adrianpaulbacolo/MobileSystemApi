<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>
<%@ Import Namespace="W88.Utilities" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes(RouteTable.Routes);
        GlobalConfiguration.Configuration.EnsureInitialized();
    }

    void RegisterRoutes(RouteCollection routes)
    {
        var doc = XDocument.Load(Server.MapPath("~/") + @"/App_Data/MapRoutes.xml");
        routes.RouteExistingFiles = true;

        if (doc.Root != null)
        {
            foreach (var xeMapRoute in doc.Root.Elements())
            {
                var routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("reroute", xeMapRoute.Value);
                if (xeMapRoute.Attribute("AbsoluteURL") != null && !string.IsNullOrEmpty(xeMapRoute.Attribute("AbsoluteURL").Value))
                {
                    routes.MapPageRoute(Convert.ToString(xeMapRoute.Name), Convert.ToString(xeMapRoute.Value), Convert.ToString(xeMapRoute.Attribute("AbsoluteURL").Value));
                }
            }
        }

        var logoutRoute = new Route("Logout", new PageRouteHandler("~/Default.aspx"));
        logoutRoute.DataTokens = new RouteValueDictionary { { "logout", "true" } };

        var expireRoute = new Route("Expire", new PageRouteHandler("~/Default.aspx"));
        expireRoute.DataTokens = new RouteValueDictionary { { "expire", "true" } };

        var invalidRoute = new Route("Invalid", new PageRouteHandler("~/Default.aspx"));
        invalidRoute.DataTokens = new RouteValueDictionary { { "invalid", "true" } };

        routes.Add(logoutRoute);
        routes.Add(expireRoute);
        routes.Add(invalidRoute);

        var error404 = new Route("404", new PageRouteHandler("~/_Static/Pages/404.aspx"));
        var error500 = new Route("500", new PageRouteHandler("~/_Static/Pages/500.aspx"));
        routes.Add(error404);
        routes.Add(error500);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started   
        var isSsl = HttpContext.Current.Request.IsSecureConnection.Equals(true);
        var isHttpsOnly = Common.GetAppSetting<string>("httpsOnly").Equals("1");
        if (isHttpsOnly && !isSsl)
        {
            Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl, false);
        }
        
        if (!string.IsNullOrWhiteSpace(LanguageHelpers.SelectedLanguage)) return;
        var pageHeaders = new W88.Utilities.Geo.Location().CheckCdn(HttpContext.Current.Request);
        if (!string.IsNullOrWhiteSpace(pageHeaders.Cdn) && !string.IsNullOrWhiteSpace(pageHeaders.Key))
        {
            LanguageHelpers.SelectedLanguage = pageHeaders.CountryCode;
        }
        else
        {
            var myUri = new Uri(HttpContext.Current.Request.Url.ToString());
            var host = myUri.Host.Split('.');
            if (host.Count() > 1)
            {
                LanguageHelpers.SelectedLanguage = LanguageHelpers.GetLanguageByDomain("." + host[1] + "." + host[2]);
            }
            else
            {
                LanguageHelpers.SelectedLanguage = LanguageHelpers.GetLanguageByDomain("default");
            }
        }
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_PostAuthorizeRequest()
    {
        if (IsWebApiRequest())
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
    }

    private static bool IsWebApiRequest()
    {
        return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/api");
    }
</script>
