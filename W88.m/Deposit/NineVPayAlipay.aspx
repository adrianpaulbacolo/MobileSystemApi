<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="NineVPayAlipay.aspx.cs" Inherits="Deposit_NineVPayAlipay" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/alipay.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>");
            _w88_paymentSvc.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "deposit"
                });

            window.w88Mobile.Gateways.Alipay.Initialize();

            $('#form1').submit(function (e) {
                e.preventDefault();
                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx",
                    MethodId: "<%=base.PaymentMethodId%>"
                };
                var action = "/Deposit/Pay.aspx";
                var params = decodeURIComponent($.param(data));
                window.open(action + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvc.onTransactionCreated($(this));
                return;
            });
        });
    </script>
</asp:Content>
