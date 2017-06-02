using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_AjaxHandlers_ProcessLogin : System.Web.UI.Page, System.Web.SessionState.IReadOnlySessionState
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeErrors = commonVariables.ErrorsXML;

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
        string strSessionVCode = string.Empty;
        string strProcessCode = string.Empty;
        string strProcessMessage = string.Empty;
        string strCountryCode = string.Empty;
        string strLastLoginIP = string.Empty;
        string strPermission = string.Empty;
        int login_attemps = 0; 
        bool runIovation = false;

        System.Xml.XmlDocument xdResponse = new System.Xml.XmlDocument();

        #endregion

        #region populateVariables
        lngOperatorId = long.Parse(commonVariables.OperatorId);
        strMemberCode = Request.Form.Get("txtUsername");
        strPassword = Request.Form.Get("txtPassword");
        strSiteURL = commonVariables.SiteUrl;
        strLoginIp = string.IsNullOrEmpty(Request.Form.Get("txtIPAddress")) ? commonIp.UserIP : Request.Form.Get("txtIPAddress");
        strDeviceId = HttpContext.Current.Request.UserAgent;
        strVCode = Request.Form.Get("txtCaptcha");
        strSessionVCode = commonVariables.GetSessionVariable("vCode");
        strCountryCode = Request.Form.Get("txtCountry");
        strPermission = Request.Form.Get("txtPermission");
        login_attemps = int.Parse(Request.Form.Get("login_attemps"));
        #endregion

        #region parametersValidation
        if (string.IsNullOrEmpty(strMemberCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", xeErrors); isProcessAbort = true; }
        else if (string.IsNullOrEmpty(strPassword)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/MissingPassword", xeErrors); isProcessAbort = true; }
        else if (login_attemps > 2 && string.IsNullOrEmpty(strVCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceString("MissingVCode", xeErrors); isProcessAbort = true; }
        else if (commonValidation.isInjection(strMemberCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors); isProcessAbort = true; }
        else if (commonValidation.isInjection(strPassword)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidPassword", xeErrors); isProcessAbort = true; }
        else if (login_attemps > 2 && commonValidation.isInjection(strVCode)) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceString("IncorrectVCode", xeErrors); isProcessAbort = true; }
        else if (login_attemps > 2 && string.Compare(commonEncryption.encrypting(strVCode), strSessionVCode, true) != 0) { strProcessCode = "-1"; strProcessMessage = commonCulture.ElementValues.getResourceString("IncorrectVCode", xeErrors); isProcessAbort = true; }
        else
        {
            strPassword = commonEncryption.Encrypt(strPassword);
        }

        strProcessRemark = string.Format("MemberCode: {0} | Password: {1} | VCode: {2} | EVCode: {3} | SVCode: {4} | IP: {5} | Country: {6} | ProcessCode: {7}", strMemberCode, strPassword, strVCode, commonEncryption.encrypting(strVCode), strSessionVCode, strLoginIp, strCountryCode, strProcessCode);

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "ParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

        #endregion

        #region initiateLogin
        if (!isProcessAbort)
        {
            try
            {

                //using (wsAffiliateMS1.affiliateWSSoapClient svcInstance = new wsAffiliateMS1.affiliateWSSoapClient())
                using (mwsAffiliate.mws_affiliateSoapClient svcInstance = new mwsAffiliate.mws_affiliateSoapClient())
                {
                    System.Data.DataSet dsSignin = null;
                    //dsSignin = svcInstance.MemberSignin(lngOperatorId, strMemberCode, strPassword, strSiteURL, strLoginIp, strDeviceId);
                    dsSignin = svcInstance.MobileMemberSignin(lngOperatorId, strMemberCode, strPassword, strSiteURL, strLoginIp, strDeviceId);

                    if (dsSignin.Tables[0].Rows.Count > 0)
                    {

                        strProcessRemark = string.Format("OpID: {0} | MemberCode: {1} | Password: {2} | URL: {3} | LoginIp: {4} | Device: {5}", lngOperatorId, strMemberCode, strPassword, strSiteURL, strLoginIp, strDeviceId);

                        intProcessSerialId += 1;
                        commonAuditTrail.appendLog("system", strPageName, "ParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);


                        strProcessCode = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                        switch (strProcessCode)
                        {
                            case "0":
                                strProcessMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                                break;
                            case "1":
                                string strMemberSessionId = Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]);
                                HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
                                HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["affiliateID"]));
                                //affiliate id
                                HttpContext.Current.Session.Add("AffiliateId", Convert.ToString(dsSignin.Tables[0].Rows[0]["affiliateID"]));
                                HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
                                HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
                                HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currency"]));
                                HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
                                //HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
                                //HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
                                HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));

                                commonCookie.CookieAffiliateId = Convert.ToString(dsSignin.Tables[0].Rows[0]["affiliateID"]);
                                commonCookie.CookieS = strMemberSessionId;
                                commonCookie.CookieG = strMemberSessionId;
                                HttpContext.Current.Session.Add("LoginStatus", "success");

                                //strLastLoginIP = Convert.ToString(dsSignin.Tables[0].Rows[0]["lastLoginIP"]);
                                if (HttpContext.Current.Request.Cookies[strMemberCode] == null) { runIovation = true; }
                                else if (HttpContext.Current.Request.Cookies[strMemberCode] != null && string.Compare(strLastLoginIP, strLoginIp, true) != 0) { runIovation = true; }
                                if (runIovation) { this.IovationSubmit(ref intProcessSerialId, strProcessId, strPageName, strMemberCode, strLoginIp, strPermission); }
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
                            case "24":
                                strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/AccountPending", xeErrors);
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
        System.Xml.XmlNode xnRootNode = xdResponse.CreateElement("Login");
        System.Xml.XmlNode xnCodeNode = xdResponse.CreateElement("ErrorCode");
        System.Xml.XmlNode xnMessageNode = xdResponse.CreateElement("Message");
        
        xnCodeNode.InnerText = strProcessCode;
        xnMessageNode.InnerText = strProcessMessage;
        xnRootNode.AppendChild(xnCodeNode);
        xnRootNode.AppendChild(xnMessageNode);
        xdResponse.AppendChild(xnRootNode);

        Response.ContentType = "text/xml";
        Response.Write(xdResponse.DocumentElement.OuterXml);
        Response.End();
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