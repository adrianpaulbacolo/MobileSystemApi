<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="Cubits.aspx.cs" Inherits="Withdrawal_Cubits" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" onKeyPress="return NotAllowDecimal(event);" />
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" />
            <asp:TextBox ID="txtAddress" runat="server" data-clear-btn="true" />
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">
    <link href="/_Static/Css/payment.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvc.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvc.DisplaySettings(
                "<%=base.PaymentMethodId %>"
                , {
                    type: "<%=base.PaymentType %>"
                });

            $('label[id$="lblAddress"]').text(_w88_contents.translate("LABEL_ADDRESS")),
            $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
            $("#paymentNoteContent").text(_w88_contents.translate("LABEL_MSG_2208121"));

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.w88Mobile.FormValidator.disableSubmitButton('input[id$="btnSubmit"]');

                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                    AccountName: $('input[id$="txtAddress"]').val(),
                }

                _w88_paymentSvc.SendDeposit("/payments/" + "<%=base.PaymentMethodId %>", "POST", data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:
                            w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p> <p>" + _w88_contents.translate("LABEL_TRANSACTION_ID") + ": " + response.ResponseData.TransactionId + "</p>", function () {
                                window.location.reload();
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
                    w88Mobile.FormValidator.enableSubmitButton('input[id$="btnSubmit"]');
                    GPINTMOBILE.HideSplash();
                });
            });
        });
    </script>
</asp:Content>
