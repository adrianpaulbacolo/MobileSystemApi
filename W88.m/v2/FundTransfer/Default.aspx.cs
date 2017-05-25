using System;
using Helpers;

public partial class v2_FundTransfer_Default : FundsBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", Pages.Funds);
        base.OnLoad(e);
    }
}