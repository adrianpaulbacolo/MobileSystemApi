using System;
using System.Text;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Shared.Helpers;
using W88.Rewards.BusinessLogic.Rewards.Helpers;


public partial class _Index : BasePage
{
    protected XElement XeErrors = null;
    protected XElement LeftMenu = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        var localResx = "~/default.{0}.aspx";
        var headerResx = "~/rewards.header.{0}.aspx";
        var language = HttpContext.Current.Request.QueryString["lang"];

        if (!string.IsNullOrEmpty(language))
        {
            localResx = string.Format(localResx, language);
            headerResx = string.Format(headerResx, language);
            LanguageHelpers.SelectedLanguage = language;
        }
        else
        {
            localResx = string.Format(localResx, LanguageHelpers.SelectedLanguage);
            headerResx = string.Format(headerResx, LanguageHelpers.SelectedLanguage);          
        }

        XeErrors = CultureHelpers.AppData.GetRootResource("Errors");
        LeftMenu = CultureHelpers.AppData.GetRootResource("leftMenu");

        #region labels
        if (HasSession && UserSessionInfo != null)
        {
            if (string.IsNullOrEmpty(MemberSession.FirstName))
            {
                usernameLabel.Visible = false;
            }
            else
            {
                usernameLabel.InnerText = UserSessionInfo.MemberCode;
            }
            var pointsLabelText = (string) HttpContext.GetLocalResourceObject(localResx, "lbl_points");
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(pointsLabelText)
                .Append(": ")
                .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPoints) : "0");
            pointsLabel.InnerText = stringBuilder.ToString();

            var pointLevelLabelText = (string) HttpContext.GetLocalResourceObject(headerResx, "lbl_point_level");
            stringBuilder = new StringBuilder();
            stringBuilder.Append(pointLevelLabelText)
                .Append(" ")
                .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPointLevel) : "0");
            pointLevelLabel.InnerText = stringBuilder.ToString();
            divLvl.Visible = true;
        }
        #endregion

        #region Catalogue          
        Listview1.DataSource = (new RewardsHelper()).GetCatalogueSet(MemberSession);
        Listview1.DataBind();
        #endregion
    }
}
