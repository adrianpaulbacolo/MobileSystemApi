<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
    }

    void RegisterRoutes(System.Web.Routing.RouteCollection routes) 
    {
        routes.RouteExistingFiles = true;
        routes.MapPageRoute("Captcha", "captcha", "~/_Secure/Captcha.aspx");
        routes.MapPageRoute("Forbidden", "forbidden", "~/_Static/forbidden.aspx");
        routes.MapPageRoute("404", "404", "~/_Static/404.aspx");
        routes.MapPageRoute("Enhancement", "enhancement", "~/_Static/enhancement.aspx");
        routes.MapPageRoute("Error", "error", "~/_Static/error.aspx");
        
        routes.MapPageRoute("Login", "_Secure/Login", "~/_Secure/Login.aspx");
        routes.MapPageRoute("MainIndex", "Index", "~/Index.aspx");
        routes.MapPageRoute("Lang", "Lang", "~/Default.aspx");
        routes.MapPageRoute("ContactUs", "ContactUs", "~/ContactUs.aspx");
        routes.MapPageRoute("Promotions", "Promotions", "~/Promotions.aspx");
        routes.MapPageRoute("Slots", "Slots", "~/Slots/Default.aspx");

        System.Web.Routing.Route rtLogout = new System.Web.Routing.Route("Logout", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtLogout.DataTokens = new System.Web.Routing.RouteValueDictionary { { "logout", "true" } };

        System.Web.Routing.Route rtExpire = new System.Web.Routing.Route("Expire", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtExpire.DataTokens = new System.Web.Routing.RouteValueDictionary { { "expire", "true" } };

        System.Web.Routing.Route rtInvalid = new System.Web.Routing.Route("Invalid", new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
        rtInvalid.DataTokens = new System.Web.Routing.RouteValueDictionary { { "invalid", "true" } };

        routes.Add(rtLogout);
        routes.Add(rtExpire);
        routes.Add(rtInvalid);
        //routes.MapPageRoute("Logout", "Logout", "~/Default.aspx?logout=true");
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

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
