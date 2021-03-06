﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

/// <summary>
/// Summary description for PaymentBasePage
/// </summary>
public class PaymentBasePage : BasePage
{
    #region Properties

    #region XElements
    protected XElement xeResources = null;
    private XElement xeDefaultResources = null;
    protected XElement xeErrors = null;
    protected XElement xeResponse = null;

    protected DataTable autoRouteBanks = null;
    #endregion

    #region Common
    /// <summary>
    /// XML Name to use in Translation inside the AppData
    /// </summary>
    protected string PageName { get; set; }
    protected commonVariables.PaymentTransactionType PaymentType { get; set; }
    protected string PaymentMethodId { get; set; }

    protected bool IsPageRefresh = false;

    protected string ThankYouPage = "/Deposit/ThankYou.aspx";

    protected string strMerchantId = string.Empty;
    protected string strOperatorId = string.Empty;
    protected string strMemberCode = string.Empty;
    protected string strMemberID = string.Empty;
    protected string strMemberName = string.Empty;
    protected string strCurrencyCode = string.Empty;
    protected string strCountryCode = string.Empty;
    protected string strRiskId = string.Empty;
    protected string strPaymentGroup = string.Empty;
    protected string strSelectedLanguage = string.Empty;
    protected string strSiteUrl = string.Empty;

    protected string strMethodsUnAvailable = string.Empty;
    protected string strMethodId = string.Empty;

    protected string strMode = string.Empty;
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

    protected string V2DepositPath = "/v2/Deposit/";
    protected string V2WithdrawalPath = "/v2/Withdrawal/";
    #endregion

    #region Labels

    protected string strlblMode = string.Empty;
    protected string strtxtMode = string.Empty;

    protected string strlblMinMaxLimit = string.Empty;
    protected string strtxtMinMaxLimit = string.Empty;

    protected string strlblDailyLimit = string.Empty;
    protected string strtxtDailyLimit = string.Empty;

    protected string strlblTotalAllowed = string.Empty;
    protected string strtxtTotalAllowed = string.Empty;

    protected string strlblAmount = string.Empty;
    protected string strtxtAmount = string.Empty;

    protected string strbtnSubmit = string.Empty;
    protected string strbtnCancel = string.Empty;

    protected string strlblBank = string.Empty;
    protected string strlblBankName = string.Empty;
    protected string strdrpBank = string.Empty;
    protected string strdrpOtherBank = string.Empty;

    protected string strlblTransactionId = string.Empty;

    protected string strUnlimited = string.Empty;

    protected string strlblMessage = string.Empty;
    protected string strlblVendorNote = string.Empty;

    protected string strlblAccountName = string.Empty;
    protected string strlblAccountNumber = string.Empty;

    #endregion

    #endregion

    protected override void OnPreInit(EventArgs e)
    {
        this.isPublic = false;
        UserSession.checkSession();
    }

    protected override void OnPreLoad(EventArgs e)
    {
        base.OnPreLoad(e);


        InitialiseVariables();
        InitialisePaymentLimits();

        if (!string.IsNullOrEmpty(PaymentMethodId))
        {
            try
            {
                Page.Items.Add("Parent", "/v2/Funds.aspx");
            }
            catch (Exception ex) { }
        }
    }
    protected void InitialiseVariables()
    {
        strOperatorId = commonVariables.OperatorId;

        strMemberCode = base.userInfo.MemberCode;
        strMemberID = base.userInfo.MemberId;
        strMemberName = commonVariables.GetSessionVariable("MemberName");

        strCurrencyCode = commonCookie.CookieCurrency;
        strCountryCode = commonVariables.GetSessionVariable("CountryCode");

        strRiskId = commonVariables.GetSessionVariable("RiskId");

        strPaymentGroup = commonVariables.GetSessionVariable("PaymentGroup");

        strSelectedLanguage = commonVariables.SelectedLanguage;

        strSiteUrl = commonVariables.SiteUrl;

        xeErrors = commonVariables.ErrorsXML;

        commonCulture.appData.getRootResource(PaymentType + "/Default.aspx", out xeDefaultResources);

        commonCulture.appData.getRootResource(PaymentType + "/" + PageName, out xeResources);

        strUnlimited = commonCulture.ElementValues.getResourceString("unlimited", xeDefaultResources);
    }

    protected void InitialiseLabels()
    {
        strlblMode = commonCulture.ElementValues.getResourceString("lblMode", xeDefaultResources);
        strMode = strMode.Equals("offline", StringComparison.OrdinalIgnoreCase) ? commonCulture.ElementValues.getResourceString("offline", xeDefaultResources) : commonCulture.ElementValues.getResourceString("online", xeDefaultResources);
        strtxtMode = string.Format(": {0}", strMode);

        strlblMinMaxLimit = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeDefaultResources);
        strtxtMinMaxLimit = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);

        strlblDailyLimit = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeDefaultResources);
        strtxtDailyLimit = string.Format(": {0}", strDailyLimit);

        strlblTotalAllowed = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeDefaultResources);
        strtxtTotalAllowed = string.Format(": {0}", strTotalAllowed);

        strlblAmount = commonCulture.ElementValues.getResourceString("lblAmount", xeDefaultResources);
        strtxtAmount = string.Format("{0} ({1})", strlblAmount, strCurrencyCode);

        strlblTransactionId = commonCulture.ElementValues.getResourceString("lblTransactionId", xeDefaultResources);

        strbtnSubmit = commonCulture.ElementValues.getResourceString("btnSubmit", xeDefaultResources);
        strbtnCancel = commonCulture.ElementValues.getResourceString("btnCancel", xeDefaultResources);

        strlblBank = commonCulture.ElementValues.getResourceString("lblBank", xeDefaultResources);
        strlblBankName = commonCulture.ElementValues.getResourceString("lblBankName", xeDefaultResources);
        strdrpBank = commonCulture.ElementValues.getResourceString("drpBank", xeDefaultResources);
        strdrpOtherBank = commonCulture.ElementValues.getResourceString("drpOtherBank", xeDefaultResources);

        strlblAccountName = commonCulture.ElementValues.getResourceString("lblAccountName", xeDefaultResources);
        strlblAccountNumber = commonCulture.ElementValues.getResourceString("lblAccountNumber", xeDefaultResources);

        strlblMessage = commonCulture.ElementValues.getResourceString("browserNotice", xeDefaultResources);

        strlblVendorNote = commonCulture.ElementValues.getResourceString("vendorRedirectionNote", xeDefaultResources);
    }

    protected void InitialisePaymentLimits()
    {
        if (PaymentType != 0)
        {
            if (PaymentType == commonVariables.PaymentTransactionType.Deposit)
            {
                bool isAutoRoute = Enum.IsDefined(typeof(commonVariables.AutoRouteMethod), Convert.ToInt32(PaymentMethodId));

                if (isAutoRoute)
                    InitializePaymentGatewayLimit();
                else
                    InitialiseDepositPaymentLimits();
            }
            else
                InitialiseWithdrawalPaymentLimits();
        }

        InitialiseLabels();
    }

    private void InitializePaymentGatewayLimit()
    {
        commonVariables.AutoRouteMethod autoRoute;
        Enum.TryParse(PaymentMethodId, out autoRoute);

        string gatewayGroup = GetPaymentGroup(autoRoute);

        using (var svcInstance = new svcPayMember.MemberClient())
        {
            var response = svcInstance.GetPaymentDepositGatewayLimit_Mobile(Convert.ToInt64(strOperatorId), strMemberCode);

            var gatewayGroupTable = response.Tables[0].AsEnumerable().FirstOrDefault(d => d.Field<string>("gatewayGroup").Equals(gatewayGroup, StringComparison.OrdinalIgnoreCase));

            if (gatewayGroupTable != null)
            {
                strMinLimit = Convert.ToDecimal(gatewayGroupTable["minDeposit"]).ToString(commonVariables.DecimalFormat);
                strMaxLimit = Convert.ToDecimal(gatewayGroupTable["maxDeposit"]).ToString(commonVariables.DecimalFormat);
                strMode = "online";

                autoRouteBanks = response.Tables[1];

            }
        }
    }

    private string GetPaymentGroup(commonVariables.AutoRouteMethod autoRoute)
    {
        switch (autoRoute)
        {
            case commonVariables.AutoRouteMethod.QuickOnline:
                return "Quick Online";

            case commonVariables.AutoRouteMethod.UnionPay:
                return "UnionPay";

            case commonVariables.AutoRouteMethod.TopUpCard:
                return "Top-Up Card";

            case commonVariables.AutoRouteMethod.AliPay:
                return "AliPay";

            case commonVariables.AutoRouteMethod.WeChat:
                return "WeChat";

            default:
                return "";
        }
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
            dtPaymentMethodLimits = svcInstance.getMethodLimits_Mobile(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Deposit)), false, out strProcessCode, out strProcessText);
        }

        foreach (commonVariables.DepositMethod EnumMethod in Enum.GetValues(typeof(commonVariables.DepositMethod)))
        {
            if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
            {
                sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
            }
        }

        if (!string.IsNullOrWhiteSpace(PaymentMethodId))
        {
            strMethodId = PaymentMethodId;

            if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
            {
                drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

                strMinLimit = Convert.ToDecimal(drPaymentMethodLimit["minDeposit"]).ToString(commonVariables.DecimalFormat);
                strMaxLimit = Convert.ToDecimal(drPaymentMethodLimit["maxDeposit"]).ToString(commonVariables.DecimalFormat);
                strTotalAllowed = Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]) <= 0 ? strUnlimited : Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]).ToString(commonVariables.DecimalFormat);
                strDailyLimit = Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]) == 0 ? strUnlimited : Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]).ToString(commonVariables.DecimalFormat);
                strMerchantId = Convert.ToString(drPaymentMethodLimit["merchantId"]);
                strMode = Convert.ToString(drPaymentMethodLimit["paymentMode"]);
            }
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
            dtPaymentMethodLimits = svcInstance.getMethodLimits_Mobile(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Withdrawal)), false, out strProcessCode, out strProcessText);
        }

        foreach (commonVariables.WithdrawalMethod EnumMethod in Enum.GetValues(typeof(commonVariables.WithdrawalMethod)))
        {
            if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
            {
                sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
            }
        }

        if (!string.IsNullOrWhiteSpace(PaymentMethodId))
        {
            strMethodId = PaymentMethodId;

            if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
            {
                drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

                strMinLimit = Convert.ToDecimal(drPaymentMethodLimit["minWithdrawal"]).ToString(commonVariables.DecimalFormat);
                strMaxLimit = Convert.ToDecimal(drPaymentMethodLimit["maxWithdrawal"]).ToString(commonVariables.DecimalFormat);
                strTotalAllowed = Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]) <= 0 ? strUnlimited : Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]).ToString(commonVariables.DecimalFormat);
                strDailyLimit = Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]) == 0 ? strUnlimited : Convert.ToDecimal(drPaymentMethodLimit["limitDaily"]).ToString(commonVariables.DecimalFormat);
                strMerchantId = Convert.ToString(drPaymentMethodLimit["merchantId"]);
                strMode = Convert.ToString(drPaymentMethodLimit["paymentMode"]);
            }
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }

    protected void InitialisePendingWithdrawals(bool isApp)
    {
        string strStatusCode = string.Empty;
        string strStatusText = string.Empty;

        svcPayMember.PendingWithdrawal[] arrPending = null;

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            arrPending = svcInstance.getPendingWithdrawal(Convert.ToInt64(strOperatorId), strMemberCode, out strStatusCode, out strStatusText);

            if (arrPending != null && arrPending.Length > 0)
            {
                Response.Redirect("/Withdrawal/Pending.aspx");
            }
        }

    }

    protected List<ListItem> InitializeBank(string paymentMethodBank)
    {
        List<ListItem> banks = new List<ListItem>() { new ListItem(strdrpBank, "-1") };
        try
        {
            XElement xElementBank = null;

            commonCulture.appData.getRootResource(PaymentType + "/" + paymentMethodBank, out xElementBank);

            XElement xElementBankPath = xElementBank.Element(commonVariables.GetSessionVariable("CurrencyCode"));

            if (xElementBankPath == null)
            {
                banks.AddRange(xElementBank.Elements("bank").Select(bank => new ListItem(bank.Value, bank.Attribute("id").Value)));

                strlblMessage = strlblMessage.Replace("{BANK}", string.Join(", ", banks.Where(b => b.Text.Contains("*")).Select(b => b.Value)));
            }
            else
                banks.AddRange(xElementBankPath.Elements("bank").Select(bank => new ListItem(bank.Value, bank.Attribute("id").Value)));
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", PageName, "InitializeBank", string.Empty, string.Empty, string.Empty, "-99", "exception", ex.Message, string.Empty, string.Empty, true);
        }

        return banks;
    }

    protected string GetPaymentGatewayMerchantSetting(commonVariables.DepositMethod paymentMethod)
    {
        string key = string.Empty;

        try
        {
            string paymentKey = ConfigurationManager.AppSettings["PaymentPrivateKey"];

            IEnumerable<XElement> result;
            string mapLoc = HttpContext.Current.Request.MapPath("~/App_Data/_PaymentGateway/" + PageName + ".xml");
            XElement root = XElement.Load(mapLoc);

            if (string.IsNullOrWhiteSpace(strMerchantId))
            {
                result = root.Elements("account").Take(1);
            }
            else
            {
                result = from el in root.Elements("account")
                         where (string)el.Element("merchantId") == strMerchantId
                         select el;
            }

            foreach (XElement el in result)
            {
                switch (paymentMethod)
                {
                    case commonVariables.DepositMethod.BofoPay:
                        key = commonEncryption.decrypting(el.Element("key").Value, paymentKey) + "|" + commonEncryption.decrypting(el.Element("terminalId").Value, paymentKey);
                        break;

                    default:
                        key = commonEncryption.decrypting(el.Element("key").Value, paymentKey);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", PageName, "GetPaymentGatewayMerchantSetting", string.Empty, string.Empty, string.Empty, string.Empty, "Exception", "Message:" + ex.Message + "|StackTrace: " + ex.StackTrace, string.Empty, string.Empty, true);
        }

        return key;
    }

    protected CommonStatus GetErrors(string elementPath)
    {
        CommonStatus status = new CommonStatus();

        status.AlertCode = "-1";
        status.AlertMessage = commonCulture.ElementValues.getResourceXPathString(PaymentType.ToString() + elementPath, xeErrors);
        status.IsProcessAbort = true;

        return status;
    }

    protected CommonStatus GetErrors(string elementPath, string strTransferId, string elementPath2)
    {
        CommonStatus status = new CommonStatus();

        status.AlertCode = "-1";
        status.AlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString(PaymentType.ToString() + elementPath, xeErrors), commonCulture.ElementValues.getResourceXPathString(PaymentType.ToString() + "/error" + strTransferId, xeErrors));
        status.IsProcessAbort = true;

        return status;
    }

}

public class CommonStatus
{
    public string AlertCode { get; set; }
    public bool IsProcessAbort { get; set; }
    public string AlertMessage { get; set; }
}