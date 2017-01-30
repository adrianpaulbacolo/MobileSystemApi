using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_WingMoney : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.WingMoney);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.WingMoney);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            base.InitialisePendingWithdrawals(sender.ToString().Contains("app"));
            InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblWithdrawAmount.Text = base.strlblAmount;

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        lblTransactionId = base.strlblTransactionId;
    }
}
