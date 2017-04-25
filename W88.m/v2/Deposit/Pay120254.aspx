<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120254.aspx.cs" Inherits="v2_Deposit_Pay120254" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div id="PaymentAmount" style="display: block">
        <div class="form-group pay-note">
            <span id="paymentNote"></span>
            <p id="paymentNoteContent"></p>
        </div>
        <div class="form-group">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
        </div>
    </div>

    <div id="PaymentInfo" style="display: none">
        <div class="form-group">
            <div class="col-xs-6">
                <span id="lblStatus"></span>
            </div>
            <div class="col-xs-6">
                <span id="txtStatus"></span>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-6">
                <span id="lblTransactionId"></span>
            </div>
            <div class="col-xs-6">
                <span id="txtTransactionId"></span>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-6">
                <span id="lblAmount2"></span>
            </div>
            <div class="col-xs-4">
                <span id="txtStep2Amount"></span>
            </div>
            <div class="col-xs-2">
                <a href="#" id="copyAmount"></a>
            </div>
        </div>
        <div class="form-group pay-note">
            <div class="col-xs-12">
                <span id="paymentNote2"></span>
                <p id="paymentNoteContent2"></p>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-6">
                <span id="lblBankName"></span>
            </div>
            <div class="col-xs-6">
                <span id="txtBankName"></span>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-6">
                <span id="lblBankHolderName"></span>
            </div>
            <div class="col-xs-4">
                <span id="txtBankHolderName"></span>
            </div>
            <div class="col-xs-2">
                <a href="#" id="copyAccountName"></a>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-6">
                <span id="lblBankAccountNo"></span>
            </div>
            <div class="col-xs-4">
                <span id="txtBankAccountNo"></span>
            </div>
            <div class="col-xs-2">
                <a href="#" id="copyAccountNo"></a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/sdapay.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.SDAPay.init("<%=base.PaymentMethodId %>");

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    if (window.w88Mobile.Gateways.SDAPay.step == 1) {
                        var data = {
                            Amount: $('input[id$="txtAmount"]').autoNumeric('get')
                        };

                        window.w88Mobile.Gateways.SDAPay.step2("<%=base.PaymentMethodId %>", data);
                    } else {
                        window.open(window.w88Mobile.Gateways.SDAPay.bankUrl);
                    }
                }
            });


            $('#copyAmount').on('click', function () {
                var amount = $("#txtStep2Amount").text().slice(2); //this will removed the ": "
                window.w88Mobile.Gateways.SDAPay.copytoclipboard(amount);
            });

            $('#copyAccountName').on('click', function () {
                var accountName = $("#txtBankHolderName").text().slice(2); //this will removed the ": "
                window.w88Mobile.Gateways.SDAPay.copytoclipboard(accountName);
            });

            $('#copyAccountNo').on('click', function () {
                var accountNo = $("#txtBankAccountNo").text().slice(2); //this will removed the ": "
                window.w88Mobile.Gateways.SDAPay.copytoclipboard(accountNo);
            });

            (function (a) { (jQuery.browser = jQuery.browser || {}).ios = /ip(hone|od|ad)/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);

            if ($.browser.ios) {
                $(".col-xs-2").hide();
            }
            else {
                $(".col-xs-2").show();
            }

        });

    </script>
</asp:Content>

