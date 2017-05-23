using System;
using Models;

public partial class v2_Funds : FundsBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Dashboard);
        base.OnLoad(e);
    }
}