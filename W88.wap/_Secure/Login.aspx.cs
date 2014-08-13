using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Login : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected string strProcessCode = string.Empty;
    protected string strProcessMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        string strLanguage = string.Empty;

        strLanguage = Request.QueryString.Get("lang");

        commonVariables.SelectedLanguage = string.IsNullOrEmpty(strLanguage) ? (string.IsNullOrEmpty(commonVariables.SelectedLanguage) ? "en-us" : commonVariables.SelectedLanguage) : strLanguage;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/_Secure/Login.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            lblUsername.InnerText = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
            lblPassword.InnerText = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblCaptcha.InnerText = commonCulture.ElementValues.getResourceString("lblCaptcha", xeResources);
            btnSubmit.Value = commonCulture.ElementValues.getResourceString("btnLogin", xeResources);
                        
            txtUsername.Focus();
            if (!string.IsNullOrEmpty(Request.QueryString.Get("Code"))) 
            {
                hidCode.Value = Request.QueryString.Get("Code");
            }
            if (!string.IsNullOrEmpty(Request.QueryString.Get("IP")))
            {
                hidIP.Value = Request.QueryString.Get("IP");
            }
        }        
    }

    protected void btnSubmit_Click(object sender, EventArgs e) 
    {
        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "ProcessLogin";

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = long.MinValue;
        string strMemberCode = string.Empty;
        string strPassword = string.Empty;
        string strSiteURL = string.Empty;
        string strLoginIp = string.Empty;
        string strDeviceId = string.Empty;

        string strVCode = string.Empty;
        string strSVCode = string.Empty;
        //string strProcessCode = string.Empty;
        //string strProcessMessage = string.Empty;
        string strCountryCode = string.Empty;
        string strLastLoginIP = string.Empty;
        string strPermission = string.Empty;

        bool runIovation = false;

        System.Xml.XmlDocument xdResponse = new System.Xml.XmlDocument();
        #endregion
        
        #region populateVariables
        lngOperatorId = long.Parse(commonVariables.OperatorId);
        strMemberCode = txtUsername.Value;
        strPassword = txtPassword.Value;
        strVCode = txtCaptcha.Value;
        strSVCode = commonVariables.GetSessionVariable("vCode");
        strLoginIp = string.IsNullOrEmpty(Request.Form.Get("txtIPAddress")) ? commonIp.UserIP : Request.Form.Get("txtIPAddress");
        strDeviceId = HttpContext.Current.Request.UserAgent;
        strSiteURL = commonVariables.SiteUrl;
        #endregion

        #region parametersValidation
        if (string.IsNullOrEmpty(strMemberCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", xeErrors); isProcessAbort = true; }
        else if (string.IsNullOrEmpty(strPassword)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/MissingPassword", xeErrors); isProcessAbort = true; }
        else if (string.IsNullOrEmpty(strVCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceString("MissingVCode", xeErrors); isProcessAbort = true; }
        else if (commonValidation.isInjection(strMemberCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors); isProcessAbort = true; }
        else if (commonValidation.isInjection(strPassword)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidPassword", xeErrors); isProcessAbort = true; }
        else if (commonValidation.isInjection(strVCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceString("IncorrectVCode", xeErrors); isProcessAbort = true; }
        else if (string.Compare(commonEncryption.encrypting(strVCode), strSVCode, true) != 0) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceString("IncorrectVCode", xeErrors); isProcessAbort = true; }
        else
        {
            strPassword = commonEncryption.Encrypt(strPassword);
        }

        strProcessRemark = string.Format("MemberCode: {0} | Password: {1} | VCode: {2} | SVCode: {3} | IP: {4} | Country: {5}", strMemberCode, strPassword, strVCode, strSVCode, strLoginIp, strCountryCode);

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "ParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        #endregion      
        
        #region initiateLogin
        if (!isProcessAbort)
        {
            try
            {
                using (wsMemberMS1.memberWSSoapClient svcInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    System.Data.DataSet dsSignin = null;
                    dsSignin = svcInstance.MemberSignin(lngOperatorId, strMemberCode, strPassword, strSiteURL, strLoginIp, strDeviceId);

                    if (dsSignin.Tables[0].Rows.Count > 0)
                    {
                        strProcessCode = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                        switch (strProcessCode)
                        {
                            case "0":
                                strProcessMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                                break;
                            case "1":
                                string strMemberSessionId = Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]);
                                HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
                                HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberId"]));
                                HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
                                HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
                                HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currency"]));
                                HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
                                HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
                                //HttpContext.Current.Session.Add("PaymentGroup", "A"); //Convert.ToString(dsSignin.Tables[0].Rows[0]["paymentGroup"]));
                                HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
                                HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));

                                commonCookie.CookieS = strMemberSessionId;
                                commonCookie.CookieG = strMemberSessionId;
                                HttpContext.Current.Session.Add("LoginStatus", "success");

                                strLastLoginIP = Convert.ToString(dsSignin.Tables[0].Rows[0]["lastLoginIP"]);
                                if (HttpContext.Current.Request.Cookies[strMemberCode] == null) { runIovation = true; }
                                else if (HttpContext.Current.Request.Cookies[strMemberCode] != null && string.Compare(strLastLoginIP, strLoginIp, true) != 0) { runIovation = true; }
                                if (runIovation) { this.IovationSubmit(ref intProcessSerialId, strProcessId, strPageName, strMemberCode, strLoginIp, strPermission); }

                                Response.Redirect("/Index");
                                break;
                            case "21":
                                strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors);
                                break;
                            case "22":
                                strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InactiveAccount", xeErrors);
                                break;
                            case "23":
                                strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidPassword", xeErrors);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strProcessCode = "0";
                strProcessMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                strProcessRemark = string.Format("{0} | Message: {1}", strProcessRemark, ex.Message);
            }

            strProcessRemark = string.Format("{0} | strProcessCode: {1}", strProcessRemark, strProcessCode);

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", strPageName, "MemberSignin", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
        #endregion

        #region Response
        txtMessage.InnerText = strProcessMessage;
        #endregion
    }

    protected void IovationSubmit(ref int intProcessSerialId, string strProcessId, string strPageName, string strUsername, string strIPAddress, string strPermission)
    {
        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isSystemError = false;

        customConfig.IovationSettings ioSettings = new customConfig.IovationSettings("W88");
        string strCheckTransactionUrl = ioSettings.Values.Get("CheckTransactionUrl");
        string strGetEvidenceUrl = ioSettings.Values.Get("GetEvidenceUrl");
        string strAccountPrefix = ioSettings.Values.Get("AccountPrefix");
        string strSubscriberID = ioSettings.Values.Get("SubscriberId");
        string strSubscriberAccount = ioSettings.Values.Get("SubscriberAccount");
        string strSubscriberPassCode = ioSettings.Values.Get("SubscriberPassCode");
        string strServiceEnabled = ioSettings.Values.Get("ServiceEnabled");
        string strUserAccountCode = string.Format("{0}{1}", strAccountPrefix, strUsername);
        string strExceptions = ioSettings.Values.Get("Exceptions");

        List<string> lstPermission = strExceptions.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        if (lstPermission.FindIndex(x => x.Trim().Equals(strPermission, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            return;
        }
        else if (string.Compare(strServiceEnabled, "true", true) != 0)
        {
            return;
        }

        strProcessRemark = string.Format("CheckTransactionURL: {0} | GetEvidenceURL: {1} | AccountPrefix: {2} | SubscriberID: {3} | SubscriberAccount: {4} | SubscriberPassCode: {5} | UserAccountCode : {6}",
            strCheckTransactionUrl, strGetEvidenceUrl, strAccountPrefix, strSubscriberID, strSubscriberAccount, strSubscriberPassCode, strUserAccountCode);

        try
        {
            using (CheckTransactionDetailsService ioInstance = new CheckTransactionDetailsService(strCheckTransactionUrl))
            {
                CheckTransactionDetails ioRequest = new CheckTransactionDetails();

                ioRequest.accountcode = strUserAccountCode;
                ioRequest.enduserip = strIPAddress;

                ioRequest.beginblackbox = HttpContext.Current.Request.Form.Get("ioBlackBox");
                ioRequest.subscriberid = strSubscriberID;
                ioRequest.subscriberpasscode = strSubscriberPassCode;
                ioRequest.subscriberaccount = strSubscriberAccount;
                ioRequest.type = "login";

                CheckTransactionDetailsResponse ioResponse = new CheckTransactionDetailsResponse();

                ioResponse = ioInstance.CheckTransactionDetails(ioRequest);

                #region setIovationCookie
                HttpCookie cookie = new HttpCookie(strUsername);
                cookie.Value = ioResponse.result;
                if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
                cookie.Expires = System.DateTime.Now.AddDays(Convert.ToInt32(ioSettings.Values.Get("ServiceDays")));
                HttpContext.Current.Response.Cookies.Add(cookie);
                #endregion
            }
        }
        catch (Exception ex)
        {
            strResultCode = "31";
            strResultDetail = "Error:Iovation";
            strErrorCode = Convert.ToString(ex.HResult);
            strErrorDetail = ex.Message;

            isSystemError = true;
        }


        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "Iovation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
    }
}