<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="QuickOnline.aspx.cs" Inherits="Deposit_QuickOnline" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
            <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
        </li>
        <li class="item item-checkbox">
            <asp:Label ID="lblSwitchLine" runat="server" AssociatedControlID="isSwitchLine" />
            <asp:CheckBox type="checkbox" ID="isSwitchLine" runat="server" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/autoroute.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvc.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "<%=base.PaymentType %>"
                });

            window.w88Mobile.Gateways.AutoRoute.Initialize();

            $('#form1').submit(function (e) {
                e.preventDefault();
                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    BankText: $('[id$="drpBank"] option:selected').text(),
                    BankValue: $('[id$="drpBank"] option:selected').val(),
                    ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx",
                    SwitchLine: $('input[id$="isSwitchLine"]').is(':checked'),
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
