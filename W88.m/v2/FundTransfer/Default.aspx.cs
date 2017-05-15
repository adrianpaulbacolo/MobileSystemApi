using System;

public partial class v2_FundTransfer_Default : FundsBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Funds.aspx");
        base.OnLoad(e);
    }
}