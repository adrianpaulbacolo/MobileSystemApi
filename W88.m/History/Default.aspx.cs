using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _History_Default : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        InitializeWalletBalance();
    }

    public void InitializeWalletBalance()
    {
        string[] keys = { "0"};

        for (int x = 0; x < keys.Length; x++)
        {
            getWalletBalance(keys[x]);
        }
    }



    private class Wallet
    {
        public const string MAIN = "0";
    }



    public void getWalletBalance(string walletId)
    {
        string strOperatorId = commonVariables.OperatorId;
        string strMemberCode = string.Empty;
        string strSiteUrl = commonVariables.SiteUrl;

        string processCode = string.Empty;
        string processText = string.Empty;

        string strWalletId = string.Empty;
        string strWalletAmount = string.Empty;
        string strProductCurrency = string.Empty;

        strWalletId = walletId;

        strMemberCode = commonVariables.GetSessionVariable("MemberCode");

        if (!string.IsNullOrEmpty(strMemberCode) && !string.IsNullOrEmpty(strOperatorId))
        {
            using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
            {
                strWalletAmount = svcInstance.getWalletBalance(strOperatorId, strSiteUrl, strMemberCode, strWalletId, out strProductCurrency);
            }
        }
        else { strWalletAmount = "0"; }

        if (walletId == Wallet.MAIN)
        {
            Session["MAIN"] = strWalletAmount;
        }
    }
}
