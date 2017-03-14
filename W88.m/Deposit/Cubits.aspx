<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="Cubits.aspx.cs" Inherits="Deposit_Cubits" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" onKeyPress="return ValidatePositiveDecimal(this, event, Cookies().getCookie('currencyCode'));" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).ready(function () {
                _w88_paymentSvc.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>", "<%=base.strMemberID %>");
                _w88_paymentSvc.DisplaySettings(
                    "<%=base.PaymentMethodId %>"
                   , {
                       type: "deposit"
                       , countryCode: "<%=base.strCountryCode %>"
                       , memberId: "<%=base.strMemberID %>"
                       , notice: '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>'
                   });

                $('#form1').submit(function (e) {
                    var hasPositiveDecimal = PositiveDecimal($('input[id$="txtAmount"]').val(), Cookies().getCookie('currencyCode'));

                    if (!hasPositiveDecimal) {
                        return;
                    }

                    e.preventDefault();
                    var data = {
                        Amount: $('input[id$="txtAmount"]').val(),
                        MethodId: "<%=base.PaymentMethodId%>"
                    };
                    var action = "/Deposit/Pay.aspx";
                    var params = decodeURIComponent($.param(data));
                    window.open(action + "?" + params, "<%=base.PageName%>");
                    _w88_paymentSvc.onTransactionCreated($(this));
                    return;
                });
            });
        });
    </script>
</asp:Content>
