<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_Promotion" %>

using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

public class AjaxHandlers_ASHX_Promotion : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string countryCode = "";

        if (commonVariables.CDNCountryCode == "MY" && commonVariables.SelectedLanguage == "en-us")
        {
            countryCode = ".my";
        }
        //else if ((string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "aud", true) == 0) && commonVariables.SelectedLanguage == "en-us")
        //{
        //    countryCode = ".au";
        //}

        string selectedLanguage = commonVariables.SelectedLanguage + countryCode;

        if (commonVariables.CDNCountryCode == "MY" && commonVariables.SelectedLanguage == "zh-cn")
        {
            selectedLanguage = "zh-my";
        }

        var path = string.Format("promotions.{0}.htm", selectedLanguage);
        path = context.Server.MapPath("~/_static/promotions/") + path;
   
        List<string> list = System.IO.Directory.GetFiles(context.Server.MapPath("~/_static/promotions/"), string.Format("promotion*{0}.htm", selectedLanguage)).ToList();

        string pattern = @"promotions\.(\d{8})\." + commonVariables.SelectedLanguage;
        if (commonVariables.CDNCountryCode == "MY" && commonVariables.SelectedLanguage == "zh-cn")
        {
            pattern = @"promotions\.(\d{8})\." +  selectedLanguage;
        }

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