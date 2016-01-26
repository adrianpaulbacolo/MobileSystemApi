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
    protected XElement xeResources = null;
    protected XElement xeErrors = null;
    protected XElement xeResponse = null;
    
    /// <summary>
    /// XML Name to use in Translation inside the AppData
    /// </summary>
    protected string PageName { get; set; }
    protected commonVariables.PaymentTransactionType PaymentType { get; set; }
    protected string PaymentMethodId { get; set; }

    protected bool IsPageRefresh = false;

    protected string strOperatorId = string.Empty;
    protected string strMemberCode = string.Empty;
    protected string strMemberID = string.Empty;
    protected string strCurrencyCode = string.Empty;
    protected string strCountryCode = string.Empty;
    protected string strRiskId = string.Empty;
    protected string strPaymentGroup = string.Empty;
    protected string strSelectedLanguage = string.Empty;
    protected string strSiteUrl = string.Empty;

    protected string strMethodsUnAvailable = string.Empty;
    protected string strMethodId = string.Empty;

    protected string strMinLimit = string.Empty;
    protected string strMaxLimit = string.Empty;
    protected string strTotalAllowed = string.Empty;
    protected string strDailyLimit = string.Empty;

    protected string strResultCode = string.Empty;
    protected string strResultDetail = string.Empty;
    protected string strErrorCode = string.Empty;

    protected string strErrorDetail = string.Empty;
    protected int intProcessSerialId = 0;
    protected string strProcessId = Guid.NewGuid().ToString().ToUpper();

    protected bool isSystemError = false;
    protected bool isProcessAbort = false;

    protected void InitialiseVariables()
    {
        strOperatorId = commonVariables.OperatorId;

        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        strMemberID = commonVariables.GetSessionVariable("memberId");

        strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");
        strCountryCode = commonVariables.GetSessionVariable("CountryCode");

        strRiskId = commonVariables.GetSessionVariable("RiskId");

        strPaymentGroup = commonVariables.GetSessionVariable("PaymentGroup");

        strSelectedLanguage = commonVariables.SelectedLanguage;

        strSiteUrl = commonVariables.SiteUrl;

        xeErrors = commonVariables.ErrorsXML;

        if (PaymentType == commonVariables.PaymentTransactionType.Deposit)
            commonCulture.appData.getRootResource("/Deposit/" + PageName, out xeResources);
        else
            commonCulture.appData.getRootResource("/Withdrawal/" + PageName, out xeResources);
    }

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
        if (PaymentType == commonVariables.PaymentTransactionType.Deposit)
            InitialiseDepositPaymentLimits();
        else
            InitialiseWithdrawalPaymentLimits();
    }

    private void InitialiseDepositPaymentLimits()
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

        strMethodId = PaymentMethodId;

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            strMinLimit = Convert.ToDecimal(drPaymentMethodLimit["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(drPaymentMethodLimit["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]) <= 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]).ToString(commonVariables.DecimalFormat);
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }

    private void InitialiseWithdrawalPaymentLimits()
    {
        string strProcessCode = string.Empty;
        string strProcessText = string.Empty;

        DataTable dtPaymentMethodLimits = null;
        DataRow drPaymentMethodLimit = null;

        StringBuilder sbMethodsUnavailable = new StringBuilder();

        strMethodId = "0";

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Withdrawal)), false, out strProcessCode, out strProcessText);
        }

        foreach (commonVariables.WithdrawalMethod EnumMethod in Enum.GetValues(typeof(commonVariables.WithdrawalMethod)))
        {
            if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
            {
                sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
            }
        }

        strMethodId = PaymentMethodId;

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            strMinLimit = Convert.ToDecimal(drPaymentMethodLimit["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(drPaymentMethodLimit["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]) <= 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]).ToString(commonVariables.DecimalFormat);
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }


    protected void GetMainWalletBalance(string walletId)
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

    protected void InitialisePendingWithdrawals()
    {
        string strStatusCode = string.Empty;
        string strStatusText = string.Empty;

        svcPayMember.PendingWithdrawal[] arrPending = null;

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            arrPending = svcInstance.getPendingWithdrawal(Convert.ToInt64(strOperatorId), strMemberCode, out strStatusCode, out strStatusText);

            if (arrPending != null && arrPending.Length > 0)
            {
                if (Request.QueryString["source"] == "app")
                    Response.Redirect("/Withdrawal/Pending_app.aspx");
                else
                    Response.Redirect("/Withdrawal/Pending.aspx");
            }
        }

    }
}