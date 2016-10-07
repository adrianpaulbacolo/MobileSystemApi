using System;
using System.Text;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;

public partial class _Index : CatalogueBasePage
{
    protected async void Page_Load(object sender, EventArgs e)
    {
        SetLabels();
        #region Catalogue          
        Listview1.DataSource = await RewardsHelper.GetCatalogueSet(MemberSession);
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
        var pointsLabelText = RewardsHelper.GetTranslation(TranslationKeys.Label.Points);
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(pointsLabelText)
            .Append(": ")
            .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPoints) : "0");
        pointsLabel.InnerText = stringBuilder.ToString();

        var pointLevelLabelText = RewardsHelper.GetTranslation(TranslationKeys.Label.PointLevel);
        stringBuilder = new StringBuilder();
        stringBuilder.Append(pointLevelLabelText)
            .Append(" ")
            .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPointLevel) : "0");
        pointLevelLabel.InnerText = stringBuilder.ToString();
            divLvl.Visible = true;
        }
        #endregion
}
