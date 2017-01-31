<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="KexunPay.aspx.cs" Inherits="Deposit_KexunPay" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <asp:Label ID="lblNote" runat="server"></asp:Label>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>");
            _w88_paymentSvc.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "deposit"
                    , countryCode: "<%=base.strCountryCode %>"
                    , memberId: "<%=base.strMemberID %>"
                    , notice: '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>'
                });

            $('#form1').submit(function (e) {
                e.preventDefault();
                var data = {
                    Amount: $('input[id$="txtDepositAmount"]').val(),
                    ThankYouPage: location.protocol + "//" + location.host + "/Index",
                    MethodId: "<%=base.PaymentMethodId%>"
                };
                var action = "/Deposit/Pay.aspx";
                var params = decodeURIComponent($.param(data));
                window.open(action + "?" + params, "<%=base.PageName%>");
                _.first($(this)).reset();
                return;
            });

        });

    </script>
    <style>
        li.ali-pay-note {
            font-size: 70%;
        }

            li.ali-pay-note #lblNote span {
                color: red;
                font-weight: bold;
            }

            li.ali-pay-note #lblNote p {
                padding-top: 5px;
            }
    </style>
</asp:Content>
