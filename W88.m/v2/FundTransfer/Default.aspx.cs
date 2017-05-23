using System;
using Models;

public partial class v2_FundTransfer_Default : FundsBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Funds);
        base.OnLoad(e);
    }
}