<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="PaySec.aspx.cs" Inherits="Deposit_PaySec" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" onKeyPress="return NotAllowDecimal(event);" />
            <span id="amtErr" hidden style="color: red !important"></span>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvc.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "<%=base.PaymentType %>"
                });

            $('#amtErr').text(_w88_contents.translate("MESSAGES_WHOLE_NUMBER"));

            window.setInterval(function () {
                CheckWholeNumber($('input[id$="txtAmount"]'));
            }, 500);

            $('#form1').submit(function (e) {
                if (!CheckWholeNumber($('input[id$="txtAmount"]'))) {
                    e.preventDefault();
                    return;
                }

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
