<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay1202114.aspx.cs" Inherits="v2_Deposit_Pay1202114" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">

    <div class="form-group ali-pay-note">
        <span id="paymentNote"></span>
        <p id="paymentNoteContent"></p>
    </div>
    <div class="form-group">
        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" onKeyPress="return ValidatePositiveDecimal(this, event);" />
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/wechat.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

    <script type="text/javascript">

        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        if (isAndroid) {
            $('#<%=txtAmount.ClientID%>').keypress(function (event) {
                return TwoDecimalAndroid($(this), event);
            });
        }

        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("deposit", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "deposit"
                    , countryCode: "<%=base.strCountryCode %>"
                    , memberId: "<%=base.strMemberID %>"
                    , notice: '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>'
                });

            window.w88Mobile.Gateways.WeChatV2.init();

            $('#form1').submit(function (e) {

                var hasOneDecimal = PositiveOneDecimalValidation($('#<%=txtAmount.ClientID%>').val());

                if (!hasOneDecimal) {
                    return;
                }

                e.preventDefault();
                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
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

