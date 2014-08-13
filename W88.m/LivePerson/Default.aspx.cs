using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LivePerson_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);
        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
        string strLPNumber = string.Empty;
        string strAppKey = string.Empty;
        string strSkill = string.Empty;
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");

        if (!Page.IsPostBack)
        {

            System.Text.StringBuilder sbJSDictionary = new System.Text.StringBuilder();
            foreach (System.Xml.Linq.XElement xeLabel in xeResources.Elements())
            {
                sbJSDictionary.Append("dictionary.setData('" + xeLabel.Name + "', '" + xeLabel.Value + "');");
            }

            strLPNumber = opSettings.Values.Get("LPNumber");
            strAppKey = opSettings.Values.Get("LPAppKey");
            strSkill = commonCulture.ElementValues.getResourceString("lblSkill", xeResources);

            btnSend.InnerText = commonCulture.ElementValues.getResourceString("btnSend", xeResources);
            btnReqChat.InnerText = commonCulture.ElementValues.getResourceString("btnReqChat", xeResources);
            btnEndChat.InnerText = commonCulture.ElementValues.getResourceString("btnEndChat", xeResources);
            //LPVariables.SessionId = '" + System.Guid.NewGuid().ToString().ToUpper() + "';
            if (litScript != null) { litScript.Text += "<script type='text/javascript'> $(function () { LPVariables.LPNumber = '" + strLPNumber + "'; LPVariables.AppKey = '" + commonEncryption.decrypting(strAppKey) + "'; LPVariables.Skill = '" + strSkill + "'; LPVariables.VisitorName = '" + commonVariables.GetSessionVariable("MemberCode") + "'; LPVariables.SessionId = '" + System.Guid.NewGuid().ToString().ToUpper() + "'; " + Convert.ToString(sbJSDictionary) + " });</script>"; }
        }
    }
}