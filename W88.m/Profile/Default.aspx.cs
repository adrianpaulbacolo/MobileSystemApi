using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Profile_Default : BasePage 
{
    protected override void OnPreInit(EventArgs e)
    {
        this.isPublic = false;
    }
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


    public static int getCurrentPoints()
    {
        int total = 0;
        int claim = 0;
        int current = 0;
        int cart = 0;

        try
        {

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["MemberId"]))
            {
                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    string strMemberCode = string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["MemberCode"] as string) ? string.Empty : Convert.ToString(System.Web.HttpContext.Current.Session["MemberCode"]);

                    System.Data.DataSet ds = sClient.getRedemptionDetail(commonVariables.OperatorId, strMemberCode);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                claim = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                            }

                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                cart = int.Parse(ds.Tables[2].Rows[0][0].ToString());
                            }
                            claim = claim + cart;
                        }

                    }
                    current = total - claim;

                    HttpContext.Current.Session["pointsBalance"] = current;

                    return current;

                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            //throw;
            return 0;
        }
    }
}
