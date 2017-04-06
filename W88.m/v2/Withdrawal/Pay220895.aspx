<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay220895.aspx.cs" Inherits="v2_Withdrawal_Pay220895" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" required data-paylimit="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAcctName" runat="server" AssociatedControlID="txtAccountName" />
        <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" required data-accountName="0" />
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
                        amount: $('input[id$="txtAmount"]').val(),
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
                        Amount: $('input[id$="txtAmount"]').val(),
                        AccountName: $('input[id$="txtAccountName"]').val(),
                    };

                    _w88_paymentSvcV2.CreateWithdraw(data, "<%=base.PaymentMethodId %>");
                }
            });
        });
    </script>
</asp:Content>
