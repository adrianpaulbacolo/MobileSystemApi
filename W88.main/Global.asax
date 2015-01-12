<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        maproutes_register(System.Web.Routing.RouteTable.Routes);
    }

    void maproutes_register(System.Web.Routing.RouteCollection routes)
    {
        System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(Server.MapPath("~/") + @"/App_data/maproutes.xml");
        //System.Web.Routing.Route rtElement = null;
        System.Web.Routing.RouteValueDictionary rvdRoutes = null;

        routes.RouteExistingFiles = true;

        foreach (System.Xml.Linq.XElement xeMapRoute in doc.Root.Elements())
        {
            rvdRoutes = new System.Web.Routing.RouteValueDictionary();
            rvdRoutes.Add("reroute", xeMapRoute.Value);
            if (xeMapRoute.Attribute("path") != null)
            {
                string strFilePath = Convert.ToString(xeMapRoute.Attribute("path").Value);
                string strUrlPath = Convert.ToString(xeMapRoute.Value);
                string strRouteId = Convert.ToString(xeMapRoute.Name);
                
                if (!string.IsNullOrEmpty(strFilePath))
                {
                    if (xeMapRoute.Attribute("type") != null && string.Compare(xeMapRoute.Attribute("type").Value, "handler", true) == 0) 
                    {
                        routes.Add(new System.Web.Routing.Route(strUrlPath, new HttpHandlerRouteHandler(strFilePath))); 
                    }
                    else 
                    {
                        routes.MapPageRoute(strRouteId, strUrlPath, strFilePath); 
                    }
                }
            }
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
