using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Funds_Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitializeWalletBalance();
    }

    public void InitializeWalletBalance()
    {
        string[] keys = { "0", "2", "7", "1", "3", "4", "6", "13", "12" };

        for (int x = 0; x < keys.Length; x++)
        {
            getWalletBalance(keys[x]);
        }
    }



    private class Wallet
    {
        public const string MAIN = "0";
        public const string ASPORTS = "2";
        public const string LOTTERY = "1";
        public const string CASINO = "3";
        public const string PLAYTECH = "4";
        public const string POKER = "6";
        public const string SBTECH = "7";
        public const string SBO = "13";
        public const string NETENT = "12";
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
        if (walletId == Wallet.ASPORTS)
        {
            Session["ASPORTS"] = strWalletAmount;
        }
        else if (walletId == Wallet.SBO)
        {
            Session["SBO"] = strWalletAmount;
        }
        else if (walletId == Wallet.CASINO)
        {
            Session["CASINO"] = strWalletAmount;
        }
        else if (walletId == Wallet.PLAYTECH)
        {
            Session["PLAYTECH"] = strWalletAmount;
        }
        else if (walletId == Wallet.SBTECH)
        {
            Session["SBTECH"] = strWalletAmount;
        }
        else if (walletId == Wallet.LOTTERY)
        {
            Session["LOTTERY"] = strWalletAmount;
        }
        else if (walletId == Wallet.NETENT)
        {
            Session["NETENT"] = strWalletAmount;
        }
        else if (walletId == Wallet.POKER)
        {
            Session["POKER"] = strWalletAmount;
        }
    }
}