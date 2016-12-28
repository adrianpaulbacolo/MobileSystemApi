using System;
using System.Activities.Validation;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Helpers;
using svcPayMember;

/// <summary>
/// common functionalities of payment/deposit method
/// </summary>
public static class commonPaymentMethodFunc
{

    public static MemberSecondaryBank[] GetSecondaryBanks()
    {
        using (var client = new MemberClient())
        {
            string statusCode, statusText;
            return client.getSecondaryBankAccounts(Convert.ToInt64(commonVariables.OperatorId), commonCookie.CookieCurrency, "vn", out statusCode, out statusText);

        }
    }

    public static Task<getWalletBalanceResponse> GetWalletBalanceAsync(int walletId)
    {
        using (var svcInstance = new MemberClient())
        {
            var member = new Members();
            var info = member.MemberData();

            if (string.IsNullOrWhiteSpace(info.MemberCode))
            {
                member.CheckMemberSession(info.CurrentSessionId);
                info = member.MemberData();
            }

            var request = new getWalletBalanceRequest(commonVariables.OperatorId, commonVariables.SiteUrl, info.MemberCode, Convert.ToString(walletId));
            return svcInstance.getWalletBalanceAsync(request);
        }
    }


    public static Task<string> GetWalletBalancesAsync()
    {
        using (var svcInstance = new MemberClient())
        {
            var member = new Members();
            var info = member.MemberData();

            if (string.IsNullOrWhiteSpace(info.MemberCode))
            {
                member.CheckMemberSession(info.CurrentSessionId);
                info = member.MemberData();
            }

            return svcInstance.getBalancesAsync(commonVariables.OperatorId, commonVariables.SiteUrl, info.MemberCode);
        }
    }

    public static ICollection<KeyValuePair<int, string>> GetWallets()
    {
        ICollection<KeyValuePair<int, string>> wallet = new Dictionary<int, string>();
        var obj = new Wallets();

        foreach (var info in obj.WalletInfo.OrderBy(x => x.SelectOrder))
        {
            var selectName = string.IsNullOrWhiteSpace(info.SelectName) ? info.Name : info.SelectName;
            wallet.Add(new KeyValuePair<int, string>(Convert.ToInt32(info.Id), selectName));
        }

        return wallet;
    }


    public static void GetDepositMethodList(string methodsUnAvailable, HtmlGenericControl depositTabs, string sourcePage, bool isApp, string currencyCode)
    {
        var depositList = Enum.GetValues(typeof(commonVariables.DepositMethod));

        string[] methodUnavailable = methodsUnAvailable.Split('|');

        bool hasMethod = false;

        HtmlGenericControl depositTabsList = new HtmlGenericControl("ul");
        foreach (commonVariables.DepositMethod depositItem in depositList)
        {
            string paymentCode = Convert.ToString((int)depositItem);

            bool isUnavailable = methodUnavailable.Contains(paymentCode);
            if (!isUnavailable)
            {
                hasMethod = true;
                SetDepositMethodListLink(depositItem, depositTabsList, sourcePage, currencyCode);
            }
        }

        if (hasMethod) depositTabs.Controls.Add(depositTabsList);

    }

    private static void SetDepositMethodListLink(commonVariables.DepositMethod paymentCode, HtmlGenericControl depositTabs, string sourcePage, string currencyCode)
    {
        HtmlGenericControl anchor;
        HtmlGenericControl list;

        bool isApp = commonCookie.CookieIsApp == "1";

        switch (paymentCode)
        {
            case commonVariables.DepositMethod.JutaPay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                anchor.Attributes.Add("href", "/Deposit/JutaPay.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.FastDeposit:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/FastDeposit_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/FastDeposit.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.NextPay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/NextPay_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/NextPay.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.WingMoney:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/WingMoney_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/WingMoney.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.SDPay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/SDPay_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/SDPay.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.SDAPayAlipay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/SDAPay_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/SDAPay.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.Help2Pay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/Help2Pay_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/Help2Pay.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.DaddyPay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/DaddyPay_app.aspx?value=1");
                else
                    anchor.Attributes.Add("href", "/Deposit/DaddyPay.aspx?value=1");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.BaokimScratchCard:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                anchor.Attributes.Add("href", "/Deposit/BaokimScratchCard.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;
            //case commonVariables.DepositMethod.DaddyPayQR:
            //    list = CreateMethodListControl(paymentCode);

            //    anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

            //    if (isApp)
            //        anchor.Attributes.Add("href", "/Deposit/DaddyPay_app.aspx?value=2");
            //    else
            //        anchor.Attributes.Add("href", "/Deposit/DaddyPay.aspx?value=2");

            //    list.Controls.Add(anchor);
            //    depositTabs.Controls.Add(list);
            //    break;

            case commonVariables.DepositMethod.Neteller:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/Neteller_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/Neteller.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.ECPSS:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/ECPSS_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/ECPSS.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.PaySec:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                anchor.Attributes.Add("href", "/Deposit/PaySec.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.BofoPay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/BofoPay_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/BofoPay.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.AllDebit:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/AllDebit_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/AllDebit.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.JTPayWeChat:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                anchor.Attributes.Add("href", "/Deposit/WeChat");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.JTPayAliPay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                anchor.Attributes.Add("href", "/Deposit/AliPay");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.EGHL:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                if (isApp)
                    anchor.Attributes.Add("href", "/Deposit/EGHL_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Deposit/EGHL.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.NganLuong:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                anchor.Attributes.Add("href", "/Deposit/NganLuong.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.ShengPayAliPay:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);
                anchor.Attributes.Add("href", "/Deposit/ShengPayAliPay.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            case commonVariables.DepositMethod.VenusPoint:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage, currencyCode);

                anchor.Attributes.Add("href", "/Deposit/VenusPoint.aspx");

                list.Controls.Add(anchor);
                depositTabs.Controls.Add(list);
                break;

            default:
                break;
        }
    }


    private static HtmlGenericControl CreateMethodListControl(commonVariables.DepositMethod paymentCode)
    {
        HtmlGenericControl list = new HtmlGenericControl("li");
        list.ID = string.Format("d{0}", paymentCode);

        return list;
    }

    private static HtmlGenericControl CreateMethodListControl(commonVariables.WithdrawalMethod paymentCode)
    {
        HtmlGenericControl list = new HtmlGenericControl("li");
        list.ID = string.Format("w{0}", paymentCode);

        return list;
    }

    private static HtmlGenericControl CreateMethodLinkControl(string paymentId, string paymentCode, string sourcePage, string currencyCode = null)
    {
        HtmlGenericControl anchor = new HtmlGenericControl("a");
        anchor.Attributes.Add("class", "ui-link ui-btn");
        anchor.Attributes.Add("data-ajax", "false");

        if (currencyCode != null && currencyCode.Equals("IDR", StringComparison.OrdinalIgnoreCase) && paymentId.Equals("dEGHL", StringComparison.OrdinalIgnoreCase))
        {
            anchor.InnerText = "ATM Online";
        }
        else
        {
            anchor.InnerText = commonCulture.ElementValues.getResourceString(paymentId, commonVariables.PaymentMethodsXML);
        }

        if (string.Compare(sourcePage, paymentCode, true) == 0)
        {
            anchor.Attributes.Add("class", "btn-primary");
        }

        return anchor;
    }

    public static void GetWithdrawalMethodList(string methodsUnAvailable, HtmlGenericControl withdrawalTabs, string sourcePage, bool isApp)
    {
        var withdrawalList = Enum.GetValues(typeof(commonVariables.WithdrawalMethod));

        string[] methodUnavailable = methodsUnAvailable.Split('|');
        foreach (commonVariables.WithdrawalMethod withdrawalItem in withdrawalList)
        {
            string paymentCode = Convert.ToString((int)withdrawalItem);

            bool isUnavailable = methodUnavailable.Contains(paymentCode);
            if (!isUnavailable)
            {
                SetWithdrawalMethodListLink(withdrawalItem, withdrawalTabs, sourcePage);
            }
        }
    }

    private static void SetWithdrawalMethodListLink(commonVariables.WithdrawalMethod paymentCode, HtmlGenericControl withdrawalTabs, string sourcePage)
    {
        HtmlGenericControl anchor;
        HtmlGenericControl list;

        bool isApp = commonCookie.CookieIsApp == "1";

        switch (paymentCode)
        {
            case commonVariables.WithdrawalMethod.BankTransfer:

                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage);

                if (isApp)
                    anchor.Attributes.Add("href", "/Withdrawal/BankTransfer_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Withdrawal/BankTransfer.aspx");

                list.Controls.Add(anchor);
                withdrawalTabs.Controls.Add(list);
                break;

            case commonVariables.WithdrawalMethod.VenusPoint:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage);

                anchor.Attributes.Add("href", "/Withdrawal/VenusPoint.aspx");

                list.Controls.Add(anchor);
                withdrawalTabs.Controls.Add(list);
                break;

            case commonVariables.WithdrawalMethod.WingMoney:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage);

                if (isApp)
                    anchor.Attributes.Add("href", "/Withdrawal/WingMoney_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Withdrawal/WingMoney.aspx");

                list.Controls.Add(anchor);
                withdrawalTabs.Controls.Add(list);
                break;

            case commonVariables.WithdrawalMethod.Neteller:
                list = CreateMethodListControl(paymentCode);

                anchor = CreateMethodLinkControl(list.ID, paymentCode.ToString(), sourcePage);
                if (isApp)
                    anchor.Attributes.Add("href", "/Withdrawal/Neteller_app.aspx");
                else
                    anchor.Attributes.Add("href", "/Withdrawal/Neteller.aspx");

                list.Controls.Add(anchor);
                withdrawalTabs.Controls.Add(list);
                break;

            default:
                break;
        }
    }
}