<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120212.aspx.cs" Inherits="v2_Deposit_Pay120212" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap">
            <asp:Label ID="lblMessage" runat="server" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/nganluong.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.NganLuong.Initialize();
            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            payments.send("", function (response) {
                window.w88Mobile.FormValidator.disableSubmitButton('button[id$="btnSubmit"]');

                switch (response.ResponseCode) {
                    case 1:
                        window.w88Mobile.FormValidator.enableSubmitButton('button[id$="btnSubmit"]');
                        $('button[id$="btnSubmit"]').click(function (e) {
                            window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                        });
                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage);
                        break;
                }
            },
                function () {
                    w88Mobile.FormValidator.enableSubmitButton('button[id$="btnSubmit"]');
                    GPINTMOBILE.HideSplash();
                });
        });
    </script>
</asp:Content>

