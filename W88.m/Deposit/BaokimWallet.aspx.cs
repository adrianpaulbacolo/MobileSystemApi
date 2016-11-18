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
        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"), base.strCurrencyCode);

        if (!Page.IsPostBack)
        {
            btnSubmit.Text = strbtnSubmit;
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);
            lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblAmount", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);

            InitializeLabels();

            ProcessCallBack();
        }
    }

    private void InitializeLabels()
    {
        lblMode.Text = base.strlblMode;
        txtMode.Text = base.strtxtMode;

        lblMinMaxLimit.Text = base.strlblMinMaxLimit;
        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;

        lblDailyLimit.Text = base.strlblDailyLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;

        lblTotalAllowed.Text = base.strlblTotalAllowed;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;

        lblDepositAmount.Text = base.strlblAmount;

        btnSubmit.Text = base.strbtnSubmit;

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