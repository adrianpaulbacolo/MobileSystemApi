<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="WingMoney.aspx.cs" Inherits="Deposit_WingMoney" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" />
            <asp:TextBox ID="txtReferenceId" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-select" id="divDepositDateTime" runat="server">
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
        <li class="item item-input">
            <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
            <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
            <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");

                payments.init();

                window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                var depositDateTime = new Date($('#<%=drpDepositDate.ClientID%>').val());
                depositDateTime.setHours($('#<%=drpHour.ClientID%>').val());
                    depositDateTime.setMinutes($('#<%=drpMinute.ClientID%>').val());

                    var data = {
                        Amount: $('#<%=txtAmount.ClientID%>').val(),
                    AccountName: $('#<%=txtAccountName.ClientID%>').val(),
                    AccountNumber: $('#<%=txtAccountNumber.ClientID%>').val(),
                    ReferenceId: $('#<%=txtReferenceId.ClientID%>').val(),
                    DepositDateTime: depositDateTime.toLocaleDateString() + " " + depositDateTime.toLocaleTimeString()
                }

                    payments.send(data, function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>", function () {
                                $('#form1')[0].reset();
                            });

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
