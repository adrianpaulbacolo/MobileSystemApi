using System;
using System.Text;
using System.Web;

public partial class _Index : CatalogueBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetLabels();
        #region Catalogue          
        Listview1.DataSource = RewardsHelper.GetCatalogueSet(MemberSession);
        Listview1.DataBind();
        #endregion
    }

    protected override void SetLabels()
    {
        #region labels
        if (!HasSession)
        {
            return;
        }

        if (string.IsNullOrEmpty(MemberSession.FirstName))
        {
            usernameLabel.Visible = false;
        }
        else
        {
            usernameLabel.InnerText = UserSessionInfo.MemberCode;
        }
        var pointsLabelText = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_points");
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(pointsLabelText)
            .Append(": ")
            .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPoints) : "0");
        pointsLabel.InnerText = stringBuilder.ToString();

        var pointLevelLabelText = (string)HttpContext.GetLocalResourceObject(HeaderResx, "lbl_point_level");
        stringBuilder = new StringBuilder();
        stringBuilder.Append(pointLevelLabelText)
            .Append(" ")
            .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPointLevel) : "0");
        pointLevelLabel.InnerText = stringBuilder.ToString();
            divLvl.Visible = true;
        }
        #endregion
}
