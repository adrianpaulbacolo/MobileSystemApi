using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeErrors = null;
        xeErrors = commonVariables.ErrorsXML;
        
        #region Logout
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["logout"]), "true", true) == 0) { commonVariables.ClearSessionVariables(); commonCookie.ClearCookies(); }
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["expire"]), "true", true) == 0) { commonVariables.ClearSessionVariables(); commonCookie.ClearCookies(); txtMessage.InnerHtml = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors).Replace("\\n", "<br />"); }
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["invalid"]), "true", true) == 0) { commonVariables.ClearSessionVariables(); commonCookie.ClearCookies(); txtMessage.InnerHtml = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors).Replace("\\n", "<br />"); }
        #endregion

        if (!Page.IsPostBack) 
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string arrStrLanguageSelection = opSettings.Values.Get("LanguageSelection");
            List<string> lstLanguageSelection = arrStrLanguageSelection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

            foreach (string language in lstLanguageSelection)
            {
                System.Web.UI.HtmlControls.HtmlTableRow trRow = new System.Web.UI.HtmlControls.HtmlTableRow();
                System.Web.UI.HtmlControls.HtmlTableCell tdCell = new System.Web.UI.HtmlControls.HtmlTableCell();

                tdCell.InnerHtml = string.Format("<a href='/_Secure/Login?lang={0}'>{0}</a>", language.Trim());
                trRow.Cells.Add(tdCell);
                tblLanguage.Rows.Add(trRow);
            }
        }
    }
}