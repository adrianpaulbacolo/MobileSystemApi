<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_Promotion" %>

using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

public class AjaxHandlers_ASHX_Promotion : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        var path = string.Format("promotions.{0}.htm", commonVariables.SelectedLanguage + ((string.Compare(commonVariables.GetSessionVariable("CountryCode"), "my", true) == 0) && commonVariables.SelectedLanguage == "en-us" ? ".my" : ""));
        path = context.Server.MapPath("~/_static/promotions/") + path;
        string selectedLanguage = commonVariables.SelectedLanguage + ((string.Compare(commonVariables.GetSessionVariable("CountryCode"), "my", true) == 0) && commonVariables.SelectedLanguage == "en-us" ? ".my" : "");

        List<string> list = System.IO.Directory.GetFiles(context.Server.MapPath("~/_static/promotions/"), string.Format("promotion*{0}.htm", selectedLanguage)).ToList();
        string pattern = @"promotions\.(\d{8})\." + commonVariables.SelectedLanguage;
        var newList = list.Where(x => System.Text.RegularExpressions.Regex.Matches(x, pattern).Count > 0)
                            .Where(x => int.Parse(System.Text.RegularExpressions.Regex.Match(x, pattern).Groups[1].Value) <= int.Parse(DateTime.Now.ToString("yyyyMMdd")))
                            .OrderByDescending(x => x);
        if (!string.IsNullOrEmpty(newList.FirstOrDefault()))
        {
            path = newList.FirstOrDefault();
        }

        string replacePathText = System.IO.File.ReadAllText(path);

        context.Response.ContentType = "text/HTML";
        context.Response.Write(replacePathText);
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}