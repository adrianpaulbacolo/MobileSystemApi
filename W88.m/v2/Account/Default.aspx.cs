using System;
using Models;

public partial class v2_Account_Default : BasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Dashboard);
        base.OnLoad(e);
    }

    protected override void OnPreInit(EventArgs e)
    {
        this.isPublic = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}