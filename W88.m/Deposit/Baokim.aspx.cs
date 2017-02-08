using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Deposit_Baokim : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.Baokim);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.Baokim);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblDepositAmount.Text = base.strlblAmount;
        }
    }
}