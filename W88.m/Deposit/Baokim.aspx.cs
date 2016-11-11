using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class Deposit_Baokim : PaymentBasePage
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

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", lblDepositAmount.Text, strCurrencyCode));

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);

            InitializeLabels();
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
        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.strtxtAmount);

        btnSubmit.Text = base.strbtnSubmit;

    }


    [WebMethod]
    public static string PostData(string jsonData)
    {
        var postInfo = commonFunctions.Deserialize<dynamic>(jsonData);
        var key = ConfigurationManager.AppSettings.Get("PaymentPrivateKey");

        using (var client = new WebClient())
        {
            client.QueryString.Add("signature", WebUtility.UrlEncode(postInfo.FormData.signInfo));

            var user = commonEncryption.decrypting(postInfo.FormData.uname, key);
            var pass = commonEncryption.decrypting(postInfo.FormData.pass, key);
            client.Credentials = new NetworkCredential(user, pass);
          
            var nameValueCollection = new NameValueCollection
            {
                {"order_id", postInfo.FormData.order_id},
                {"business", commonEncryption.decrypting(postInfo.FormData.business, key)},
                {"total_amount", postInfo.FormData.total_amount},
                {"url_success", postInfo.FormData.url_success},
                {"payer_name", postInfo.FormData.payer_name},
                {"payer_email", postInfo.FormData.payer_email},
                {"payer_phone_no", postInfo.FormData.payer_phone_no},
                {"bank_payment_method_id", postInfo.FormData.bank_payment_method_id}
            };

            var response = Encoding.UTF8.GetString(client.UploadValues(postInfo.PostUrl, nameValueCollection));

            var json = (JObject)JsonConvert.DeserializeObject(response);

            //var nextAction = json.Property("next_action");
            var redirectUrl = json.Property("redirect_url");
            var guideUrl = json.Property("guide_url");
            //var rvId = json.Property("rvid");
            //var error = json.Property("error");
            //var errorCode = json.Property("error_code");

            return  redirectUrl != null ? redirectUrl.Value.ToString() : guideUrl.Value.ToString();
        }
    }
}