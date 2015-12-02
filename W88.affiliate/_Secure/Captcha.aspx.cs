using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Captcha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strVCode = string.Empty;
        strVCode = commonFunctions.generateCaptcha(HttpContext.Current, 4, commonFunctions.randomType.numeric, 600, 90, System.Drawing.Color.FromArgb(65, 65, 65), System.Drawing.Color.FromArgb(65, 65, 65), System.Drawing.Color.GhostWhite);

        string strProcessRemark = "generateCaptcha:" + strVCode;
        int intProcessSerialId = 0;
        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", "Captcha", "ParameterValidation", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);


        if (!Page.IsPostBack)
        {
            //System.Web.HttpContext.Current.Session["vCode"] = commonEncryption.encrypting(strVCode);
        }
    }
}