<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="NextPay.aspx.cs" Inherits="Deposit_NextPay" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBanks" />
            <asp:DropDownList ID="drpBanks" runat="server">
            </asp:DropDownList>
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/nextpay.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            window.w88Mobile.Gateways.GNextPay.init("<%=base.PaymentMethodId %>");

            $('#form1').submit(function (e) {
                e.preventDefault();
                w88Mobile.FormValidator.disableSubmitButton('input[id$="btnSubmit"]');

                var data = {
                    Amount: $('input[id$="txtDepositAmount"]').val(),
                    Bank: {
                        Text: $('select[id$="drpBanks"] option:selected').text(),
                        Value: $('select[id$="drpBanks"] option:selected').val()
                    },
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
