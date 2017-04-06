<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay110101.aspx.cs" Inherits="v2_Deposit_Pay110101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">

    <div class="form-group">
        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" required data-paylimit="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" />
        <asp:TextBox ID="txtReferenceId" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" />
        <asp:DropDownList ID="drpSystemAccount" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group" id="divDepositDateTime" runat="server">
        <asp:Label ID="lblDepositDateTime" runat="server" AssociatedControlID="drpDepositDate" />
        <div class="row thin-gutter">
            <div class="col-xs-6">
                <asp:DropDownList ID="drpDepositDate" runat="server" CssClass="form-control" />
            </div>
            <div class="col-xs-3">
                    <asp:DropDownList ID="drpHour" runat="server" CssClass="form-control" />
            </div>
            <div class="col-xs-3">
                <asp:DropDownList ID="drpMinute" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="lblDepositChannel" runat="server" AssociatedControlID="drpDepositChannel" />
        <asp:DropDownList ID="drpDepositChannel" runat="server" CssClass="form-control" required data-selectequals="-1"/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
        <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group" id="divBankName" style="display: none;">
        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" />
        <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-accountName=""/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" required data-accountNo=""/>
    </div>
    <div class="form-group ali-pay-note">
        <p id="paymentNoteContent"></p>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/banktransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>", "<%=base.strMemberID %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.BankTransferv2.init();

            $('select[id$="drpBank"]').change(function () {
                window.w88Mobile.Gateways.BankTransferv2.toogleBank(this.value);
            });

            $('#form1').submit(function (e) {
                e.preventDefault();

                var depositDateTime = new Date($('select[id$="drpDepositDate"]').val());
                depositDateTime.setHours($('select[id$="drpHour"]').val());
                depositDateTime.setMinutes($('select[id$="drpMinute"]').val());

                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    BankText: $('select[id$="drpBank"] option:selected').text(),
                    BankValue: $('select[id$="drpBank"]').val(),
                    AccountName: $('[id$="txtAccountName"]').val(),
                    AccountNumber: $('[id$="txtAccountNumber"]').val(),
                    SystemBankText: $('select[id$="drpSystemAccount"] option:selected').text(),
                    SystemBankValue: $('select[id$="drpSystemAccount"]').val(),
                    BankName: $('[id$="BankName"]').val(),
                    ReferenceId: $('[id$="txtReferenceId"]').val(),
                    DepositChannelText: $('select[id$="drpDepositChannel"] option:selected').text(),
                    DepositChannelValue: $('select[id$="drpDepositChannel"]').val(),
                    DepositDateTime: _w88_paymentSvcV2.formatDateTime(depositDateTime),
                    MethodId: "<%=base.PaymentMethodId%>"
                };


                var params = decodeURIComponent($.param(data));
                window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvcV2.onTransactionCreated($(this));
                return;
            });
        });
    </script>
</asp:Content>

