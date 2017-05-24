using System;
using Models;

public partial class v2_V_Sports : BasePage
{
    protected string VBasketballUrl = W88Constant.PageNames.Login;
    protected string VFootballlUrl = W88Constant.PageNames.Login;

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Sports);

        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            VBasketballUrl = commonVSports.getSportsbookUrlBasketball;
            VFootballlUrl = commonVSports.getSportsbookUrlFootball;
        }

        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}