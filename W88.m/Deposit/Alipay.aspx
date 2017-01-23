<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Alipay.aspx.cs" Inherits="Deposit_Alipay" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/JS/modules/gateways/defaultpayments.js"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/autoroute.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <% if (commonCookie.CookieIsApp != "1")
               { %>
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <% } %>

            <h1 class="title" id="headerTitle"><%=commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div class="toggle-list-box">
                <button class="toggle-list-btn btn-active" id="activeDepositTabs"></button>
                <ul class="toggle-list hidden" id="depositTabs">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMode" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMode" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMinMaxLimit" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMinMaxLimit" runat="server" />
                        </div>
                    </li>
                    <li class="item-text-wrap ali-pay-note">
                        <span id="paymentNote"></span>
                        <p id="paymentNoteContent"></p>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" />
                        </div>
                    </li>
                </ul>
            </form>
        </div>

        <% if (commonCookie.CookieIsApp != "1")
           { %>
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <% } %>

         <style>
            li.ali-pay-note {
                font-size: 70%;
            }

             li.ali-pay-note #paymentNote {
                 color: red;
                 font-weight: bold;
             }

             li.ali-pay-note #paymentNoteContent {
                 padding-top: 5px;
             }
        </style>

        <script type="text/javascript">
            $(document).ready(function () {
                window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

                $("#paymentNote").text(_w88_contents.translate("data.LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE1"));

                $('#form1').submit(function (e) {
                    window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
                    // use api
                    e.preventDefault();

                    var data = {
                        Amount: $('#txtDepositAmount').val(),
                        ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx"
                    };

                    window.w88Mobile.Gateways.AutoRoute.Deposit(window.w88Mobile.Gateways.DefaultPayments.AutoRouteIds.AliPay, data, function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                if (response.ResponseData.VendorRedirectionUrl) {
                                    window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                                } else {
                                    if (response.ResponseData.PostUrl) {
                                        w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");

                                    w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                                    w88Mobile.PostPaymentForm.submit();
                                } else if (response.ResponseData.DummyURL) {
                                    w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> ");

                                    window.open(response.ResponseData.DummyURL, '_blank');
                                } else {
                                    w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
                                }
                        }

                        $('#form1')[0].reset();

                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage);

                        break;
                }
                    },
                    function () {
                        window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');
                        GPInt.prototype.HideSplash();
                    });
                });
            });
        </script>
    </div>
</body>
</html>
