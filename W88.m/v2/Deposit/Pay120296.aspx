﻿<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120296.aspx.cs" Inherits="v2_Deposit_Pay120296" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAcctName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-accountName=""/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblAcctNumber" runat="server" AssociatedControlID="txtAccountNumber" />
        <asp:TextBox ID="txtAccountNumber" runat="server" TextMode="Password" CssClass="form-control" required data-accountNo=""/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblVenusPoints" runat="server" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblExchangeRate" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/venuspoint.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.VenusPointV2.init();

            $('input[id$="txtAmount"]').blur(function () {
                if ($(this).val() && '<%=commonCookie.CookieCurrency%>' == "JPY") {
                    var data = {
                        amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                        currencyFrom: "JPY",
                        currencyTo: "USD"
                    };

                    window.w88Mobile.Gateways.VenusPointV2.exchangeRate(data);
                }
            });
            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {

                    e.preventDefault();
                    var data = {
                        Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                        AccountName: $('input[id$="txtAccountName"]').val(),
                        AccountNumber: $('input[id$="txtAccountNumber"]').val(),
                        ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx",
                        MethodId: "<%=base.PaymentMethodId%>"
                    };

                    var params = decodeURIComponent($.param(data));
                    window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                    _w88_paymentSvcV2.onTransactionCreated($(this));
                    return;
                }
            });
        });
    </script>
</asp:Content>

