using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_UpdateProfile : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { base.CheckLogin(); }

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/_Secure/UpdateProfile", out xeResources);
        System.Data.DataSet dsMemberProfile = null;

        string strOperatorId = string.Empty;
        string strMemberId = string.Empty;

        #region UneditableFieldsVariablesInitialise
        string strMemberCode = string.Empty;
        string strCountryCode = string.Empty;
        string strDOB = string.Empty;
        string strCurrencyCode = string.Empty;

        string strEmail = string.Empty;
        string strContact = string.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        System.DateTime dtDOB = System.DateTime.MinValue;
        #endregion

        #region AddressDetailInitialisation
        string strAddress = string.Empty;
        string strCity = string.Empty;
        string strPostal = string.Empty;
        #endregion

        #region OtherDetails
        string strGender = string.Empty;
        string strOdds = string.Empty;
        string strLanguage = string.Empty;
        string strSecurityQuestion = string.Empty;
        string strSecurityAnswer = string.Empty;
        #endregion

        if (!Page.IsPostBack)
        {
            strOperatorId = commonVariables.OperatorId;
            strMemberId = commonVariables.GetSessionVariable("MemberId");

            lblFirstName.Text = commonCulture.ElementValues.getResourceString("lblFirstName", xeResources);
            //lblLastName.Text = commonCulture.ElementValues.getResourceString("lblLastName", xeResources);
            lblDOB.Text = commonCulture.ElementValues.getResourceString("lblDOB", xeResources);
            lblCountry.Text = commonCulture.ElementValues.getResourceString("lblCountry", xeResources);

            lblGender.Text = commonCulture.ElementValues.getResourceString("lblGender", xeResources);
            lblLanguage.Text = commonCulture.ElementValues.getResourceString("lblLanguage", xeResources);
            lblOdds.Text = commonCulture.ElementValues.getResourceString("lblOdds", xeResources);

            lblAddress.Text = commonCulture.ElementValues.getResourceString("lblAddress", xeResources);
            lblCity.Text = commonCulture.ElementValues.getResourceString("lblCity", xeResources);
            lblPostal.Text = commonCulture.ElementValues.getResourceString("lblPostal", xeResources);

            lblSecurityQuestion.Text = commonCulture.ElementValues.getResourceString("lblSecurityQuestion", xeResources);
            lblSecurityAnswer.Text = commonCulture.ElementValues.getResourceString("lblSecurityAnswer", xeResources);

            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            foreach (System.Xml.Linq.XElement xeGender in xeResources.Element("drpGender").Elements())
            {
                drpGender.Items.Add(new ListItem(xeGender.Value, Convert.ToString(xeGender.Name)));
            }

            foreach (System.Xml.Linq.XElement xeLang in xeResources.Element("drpLanguage").Elements())
            {
                drpLanguage.Items.Add(new ListItem(xeLang.Value, Convert.ToString(xeLang.Name)));
            }

            foreach (System.Xml.Linq.XElement xeOdds in xeResources.Element("drpOdds").Elements())
            {
                drpOdds.Items.Add(new ListItem(xeOdds.Value, Convert.ToString(xeOdds.Attribute("id").Value)));
            }


            using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
            {
                dsMemberProfile = wsInstance.GetMemberInfo(Convert.ToInt64(strOperatorId), Convert.ToInt64(strMemberId));

                if (dsMemberProfile.Tables.Count > 0)
                {
                    if (dsMemberProfile.Tables[0].Rows.Count == 1)
                    {
                        #region UneditableFieldsVariablesPopulate
                        strEmail = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["email"]);
                        strContact = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["mobile"]);
                        strMemberCode = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["memberCode"]);
                        strFName = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["firstName"]);
                        strLName = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["lastName"]);
                        dtDOB = Convert.ToDateTime(dsMemberProfile.Tables[0].Rows[0]["dob"]);
                        strDOB = dtDOB.ToString(commonVariables.DisplayDateFormat);
                        strCurrencyCode = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["currencyCode"]);
                        strCountryCode = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["countryCode"]);
                        #endregion

                        #region AddressDetails
                        strAddress = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["address"]);
                        strCity = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["city"]);
                        strPostal = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["postal"]);
                        #endregion

                        #region OtherDetails
                        strGender = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["gender"]);
                        strOdds = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["oddsType"]);
                        strLanguage = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["languageCode"]);
                        strSecurityQuestion = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["securityQuestion"]);
                        strSecurityAnswer = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["securityAnswer"]);
                        #endregion

                        txtEmail.Text = strEmail;
                        txtUserName.Text = strMemberCode;
                        txtContact.Text = strContact;
                        txtFirstName.Text = strFName;
                        //txtLastName.Text = strLName;
                        txtDOB.Text = strDOB;
                        txtCountry.Text = strCountryCode;
                        txtCurrency.Text = commonCulture.ElementValues.getResourceXPathString("Currency/" + strCurrencyCode, xeResources);

                        txtAddress.Text = strAddress;
                        txtCity.Text = strCity;
                        txtPostal.Text = strPostal;

                        drpGender.SelectedValue = strGender;
                        drpLanguage.SelectedValue = strLanguage;
                        drpOdds.SelectedValue = "3";

                        txtSecurityAnswer.Text = strSecurityAnswer;
                    }
                }
            }

            foreach (System.Xml.Linq.XElement xeSQ in xeResources.Element("drpSecurityQuestion").Elements())
            {
                if (string.Compare(Convert.ToString(xeSQ.Name).Substring(2), "0", true) != 0)
                {
                    drpSecurityQuestion.Items.Add(new ListItem(xeSQ.Value, Convert.ToString(xeSQ.Name).Substring(2)));
                }
                else if (string.IsNullOrEmpty(strSecurityQuestion) || Convert.ToInt32(strSecurityQuestion) < 1)
                {
                    drpSecurityQuestion.Items.Add(new ListItem(xeSQ.Value, Convert.ToString(xeSQ.Name).Substring(2)));
                }
            }

            drpSecurityQuestion.SelectedIndex = Convert.ToInt32(strSecurityQuestion);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "UpdateProfile";

        string strProcessCode = string.Empty;

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = 1;
        
        string strEmail = string.Empty;
        string strContact = string.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        string strDOB = string.Empty;
        string strCountryCode = string.Empty;
        string strCurrencyCode = string.Empty;
        System.DateTime dtDOB = System.DateTime.MinValue;        

        string strGender = string.Empty;
        string strLanguageCode = string.Empty;
        string strOddsType = string.Empty;
        string strAddress = string.Empty;
        string strCity = string.Empty;
        string strPostal = string.Empty;
        string strSecurityQuestion = string.Empty;
        string strSecurityAnswer = string.Empty;

        string strMemberMS1Id = string.Empty;
        string strPassword = string.Empty;
        string strPasswordEncrypted = string.Empty;

        int intResult = int.MinValue;

        #region populateVariables
        strAlertCode = "-1";

        strMemberMS1Id = commonVariables.GetSessionVariable("MemberId");
        strEmail = txtEmail.Text;
        strContact = txtContact.Text;
        strPassword = txtPassword.Text;
        strFName = txtFirstName.Text;
        //strLName = txtLastName.Text;
        strLName = string.Empty;
        strCountryCode = txtCountry.Text;
        strDOB = txtDOB.Text;
        dtDOB = commonConversion.convertDateTime(strDOB, commonVariables.DisplayDateFormat);

        strGender = drpGender.SelectedValue;
        strLanguageCode = drpLanguage.SelectedValue;
        strOddsType = drpOdds.SelectedValue;
        strAddress = txtAddress.Text;
        strCity = txtCity.Text;
        strPostal = txtPostal.Text;
        strSecurityQuestion = drpSecurityQuestion.SelectedValue;
        strSecurityAnswer = txtSecurityAnswer.Text;

        #endregion

        #region parametersValidation

        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strAddress))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingAddress", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strCity))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingCity", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strPostal))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingPostal", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strSecurityAnswer))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingSecurityAnswer", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strAddress))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidAddress", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strCity))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidCity", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strPostal))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidPostal", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strSecurityAnswer))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidSecurityAnswer", xeErrors);
            isProcessAbort = true;
        }
        else
        {
            strPasswordEncrypted = commonEncryption.Encrypt(strPassword);
        }

        #endregion

        if (!isProcessAbort)
        {
            try
            {
                using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    intResult = wsInstance.MemberProfileUpdate(Convert.ToInt64(strMemberMS1Id), strEmail, strContact,
                        strAddress, strCity, strPostal, strGender, strFName, strLName, dtDOB, strPasswordEncrypted, strLanguageCode,
                        Convert.ToInt32(strSecurityQuestion), strSecurityAnswer, Convert.ToInt32(strOddsType));

                    strProcessRemark = string.Format("OperatorId: {0} | MemberId: {1} | Password: {2} | Email: {3} | Contact: {4} | Address: {5} | City: {6} | Postal: {6} | Country: {8} | Currency: {9} | Gender: {10} | OddsType: {11} | Language: {12} | FName: {13} | LName: {14} | DOB: {15} | REMOTEIP: {16} | FORWARDEDIP: {17} | REQUESTERIP: {18}",
                        lngOperatorId, strMemberMS1Id, strPasswordEncrypted, strEmail, strContact, strAddress, strCity, strPostal, strCountryCode, strCurrencyCode, strGender, strOddsType, strLanguageCode, strFName, strLName, dtDOB, commonIp.remoteIP, commonIp.forwardedIP, commonIp.requesterIP);

                    intProcessSerialId += 1;
                    commonAuditTrail.appendLog("system", strPageName, "UpdateProfile", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
                }
            }
            catch (Exception) { }

            switch (intResult)
            {
                case 1: // success
                    strAlertCode = "1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/Success", xeErrors);
                    break;
                case 10: // wrong password
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/IncorrectPassword", xeErrors);
                    break;
                default: // general error
                    strAlertMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                    break;
            }
        }
    }
}