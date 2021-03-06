﻿<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay110101.aspx.cs" Inherits="v2_Deposit_Pay110101" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
    </div>
    <div class="form-group">
        <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" />
        <asp:DropDownList ID="drpSystemAccount" runat="server" CssClass="form-control" required data-selectequals="-1" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblDepositChannel" runat="server" AssociatedControlID="drpDepositChannel" />
        <asp:DropDownList ID="drpDepositChannel" runat="server" CssClass="form-control" required data-selectequals="-1" />
    </div>
    <div class="form-group depositdatetime" id="divDepositDateTime" runat="server">
        <asp:Label ID="lblDepositDateTime" runat="server" AssociatedControlID="txtDepositDate" />
        <div class="row thin-gutter">
            <div class="col-xs-6 col-sm-6 datetime-left">
                <asp:TextBox ID="txtDepositDate" type="text" runat="server" CssClass="form-control " data-date-box="payment" />
            </div>
            <div class="col-xs-6 col-sm-6">
                <asp:TextBox ID="txtDepositTime" type="text" runat="server" CssClass="form-control" data-date-box="time"/>
            </div>
        </div>
    </div>
    <div class="form-group">
        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
        <asp:DropDownList ID="drpBank" runat="server" CssClass="form-control" required data-selectequals="-1" />
    </div>
    <div class="form-group bankname" hidden>
        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" />
        <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-require="" />
    </div>
    <div class="form-group accountnumber">
        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" required data-require="" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" />
        <asp:TextBox ID="txtReferenceId" runat="server" CssClass="form-control" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/banktransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            _w88_banktransfer.init("<%=base.PaymentType %>");

            $('#form1').validator().on('submit', function (e) {
                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    var depositDateTime = new Date($('input[id$="txtDepositDate"]').datebox('getTheDate'));
                    depositDateTime.setHours($('input[id$="txtDepositTime"]').datebox('getTheDate').getHours());
                    depositDateTime.setMinutes($('input[id$="txtDepositTime"]').datebox('getTheDate').getMinutes());

                    var data = {
                        Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                        Bank: { Text: $('select[id$="drpBank"] option:selected').text(), Value: $('select[id$="drpBank"]').val() },
                        AccountName: $('[id$="txtAccountName"]').val(),
                        AccountNumber: $('[id$="txtAccountNumber"]').val(),
                        SystemBank: { Text: $('select[id$="drpSystemAccount"] option:selected').text(), Value: $('select[id$="drpSystemAccount"]').val() },
                        BankName: $('[id$="txtBankName"]').val(),
                        ReferenceId: $('[id$="txtReferenceId"]').val(),
                        DepositChannel: { Text: $('select[id$="drpDepositChannel"] option:selected').text(), Value: $('select[id$="drpDepositChannel"]').val() },
                        DepositDateTime: _w88_paymentSvcV2.formatDateTime(depositDateTime),
                        MethodId: "<%=base.PaymentMethodId%>"
                    };

                    _w88_banktransfer.createDeposit($(this), data, "<%=base.PaymentMethodId%>");
                }
            });
        });
    </script>
</asp:Content>