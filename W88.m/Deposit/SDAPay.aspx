<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="SDAPay.aspx.cs" Inherits="Deposit_SDAPay" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="item-text-wrap ali-pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </li>
        <li class="item item-input">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
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

            setTranslations();
            function setTranslations() {
                if (_w88_contents.translate("LABEL_MSG_120254") != "LABEL_MSG_120254") {
                    $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                    $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_120254"));
                } else {
                    window.setInterval(function () {
                        setTranslations();
                    }, 500);
                }
            }

            $('#form1').submit(function (e) {
                e.preventDefault();
                var data = {
                    Amount: $('input[id$="txtAmount"]').val(),
                };

                _w88_paymentSvc.SendDeposit("/payments/" + "<%=base.PaymentMethodId %>", "POST", data, function (response) {
                    switch (response.ResponseCode) {
                        case 1:

                            w88Mobile.Growl.shout(_w88_contents.translate("MESSAGES_CHECK_HISTORY"));
                            window.location.replace('SDAPay2.aspx?id=' + response.ResponseData.TransactionId)

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
                    GPInt.prototype.HideSplash();
                });
            });
        });
    </script>
</asp:Content>
