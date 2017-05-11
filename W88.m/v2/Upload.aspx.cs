using System;

public partial class v2_Upload : BasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Dashboard.aspx");
        base.OnLoad(e);
    }
}