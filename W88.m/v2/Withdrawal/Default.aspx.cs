using System;

public partial class v2_Withdrawal_Default : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
    }

    protected override void OnLoad(EventArgs e)
    {
        Page.Title = commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML);
        Page.Items.Add("Parent", "/v2/Funds.aspx");
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}