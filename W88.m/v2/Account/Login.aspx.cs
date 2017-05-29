using System;
using System.Web.UI;
using System.Xml.Linq;

public partial class _v2_Account_Login : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
         if (!Page.IsPostBack)
        {
            vCode.Value = commonVariables.GetSessionVariable("vCode");
        }
    }
}