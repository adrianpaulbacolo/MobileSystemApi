using System;

public partial class v2_Withdrawal_Pending : FundsBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Funds.aspx");
        base.OnLoad(e);
    }
}