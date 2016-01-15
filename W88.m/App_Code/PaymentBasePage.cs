using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

/// <summary>
/// Summary description for PaymentBasePage
/// </summary>
public class PaymentBasePage : BasePage
{
    protected bool IsPageRefresh = false;
    protected XElement xeResources = null;

    protected string strOperatorId = commonVariables.OperatorId;
    protected string strMemberCode = commonVariables.GetSessionVariable("MemberCode");
    protected string strMemberID = commonVariables.GetSessionVariable("memberId");
    protected string strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");
    protected string strRiskId = commonVariables.GetSessionVariable("RiskId");
    protected string strPaymentGroup = commonVariables.GetSessionVariable("PaymentGroup");
    protected string strSelectedLanguage = commonVariables.SelectedLanguage;
    protected string strSiteUrl = commonVariables.SiteUrl;

    protected XElement xeErrors = commonVariables.ErrorsXML;
    protected string strMethodsUnAvailable;

    protected string strMinLimit;
    protected string strMaxLimit;
    protected string strTotalAllowed;
    protected string strDailyLimit;

    protected string strMethodId;
    protected string paymentMethodId;


    protected void CancelUnexpectedRePost()
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
        }
        else
        {
            if (string.IsNullOrEmpty(ViewState["postids"] as string))
            {
                IsPageRefresh = true;
            }
            else
            {
                if (string.IsNullOrEmpty(Session["postid"] as string))
                {
                    IsPageRefresh = true;
                }
                else if (ViewState["postids"].ToString() != Session["postid"].ToString())
                {
                    IsPageRefresh = true;
                }
            }

            Session["postid"] = System.Guid.NewGuid().ToString();
            ViewState["postids"] = Session["postid"];
        }
    }

    protected void InitialisePaymentLimits()
    {
        string strProcessCode = string.Empty;
        string strProcessText = string.Empty;

        DataTable dtPaymentMethodLimits = null;
        DataRow drPaymentMethodLimit = null;

        StringBuilder sbMethodsUnavailable = new StringBuilder();

        strMethodId = "0";

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Deposit)), false, out strProcessCode, out strProcessText);
        }

        foreach (commonVariables.DepositMethod EnumMethod in Enum.GetValues(typeof(commonVariables.DepositMethod)))
        {
            if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
            {
                sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
            }
        }

        strMethodId = paymentMethodId;

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            strMinLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]) <= 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]).ToString(commonVariables.DecimalFormat);
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }

    protected void getMainWalletBalance(string walletId)
    {
        string strProductCurrency = string.Empty;

        if (!string.IsNullOrEmpty(strMemberCode) && !string.IsNullOrEmpty(strOperatorId))
        {
            using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
            {
                Session["MAIN"] = svcInstance.getWalletBalance(strOperatorId, strSiteUrl, strMemberCode, walletId, out strProductCurrency);
            }
        }
        else
        {
            Session["MAIN"] = "0.00";
        }
    }
}