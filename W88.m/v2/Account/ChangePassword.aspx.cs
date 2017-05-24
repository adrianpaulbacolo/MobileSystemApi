using System;

public partial class v2_Account_ChangePassword : BasePage
{
    protected override void OnPreInit(EventArgs e)
    {
        this.isPublic = false;
    }

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Dashboard.aspx");
        base.OnLoad(e);
    }
}