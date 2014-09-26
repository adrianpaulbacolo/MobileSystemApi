using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_UpdatePassword : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { base.CheckLogin(); }

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/_Secure/UpdatePassword", out xeResources);
        System.Data.DataSet dsMemberProfile = null;

        string strOperatorId = string.Empty;
        string strMemberId = string.Empty;

        #region UneditableFieldsVariablesInitialise
        string strMemberCode = string.Empty;
        string strCurrencyCode = string.Empty;
        string strEmail = string.Empty;
        string strContact = string.Empty;
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

            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblPasswordNew.Text = commonCulture.ElementValues.getResourceString("lblPasswordNew", xeResources);
            lblPasswordConfirm.Text = commonCulture.ElementValues.getResourceString("lblPasswordConfirm", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);
            
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
                        strCurrencyCode = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["currencyCode"]);
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
                        txtCurrency.Text = commonCulture.ElementValues.getResourceXPathString("Currency/" + strCurrencyCode, xeResources);                        
                    }
                }
            }
        }
    }

    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "UpdatePassword";

        string strProcessCode = string.Empty;

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = 1;

        string strMemberMS1Id = string.Empty;
        string strPassword = string.Empty;
        string strPasswordEncrypted = string.Empty;
        string strPasswordNew = string.Empty;
        string strPasswordNewEncrypted = string.Empty;
        string strPasswordConfirm = string.Empty;

        int intResult = int.MinValue;

        #region populateVariables
        strAlertCode = "-1";

        strMemberMS1Id = commonVariables.GetSessionVariable("MemberId");
        strPassword = txtPassword.Text;
        strPasswordNew = txtPasswordNew.Text;
        strPasswordConfirm = txtPasswordConfirm.Text;

        #endregion

        #region parametersValidation

        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strPassword))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPassword", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strPasswordNew))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordNew", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strPasswordConfirm))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordConfirm", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strPassword))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/InvalidPassword", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strPasswordNew))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/InvalidPassword", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strPasswordConfirm))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/InvalidPassword", xeErrors);
            isProcessAbort = true;
        }
        else if (string.Compare(strPasswordNew, strPasswordConfirm, true) != 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/UnmatchedPassword", xeErrors);
            isProcessAbort = true;
        }

        else
        {
            strPasswordEncrypted = commonEncryption.Encrypt(strPassword);
            strPasswordNewEncrypted = commonEncryption.Encrypt(strPasswordNew);
        }

        #endregion

        if (!isProcessAbort)
        {
            try
            {
                using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    intResult = wsInstance.MemberChangePassword(Convert.ToInt64(strMemberMS1Id), strPasswordEncrypted, strPasswordNewEncrypted);

                    strProcessRemark = string.Format("OperatorId: {0} | MemberId: {1} | Password: {2} | PasswordNew: {3} | REMOTEIP: {4} | FORWARDEDIP: {5} | REQUESTERIP: {6}", lngOperatorId, strMemberMS1Id, strPasswordEncrypted, strPasswordNewEncrypted, commonIp.remoteIP, commonIp.forwardedIP, commonIp.requesterIP);

                    intProcessSerialId += 1;
                    commonAuditTrail.appendLog("system", strPageName, "UpdatePassword", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
                }
            }
            catch (Exception) { }

            switch (intResult)
            {
                case 1: // success
                case 10:
                    strAlertCode = "1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/Success", xeErrors);
                    break;
                case 11: // wrong password
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/IncorrectPassword", xeErrors);
                    break;
                default: // general error
                    strAlertMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                    break;
            }
        }
    }
}