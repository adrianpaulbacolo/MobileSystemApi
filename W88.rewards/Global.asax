<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="W88.API" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes(RouteTable.Routes);
        GlobalConfiguration.Configure(WebApiConfig.Register);
        GlobalConfiguration.Configuration.EnsureInitialized();
    }

    void RegisterRoutes(RouteCollection routes)
    {
        XDocument doc = XDocument.Load(Server.MapPath("~/") + @"/App_Data/MapRoutes.xml");
        routes.RouteExistingFiles = true;

        foreach (XElement xeMapRoute in doc.Root.Elements())
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("reroute", xeMapRoute.Value);
            if (xeMapRoute.Attribute("AbsoluteURL") != null && !string.IsNullOrEmpty(xeMapRoute.Attribute("AbsoluteURL").Value))
            {
                routes.MapPageRoute(Convert.ToString(xeMapRoute.Name), Convert.ToString(xeMapRoute.Value), Convert.ToString(xeMapRoute.Attribute("AbsoluteURL").Value));              
            }
        }

        Route logoutRoute = new Route("Logout", new PageRouteHandler("~/Default.aspx"));
        logoutRoute.DataTokens = new RouteValueDictionary { { "logout", "true" } };

        Route expireRoute = new Route("Expire", new PageRouteHandler("~/Default.aspx"));
        expireRoute.DataTokens = new RouteValueDictionary { { "expire", "true" } };

        Route invalidRoute = new Route("Invalid", new PageRouteHandler("~/Default.aspx"));
        invalidRoute.DataTokens = new RouteValueDictionary { { "invalid", "true" } };

        routes.Add(logoutRoute);
        routes.Add(expireRoute);
        routes.Add(invalidRoute);
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
        bool isSsl = HttpContext.Current.Request.IsSecureConnection.Equals(true);
        bool isHttpsOnly = ConfigurationManager.AppSettings.Get("httpsOnly").Equals("1");
        if (isHttpsOnly && !isSsl)
        {
            Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
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
            string[] host = myUri.Host.Split('.');
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
