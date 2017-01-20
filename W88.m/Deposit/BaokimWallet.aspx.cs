using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Deposit_BaokimWallet : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.Baokim);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.Baokim);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblDepositAmount.Text = base.strlblAmount;
            ProcessCallBack();
        }
    }

    private void ProcessCallBack()
    {
        var merchantAcctId = Request.Params["merchant_id"];
        var amount = Request.Params["requestAmount"];
        var email = Request.Params["email"];
        var phone = Request.Params["phone_no"];
        var accepted = Request.Params["accepted"];
        var message = Request.Params["message"];
        var checksum = Request.Params["checksum"];

        if (string.IsNullOrWhiteSpace(amount) || string.IsNullOrWhiteSpace(email))
        {
            Response.Redirect("Baokim.aspx");
        }
        else
        {
            if (!commonValidation.IsValidString(amount, 16))
            {
                //commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "invalid strAmount", Convert.ToString(processSerialId), processId, false);
                Response.Write("invalid parameter");
                return;
            }

            if (!commonValidation.IsValidAmount(amount))
            {
                //commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "invalid requestAmount", Convert.ToString(processSerialId), processId, false);
                Response.Write("invalid parameter");
                return;
            }

            if (!commonValidation.IsEmail(email))
            {
                //commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "invalid email", Convert.ToString(processSerialId), processId, false);
                Response.Write("invalid parameter");
                return;
            }

            //commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);
            hfAmount.Value = amount;
            hfEmail.Value = email;
            hfAccepted.Value = accepted;
            hfChkSum.Value = checksum;
            hfMessage.Value = message;
            hfPhone.Value = phone;
            MchtId.Value = merchantAcctId;
        }
       
    }

}