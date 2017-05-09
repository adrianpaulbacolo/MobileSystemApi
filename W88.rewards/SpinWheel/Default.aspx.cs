using System;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Rewards.Models.SpinWheel;
using W88.Utilities;
using W88.Utilities.Constant;


public partial class SpinWheel_Default : BasePage
{
    protected string SpinWheelRequest
    {
        get
        {
            var request = new SpinWheelRequest();
            request.OperatorId = Encryption.Encrypt(EncryptionType.TripleDESCS, Convert.ToString(Settings.OperatorId));
            request.MemberId = Encryption.Encrypt(EncryptionType.TripleDESCS, Convert.ToString(UserSessionInfo.MemberId));
            request.LanguageCode = Encryption.Encrypt(EncryptionType.TripleDESCS, Language);
            request.MemberCode = Encryption.Encrypt(EncryptionType.TripleDESCS, UserSessionInfo.MemberCode);
            request.RiskId = Encryption.Encrypt(EncryptionType.TripleDESCS, UserSessionInfo.RiskId);
            request.IsMobile = true;
            return Common.SerializeObject(request);
        }
    }

    protected string PointLevelInfo
    {
        get
        {
            return Common.SerializeObject(new PointLevelInfo()
            {
                CurrencyCode = UserSessionInfo.CurrencyCode,
                Language = Language,
                MemberId = UserSessionInfo.MemberId,
                MemberCode = UserSessionInfo.MemberCode
            });
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HasSession)
        {
            Response.Redirect(string.Format("/Default.aspx?lang={0}", Language), true);
        }
        PageTitle = RewardsHelper.GetTranslation(TranslationKeys.SpinWheel.WheelLabel, Language);
    }
}