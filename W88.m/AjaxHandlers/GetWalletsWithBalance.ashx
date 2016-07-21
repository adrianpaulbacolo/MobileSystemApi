<%@ WebHandler Language="C#" Class="GetWalletsWithBalance" %>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;

public class GetWalletsWithBalance : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{

    private readonly ICollection<KeyValuePair<int, string>> _balance = new Collection<KeyValuePair<int, string>>();

    public void ProcessRequest(HttpContext context)
    {

        if (!context.Request.ContentType.Contains("json")) return;
        
        var serializer = new JavaScriptSerializer();
            var balances = commonPaymentMethodFunc.GetWalletBalancesAsync();
            var xe = XElement.Parse(balances.Result);
            var balanceSection = xe.Elements("balance");

            foreach (var pair in balanceSection)
            {
            var value = (pair.Value == "*" || pair.Value == "-")
                ? "00.00"
                : string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(pair.Value));
                _balance.Add(new KeyValuePair<int, string>(Convert.ToInt16(pair.Attribute("id").Value), value));
            }

        context.Response.ContentType = "text/json";
        context.Response.Write(serializer.Serialize(_balance));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}