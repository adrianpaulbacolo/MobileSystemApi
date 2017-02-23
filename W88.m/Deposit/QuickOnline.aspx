<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="QuickOnline.aspx.cs" Inherits="Deposit_QuickOnline" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-select">
            <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
            <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
        </li>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/autoroute.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
            $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

                window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

                $('#form1').submit(function (e) {
                      window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                    // use api
                    e.preventDefault();

                    var data = {
                          Amount: $('#<%=txtDepositAmount.ClientID%>').val(),
                          Bank: { Text: $('#<%=drpBank.ClientID%> option:selected').text(), Value: $('#<%=drpBank.ClientID%>').val() },
                        ThankYouPage: location.protocol + "//" + location.host + "/Deposit/Thankyou.aspx"
                    };

                    window.w88Mobile.Gateways.AutoRoute.Deposit(window.w88Mobile.Gateways.DefaultPayments.AutoRouteIds.QuickOnline, data, function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                if (response.ResponseData.VendorRedirectionUrl) {
                                    window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                                } else {
                                    if (response.ResponseData.PostUrl) {
                                        w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");

                                        w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                                        w88Mobile.PostPaymentForm.submit();
                                    } else if (response.ResponseData.DummyURL) {
                                        w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> ");

                                        if (response.ResponseData.FormData) {
                                            w88Mobile.PostPaymentForm.create(response.ResponseData.FormData, response.ResponseData.DummyURL, "body");
                                            w88Mobile.PostPaymentForm.submit();
                                        } else {
                                            window.open(response.ResponseData.DummyURL, '_blank');
                                        }

                                    } else {
                                        w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
                                    }
                                }

                                $('#form1')[0].reset();

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
                        window.w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                        GPInt.prototype.HideSplash();
                    });
                });
            });
    </script>
</asp:Content>
