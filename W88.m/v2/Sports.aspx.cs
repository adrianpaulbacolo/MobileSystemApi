using System;
using System.CodeDom.Compiler;
using Models;

public partial class v2_Sports : BasePage
{
    protected string ASportsUrl = W88Constant.PageNames.Login;
    protected string ESportsUrl = W88Constant.PageNames.Login;
    protected string XSportsUrl = W88Constant.PageNames.Login;
    protected bool DisplayXSports = false;
    protected int DeviceId = 0;

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Dashboard);
        DeviceId = commonFunctions.getMobileDevice(Request);

        if (!string.IsNullOrWhiteSpace(commonCookie.CookieCurrency))
        {
            if ((!commonCookie.CookieCurrency.Equals("rmb", StringComparison.OrdinalIgnoreCase)) &&
                (!commonCookie.CookieCurrency.Equals("usd", StringComparison.OrdinalIgnoreCase)))
            {
                DisplayXSports = true;
            }
        }
        else if (!commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase))
        {
            DisplayXSports = true;
        }

        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            ASportsUrl = commonASports.getSportsbookUrl;
            ESportsUrl = commonESports.getSportsbookUrl;
            XSportsUrl = commonXSports.SportsBookUrl;
        }

        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}