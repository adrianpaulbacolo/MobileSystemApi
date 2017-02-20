<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="MoneyTransfer.aspx.cs" Inherits="Deposit_MoneyTransfer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
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
    <script type="text/javascript" src="/_Static/JS/modules/gateways/moneytransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvc.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "<%=base.PaymentType %>"
                });

            window.w88Mobile.Gateways.MoneyTransfer.Initialize("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('input[id$="btnSubmit"]');

                var depositDateTime = new Date($('select[id$="drpDepositDate"]').val());
                depositDateTime.setHours($('select[id$="drpHour"]').val());
                depositDateTime.setMinutes($('select[id$="drpMinute"]').val());

                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    ReferenceId: $('input[id$="txtReferenceId"]').val(),
                    DepositDateTime: _w88_paymentSvc.formatDateTime(depositDateTime),
                    SystemBank: { Text: $('select[id$="drpSystemAccount"] option:selected').text(), Value: $('select[id$="drpSystemAccount"] option:selected').val() },
                    AccountName: $('input[id$="txtAccountName"]').val(),
                    AccountNumber: $('input[id$="txtAccountNumber"]').val(),
                }

                _w88_paymentSvc.SendDeposit("/payments/" + "<%=base.PaymentMethodId %>", "POST", data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + _w88_contents.translate("LABEL_TRANSACTION_ID") + ": " + response.ResponseData.TransactionId + "</p>");

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
                    w88Mobile.FormValidator.enableSubmitButton('input[id$="btnSubmit"]');
                    GPINTMOBILE.HideSplash();
                });
            });
        });
    </script>
</asp:Content>
