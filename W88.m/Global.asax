<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
    }

    void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(Server.MapPath("~/") + @"/App_data/MapRoutes.xml");
        System.Web.Routing.Route rtElement = null;
        System.Web.Routing.RouteValueDictionary rvdRoutes = null;

        routes.RouteExistingFiles = true;

        foreach (System.Xml.Linq.XElement xeMapRoute in doc.Root.Elements())
        {
            rvdRoutes = new System.Web.Routing.RouteValueDictionary();
            rvdRoutes.Add("reroute", xeMapRoute.Value);
            if (xeMapRoute.Attribute("AbsoluteURL") != null)
            {
                if (!string.IsNullOrEmpty(xeMapRoute.Attribute("AbsoluteURL").Value))
                {
                    routes.MapPageRoute(Convert.ToString(xeMapRoute.Name), Convert.ToString(xeMapRoute.Value), Convert.ToString(xeMapRoute.Attribute("AbsoluteURL").Value));
                }
            }
        }

        System.Web.Routing.Route rtLogout = new System.Web.Routing.Route("Logout", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtLogout.DataTokens = new System.Web.Routing.RouteValueDictionary { { "logout", "true" } };

        System.Web.Routing.Route rtExpire = new System.Web.Routing.Route("Expire", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtExpire.DataTokens = new System.Web.Routing.RouteValueDictionary { { "expire", "true" } };

        System.Web.Routing.Route rtInvalid = new System.Web.Routing.Route("Invalid", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtInvalid.DataTokens = new System.Web.Routing.RouteValueDictionary { { "invalid", "true" } };
        
        // JTPAY
        System.Web.Routing.Route weChat = new System.Web.Routing.Route("Deposit/WeChat", new System.Web.Routing.PageRouteHandler("~/Deposit/JTPay.aspx"));
        weChat.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "wechat" } };
        System.Web.Routing.Route aliPay = new System.Web.Routing.Route("Deposit/AliPay", new System.Web.Routing.PageRouteHandler("~/Deposit/JTPay.aspx"));
        aliPay.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "alipay" } };

        System.Web.Routing.Route rtError400 = new System.Web.Routing.Route("400", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/400.aspx"));
        System.Web.Routing.Route rtError403 = new System.Web.Routing.Route("403", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/403.aspx"));
        System.Web.Routing.Route rtError404 = new System.Web.Routing.Route("404", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/404.aspx"));
        System.Web.Routing.Route rtError408 = new System.Web.Routing.Route("408", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/408.aspx"));
        System.Web.Routing.Route rtError500 = new System.Web.Routing.Route("500", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/500.aspx"));
        System.Web.Routing.Route rtError502 = new System.Web.Routing.Route("502", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/502.aspx"));

        routes.Add(rtLogout);
        routes.Add(rtExpire);
        routes.Add(rtInvalid);
        routes.Add(weChat);
        routes.Add(aliPay);
        routes.Add(rtError400);
        routes.Add(rtError403);
        routes.Add(rtError404);
        routes.Add(rtError408);
        routes.Add(rtError500);
        routes.Add(rtError502);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        bool isSsl = HttpContext.Current.Request.IsSecureConnection.Equals(true);
        bool isHttpsOnly = System.Configuration.ConfigurationManager.AppSettings.Get("httpsOnly").Equals("1");
        if (isHttpsOnly && !isSsl)
        {
            Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
        }
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
