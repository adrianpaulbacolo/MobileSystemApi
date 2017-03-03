using System;

public partial class _Secure_VIP_login : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        if (IsVip)
        {
            Response.Redirect("/Index.aspx", false);
        }
    }
}