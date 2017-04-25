using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Withdrawal_Neteller : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.Neteller);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.Neteller);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(string.Concat(V2WithdrawalPath, "Pay", PaymentMethodId, ".aspx"));
            base.InitialisePendingWithdrawals(sender.ToString().Contains("app"));
            InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        XElement _xeRegisterResources;
        commonCulture.appData.getRootResource("/_Secure/Register.aspx", out _xeRegisterResources);

        lblAmount.Text = base.strlblAmount;

        lblAccountName.Text = "Neteller " + commonCulture.ElementValues.getResourceString("lblUsername", _xeRegisterResources);

        lblTransactionId = base.strlblTransactionId;
    }
}