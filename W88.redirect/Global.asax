<%@ Application Language="C#" %>

<script runat="server">

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
            if (xeMapRoute.Attribute("absoluteURL") != null) { if (!string.IsNullOrEmpty(xeMapRoute.Attribute("absoluteURL").Value)) { rvdRoutes.Add("absoluteURL", xeMapRoute.Attribute("absoluteURL").Value); } }

            rtElement = new System.Web.Routing.Route(xeMapRoute.Value, new System.Web.Routing.PageRouteHandler("~/Default.aspx"));
            rtElement.DataTokens = rvdRoutes;
            routes.Add(rtElement);
        }
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
