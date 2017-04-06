using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_MoneyTransfer : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        var money = this.RouteData.DataTokens["money"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        switch (money)
        {
            case "wing":
                base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.WingMoney);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.WingMoney);
                break;
            case "true":
                base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.TrueMoney);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.TrueMoney);
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(string.Concat(V2WithdrawalPath, "Pay", PaymentMethodId, ".aspx"));
            base.InitialisePendingWithdrawals(sender.ToString().Contains("app"));
        }
    }
}