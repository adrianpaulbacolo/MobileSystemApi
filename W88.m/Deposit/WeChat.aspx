<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="WeChat.aspx.cs" Inherits="Deposit_WeChat" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
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

    <script type="text/javascript" src="/_Static/JS/modules/gateways/autoroute.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            $('span[id$="paymentNote"]').text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
            $('p[id$="paymentNoteContent"]').text(_w88_contents.translate("LABEL_PAYMENT_NOTE0"));

            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('input[id$="btnSubmit"]');
                // use api
                e.preventDefault();

                var data = {
                    Amount: $('input[id$="txtDepositAmount"]').val(),
                    ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx"
                };

                window.w88Mobile.Gateways.AutoRoute.Deposit(window.w88Mobile.Gateways.DefaultPayments.AutoRouteIds.WeChat, data, function (response) {
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
</asp:Content>