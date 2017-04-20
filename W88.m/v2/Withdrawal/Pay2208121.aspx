<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay2208121.aspx.cs" Inherits="v2_Withdrawal_Pay2208121" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group pay-note">
        <span id="paymentNote"></span>
        <p id="paymentNoteContent"></p>
    </div>
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" />
        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" required data-address=""/>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/cubits.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            if (_.isEqual('<%= commonCookie.CookieCurrency %>', "JPY")) {
                $('input[id$="txtAmount"]').attr('data-numeric', 0)
            }
            else if (_.isEqual('<%= commonCookie.CookieCurrency %>', "USD")) {
                $('input[id$="txtAmount"]').attr('data-numeric', '')
            }

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.Cubits.init("<%=base.PaymentMethodId %>", true);

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {

                    e.preventDefault();

                    var data = {
                        Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                        AccountName: $('input[id$="txtAddress"]').val()
                    };

                    _w88_paymentSvcV2.CreateWithdraw(data, "<%=base.PaymentMethodId %>");

                }
            });
        });
    </script>
</asp:Content>

