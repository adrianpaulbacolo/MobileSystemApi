<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay110101.aspx.cs" Inherits="v2_Deposit_Pay110101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">

    <div class="form-group">
        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" />
        <asp:TextBox ID="txtReferenceId" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" />
        <asp:DropDownList ID="drpSystemAccount" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblDepositDateTime" runat="server" AssociatedControlID="drpDepositDate" />
    </div>

    <div class="form-group" id="divDepositDateTime" runat="server">
        <div class="col-xs-6">
            <label>
                <asp:DropDownList ID="drpDepositDate" runat="server" CssClass="form-control" />
            </label>
        </div>
        <div class="col-xs-3">
            <label>
                <asp:DropDownList ID="drpHour" runat="server" CssClass="form-control" />
            </label>
        </div>
        <div class="col-xs-3">
            <label>
                <asp:DropDownList ID="drpMinute" runat="server" CssClass="form-control" />
            </label>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="lblDepositChannel" runat="server" AssociatedControlID="drpDepositChannel" />
        <asp:DropDownList ID="drpDepositChannel" runat="server" CssClass="form-control" />
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
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group ali-pay-note">
        <p id="paymentNoteContent"></p>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/fastdep.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>", "<%=base.strMemberID %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", {
                type: "deposit",
                countryCode: "<%=base.strCountryCode %>",
                memberId: "<%=base.strMemberID %>",
                notice: '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>'
            });

            window.w88Mobile.Gateways.FastDepositv2.init();

            $('[id$="drpBank"]').change(function () {
                window.w88Mobile.Gateways.FastDepositv2.toogleBank(this.value);
            });

            var depositDateTime = new Date($('[id$="drpDepositDate"]').val());
            depositDateTime.setHours($('[id$="drpHour"]').val());
            depositDateTime.setMinutes($('[id$="drpMinute"]').val());

            $('#form1').submit(function (e) {
                e.preventDefault();

                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    BankText: $('[id$="drpBank"] option:selected').text(),
                    BankValue: $('[id$="drpBank"]').val(),
                    AccountName: $('[id$="txtAccountName"]').val(),
                    AccountNumber: $('[id$="txtAccountNumber"]').val(),
                    SystemBankText: $('[id$="drpSystemAccount"] option:selected').text(),
                    SystemBankValue: $('[id$="drpSystemAccount"]').val(),
                    BankName: $('[id$="BankName"]').val(),
                    ReferenceId: $('[id$="ReferenceId"]').val(),
                    DepositChannelText: $('[id$="drpDepositChannel"] option:selected').text(),
                    DepositChannelValue: $('[id$="drpDepositChannel"]').val(),
                    DepositDateTime: window.w88Mobile.Gateways.DefaultPayments.formatDateTime(depositDateTime),
                };

                var action = "/Deposit/Pay.aspx";
                var params = decodeURIComponent($.param(data));
                window.open(action + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvcV2.onTransactionCreated($(this));
                return;
            });
        });
    </script>
</asp:Content>

