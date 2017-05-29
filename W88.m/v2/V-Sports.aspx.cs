using System;
using Helpers;

public partial class v2_V_Sports : BasePage
{
    protected string VBasketballUrl = Pages.Login;
    protected string VFootballlUrl = Pages.Login;

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", Pages.Sports);

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