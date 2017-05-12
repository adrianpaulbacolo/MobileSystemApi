<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pending.aspx.cs" Inherits="v2_Withdrawal_Pending" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblTransactionId" runat="server" AssociatedControlID="txtTransactionId"/>
        <asp:Label ID="txtTransactionId" runat="server"  CssClass="form-control"/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblRequestTime" runat="server" AssociatedControlID="txtRequestTime"/>
         <asp:Label ID="txtRequestTime" runat="server"  CssClass="form-control"/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblPaymentMethod" runat="server" AssociatedControlID="txtPaymentMethod" />
        <asp:Label ID="txtPaymentMethod" runat="server"  CssClass="form-control"/>
    </div>
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount"/>
        <asp:Label ID="txtAmount" runat="server"  CssClass="form-control"/>
    </div>
    <div class="form-group">
        <button type="submit" id="btnCancel" class="btn btn-block btn-primary"></button>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
     _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType%>", "<%=base.PaymentMethodId %>");/assets/js/gateways/pending.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            window.w88Mobile.Gateways.Pending.init();

            $("#btnCancel").click(function (e) {
                e.preventDefault();
                window.w88Mobile.Gateways.Pending.cancel();
            });
        });
    </script>

</asp:Content>

