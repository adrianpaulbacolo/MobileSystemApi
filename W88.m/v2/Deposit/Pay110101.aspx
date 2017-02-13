<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay110101.aspx.cs" Inherits="v2_Deposit_Pay110101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" />
            <asp:TextBox ID="txtReferenceId" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" />
            <asp:DropDownList ID="drpSystemAccount" runat="server" data-corners="false" />
        </li>
        <li class="item item-select div-fastdeposit-depositdatetime" id="divDepositDateTime" runat="server">
            <div class="row">
                <asp:Label ID="lblDepositDateTime" runat="server" AssociatedControlID="drpDepositDate" />
            </div>
            <div class="row">
                <div class="col">
                    <asp:DropDownList ID="drpDepositDate" runat="server" />
                </div>
                <div class="col">
                    <asp:DropDownList ID="drpHour" runat="server" />
                </div>
                <div class="col">
                    <asp:DropDownList ID="drpMinute" runat="server" />
                </div>
            </div>
        </li>
        <li class="item item-select">
            <asp:Label ID="lblDepositChannel" runat="server" AssociatedControlID="drpDepositChannel" />
            <asp:DropDownList ID="drpDepositChannel" runat="server" data-corners="false" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
            <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
        </li>
        <li class="item item-input" id="divBankName" style="display: none;">
            <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" />
            <asp:TextBox ID="txtBankName" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
            <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
            <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
        </li>

        <li class="item-text-wrap ali-pay-note">
            <p id="paymentNoteContent"></p>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/fastdep.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");
            window.w88Mobile.Gateways.FastDeposit.GetBankDetails();

            $('#<%=drpBank.ClientID%>').change(function () {
                window.w88Mobile.Gateways.FastDeposit.ToogleBank(this.value);
            });
            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('button[id$="btnSubmit"]');

                var depositDateTime = new Date($('#<%=drpDepositDate.ClientID%>').val());
                depositDateTime.setHours($('#<%=drpHour.ClientID%>').val());
                depositDateTime.setMinutes($('#<%=drpMinute.ClientID%>').val());

                var data = {
                    Amount: $('#<%=txtAmount.ClientID%>').val(),
                    Bank: {
                        Text: $('#<%=drpBank.ClientID%> option:selected').text(),
                        Value: $('#<%=drpBank.ClientID%>').val()
                    },
                    AccountName: $('#<%=txtAccountName.ClientID%>').val(),
                    AccountNumber: $('#<%=txtAccountNumber.ClientID%>').val(),
                    SystemBank: {
                        Text: $('#<%=drpSystemAccount.ClientID%> option:selected').text(),
                        Value: $('#<%=drpSystemAccount.ClientID%>').val()
                    },
                    BankName: $('#<%=txtBankName.ClientID%>').val(),
                    ReferenceId: $('#<%=txtReferenceId.ClientID%>').val(),
                    DepositChannel: {
                        Text: $('#<%=drpDepositChannel.ClientID%> option:selected').text(),
                        Value: $('#<%=drpDepositChannel.ClientID%>').val()
                    },
                    DepositDateTime: window.w88Mobile.Gateways.DefaultPayments.formatDateTime(depositDateTime),
                };

                payments.send(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
                            $('#form1')[0].reset();
                            window.w88Mobile.Gateways.FastDeposit.ToogleBank($('#<%=drpBank.ClientID%>').val());
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
                        window.w88Mobile.FormValidator.enableSubmitButton('button[id$="btnSubmit"]');
                        GPInt.prototype.HideSplash();
                    });
            });
        });
    </script>
</asp:Content>

