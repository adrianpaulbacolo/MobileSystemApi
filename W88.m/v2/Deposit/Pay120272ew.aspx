<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120272ew.aspx.cs" Inherits="v2_Deposit_Pay120272ew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
        <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" Enabled="False" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" Enabled="False" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblOtp" runat="server" AssociatedControlID="txtOtp" />
        <asp:TextBox ID="txtOtp" runat="server" data-clear-btn="true" required/>
    </div>
    <div class="form-group">
        <p id="notice" style="color: #ff0000"></p>
    </div>

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
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/baokim.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            var selectName = '<%=strdrpBank%>';

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            $('input[id$="txtAmount"]').val($("#<%=hfAmount.ClientID%>").val());
            $('input[id$="txtEmail"]').val($("#<%=hfEmail.ClientID%>").val());

            window.w88Mobile.Gateways.BaokimV2.method = "EWALLETCB";
            window.w88Mobile.Gateways.BaokimV2.init();

            var data = {
                Method: window.w88Mobile.Gateways.BaokimV2.method,
                Amount: $("#<%=hfAmount.ClientID%>").val(),
                Email: $("#<%=hfEmail.ClientID%>").val(),
                Phone: $("#<%=hfPhone.ClientID%>").val(),
                Accepted: $("#<%=hfAccepted.ClientID%>").val(),
                Message: $("#<%=hfMessage.ClientID%>").val(),
                CheckSum: $("#<%=hfChkSum.ClientID%>").val(),
            };

            window.w88Mobile.Gateways.BaokimV2.verifyOtp(data, function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');

                        $("#<%=TransactionId.ClientID%>").val(response.ResponseData.TransactionId);
                        $("#<%=VendorTransID.ClientID%>").val(response.ResponseData.VendorTransactionId);
                        $("#<%=MchtId.ClientID%>").val(response.ResponseData.MerchantId);
                        break;
                    default:
                        w88Mobile.Growl.shout(response.ResponseMessage);
                        break;
                }
            });

            $('button[id$="btnSubmitPlacement"]').click(function (e) {
                e.preventDefault();

                var walletData = {
                    MerchantId: $("#<%=MchtId.ClientID%>").val(),
                    VendorTransactionId: $("#<%=VendorTransID.ClientID%>").val(),
                    Otp: $('input[id$="txtOtp"]').val()
                };

                window.w88Mobile.Gateways.BaokimV2.validateWallet(walletData, $("#<%=TransactionId.ClientID%>").val());
            });

        });
    </script>
</asp:Content>

