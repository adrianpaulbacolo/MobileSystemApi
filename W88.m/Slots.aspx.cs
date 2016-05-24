using System;
using System.Text;
using System.Web.UI;

public partial class Slots : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckSupportedCurrency();

        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML));
    }

    private void CheckSupportedCurrency()
    {
        var currency = commonVariables.GetSessionVariable("CurrencyCode");
        if (!currency.Contains("KRW") && !currency.Contains("VND") && !currency.Contains("IDR"))
        {
            RenderApollo();
        }
    }

    private void RenderApollo()
    {
        var title = commonCulture.ElementValues.getResourceXPathString("Products/ClubApollo/Label", commonVariables.ProductsXML);
        var apollo = new StringBuilder();
        apollo.Append("<li class='col col-33'>");
        apollo.Append("<a href='/ClubApollo' class='card' data-ajax='false'>");
        apollo.Append("<img src='/_Static/Images/bnr-clubapollo.jpg' class='img-responsive'>");
        apollo.Append("<div class='title'>" + title + "</div>");
        apollo.Append("</a></li>");
        ltlApollo.Text = apollo.ToString();
    }
}