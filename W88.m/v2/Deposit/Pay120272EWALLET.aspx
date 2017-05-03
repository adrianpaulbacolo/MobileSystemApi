<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120272EWALLET.aspx.cs" Inherits="v2_Deposit_Pay120272" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div class="form-group">
        <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
    </div>
    <div class="form-group">
        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
        <asp:TextBox ID="txtEmail" runat="server" type="email" CssClass="form-control" required />
    </div>
    <div class="form-group otp">
        <asp:Label ID="lblOtp" runat="server" AssociatedControlID="txtOtp" />
        <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control"/>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/baokim.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var method = "EWALLET";

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>", method);
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.BaokimV2.init(method);

            var ewalletcb = {};

            if (!_.isEqual(window.w88Mobile.Gateways.BaokimV2.method, method)) {
                var data = {
                    Method: window.w88Mobile.Gateways.BaokimV2.method,
                    Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                    Email: $('input[id$="txtEmail"]').val(),
                    Phone: getQueryStringValue("phone_no"),
                    Accepted: getQueryStringValue("accepted"),
                    Message: getQueryStringValue("message"),
                    CheckSum: getQueryStringValue("checksum"),
                };

                window.w88Mobile.Gateways.BaokimV2.verifyOtp(data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');

                            ewalletcb.TransactionId = response.ResponseData.TransactionId;
                            ewalletcb.VendorTransactionId = response.ResponseData.VendorTransactionId;
                            ewalletcb.MerchantId = response.ResponseData.MerchantId;
                            break;
                        default:
                            w88Mobile.Growl.shout(response.ResponseMessage);
                            break;
                    }
                });
            }

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    if (!_.isEqual(window.w88Mobile.Gateways.BaokimV2.method, method)) {
                        var walletData = {
                            MerchantId: ewalletcb.MerchantId,
                            VendorTransactionId: ewalletcb.VendorTransactionId,
                            Otp: $('input[id$="txtOtp"]').val()
                        };

                        window.w88Mobile.Gateways.BaokimV2.validateWallet(walletData, ewalletcb.TransactionId);
                    }
                    else {
                        var data = {
                            Method: window.w88Mobile.Gateways.BaokimV2.method,
                            Amount: $('input[id$="txtAmount"]').autoNumeric('get'),
                            Email: $('input[id$="txtEmail"]').val(),
                            MethodId: "<%=base.PaymentMethodId%>",
                            ThankYouPage: location.protocol + "//" + location.host + "/v2/Deposit/Pay120272EWALLET.aspx?requestAmount=" + $('input[id$="txtAmount"]').autoNumeric('get'),
                        };

                        window.w88Mobile.Gateways.BaokimV2.createWalletDeposit("<%=base.PaymentMethodId%>", data);
                    }
                }
            });

        });

    </script>
</asp:Content>

