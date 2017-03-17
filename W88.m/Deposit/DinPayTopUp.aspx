<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="DinPayTopUp.aspx.cs" Inherits="Deposit_Tonghui" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-select" runat="server">
            <asp:Label ID="lblCardType" runat="server" AssociatedControlID="drpCardType" />
            <asp:DropDownList ID="drpCardType" runat="server" data-corners="false">
            </asp:DropDownList>
        </li>
        <li class="item item-select">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="drpAmount" />
            <asp:DropDownList ID="drpAmount" runat="server" data-corners="false">
            </asp:DropDownList>
        </li>
        <li class="item item-select">
            <asp:Label ID="lblCardNo" runat="server" AssociatedControlID="txtCardNo" />
            <asp:TextBox ID="txtCardNo" runat="server" data-mini="true" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblPin" runat="server" AssociatedControlID="txtPin" />
            <asp:TextBox ID="txtPin" runat="server" data-mini="true" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/dinpaytopup.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvc.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "<%=base.PaymentType %>"
                });

            window.w88Mobile.Gateways.DinPayTopUp.Initialize("<%=base.PaymentMethodId %>");

            $('select[id$="drpCardType"]').change(function () {
                window.w88Mobile.Gateways.DinPayTopUp.SetFee($('select[id$="drpCardType"]').val());
                window.w88Mobile.Gateways.DinPayTopUp.SetDenom($('select[id$="drpCardType"]').val());
            });

            $('#form1').submit(function (e) {
                e.preventDefault();
                var data = {
                    Amount: $('select[id$="drpAmount"]').val(),
                    CardTypeValue: $('select[id$="drpCardType"]').val(),
                    CardNumber: $('input[id$="txtCardNo"]').val(),
                    CCV: $('input[id$="txtPin"]').val(),
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
