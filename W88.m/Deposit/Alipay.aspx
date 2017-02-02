<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="Alipay.aspx.cs" Inherits="Deposit_Alipay" %>

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
    <script type="text/javascript" src="/_Static/JS/modules/gateways/alipay.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/gateways/autoroute.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

                window.w88Mobile.Gateways.Alipay.Initialize();

                $('#form1').submit(function (e) {
                    window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                    // use api
                    e.preventDefault();

                    var data = {
                        Amount: $('#<%=txtDepositAmount.ClientID%>').val(),
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
                        window.w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                        GPInt.prototype.HideSplash();
                    });
                });
            });
    </script>
</asp:Content>
