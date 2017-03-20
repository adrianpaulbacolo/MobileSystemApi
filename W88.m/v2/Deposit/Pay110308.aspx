<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay110308.aspx.cs" Inherits="v2_Deposit_Pay110308" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" />
        <asp:TextBox ID="txtReferenceId" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" />
        <asp:DropDownList ID="drpSystemAccount" runat="server" CssClass="form-control" required data-selectequals="-1"/>
    </div>
    <div class="form-group">
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
        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-accountName=""/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" required data-accountNo=""/>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/moneytransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.MoneyTransfer.init("<%=base.PaymentMethodId %>", true);

            $('#form1').submit(function (e) {
                e.preventDefault();

                var depositDateTime = new Date($('#<%=drpDepositDate.ClientID%>').val());
                depositDateTime.setHours($('#<%=drpHour.ClientID%>').val());
                depositDateTime.setMinutes($('#<%=drpMinute.ClientID%>').val());

                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    AccountName: $('input[id$="txtAccountName"]').val(),
                    AccountNumber: $('input[id$="txtAccountNumber"]').val(),
                    SystemBankText: $('select[id$="drpSystemAccount"] option:selected').text(),
                    SystemBankValue: $('select[id$="drpSystemAccount"]').val(),
                    ReferenceId: $('input[id$="txtReferenceId"]').val(),
                    DepositDateTime: _w88_paymentSvcV2.formatDateTime(depositDateTime),
                    ThankYouPage: location.protocol + "//" + location.host + "/Index",
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

