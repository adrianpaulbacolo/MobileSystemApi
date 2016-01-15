using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// common functionalities of payment/deposit method
/// </summary>
public static class commonPaymentMethodFunc
{
    public static void getDepositMethodList(string methodsUnAvailable, HtmlGenericControl depositTabs, string sourcePage = null)
    {
        var depositList = Enum.GetValues(typeof(commonVariables.DepositMethod));

        string[] methodUnavailable = methodsUnAvailable.Split('|');
        foreach (commonVariables.DepositMethod depositItem in depositList)
        {
            string depositCode = Convert.ToString((int)depositItem);

            bool isUnavailable = methodUnavailable.Contains(depositCode);
            if (!isUnavailable)
            {
                setDepositMethodListLink(depositCode, depositTabs, sourcePage);
            }
        }
    }

    private static void setDepositMethodListLink(string depositCode, HtmlGenericControl depositTabs, string sourcePage)
    {
        HtmlGenericControl anchor;
        HtmlGenericControl list;
        switch (depositCode)
        {
            case "110101": //commonVariables.DepositMethod.FastDeposit:

                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);

                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/FastDeposit");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "default", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case "120204": //commonVariables.DepositMethod.NextPay:
                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);

                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/NextPay");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("nextpay", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "nextpay", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case "110308": //commonVariables.DepositMethod.WingMoney:
                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);

                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/WingMoney");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "wingmoney", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case "120203": //commonVariables.DepositMethod.SDPay:
                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);


                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/SDPay");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "sdpay", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case "120227": //commonVariables.DepositMethod.Help2Pay:
                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);

                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/Help2Pay");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("help2pay", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "help2pay", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case "120243": //commonVariables.DepositMethod.DaddyPay:
                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);

                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/DaddyPay.aspx?value=1");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("daddypay", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "daddypay", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case "120244": //commonVariables.DepositMethod.DaddyPayQR:
                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);

                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/DaddyPay.aspx?value=2");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("daddypayqr", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "daddypayqr", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case "120214": //commonVariables.DepositMethod.Neteller:
                list = new HtmlGenericControl("li");
                list.ID = string.Format("d{0}", depositCode);

                anchor = new HtmlGenericControl("a");

                anchor.Attributes.Add("href", "/Deposit/Neteller.aspx");
                anchor.Attributes.Add("class", "ui-link ui-btn");
                anchor.InnerText = commonCulture.ElementValues.getResourceString("neteller", commonVariables.LeftMenuXML);

                if (string.Compare(sourcePage, "neteller", true) == 0)
                {
                    anchor.Attributes.Add("class", "btn-primary");
                }

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            default:
                break;
        }
    }

}