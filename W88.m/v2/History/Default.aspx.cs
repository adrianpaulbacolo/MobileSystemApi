using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class History_Default : BasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Title = "History";
        Page.Items.Add("Parent", Pages.Funds);
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