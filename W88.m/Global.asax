<%@ Application Language="C#" %>
<%@ Import Namespace="customConfig" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);

      
    }

    void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath(@"~/App_Data/MapRoutes.xml"));
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

        routes.MapPageRoute("download", "v2/Downloads", "~/v2/Downloads.aspx");
        routes.MapPageRoute("downloads", "v2/Downloads/{item}", "~/v2/DownloadItem.aspx");

        System.Web.Routing.Route rtLogout = new System.Web.Routing.Route("Logout", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtLogout.DataTokens = new System.Web.Routing.RouteValueDictionary { { "logout", "true" } };

        System.Web.Routing.Route rtExpire = new System.Web.Routing.Route("Expire", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtExpire.DataTokens = new System.Web.Routing.RouteValueDictionary { { "expire", "true" } };

        System.Web.Routing.Route rtInvalid = new System.Web.Routing.Route("Invalid", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtInvalid.DataTokens = new System.Web.Routing.RouteValueDictionary { { "invalid", "true" } };

        routes.Add(rtLogout);
        routes.Add(rtExpire);
        routes.Add(rtInvalid);

        // WINGMONEY
        System.Web.Routing.Route wingmoney_dep = new System.Web.Routing.Route("Deposit/110308", new System.Web.Routing.PageRouteHandler("~/Deposit/MoneyTransfer.aspx"));
        wingmoney_dep.DataTokens = new System.Web.Routing.RouteValueDictionary { { "money", "wing" } };
        System.Web.Routing.Route wingmoney_with = new System.Web.Routing.Route("Withdrawal/210709", new System.Web.Routing.PageRouteHandler("~/Withdrawal/MoneyTransfer.aspx"));
        wingmoney_with.DataTokens = new System.Web.Routing.RouteValueDictionary { { "money", "wing" } };

        routes.Add(wingmoney_dep);
        routes.Add(wingmoney_with);

        // TRUEMONEY
        System.Web.Routing.Route truemoney_dep = new System.Web.Routing.Route("Deposit/1103132", new System.Web.Routing.PageRouteHandler("~/Deposit/MoneyTransfer.aspx"));
        truemoney_dep.DataTokens = new System.Web.Routing.RouteValueDictionary { { "money", "true" } };
        System.Web.Routing.Route truemoney_with = new System.Web.Routing.Route("Withdrawal/2107138", new System.Web.Routing.PageRouteHandler("~/Withdrawal/MoneyTransfer.aspx"));
        truemoney_with.DataTokens = new System.Web.Routing.RouteValueDictionary { { "money", "true" } };
        
        routes.Add(truemoney_dep);
        routes.Add(truemoney_with);

        // TONGHUI
        System.Web.Routing.Route tonghui_pay = new System.Web.Routing.Route("Deposit/120275", new System.Web.Routing.PageRouteHandler("~/Deposit/Tonghui.aspx"));
        tonghui_pay.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "pay" } };
        System.Web.Routing.Route tonghui_aliPay = new System.Web.Routing.Route("Deposit/120293", new System.Web.Routing.PageRouteHandler("~/Deposit/Tonghui.aspx"));
        tonghui_aliPay.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "alipay" } };
        System.Web.Routing.Route tonghui_weChat = new System.Web.Routing.Route("Deposit/120277", new System.Web.Routing.PageRouteHandler("~/Deposit/Tonghui.aspx"));
        tonghui_weChat.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "wechat" } };

        routes.Add(tonghui_pay);
        routes.Add(tonghui_aliPay);
        routes.Add(tonghui_weChat);

        // JTPAY
        System.Web.Routing.Route weChat = new System.Web.Routing.Route("Deposit/1202123", new System.Web.Routing.PageRouteHandler("~/Deposit/JTPay.aspx"));
        weChat.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "wechat" } };
        System.Web.Routing.Route aliPay = new System.Web.Routing.Route("Deposit/1202122", new System.Web.Routing.PageRouteHandler("~/Deposit/JTPay.aspx"));
        aliPay.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "alipay" } };

        routes.Add(weChat);
        routes.Add(aliPay);

        // AIFU
        System.Web.Routing.Route aifuWeChat = new System.Web.Routing.Route("Deposit/1202133", new System.Web.Routing.PageRouteHandler("~/Deposit/Aifu.aspx"));
        aifuWeChat.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "wechat" } };
        System.Web.Routing.Route aifuAliPay = new System.Web.Routing.Route("Deposit/1202134", new System.Web.Routing.PageRouteHandler("~/Deposit/Aifu.aspx"));
        aifuAliPay.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "alipay" } };

        routes.Add(aifuWeChat);
        routes.Add(aifuAliPay);

        // ALIPAY TRANSFER
        System.Web.Routing.Route alipaytransfer = new System.Web.Routing.Route("Deposit/1204131", new System.Web.Routing.PageRouteHandler("~/Deposit/SDAPay.aspx"));
        alipaytransfer.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "alipaytransfer" } };
        System.Web.Routing.Route alipaytransfer2 = new System.Web.Routing.Route("Deposit/1204131/2", new System.Web.Routing.PageRouteHandler("~/Deposit/SDAPay2.aspx"));
        alipaytransfer2.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "alipaytransfer" } };

        routes.Add(alipaytransfer);
        routes.Add(alipaytransfer2);

        // SDAPAYALIPAY
        System.Web.Routing.Route sdapayalipay = new System.Web.Routing.Route("Deposit/120254", new System.Web.Routing.PageRouteHandler("~/Deposit/SDAPay.aspx"));
        sdapayalipay.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "sdapayalipay" } };
        System.Web.Routing.Route sdapayalipay2 = new System.Web.Routing.Route("Deposit/120254/2", new System.Web.Routing.PageRouteHandler("~/Deposit/SDAPay2.aspx"));
        sdapayalipay2.DataTokens = new System.Web.Routing.RouteValueDictionary { { "type", "sdapayalipay" } };

        routes.Add(sdapayalipay);
        routes.Add(sdapayalipay2);

        // ERRORS
        System.Web.Routing.Route rtError400 = new System.Web.Routing.Route("400", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/400.aspx"));
        System.Web.Routing.Route rtError403 = new System.Web.Routing.Route("403", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/403.aspx"));
        System.Web.Routing.Route rtError404 = new System.Web.Routing.Route("404", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/404.aspx"));
        System.Web.Routing.Route rtError408 = new System.Web.Routing.Route("408", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/408.aspx"));
        System.Web.Routing.Route rtError500 = new System.Web.Routing.Route("500", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/500.aspx"));
        System.Web.Routing.Route rtError502 = new System.Web.Routing.Route("502", new System.Web.Routing.PageRouteHandler("~/_Static/Pages/502.aspx"));

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

    private void Session_Start(object sender, EventArgs e)
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
