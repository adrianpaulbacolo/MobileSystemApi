<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="VenusPoint.aspx.cs" Inherits="Deposit_VenusPoint" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">

    <ul class="list fixed-tablet-size">
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAcctName" runat="server" AssociatedControlID="txtAccountName" />
            <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAcctNumber" runat="server" AssociatedControlID="txtAccountNumber" />
            <asp:TextBox ID="txtAccountNumber" runat="server" TextMode="Password" data-clear-btn="true" />
        </li>
        <li class="item-text-wrap">
            <asp:Label ID="lblVenusPoints" runat="server" />
        </li>
        <li class="item-text-wrap">
            <asp:Label ID="lblExchangeRate" runat="server" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/venuspoint.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

                window.w88Mobile.Gateways.VenusPoint.Initialize();

                $('#form1').submit(function (e) {
                    e.preventDefault();
                    window.w88Mobile.FormValidator.disableSubmitButton('input[id$="btnSubmit"]');

                    var data = {
                        Amount: $('input[id$="txtDepositAmount"]').val(),
                        AccountName: $('input[id$="txtAccountName"]').val(),
                        AccountNumber: $('input[id$="txtAccountNumber"]').val()
                    };

                    window.w88Mobile.Gateways.VenusPoint.Deposit(data, function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
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
                        window.w88Mobile.FormValidator.enableSubmitButton('input[id$="btnSubmit"]');
                        GPInt.prototype.HideSplash();
                    });
                });

            $('input[id$="txtDepositAmount"]').blur(function () {
                    if ($(this).val() && '<%=commonCookie.CookieCurrency%>' == "JPY") {
                        var data = {
                            amount: $('input[id$="txtDepositAmount"]').val(),
                            currencyFrom: "JPY",
                            currencyTo: "USD"
                        }

                        window.w88Mobile.Gateways.VenusPoint.ExchangeRate(data);
                    }
                });
            });
    </script>
</asp:Content>
