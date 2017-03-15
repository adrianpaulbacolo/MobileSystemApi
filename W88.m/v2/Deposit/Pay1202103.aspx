<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay1202103.aspx.cs" Inherits="v2_Deposit_Pay1202103" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" onKeyPress="return NotAllowDecimal(event);" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
      <script type="text/javascript" src="/_static/v2/assets/js/gateways/iwallet.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

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

                var params = decodeURIComponent($.param(data));
                window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvcV2.onTransactionCreated($(this));
                return;
            });
        });
    </script>
</asp:Content>

