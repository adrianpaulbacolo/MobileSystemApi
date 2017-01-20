<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="BaokimWallet.aspx.cs" Inherits="Deposit_BaokimWallet" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
            <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" Enabled="False" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" Enabled="False" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblOtp" runat="server" AssociatedControlID="txtOtp" required />
            <asp:TextBox ID="txtOtp" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <p id="notice" style="color: #ff0000"></p>
        </li>
    </ul>

    <asp:HiddenField ID="hfAmount" runat="server" />
    <asp:HiddenField ID="hfEmail" runat="server" />
    <asp:HiddenField ID="hfAccepted" runat="server" />
    <asp:HiddenField ID="hfPhone" runat="server" />
    <asp:HiddenField ID="hfMessage" runat="server" />
    <asp:HiddenField ID="hfChkSum" runat="server" />
    <asp:HiddenField ID="TransactionId" runat="server" />
    <asp:HiddenField ID="MchtId" runat="server" />
    <asp:HiddenField ID="VendorTransID" runat="server" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/baokim.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            $("#ContentPlaceHolder1_ContentPlaceHolder2_txtDepositAmount").val($("#<%=hfAmount.ClientID%>").val());
            $("#ContentPlaceHolder1_ContentPlaceHolder2_txtEmail").val($("#<%=hfEmail.ClientID%>").val());

            window.w88Mobile.Gateways.Baokim.method = "EWALLETCB";
            window.w88Mobile.Gateways.Baokim.Initialize();

            var data = {
                Method: window.w88Mobile.Gateways.Baokim.method,
                Amount: $("#<%=hfAmount.ClientID%>").val(),
                   Email: $("#<%=hfEmail.ClientID%>").val(),
                   Phone: $("#<%=hfPhone.ClientID%>").val(),
                   Accepted: $("#<%=hfAccepted.ClientID%>").val(),
                   Message: $("#<%=hfMessage.ClientID%>").val(),
                   CheckSum: $("#<%=hfChkSum.ClientID%>").val(),
               };

            window.w88Mobile.Gateways.Baokim.deposit(data, function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');

                        $("#<%=TransactionId.ClientID%>").val(response.ResponseData.TransactionId);
                           $("#<%=VendorTransID.ClientID%>").val(response.ResponseData.VendorTransactionId);
                           $("#<%=MchtId.ClientID%>").val(response.ResponseData.MerchantId);
                           break;
                       default:
                           w88Mobile.Growl.shout(response.ResponseMessage);
                           //window.location.replace('/Funds.aspx');
                           break;
                   }
               });

            $('#ContentPlaceHolder1_btnSubmit').click(function (e) {
                e.preventDefault();

                var walletData = {
                    MerchantId: $("#<%=MchtId.ClientID%>").val(),
                       VendorTransactionId: $("#<%=VendorTransID.ClientID%>").val(),
                       Otp: $("#ContentPlaceHolder1_ContentPlaceHolder2_txtOtp").val()
                   };

                window.w88Mobile.Gateways.Baokim.validateWallet(walletData, $("#<%=TransactionId.ClientID%>").val(), function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            window.w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                            w88Mobile.Growl.shout(response.ResponseMessage);
                            window.location.replace('/Funds.aspx');
                            break;
                        default:
                            w88Mobile.Growl.shout(response.ResponseMessage);
                            break;
                    }
                });
            });

        });
    </script>
</asp:Content>
