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
        string[] keys = { "0", "2", "7", "1", "3", "4", "6", "13", "12", "8" };

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
        public const string PMAHJONG = "8";
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
        else if (walletId == Wallet.PMAHJONG)
        {
            Session["PMAHJONG"] = strWalletAmount;
        }
    }

    protected void aSportsBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "2";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void eSportsBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "4";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void wSportsBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "3";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void lotteryBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "7";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void casinoBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "5";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void nuovoBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "0";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void clubPalazzoBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "6";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void pokerBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "8";
        Response.Redirect("FundTransfer/Default.aspx");
    }
    protected void texasmahjongBtn_Click(object sender, EventArgs e)
    {
        Session["Wallet"] = "9";
        Response.Redirect("FundTransfer/Default.aspx");
    }
}