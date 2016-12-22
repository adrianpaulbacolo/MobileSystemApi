using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wsMemberMS1;

public partial class Profile_RebateWeeklyClaim : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        lblMemberCode.Text = userInfo.MemberCode;
    }
    public string check_promo_code(string code)
    {
        try
        {
            using (var client = new memberWSSoapClient())
            {
                return client.CheckClaimedRebateCodes(long.Parse(userInfo.MemberId), code);
            }
        }
        catch (Exception)
        {
            return "";
        }


    }
}