using System;
using System.Linq;
using System.Text;
using Helpers;

public partial class Funds : PaymentBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckAgent();
        BuildUiFunds();

        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceString("funds", commonVariables.LeftMenuXML));
    }

    private void BuildUiFunds()
    {
        var obj = new Wallets();

        if (!obj.WalletInfo.Any()) return;

        var builder = new StringBuilder();
        builder.Append("<ul class='row row-uc row-bordered bg-gradient'>");

        foreach (var info in obj.WalletInfo.Where(info => info.Id != 0))
        {
            var curr = commonCookie.CookieCurrency;
            if (!string.IsNullOrWhiteSpace(info.CurrencyLabel))
                curr = info.CurrencyLabel;

            builder.Append("<li class='col col-50'>");
            builder.Append(string.Format("<a class='fundsType' walletid='{0}' href='#'>", info.Id));
            builder.Append("<div class='wallet'>");
            builder.Append(string.Format("<label class='label'>{0}</label>", info.Name));
            builder.Append(string.Format("<h4 class='value' id='{0}'></h4>", info.Id));
            builder.Append(string.Format("<small class='currency'>{0}</small>", curr));
            builder.Append("</div></a></li>");
        }

        builder.Append("</ul>");
        ltlFunds.Text = builder.ToString();
        ltlNote.Text = obj.FundsPageNote;

    }

    private void CheckAgent()
    {
        var userAgent = Request.UserAgent.ToString();
        if (userAgent.ToLower().Contains("clubw"))
        {
            Response.Redirect("/FundsNew.aspx");
        }
    }
}