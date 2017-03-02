<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="v2_Deposit_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="Server">
    <div class="empty-state" id="loader"></div>
    <div class="empty-state" hidden>
        <div class="empty-state-icon">
            <i class="ion ion-alert"></i>
        </div>
        <p id="paymentNote">
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="InnerScriptPlaceHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/defaultpayments.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/gateway.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType%>", "<%=base.PaymentMethodId %>");
         });
    </script>

</asp:Content>

