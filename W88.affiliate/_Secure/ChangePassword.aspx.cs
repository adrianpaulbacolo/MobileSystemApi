using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_ChangePassword : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;
    protected System.Xml.Linq.XElement xeResourcesSecQues = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { 
        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) 
        { 
            Response.Redirect("/Index",true); 
        }  
    }
  
    protected void Page_Load(object sender, EventArgs e)
    {
        string strOperatorId = commonVariables.OperatorId;
        string strAffiliateId = string.Empty;
        xeErrors = commonVariables.ErrorsXML;
    
        commonCulture.appData.getRootResource("/AccountInfo.aspx", out xeResources);
     
        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        //testing
        //System.Web.HttpContext.Current.Session["AffiliateId"] = "20264";

        if (!Page.IsPostBack)
        {
            lblCurrentPassword.Text = commonCulture.ElementValues.getResourceString("lblCurrentPassword", xeResources);
            lblNewPassword.Text = commonCulture.ElementValues.getResourceString("lblNewPassword", xeResources);
            lblConfirmPassword.Text = commonCulture.ElementValues.getResourceString("lblConfirmPassword", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);
            btnCancel.InnerText = commonCulture.ElementValues.getResourceString("btnCancel", xeResources);
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

        string strAffiliateId = string.Empty;
        string strPassword = string.Empty;
        string strPasswordEncrypted = string.Empty;
        string strPasswordNew = string.Empty;
        string strPasswordNewEncrypted = string.Empty;
        string strPasswordConfirm = string.Empty;

        int intResult = int.MinValue;

        #region populateVariables
        strAlertCode = "-1";

        strAffiliateId = commonCookie.CookieAffiliateId;
        strPassword = txtCurrentPassword.Text;
        strPasswordNew = txtNewPassword.Text;
        strPasswordConfirm = txtConfirmPassword.Text;

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
                using (wsAffiliateMS1.affiliateWSSoapClient wsInstanceAff = new wsAffiliateMS1.affiliateWSSoapClient("affiliateWSSoap"))
                {
                    intResult = wsInstanceAff.ChangePassword(long.Parse(commonCookie.CookieAffiliateId), strPasswordEncrypted, strPasswordNewEncrypted);

                    strProcessRemark = string.Format("OperatorId: {0} | AffiliateId: {1} | Password: {2} | PasswordNew: {3} | REMOTEIP: {4} | FORWARDEDIP: {5} | REQUESTERIP: {6}", lngOperatorId, strAffiliateId, strPasswordEncrypted, strPasswordNewEncrypted, commonIp.remoteIP, commonIp.forwardedIP, commonIp.requesterIP);

                    intProcessSerialId += 1;
                    commonAuditTrail.appendLog("system", strPageName, "UpdatePassword", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
                }
            }
            catch (Exception) { }

            switch (intResult)
            {
                case 1: // success
                //case 10:
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