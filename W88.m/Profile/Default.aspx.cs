using System;
using System.Linq;
using System.Web;

namespace Profile
{
    public partial class ProfileDefault : BasePage 
    {

    protected void Page_Load(object sender, EventArgs e)
    {
            if (Page.IsPostBack) return;

            SetTitle(commonCulture.ElementValues.getResourceString("profile", commonVariables.LeftMenuXML));
        InitializeWalletBalance();
    }

    public void InitializeWalletBalance()
    {
            var wallet = commonPaymentMethodFunc.GetWallets().Where(x => x.Key.Equals(0)).ToList();
            var wId = wallet.Where(x => x.Key.Equals(0)).Select(type => type.Key).ToList()[0];
            commonPaymentMethodFunc.GetWalletBalance(wId);
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
}
