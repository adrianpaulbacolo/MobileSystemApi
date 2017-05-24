using System;
using Models;

public partial class v2_Account_Rebates : BasePage
{
    protected string Username;

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Account);
        base.OnLoad(e);
    }

    protected override void OnPreInit(EventArgs e)
    {
        this.isPublic = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        Username = userInfo.MemberCode;
    }
}