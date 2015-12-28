using System;

public partial class _Change_Password : BasePage
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

        if (!Page.IsPostBack)
        {
            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblPasswordNew.Text = commonCulture.ElementValues.getResourceString("lblPasswordNew", xeResources);
            lblPasswordConfirm.Text = commonCulture.ElementValues.getResourceString("lblPasswordConfirm", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Variable Initialization
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
        #endregion

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
                    strAlertCode = "1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/Success", xeErrors);
                    break;
                case 10: // invalid password
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdatePassword/InvalidPassword", xeErrors);
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
