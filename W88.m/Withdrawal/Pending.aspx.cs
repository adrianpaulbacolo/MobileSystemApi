using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Withdrawal_Pending : BasePage
{
    protected System.Xml.Linq.XElement xeResources = null;
    protected System.Xml.Linq.XElement xeErrors = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");

        xeErrors = commonVariables.ErrorsXML;

        string strOperatorId = string.Empty;
        string strMemberCode = string.Empty;
        string strStatusCode = string.Empty;
        string strStatusText = string.Empty;

        System.Xml.Linq.XElement xePaymentMethods = null;

        svcPayMember.PendingWithdrawal[] arrPendingTrx = null;

        strOperatorId = commonVariables.OperatorId;
        strMemberCode = base.userInfo.MemberCode;;

        if (!Page.IsPostBack)
        {
            commonCulture.appData.getLocalResource(out xeResources);
            commonCulture.appData.getRootResource("PaymentMethods", out xePaymentMethods);

            using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
            {
                arrPendingTrx = svcInstance.getPendingWithdrawal(Convert.ToInt64(strOperatorId), strMemberCode, out strStatusCode, out strStatusText);
            }

            if (arrPendingTrx != null && arrPendingTrx.Length > 0)
            {
                System.Text.StringBuilder sbPendingTrx = new System.Text.StringBuilder();

                sbPendingTrx.Append("<table data-role='table' id='table-reflow' class='ui-responsive table-stroke'>");
                sbPendingTrx.AppendFormat("<thead><tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th></tr></thead><tbody>"
                    , commonCulture.ElementValues.getResourceString("lblTransactionId", xeResources)
                    , commonCulture.ElementValues.getResourceString("lblRequestDateTime", xeResources)
                    , commonCulture.ElementValues.getResourceString("lblPaymentMethod", xeResources)
                    , commonCulture.ElementValues.getResourceString("lblAmount", xeResources)
                    , string.Empty);

                foreach (svcPayMember.PendingWithdrawal trxWithdrawal in arrPendingTrx)
                {
                    sbPendingTrx.AppendFormat("<tr id='tr_{0}'><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td><a href='javascript:void(0)' onclick='javascript:return CancelWithdrawal(this);' data-id='{0}' data-method='{5}'>{4}</a></td></tr>"
                        ,trxWithdrawal.invId, trxWithdrawal.requestDate, commonCulture.ElementValues.getResourceString("w" + trxWithdrawal.payMethodId, xePaymentMethods), trxWithdrawal.requestAmount.ToString(commonVariables.DecimalFormat), commonCulture.ElementValues.getResourceString("btnCancelWithdrawal", xeResources), trxWithdrawal.payMethodId);
                }

                sbPendingTrx.Append("</tbody></table>");

                litPending.Text = Convert.ToString(sbPendingTrx);
            }
            else
            {
                if (litScript != null) { litScript.Text += "<script type='text/javascript'>window.location.replace('/Withdrawal/Default.aspx');</script>"; }
            }
        }
    }
}