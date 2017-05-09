<%@ Page Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="v2_Deposit_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="empty-state" id="loader"></div>
    <div class="empty-state" hidden>
        <div class="empty-state-icon">
            <i class="icon icon-error"></i>
        </div>
        <p id="paymentNote">
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/defaultpayments.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/gateways/gateway.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            pubsub.publish('startLoadItem', { selector: "" });
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType%>", "<%=base.PaymentMethodId %>");
        });
    </script>
</asp:Content>
