<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="ShengPayAliPay.aspx.cs" Inherits="Deposit_ShengPayAliPay" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
            <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
        </li>
    </ul>

    <style>
        li.ali-pay-note {
            font-size: 70%;
        }

            li.ali-pay-note #paymentNote {
                color: red;
                font-weight: bold;
            }

            li.ali-pay-note #paymentNoteContent {
                padding-top: 5px;
            }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <script type="text/javascript" src="/_Static/JS/modules/gateways/shengpay.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        
        $('#<%=txtDepositAmount.ClientID%>').bind('input keyup', function (e) {
            var key = e.keyCode;
            if ($.browser.mozilla) {
                key = e.which;
            }
            if (key != 0 && key != 8) {
                var regex = new RegExp("^[0-9]+$");
                var code = String.fromCharCode(key);
                if (!regex.test(code))
                    return false;
            }
        });

        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");
            payments.init();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            window.w88Mobile.Gateways.ShengPay.Initialize();

            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                // use api
                e.preventDefault();
                var data = {
                    Amount: $('#<%=txtDepositAmount.ClientID%>').val()
                };

                w88Mobile.Gateways.ShengPay.gatewayId = "<%=base.PaymentMethodId %>";
                w88Mobile.Gateways.ShengPay.deposit(data, function (response) {
                        switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + '<%=lblTransactionId%>' + ": " + response.ResponseData.TransactionId + "</p>");
                            window.open(response.ResponseData.PostUrl);
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
                    function () { console.log("Error connecting to api"); },
                    function () {
                        w88Mobile.FormValidator.enableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                        GPINTMOBILE.HideSplash();
                    });
            });
        });
    </script>
</asp:Content>