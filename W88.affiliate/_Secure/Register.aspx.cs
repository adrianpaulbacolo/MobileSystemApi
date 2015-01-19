using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Register : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { Response.Redirect("./Index"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        string strOperatorId = commonVariables.OperatorId;
        string strAffiliateId = string.Empty;
        xeErrors = commonVariables.ErrorsXML;
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);
        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        if (!Page.IsPostBack)
        {

                //if (string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId"))) { if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { commonVariables.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); } }
                //strAffiliateId = string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")) ? string.Empty : Convert.ToString(commonVariables.GetSessionVariable("AffiliateId"));

                lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
                txtUsername.Attributes.Add("PLACEHOLDER", lblUsername.Text);

                lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
                txtPassword.Attributes.Add("PLACEHOLDER", lblPassword.Text);

                lblEmail.Text = commonCulture.ElementValues.getResourceString("lblEmailAddress", xeResources);
                txtEmail.Attributes.Add("PLACEHOLDER", lblEmail.Text);

                lblDOB.Text = commonCulture.ElementValues.getResourceString("lblDOB", xeResources);

                lblContact.Text = commonCulture.ElementValues.getResourceString("lblContact", xeResources);
                txtContact.Attributes.Add("PLACEHOLDER", lblContact.Text);

                //lblFirstName.Text = commonCulture.ElementValues.getResourceString("lblFirstName", xeResources);
                //txtFirstName.Attributes.Add("PLACEHOLDER", lblFirstName.Text);

                //lblLastName.Text = commonCulture.ElementValues.getResourceString("lblLastName", xeResources);
                //txtLastName.Attributes.Add("PLACEHOLDER", lblLastName.Text);

                lblFullName.Text = commonCulture.ElementValues.getResourceString("lblFullName", xeResources);
                txtFullName.Attributes.Add("PLACEHOLDER", lblFullName.Text);

                lblAccount.Text = commonCulture.ElementValues.getResourceString("lblAccount", xeResources);
                txtAccount.Attributes.Add("PLACEHOLDER", lblAccount.Text);

                lblReferralID.Text = commonCulture.ElementValues.getResourceString("lblReferralID", xeResources);
                txtReferralID.Attributes.Add("PLACEHOLDER", lblReferralID.Text);

                lblAddress.Text = commonCulture.ElementValues.getResourceString("lblAddress", xeResources);
                txtAddress.Attributes.Add("PLACEHOLDER", lblAddress.Text);

                lblCity.Text = commonCulture.ElementValues.getResourceString("lblCity", xeResources);
                txtCity.Attributes.Add("PLACEHOLDER", lblCity.Text);

                lblPostal.Text = commonCulture.ElementValues.getResourceString("lblPostal", xeResources);
                txtPostal.Attributes.Add("PLACEHOLDER", lblPostal.Text);

                lblWebsiteUrl.Text = commonCulture.ElementValues.getResourceString("lblWebsiteUrl", xeResources);

                lblURL1.Text = commonCulture.ElementValues.getResourceString("lblURL1", xeResources);
                txtURL1.Attributes.Add("PLACEHOLDER", lblURL1.Text);

                lblURL2.Text = commonCulture.ElementValues.getResourceString("lblURL2", xeResources);
                txtURL2.Attributes.Add("PLACEHOLDER", lblURL2.Text);

                lblURL3.Text = commonCulture.ElementValues.getResourceString("lblURL3", xeResources);
                txtURL3.Attributes.Add("PLACEHOLDER", lblURL3.Text);

                lblDesc.Text = commonCulture.ElementValues.getResourceString("lblDesc", xeResources);
                txtDesc.Attributes.Add("PLACEHOLDER", lblDesc.Text);

                lblCaptcha.Text = commonCulture.ElementValues.getResourceString("lblCaptcha", xeResources);
                txtCaptcha.Attributes.Add("PLACEHOLDER", lblCaptcha.Text);

                lblDisclaimer.InnerText = commonCulture.ElementValues.getResourceString("lblDisclaimer", xeResources);

                btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);
                btnCancel.InnerText = commonCulture.ElementValues.getResourceString("btnCancel", xeResources);

                #region PhoneCountryCode
                System.Data.DataSet dsCountryInfo = null;

                using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    dsCountryInfo = wsInstance.GetCountryInfo(Convert.ToInt64(strOperatorId));

                    if (dsCountryInfo.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow drPhoneCountryCode in dsCountryInfo.Tables[0].Select("", "countryPhoneCode ASC"))
                        {
                            string strProcessRemark = "Register: GetCountryInfo" + strOperatorId;
                            int intProcessSerialId = 0;
                            intProcessSerialId += 1;
                            commonAuditTrail.appendLog("system", "Register", "ParameterValidation", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);

                            drpContactCountry.Items.Add(new ListItem(string.Format("+ {0}", Convert.ToString(drPhoneCountryCode["countryPhoneCode"])), Convert.ToString(drPhoneCountryCode["countryPhoneCode"])));
                        }
                    }


                }
                #endregion

                #region Currencies
                string arrStrCurrencies = opSettings.Values.Get("Currencies");
                List<string> lstCurrencies = arrStrCurrencies.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

                drpCurrency.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpCurrencySelect", xeResources), "-1"));

                foreach (string currency in lstCurrencies)
                {
                    string strProcessRemark = "currency: " + currency;
                    int intProcessSerialId = 0;
                    intProcessSerialId += 1;
                    commonAuditTrail.appendLog("system", "Register", "ParameterValidation", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);

                    drpCurrency.Items.Add(new ListItem(commonCulture.ElementValues.getResourceXPathString("Currency/" + currency, xeResources), currency));
                }
                #endregion

                #region Country
                using (wsAffiliateMS1.affiliateWSSoapClient wsInstanceAff = new wsAffiliateMS1.affiliateWSSoapClient("affiliateWSSoap"))
                {

                    System.Data.DataSet ds_country = wsInstanceAff.GetCountryList();

                    if (ds_country.Tables[0].Rows.Count > 0)
                    {
                        drpCountry.DataTextField = "countryName";
                        drpCountry.DataValueField = "countryCode";
                        drpCountry.DataSource = ds_country.Tables[0];
                        drpCountry.DataBind();

                        drpCountry.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpCountrySelect", xeResources), "-1"));
                    }
                }
                #endregion

                #region Language
                string[] langcodes = System.Configuration.ConfigurationManager.AppSettings.Get("list_language_code").Split(',');
                string[] langNames = System.Configuration.ConfigurationManager.AppSettings.Get("list_language_translation").Split(',');

                drpLanguage.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpLanguageSelect", xeResources), "-1"));

                for (int i = 0; i < langcodes.Length; i++)
                {
                    drpLanguage.Items.Add(new ListItem(langNames[i], langcodes[i]));
                }
                #endregion

                #region Commission Type
                drpCommissionType.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("lblCommissionType", xeResources), "-1"));
                drpCommissionType.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("lblRevenueShare", xeResources).ToString(), "Revenue Share"));
                #endregion

                //drpDOB.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("lblDOB", xeResources), string.Empty, true));

                int intDay = 0;
                foreach (int vintDay in new int[31]) { intDay++; drpDay.Items.Add(new ListItem((intDay).ToString("0#"), Convert.ToString(intDay))); }
                foreach (System.Xml.Linq.XElement xeMonth in xeResources.Element("Calendar").Elements()) { drpMonth.Items.Add(new ListItem(xeMonth.Value, Convert.ToString(xeMonth.Name).Replace("m", ""))); }
                for (int intYear = System.DateTime.Now.Year - 18; intYear >= System.DateTime.Now.Year - 99; intYear--) { drpYear.Items.Add(new ListItem(Convert.ToString(intYear))); }

                //txtAffiliateID.Text = strAffiliateId;
           
        }
         
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strAffiliateId = string.Empty;

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
        string strContactNumber = string.Empty;
        string strDOB = string.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        string strCurrencyCode = string.Empty;
        string strCountryCode = string.Empty;
        string strAccount = string.Empty;
        string strReferralId = string.Empty;
        string strLanguageCode = string.Empty;
        string strCommissionType = string.Empty;
        string strAddress = string.Empty;
        string strCity = string.Empty;
        string strPostal = string.Empty;
        string strUrl1 = string.Empty;
        string strUrl2 = string.Empty;
        string strUrl3 = string.Empty;
        string strDesc = string.Empty;

        string strIPAddress = string.Empty;
        string strSignUpUrl = string.Empty;
        string strVCode = string.Empty;
        string strSessionVCode = string.Empty;
        string strPermission = string.Empty;


        int intOddsType = 1;
        System.DateTime dtDOB = DateTime.MinValue;
        string strHiddenValues = hidValues.Value;

        List<string> lstValues = null;
        #endregion

        #region populateVariables

        strMemberCode = txtUsername.Text.Trim();
        strPassword = txtPassword.Text;
        strEmail = txtEmail.Text;
        strContact = txtContact.Text;
        strContactNumber = string.Format("{0}-{1}", drpContactCountry.SelectedValue, strContact);
        strDOB = string.Format("{0}-{1}-{2}", drpYear.SelectedValue, drpMonth.SelectedValue, drpDay.SelectedValue);
        //strFName = System.Text.RegularExpressions.Regex.Replace(txtFirstName.Text, @"\t|\n|\r|", "");
        //strLName = System.Text.RegularExpressions.Regex.Replace(txtLastName.Text, @"\t|\n|\r|", "");
        strFName = System.Text.RegularExpressions.Regex.Replace(txtFullName.Text, @"\t|\n|\r|", "");
        strCurrencyCode = drpCurrency.SelectedValue;
        strCountryCode = drpCountry.SelectedValue;
        strAccount = txtAccount.Text.Trim(); ;
        strReferralId = txtReferralID.Text.Trim();
        strLanguageCode = drpLanguage.SelectedValue;
        strCommissionType = drpCommissionType.SelectedValue;
        strAddress = txtAddress.Text.Trim();
        strCity = txtCity.Text.Trim();
        strPostal = txtPostal.Text.Trim();
        strUrl1 = txtURL1.Text.Trim();
        strUrl2 = txtURL2.Text.Trim();
        strUrl3 = txtURL3.Text.Trim();
        strDesc = txtDesc.Text.Trim();

        strVCode = txtCaptcha.Text;
        strSessionVCode = commonVariables.GetSessionVariable("vCode");
        strAlertCode = "-1";

        //strAffiliateId = txtAffiliateID.Text;

        System.Text.RegularExpressions.Regex rexContact = new System.Text.RegularExpressions.Regex("([0-9]{1,4})[-]([0-9]{6,12})$");
        #endregion

        #region parametersValidation

        strResultCode = "11";
        strResultDetail = "Error:ParameterValidation";

        txtCaptcha.Text = string.Empty;

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
        else if (string.IsNullOrEmpty(strFName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingFName", xeErrors);
            isProcessAbort = true;
        }
        //else if (string.IsNullOrEmpty(strLName))
        //{
        //    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingLName", xeErrors);
        //    isProcessAbort = true;
        //}
        else if (string.IsNullOrEmpty(strCurrencyCode) || string.Compare(strCurrencyCode, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingCurrency", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strCountryCode) || string.Compare(strCountryCode, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingCountryCode", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strLanguageCode) || string.Compare(strLanguageCode, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingLanguageCode", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strCommissionType) || string.Compare(strCommissionType, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingCommissionType", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strVCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingVCode", xeErrors);
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
        else if (commonValidation.isInjection(strCountryCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidCountryCode", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strFName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidFName", xeErrors);
            isProcessAbort = true;
        }
        //else if (commonValidation.isInjection(strLName))
        //{
        //    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidLName", xeErrors);
        //    isProcessAbort = true;
        //}
        else if (commonValidation.isInjection(strAccount))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidAccount", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strReferralId))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidReferralId", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strLanguageCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidLanguageCode", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strCommissionType))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidCommissionType", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strAddress))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidAddress", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strCity))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidCity", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strPostal))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidPostal", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strUrl1))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidUrl1", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strUrl2))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidUrl2", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strUrl3))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidUrl3", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strDesc))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidDesc", xeErrors);
            isProcessAbort = true;
        }

        else if (commonValidation.isInjection(strVCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidVCode", xeErrors);
            isProcessAbort = true;
        }

        else if (!DateTime.TryParse(strDOB, out dtDOB))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidDOB", xeErrors);
            isProcessAbort = true;
        }
        //else if (!chkDisclaimer.Checked)
        //{
        //    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/chkDisclaimer", xeErrors);
        //    isProcessAbort = true;
        //}

        else if (string.Compare(commonEncryption.encrypting(strVCode), strSessionVCode, true) != 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/IncorrectVCode", xeErrors);
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
        strProcessRemark = string.Format("strAlertMessage: {0} | HiddenValues: {1}", strAlertMessage, strHiddenValues);

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "ParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

        #endregion

        if (!isProcessAbort)
        {
            lstValues = strHiddenValues.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

            if (lstValues.Count > 0)
            {
                //if (lstValues[0] != null) { strCountryCode = lstValues[0]; }
                //if (lstValues[1] != null) { strSignUpUrl = string.Format("m.{0}", lstValues[1]); }
                if (lstValues[2] != null) { strIPAddress = lstValues[2]; }
                if (lstValues[3] != null) { strPermission = lstValues[3]; }
            }

            strSignUpUrl = string.Format("m.{0}", commonIp.DomainName);
            strLanguageCode = commonVariables.SelectedLanguage;

            if (string.IsNullOrEmpty(strIPAddress)) { strIPAddress = commonIp.UserIP; }

            if (string.IsNullOrEmpty(strCountryCode) || string.Compare(strCountryCode, "-", true) == 0)
            {
                using (wsIP2Loc.ServiceSoapClient wsInstance = new wsIP2Loc.ServiceSoapClient())
                {
                    wsInstance.location(strIPAddress, ref strCountryCode, ref strPermission);
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
                                                                     
            int intAffiliateId = string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")) ? (string.IsNullOrEmpty(strAffiliateId) ? 0 : Convert.ToInt32(strAffiliateId)) : Convert.ToInt32(commonVariables.GetSessionVariable("AffiliateId"));
            string strReferBy = string.Empty;
            string strDeviceId = "Mobile";

            long referralid = 0;
            try
            {
                referralid = long.Parse(strReferralId);
            }
            catch (Exception)
            {
                referralid = 0;
            }

            //System.Data.DataSet dsRegister = null;
            int result = 0;

            //using (wsMemberMS1.memberWSSoapClient svcInstance = new wsMemberMS1.memberWSSoapClient())
            using (wsAffiliateMS1.affiliateWSSoapClient svcInstance = new wsAffiliateMS1.affiliateWSSoapClient())
            {
                //dsRegister = svcInstance.MemberRegistrationNew(lngOperatorId, strMemberCode, strPasswordEncrypted, strEmail, strContactNumber,
                //            strAddress, strCity, strPostal, strCountryCode, strCurrencyCode, strGender, intOddsType, strLanguageCode,
                //            intAffiliateId, strReferBy, strIPAddress, strSignUpUrl, strDeviceId, isTestAccount, strFName, strLName, dtDOB, string.Empty);
                              
                result = svcInstance.MemberRegistration(strMemberCode, strPasswordEncrypted, strFName, strEmail, strContactNumber, strCountryCode, strLanguageCode, strCurrencyCode, lngOperatorId,
                            strAddress, strCity, strPostal, strIPAddress, strDesc, referralid, strUrl1, strUrl2, strUrl3, dtDOB, strAccount, strCommissionType, strSignUpUrl, 
                            (string)System.Configuration.ConfigurationManager.AppSettings.Get("internal_affiliate"));

                //strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | Password: {2} | Email: {3} | Contact: {4} | Address: {5} | City: {6} | Postal: {6} | Country: {8} | Currency: {9} | Gender: {10} | OddsType: {11} | Language: {12} | Affiliate: {13} | ReferBy: {14} | IP: {15} | SignUpUrl: {16} | DeviceID: {17} | TestAccount: {18} | FName: {19} | LName: {20} | DOB: {21} | REMOTEIP: {22} | FORWARDEDIP: {23} | REQUESTERIP: {24} | AffiliateID: {25}",
                //    lngOperatorId, strMemberCode, strPasswordEncrypted, strEmail, strContact, strAddress, strCity, strPostal, strCountryCode, strCurrencyCode, strGender, intOddsType, strLanguageCode, intAffiliateId, strReferBy, strIPAddress, strSignUpUrl, strDeviceId, isTestAccount, strFName, strLName, dtDOB, commonIp.remoteIP, commonIp.forwardedIP, commonIp.requesterIP, intAffiliateId);

                strProcessRemark = "exec spAffiliateMemberInsertWS " + "'" + strMemberCode + "'" + ",'" + strPasswordEncrypted + "'" + ",'" + strFName + "'" + ",'" + strEmail + "'" + ",'" + strContactNumber + "'" + ",'" + strCountryCode + "'" + ",'" + strLanguageCode + "'" + ",'" + strCurrencyCode + "'" + ",'" + lngOperatorId + "'" + ",'" + strAddress + "'" + ",'" + strCity + "'" + ",'" + strPostal + "'" + ",'" + strIPAddress + "'" + ",'" + strDesc + "'" + ",'" + referralid + "'" + ",'" + strUrl1 + "'" + ",'" + strUrl2 + "'" + ",'" + strUrl3 + "'" + ",'" + dtDOB + "'" + ",'" + strAccount + "'" + ",'" + strCommissionType + "'" + ",'" + strSignUpUrl + "'" + ",'" + (string)System.Configuration.ConfigurationManager.AppSettings.Get("internal_affiliate") + "'"; 

                intProcessSerialId += 1;
                commonAuditTrail.appendLog("system", strPageName, "RegistrationParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

                strResultCode = "21";
                strResultDetail = "Error:MemberRegistrationNew";

                //if (dsRegister.Tables[0].Rows.Count > 0)
                //{
                //    strProcessCode = Convert.ToString(dsRegister.Tables[0].Rows[0]["RETURN_VALUE"]);

                //switch (strProcessCode)
                switch (result.ToString())
                {
                    case "0":
                        strAlertMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                        break;

                    case "1":
                        strAlertCode = "1";
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/Success", xeErrors);
                        //string strMemberSessionId = Convert.ToString(dsRegister.Tables[0].Rows[0]["memberSessionId"]);
                        //HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsRegister.Tables[0].Rows[0]["memberSessionId"]));
                        //HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsRegister.Tables[0].Rows[0]["memberId"]));
                        //HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["memberCode"]));
                        //HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["countryCode"]));
                        //HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["currency"]));
                        //HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsRegister.Tables[0].Rows[0]["languageCode"]));
                        //HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsRegister.Tables[0].Rows[0]["riskId"]));
                        ////HttpContext.Current.Session.Add("PaymentGroup", "A"); //Convert.ToString(dsSignin.Tables[0].Rows[0]["paymentGroup"]));
                        //HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsRegister.Tables[0].Rows[0]["partialSignup"]));
                        //HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsRegister.Tables[0].Rows[0]["resetPassword"]));

                        //commonCookie.CookieS = strMemberSessionId;
                        //commonCookie.CookieG = strMemberSessionId;
                        //HttpContext.Current.Session.Add("LoginStatus", "success");

                        //strResultCode = "00";
                        //strResultDetail = "OK:MemberRegistrationNew";

                        //#region IOVATION
                        //this.IovationSubmit(ref intProcessSerialId, strProcessId, strPageName, strMemberCode, strIPAddress, strPermission);
                        //#endregion
                        break;

                    case "10":
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/DuplicateUsername", xeErrors);
                        break;

                    case "11":
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/DuplicateEmail", xeErrors);
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
}