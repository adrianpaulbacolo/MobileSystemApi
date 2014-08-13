<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_GetPromos" %>

using System;
using System.Web;
using System.Xml.Linq;
using System.Linq;

public class AjaxHandlers_ASHX_GetPromos : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest(HttpContext context)
    {
        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("Promos", out xeResources);

        context.Response.ContentType = "text/xml";

        System.Xml.Linq.XElement xePromos = new System.Xml.Linq.XElement("promos");
        string strFileId = string.Empty;
        int intFileId = int.MinValue;
        int intNextId = int.MinValue;
        string strPageSize = string.Empty;
        
        strFileId = context.Request.Form.Get("fileId");
        intFileId = string.IsNullOrEmpty(strFileId) ? 0 : Convert.ToInt32(strFileId);
        strPageSize = opSettings.Values.Get("IndexPromoPageSize");
        
        intNextId = intFileId + Convert.ToInt32(strPageSize);

        if (xeResources.Elements().Count() > intFileId)
        {
            while (intFileId < intNextId)
            {
                xePromos.Add(xeResources.Elements().ElementAt(intFileId));
                intFileId++;
                if (intFileId >= xeResources.Elements().Count()) { break; }
            }

            if (intFileId >= xeResources.Elements().Count()) { intFileId = -1; }
        }
        else { intFileId = -1; }

        xePromos.Add(new System.Xml.Linq.XElement("nextIndex", intFileId));

        context.Response.Write(xePromos);
        context.Response.End();
        
    }
    private void ProcessRequest2(HttpContext context)
    {
        string strFileId = context.Request.Form.Get("fileId");
        string[] arrStrFiles = System.IO.Directory.GetFiles(context.Server.MapPath("/") + @"_Static\Images\Promos\en-us\");
        System.Xml.Linq.XElement xePromos = new System.Xml.Linq.XElement("promos");
        int intFileUpper = (string.IsNullOrEmpty(strFileId) ? 0 : Convert.ToInt32(strFileId)) + 2;
        for (int intFileId = string.IsNullOrEmpty(strFileId) ? 0 : Convert.ToInt32(strFileId); intFileId <= intFileUpper; intFileId++)
        {
            if (intFileId >= arrStrFiles.Length) { break; }
            xePromos.Add(new System.Xml.Linq.XElement("promo", arrStrFiles[intFileId].Replace(context.Server.MapPath("/"), @"\"), new System.Xml.Linq.XAttribute("id", intFileId + 1)));
        }

        string strNextIndex = Convert.ToString(Convert.ToInt32(strFileId) + 3);
        if (Convert.ToInt32(strNextIndex) >= arrStrFiles.Length) { strNextIndex = "-1"; }
        xePromos.Add(new System.Xml.Linq.XElement("nextIndex", strNextIndex));
        context.Response.ContentType = "text/xml";
        context.Response.Write(xePromos.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}