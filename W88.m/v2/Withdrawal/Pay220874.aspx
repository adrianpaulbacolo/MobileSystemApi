<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay220874.aspx.cs" Inherits="v2_Withdrawal_Pay220874" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
    </div>
    <div class="form-group">
        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
        <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" CssClass="form-control" required />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/baokim.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
             _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

             window.w88Mobile.Gateways.BaokimV2.init('');

             $('#form1').validator().on('submit', function (e) {

                 if (!e.isDefaultPrevented()) {

                     e.preventDefault();

                     var data = {
                         Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                         AccountName: $('input[id$="txtEmail"]').val(),
                     };

                     _w88_paymentSvcV2.CreateWithdraw(data, "<%=base.PaymentMethodId %>");
                 }
             });
         });
    </script>
</asp:Content>

