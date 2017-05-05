<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay1204131.aspx.cs" Inherits="v2_Deposit_Pay1204131" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" runat="Server">
    <div id="PaymentAmount" style="display: block">
        <div class="form-group">
            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" />
            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required data-paylimit="0" data-numeric />
        </div>
    </div>

    <div class="payment-info" id="PaymentInfo" style="display: none">
        <div class="row no-gutter">
            <div class="col-xs-3 col-sm-5">
                <p class="payment-label" id="lblStatus"></p>
            </div>
            <div class="col-xs-6 col-sm-5">
                <p id="txtStatus"></p>
            </div>
        </div>
        <div class="row no-gutter">
            <div class="col-xs-3 col-sm-6">
                <p class="payment-label" id="lblTransactionId"></p>
            </div>
            <div class="col-xs-6 col-sm-6">
                <p id="txtTransactionId"></p>
            </div>
        </div>
        <div class="row no-gutter">
            <div class="col-xs-3 col-sm-5">
                <p class="payment-label" id="lblAmount2"></p>
            </div>
            <div class="col-xs-6 col-sm-5">
                <p id="txtStep2Amount"></p>
            </div>
            <div class="col-xs-3 col-sm-2">
                <a href="#" class="btn btn-xs btn-default" id="copyAmount"></a>
            </div>
        </div>
        <div class="row no-gutter">
            <div class="col-xs-3 col-sm-5">
                <p class="payment-label" id="lblBankName"></p>
            </div>
            <div class="col-xs-6 col-sm-5">
                <p id="txtBankName"></p>
            </div>
        </div>
        <div class="row no-gutter">
            <div class="col-xs-3 col-sm-5">
                <p class="payment-label" id="lblBankHolderName"></p>
            </div>
            <div class="col-xs-6 col-sm-5">
                <p id="txtBankHolderName"></p>
            </div>
            <div class="col-xs-3 col-sm-2">
                <a href="#" class="btn btn-xs btn-default" id="copyAccountName"></a>
            </div>
        </div>
        <div class="row no-gutter">
            <div class="col-xs-3 col-sm-5">
                <p class="payment-label" id="lblBankAccountNo"></p>
            </div>
            <div class="col-xs-6 col-sm-5">
                <p id="txtBankAccountNo"></p>
            </div>
            <div class="col-xs-3 col-sm-2">
                <a href="#" class="btn btn-xs btn-default" id="copyAccountNo"></a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" runat="Server">
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/alipaytransfer.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_static/v2/assets/js/gateways/banner.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentbanner.init("AlipayTransfer");

            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

            window.w88Mobile.Gateways.AliPayTransfer.init("<%=base.PaymentMethodId %>");

            $('#form1').validator().on('submit', function (e) {

                if (!e.isDefaultPrevented()) {
                    e.preventDefault();

                    if (window.w88Mobile.Gateways.AliPayTransfer.step == 1) {
                        var data = {
                            Amount: $('input[id$="txtAmount"]').autoNumeric('get')
                        };

                        window.w88Mobile.Gateways.AliPayTransfer.step2("<%=base.PaymentMethodId %>", data);
                    } else {
                        window.open(window.w88Mobile.Gateways.AliPayTransfer.bankUrl);
                    }
                }
            });


            $('#copyAmount').on('click', function () {
                var amount = $("#txtStep2Amount").text().slice(2); //this will removed the ": "
                window.w88Mobile.Gateways.AliPayTransfer.copytoclipboard(amount);
            });

            $('#copyAccountName').on('click', function () {
                var accountName = $("#txtBankHolderName").text().slice(2); //this will removed the ": "
                window.w88Mobile.Gateways.AliPayTransfer.copytoclipboard(accountName);
            });

            $('#copyAccountNo').on('click', function () {
                var accountNo = $("#txtBankAccountNo").text().slice(2); //this will removed the ": "
                window.w88Mobile.Gateways.AliPayTransfer.copytoclipboard(accountNo);
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

