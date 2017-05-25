using System;
using Helpers;

public partial class v2_Account_ForgotPassword : BasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", Pages.Login);
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}