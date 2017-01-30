using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using svcPayDeposit;
using System.Web.Services;
using System.Xml.Linq;

public partial class Deposit_DaddyPay : PaymentBasePage
{
    protected string strPageTitle = string.Empty;
    protected string lblTransactionId;

    protected bool isDaddyPayQR = false;

    private static string weChatNickNameNotAvailable;
    private static string weChatNickNamePendingDeposit;
    private static string serverError;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.DaddyPay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;

        if (Request.QueryString["value"].ToString() == "1")
        {
            base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.DaddyPay);
            strPageTitle = commonCulture.ElementValues.getResourceString("dDaddyPay", commonVariables.PaymentMethodsXML);
        }
        else if (Request.QueryString["value"].ToString() == "2")
        {
            isDaddyPayQR = true;
            base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.DaddyPayQR);
            strPageTitle = commonCulture.ElementValues.getResourceString("dDaddyPayQR", commonVariables.PaymentMethodsXML);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.InitializeLabels();

            string bank = isDaddyPayQR ? "DaddyPayQRBank" : "DaddyPayBank";
            drpBank.Items.AddRange(this.InitializeDaddyPayBank(bank).ToArray());

            string value = Request.QueryString["value"].ToString();
            if (value == "1")
            {
                txtAccountNo.Visible = false;
                txtAccountName.Visible = false;

                lblAccountNumber.Visible = false;
                lblAccountName.Visible = false;
            }
            else if (value == "2")
            {
                txtAccountNo.Visible = true;
                txtAccountName.Visible = true;

                lblAccountNumber.Visible = true;
                lblAccountName.Visible = true;

                this.InitializeWeChatDenominations();
            }
        }
    }

    private void InitializeLabels()
    {
        lblDepositAmount.Text = base.strlblAmount;
        lbldrpDepositAmount.Text = base.strlblAmount;

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        lblBank.Text = base.strlblBank;

        weChatNickNameNotAvailable = commonCulture.ElementValues.getResourceString("wechatNickNameNA", xeResources);
        weChatNickNamePendingDeposit = commonCulture.ElementValues.getResourceString("wechatPendingDeposit", xeResources);
        serverError = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/error" + "1", xeErrors);

        lblTransactionId = base.strlblTransactionId;
    }

    private void InitializeWeChatDenominations()
    {
        List<ListItem> denoms = new List<ListItem>();
        XElement denom = xeResources.Element("denoms");

        denoms.AddRange(denom.Elements("denom").Select(x => new ListItem(x.Value, x.Attribute("id").Value)));

        drpDepositAmount.Items.AddRange(denoms.ToArray());
    }

    protected List<ListItem> InitializeDaddyPayBank(string paymentMethodBank)
    {
        List<ListItem> banks = new List<ListItem>() { new ListItem(strdrpBank, "-1") };

        XElement xElementBank = xeResources.Element(paymentMethodBank);

        banks.AddRange(xElementBank.Elements("bank").Select(bank => new ListItem(bank.Value, bank.Attribute("id").Value)));

        return banks;
    }

    [WebMethod]
    public static string ProcessWeChatNickname(string action, string nickname)
    {
        XElement processResult;

        using (svcPayMember.MemberClient client = new svcPayMember.MemberClient())
        {
            processResult = client.processMemberWeChatNickName(Convert.ToInt64(commonVariables.OperatorId), commonVariables.GetSessionVariable("MemberCode"), action, nickname);
        }

        if (processResult.Element("error") != null)
        {
            string error = processResult.Element("error").Value;

            if (action == "changeNickname" && error == "NA")
            {
                return weChatNickNameNotAvailable;
            }

            if (action == "changeNickname" && error == "PendingDeposit")
            {
                return weChatNickNamePendingDeposit;
            }
        }

        if (processResult.Element("result") != null) //only for success cases will return with result element
        {
            return processResult.Element("result").Value;
        }
        else
        {
            return serverError;
        }
    } 
}