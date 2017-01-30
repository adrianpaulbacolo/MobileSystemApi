<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="KDPayWechat.aspx.cs" Inherits="Deposit_KDPayWechat" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <asp:Label ID="lblNote" runat="server"></asp:Label>
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
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
    <script type="text/javascript">
        $(document).ready(function () {

            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");

            payments.init();

            setTranslations();

            function setTranslations() {
                if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                    $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE0"));
                } else {
                    window.setInterval(function () {
                        setTranslations();
                    }, 500);
                }
            }

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                var data = {
                    Amount: $('#<%=txtAmount.ClientID%>').val(),
                };

                payments.send(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
                            w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                            w88Mobile.PostPaymentForm.submit();

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
                    w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                    GPINTMOBILE.HideSplash();
                });
            });
        });

    </script>
</asp:Content>

