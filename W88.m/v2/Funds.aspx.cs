using System;

public partial class v2_Funds : FundsBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Dashboard.aspx");
        base.OnLoad(e);
    }
}