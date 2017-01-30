<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="DaddyPay.aspx.cs" Inherits="Deposit_DaddyPay" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input" id="txtAmount">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-select" id="drpAmount" style="display: none;">
            <asp:Label ID="lbldrpDepositAmount" runat="server" AssociatedControlID="drpDepositAmount" />
            <asp:DropDownList ID="drpDepositAmount" runat="server" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
            <asp:DropDownList ID="drpBank" runat="server" />
        </li>
        <li class="item item-input" id="accountName">
            <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
            <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
            <asp:HiddenField ID="hfWCNickname" runat="server" ClientIDMode="Static" />
        </li>
        <li class="item item-input" id="accountNo">
            <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
            <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/daddypay.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");

            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                var data = {
                    Amount: $('#<%=drpBank.ClientID%> option:selected').val() == "40" ? $('#<%=drpDepositAmount.ClientID%> option:selected').val() : $('#<%=txtDepositAmount.ClientID%>').val(),
                    Bank: { Text: $('#<%=drpBank.ClientID%> option:selected').text(), Value: $('#<%=drpBank.ClientID%> option:selected').val() },
                    AccountName: $('#<%=txtAccountName.ClientID%>').val(),
                    AccountNumber: $('#<%=txtAccountNumber.ClientID%>').val(),
                    Method: '<%=isDaddyPayQR%>' == "True" ? "QR" : ""
                };

                window.w88Mobile.Gateways.DaddyPay.Deposit(data, function (response) {
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

                            window.w88Mobile.Gateways.DaddyPay.TooglePaymentMethod($('#<%=drpBank.ClientID%>').val());
                            break;
                    }
                },
                    function () {
                        window.w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                        GPInt.prototype.HideSplash();
                    });
            });

            $('#<%=drpBank.ClientID%>').change(function () {
                var bId = this.value;
                window.w88Mobile.Gateways.DaddyPay.TooglePaymentMethod(bId);
            });
        });
    </script>
</asp:Content>

