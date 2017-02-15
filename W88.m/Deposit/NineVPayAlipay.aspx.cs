using System;


public partial class Deposit_NineVPayAlipay : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.NineVPayAlipay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.NineVPayAlipay);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(V2DepositPath + "Pay1202105.aspx");
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblDepositAmount.Text = base.strlblAmount;
    }
}