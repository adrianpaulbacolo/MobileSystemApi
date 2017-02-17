<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Deposit_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div class="empty-state" id="loader"></div>
    <div class="empty-state" hidden>
        <div class="empty-state-icon">
            <i class="ion ion-alert"></i>
        </div>
        <p id="paymentNote">
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            GPInt.prototype.ShowSplash();
        });
    </script>
</asp:Content>