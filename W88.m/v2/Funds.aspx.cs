using System;
using Helpers;

public partial class v2_Funds : FundsBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", Pages.Dashboard);
        base.OnLoad(e);
    }
}