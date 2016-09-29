using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Register : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    public string CDNCountryCode = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { Response.Redirect("/Index"); }

        // check for country code
        checkCDN();
        CDNCountryCode = GetCountryCode(headers.cdn, headers.key);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("lang")))
        {
            commonVariables.SelectedLanguage = HttpContext.Current.Request.QueryString.Get("lang");
        }

        string strOperatorId = commonVariables.OperatorId;
        string strAffiliateId = string.Empty;
        xeErrors = commonVariables.ErrorsXML;
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);
        var opSettings = new customConfig.OperatorSettings("W88");

        if (Page.IsPostBack) return;

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("referid")))
        {
            commonCookie.CookieReferralId = HttpContext.Current.Request.QueryString.Get("referid");
        }

            if (string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")))
            {
            var affiliateId = HttpContext.Current.Request.QueryString.Get("AffiliateId");

                if (!string.IsNullOrEmpty(affiliateId))
                {
                    commonVariables.SetSessionVariable("AffiliateId", affiliateId);
                    commonCookie.CookieAffiliateId = affiliateId;
                }
            }

            if (!string.IsNullOrWhiteSpace(commonCookie.CookieAffiliateId))
            {
                strAffiliateId = commonCookie.CookieAffiliateId;
            }
            else
            {
                strAffiliateId = string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")) ? string.Empty : Convert.ToString(commonVariables.GetSessionVariable("AffiliateId"));
            }

            lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblEmail.Text = commonCulture.ElementValues.getResourceString("lblEmailAddress", xeResources);
            lblContact.Text = commonCulture.ElementValues.getResourceString("lblContact", xeResources);
            lblCurrency.Text = commonCulture.ElementValues.getResourceString("lblCurrency", xeResources);
            lblAffiliateID.Text = commonCulture.ElementValues.getResourceString("lblAffiliateID", xeResources);
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);
            btnCancel.InnerText = commonCulture.ElementValues.getResourceString("btnCancel", xeResources);

            lblDisclaimer.InnerText = commonCulture.ElementValues.getResourceString("lblDisclaimer", xeResources);
            btnTermsConditionsLink.InnerText = commonCulture.ElementValues.getResourceString("termsConditions", xeResources);
            btnTermsConditionsLink.HRef = commonCulture.ElementValues.getResourceString("termsConditionsUrl", xeResources);

            #region PhoneCountryCode
            System.Data.DataSet dsCountryInfo = null;

            using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
            {
                dsCountryInfo = wsInstance.GetCountryInfo(Convert.ToInt64(strOperatorId));

                foreach (System.Data.DataRow drPhoneCountryCode in dsCountryInfo.Tables[0].Select("", "countryPhoneCode ASC"))
                {
                    drpContactCountry.Items.Add(new ListItem(string.Format("+ {0}", Convert.ToString(drPhoneCountryCode["countryPhoneCode"])), Convert.ToString(drPhoneCountryCode["countryPhoneCode"])));
                }

                if (!string.IsNullOrEmpty(CDNCountryCode))
                {
                    System.Data.DataRow[] countrySearchResult = dsCountryInfo.Tables[0].Select("countryCode='" + CDNCountryCode + "'");
                    if (countrySearchResult.Any())
                        drpContactCountry.SelectedValue = countrySearchResult[0]["countryPhoneCode"].ToString();
                }
                else if (!string.IsNullOrEmpty(commonVariables.GetSessionVariable("countryCode")))
                {
                    System.Data.DataRow[] countrySearchResult = dsCountryInfo.Tables[0].Select("countryCode='" + commonVariables.GetSessionVariable("countryCode") + "'");
                    if (countrySearchResult.Any())
                        drpContactCountry.SelectedValue = countrySearchResult[0]["countryPhoneCode"].ToString();
                }
                else
                {
                    System.Data.DataRow[] countrySearchResult = dsCountryInfo.Tables[0].Select("countryCode='" + commonVariables.SelectedLanguageShort + "'");
                    if (countrySearchResult.Any())
                        drpContactCountry.SelectedValue = countrySearchResult[0]["countryPhoneCode"].ToString();
                }
            }
            #endregion

            #region Currencies
            string arrStrCurrencies = opSettings.Values.Get("Currencies");
            List<string> lstCurrencies = arrStrCurrencies.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

            drpCurrency.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpCurrencySelect", xeResources), "-1"));
            foreach (string currency in lstCurrencies)
            {
                drpCurrency.Items.Add(new ListItem(commonCulture.ElementValues.getResourceXPathString("Currency/" + currency, xeResources), currency));
            }
            #endregion

            //lblFirstName.Text = commonCulture.ElementValues.getResourceString("lblFirstName", xeResources);
            //lblLastName.Text = commonCulture.ElementValues.getResourceString("lblLastName", xeResources);
            lblName.Text = commonCulture.ElementValues.getResourceString("lblName", xeResources);
            lblNote.Text = commonCulture.ElementValues.getResourceString("lblNote", xeResources);
            lblDOB.Text = commonCulture.ElementValues.getResourceString("lblDOB", xeResources);


            int intDay = 0;
            foreach (int vintDay in new int[31]) { intDay++; drpDay.Items.Add(new ListItem((intDay).ToString("0#"), Convert.ToString(intDay))); }
            foreach (System.Xml.Linq.XElement xeMonth in xeResources.Element("Calendar").Elements()) { drpMonth.Items.Add(new ListItem(xeMonth.Value, Convert.ToString(xeMonth.Name).Replace("m", ""))); }
            for (int intYear = System.DateTime.Now.Year - 18; intYear >= System.DateTime.Now.Year - 99; intYear--) { drpYear.Items.Add(new ListItem(Convert.ToString(intYear))); }

            txtAffiliateID.Text = strAffiliateId;

            if (!string.IsNullOrEmpty(strAffiliateId))
            {
                txtAffiliateID.ReadOnly = true;
            }

           
        }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "Register";

        string strProcessCode = string.Empty;

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = 1;
        bool isTestAccount = false;
        string strMemberCode = string.Empty;
        string strPassword = string.Empty;
        string strPasswordEncrypted = string.Empty;
        string strEmail = string.Empty;
        string strContact = string.Empty;
        string strCurrencyCode = string.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        string strDOB = string.Empty;
        string strCountryCode = string.Empty;
        string strLanguageCode = string.Empty;
        string strIPAddress = string.Empty;
        string strSignUpUrl = string.Empty;
        string strPermission = string.Empty;
        string strContactNumber = string.Empty;
        string strAffiliateId = string.Empty;

        int intOddsType = 1;
        System.DateTime dtDOB = DateTime.MinValue;
        string strHiddenValues = hidValues.Value;
        List<string> lstValues = null;
        int affiliateId;
        #endregion

        #region populateVariables

        strMemberCode = txtUsername.Text.Trim();
        strPassword = txtPassword.Text;
        strEmail = txtEmail.Text;
        strContact = txtContact.Text;
        strCurrencyCode = drpCurrency.SelectedValue;
        // This changes is for the combined name on frontend only but on the BO everything will be saved in firstname
        strFName = System.Text.RegularExpressions.Regex.Replace(txtName.Text, @"\t|\n|\r|", "");
        strLName = string.Empty; //System.Text.RegularExpressions.Regex.Replace(txtLastName.Text, @"\t|\n|\r|", "");
        strDOB = string.Format("{0}-{1}-{2}", drpYear.SelectedValue, drpMonth.SelectedValue, drpDay.SelectedValue);
        strAlertCode = "-1";
        strContactNumber = string.Format("{0}-{1}", drpContactCountry.SelectedValue, strContact);
        strAffiliateId = txtAffiliateID.Text;

        System.Text.RegularExpressions.Regex rexContact = new System.Text.RegularExpressions.Regex("([0-9]{1,4})[-]([0-9]{6,12})$");
        #endregion

        #region parametersValidation

        strResultCode = "11";
        strResultDetail = "Error:ParameterValidation";

        if (string.IsNullOrEmpty(strMemberCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingUsername", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strPassword))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingPassword", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strEmail))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingEmail", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strContact))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingContact", xeErrors);
            isProcessAbort = true;
        }
        else if (!rexContact.IsMatch(strContactNumber))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strCurrencyCode) || string.Compare(strCurrencyCode, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingCurrency", xeErrors);
            isProcessAbort = true;
        }
        //else if (string.IsNullOrEmpty(strFName))
        //{
        //    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingFName", xeErrors);
        //    isProcessAbort = true;
        //}
        //else if (string.IsNullOrEmpty(strLName))
        //{
        //    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingLName", xeErrors);
        //    isProcessAbort = true;
        //}
        else if (string.IsNullOrEmpty(strFName))
        {
            // This changes is for the combined name on frontend only but on the BO everything will be saved in firstname
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingName", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strMemberCode) || strMemberCode.IndexOf(' ') >= 0 || !commonValidation.isAlphanumeric(strMemberCode) || strMemberCode.Length < 5 || strMemberCode.Length > 16)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidUsername", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strPassword) || strPassword.Length < 8 || strPassword.Length > 10)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidPassword", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strEmail))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidEmail", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strContact))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strCurrencyCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidCurrency", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strFName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidFName", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strLName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidLName", xeErrors);
            isProcessAbort = true;
        }
        else if (!DateTime.TryParse(strDOB, out dtDOB))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidDOB", xeErrors);
            isProcessAbort = true;
        }
        else if (!CheckOver18(Convert.ToDateTime(strDOB)))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/Required18", xeErrors);
            isProcessAbort = true;
        }
        else
        {
            strResultCode = "00";
            strResultDetail = "OK:ParameterValidation";

            strContact = strContact.TrimStart('+');
            strPasswordEncrypted = commonEncryption.Encrypt(strPassword);
        }

        strErrorDetail = strAlertMessage;
        strProcessRemark = string.Format("strAlertMessage: {0} | HiddenValues: {1} ", strAlertMessage, strHiddenValues);

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "ParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

        #endregion

        if (!isProcessAbort)
        {
            lstValues = strHiddenValues.Split(new char[] { '|' }).Select(p => p.Trim()).ToList();

            if (lstValues.Count > 0)
            {
                if (lstValues[0] != null) { strCountryCode = lstValues[0]; }
                if (lstValues.Count > 2) { strIPAddress = lstValues[2]; }
                if (lstValues.Count > 3) { strPermission = lstValues[3]; }
            }

            strSignUpUrl = string.Format("m.{0}", commonIp.DomainName);
            strLanguageCode = commonVariables.SelectedLanguage;

            if (string.IsNullOrEmpty(strIPAddress))
            {
                strIPAddress = commonIp.UserIP;
            }

            if (string.IsNullOrEmpty(strCountryCode) || string.Compare(strCountryCode, "-", true) == 0)
            {
                if (!string.IsNullOrEmpty(CDNCountryCode))
                {
                    strCountryCode = CDNCountryCode;
                }
                else
                {
                using (wsIP2Loc.ServiceSoapClient wsInstance = new wsIP2Loc.ServiceSoapClient())
                {
                    wsInstance.location(strIPAddress, ref strCountryCode, ref strPermission);
                }
            }
            }

            switch (strCountryCode.ToUpper())
            {
                case "MY":
                case "TH":
                case "VN":
                case "KH":
                    intOddsType = 1;
                    break;
                case "CN":
                    intOddsType = 2;
                    break;
                case "IN":
                case "KR":
                case "JP":
                case "AU":
                    intOddsType = 3;
                    break;
                case "ID":
                    intOddsType = 4;
                    break;

                default:
                    intOddsType = 3;
                    break;
            }

            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

            if (opSettings.Values.Get("DemoDomains").IndexOf(commonIp.DomainName) >= 0) { isTestAccount = true; }

            string strAddress = strCountryCode;
            string strCity = strCountryCode;
            string strPostal = "000000";
            string strGender = "M";
            //int intAffiliateId = string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")) ? (string.IsNullOrEmpty(strAffiliateId) ? 0 : Convert.ToInt32(strAffiliateId)) : Convert.ToInt32(commonVariables.GetSessionVariable("AffiliateId"));
            string AffiliateId;
            if (string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")))
            {
                AffiliateId = (string.IsNullOrEmpty(strAffiliateId) ? "0" : strAffiliateId);
            }
            else 
                AffiliateId = commonVariables.GetSessionVariable("AffiliateId");

            int intAffiliateId;
            try
            {
                int.TryParse(AffiliateId, out intAffiliateId);
            }
            catch
            {
                intAffiliateId = 0;
            }

            var strReferBy = commonCookie.CookieReferralId;
            string strDeviceId = "Mobile";

            System.Data.DataSet dsRegister = null;

            using (wsMemberMS1.memberWSSoapClient svcInstance = new wsMemberMS1.memberWSSoapClient())
            {
                dsRegister = svcInstance.MemberRegistrationNew(lngOperatorId, strMemberCode, strPasswordEncrypted, strEmail, strContactNumber,
                    strAddress, strCity, strPostal, strCountryCode, strCurrencyCode, strGender, intOddsType, string.IsNullOrEmpty(strLanguageCode) ? "en-us" : strLanguageCode,
                            intAffiliateId, strReferBy, strIPAddress, strSignUpUrl, strDeviceId, isTestAccount, strFName, strLName, dtDOB, string.Empty);

                strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | Password: {2} | Email: {3} | Contact: {4} | Address: {5} | City: {6} | Postal: {6} | Country: {8} | Currency: {9} | Gender: {10} | OddsType: {11} | Language: {12} | Affiliate: {13} | ReferBy: {14} | IP: {15} | SignUpUrl: {16} | DeviceID: {17} | TestAccount: {18} | FName: {19} | LName: {20} | DOB: {21} | REMOTEIP: {22} | FORWARDEDIP: {23} | REQUESTERIP: {24} | AffiliateID: {25}",
                    lngOperatorId, strMemberCode, strPasswordEncrypted, strEmail, strContact, strAddress, strCity, strPostal, strCountryCode, strCurrencyCode, strGender, intOddsType, strLanguageCode, intAffiliateId, strReferBy, strIPAddress, strSignUpUrl, strDeviceId, isTestAccount, strFName, strLName, dtDOB, commonIp.remoteIP, commonIp.forwardedIP, commonIp.requesterIP, intAffiliateId);

                intProcessSerialId += 1;
                commonAuditTrail.appendLog("system", strPageName, "RegistrationParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

                strResultCode = "21";
                strResultDetail = "Error:MemberRegistrationNew";

                if (dsRegister.Tables[0].Rows.Count > 0)
                {
                    strProcessCode = Convert.ToString(dsRegister.Tables[0].Rows[0]["RETURN_VALUE"]);

                    switch (strProcessCode)
                    {
                        case "0":
                            strAlertMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                            break;

                        case "1":
                            strAlertCode = strProcessCode;
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/Success", xeErrors);
                            string strMemberSessionId = Convert.ToString(dsRegister.Tables[0].Rows[0]["memberSessionId"]);
                            HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsRegister.Tables[0].Rows[0]["memberSessionId"]));
                            HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsRegister.Tables[0].Rows[0]["memberId"]));
                            HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["memberCode"]));
                            HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["countryCode"]));
                            HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["currency"]));
                            HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["languageCode"]));
                            HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsRegister.Tables[0].Rows[0]["riskId"]));
                            HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsRegister.Tables[0].Rows[0]["partialSignup"]));
                            HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsRegister.Tables[0].Rows[0]["resetPassword"]));

                            commonCookie.CookieS = strMemberSessionId;
                            commonCookie.CookieG = strMemberSessionId;
                            HttpContext.Current.Session.Add("LoginStatus", "success");

                            strResultCode = "00";
                            strResultDetail = "OK:MemberRegistrationNew";

                            #region IOVATION
                            //this.IovationSubmit(ref intProcessSerialId, strProcessId, strPageName, strMemberCode, strIPAddress, strPermission);
                            #endregion
                            break;

                        case "10":
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/DuplicateUsername", xeErrors);
                            break;

                        case "11":
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/DuplicateEmail", xeErrors);
                            break;
                        case "50":
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/DuplicateContact", xeErrors);
                            break;
                        default:
                            strAlertMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                            break;
                    }

                    strErrorCode = strProcessCode;
                    strErrorDetail = strAlertMessage;
                }

                intProcessSerialId += 1;
                commonAuditTrail.appendLog("system", strPageName, "MemberRegistrationNew", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
                if (strAlertCode == "1")
                {
                    Response.Redirect("/Funds.aspx?lang=" + commonVariables.SelectedLanguage.ToLower(), false);
                }
            }
        }
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
                ioRequest.type = "registration";

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

    private bool CheckOver18(DateTime dob)
    {
        DateTime now = DateTime.Today;
        int age = now.Year - dob.Year;

        if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
            age--;

        return age >= 18;
    }
}
