<%@ Page Language="C#" MasterPageFile="~/MasterPages/Payments.master" AutoEventWireup="true" CodeFile="SDAPay2.aspx.cs" Inherits="Deposit_SDAPay2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <ul class="list fixed-tablet-size">
        <li class="row">
            <div class="col col-40">
                <span id="lblStatus" />
            </div>
            <div class="col">
                <span id="txtStatus" />
            </div>
        </li>
        <li class="row">
            <div class="col col-40">
                <span id="lblTransactionId" />
            </div>
            <div class="col">
                <span id="txtTransactionId" />
            </div>
        </li>
        <li class="row">
            <div class="col col-40">
                <span id="lblAmount" />
            </div>
            <div class="col">
                <span id="txtAmount" />
            </div>
            <div class="col col-20">
                <a href="#" class="ui-btn btn-small btn-bordered" id="copyAmount"><%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
            </div>
        </li>
        <li class="row">
            <div class="col">
                <h5 style="font-style: italic">
                    <span id="lblAmountNote" /></h5>
            </div>
        </li>
        <li class="row">
            <div class="col col-40">
                <span id="lblBankName" />
            </div>
            <div class="col">
                <span id="txtBankName" />
            </div>
        </li>
        <li class="row">
            <div class="col col-40">
                <span id="lblBankHolderName" />
            </div>
            <div class="col">
                <span id="txtBankHolderName" />
            </div>
            <div class="col col-20">
                <a href="#" class="ui-btn btn-small btn-bordered" id="copyAccountName"><%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
            </div>
        </li>
        <li class="row">
            <div class="col col-40">
                <span id="lblBankAccountNo" />
            </div>
            <div class="col">
                <span id="txtBankAccountNo" class="with-colon" />
            </div>
            <div class="col col-20">
                <a href="#" class="ui-btn btn-small btn-bordered" id="copyAccountNo"><%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
            </div>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            var payments = new w88Mobile.Gateways.Payments("<%=base.PaymentMethodId %>");

            payments.init();

            $('#paymentSettings').hide();

            window.w88Mobile.Gateways.DefaultPayments.Deposit("<%=base.strCountryCode %>", "<%=base.strMemberID %>", '<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>', "<%=base.PaymentMethodId %>");

            setTranslations();
            function setTranslations() {
                if (_w88_contents.translate("LABEL_AMOUNT") != "LABEL_AMOUNT") {
                    $('#lblStatus').text(_w88_contents.translate("LABEL_FIELDS_STATUS"))
                    $('#lblTransactionId').text(_w88_contents.translate("LABEL_TRANSACTION_ID"))
                    $('#lblAmount').text(_w88_contents.translate("LABEL_AMOUNT"))
                    $('#lblBankName').text(_w88_contents.translate("LABEL_BANK_NAME"))
                    $('#lblBankHolderName').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"))
                    $('#lblBankAccountNo').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"))
                    $('#lblAmountNote').html(_w88_contents.translate("LABEL_MSG_AMOUNT_120254"))
                } else {
                    window.setInterval(function () {
                        setTranslations();
                    }, 500);
                }
            }

            var transactionId = window.location.search.substr(4); //querystring "id"
            var bankUrl = "";
            window.w88Mobile.Gateways.DefaultPayments.Send("/payments/<%=base.PaymentMethodId %>/" + transactionId, "GET", function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        $('#txtStatus').text(": " + response.ResponseData.Status)
                        $('#txtTransactionId').text(": " + response.ResponseData.TransactionId)
                        $('#txtAmount').text(": " + response.ResponseData.Amount)
                        $('#txtBankName').text(": " + response.ResponseData.Bank)
                        $('#txtBankHolderName').text(": " + response.ResponseData.AccountName)
                        $('#txtBankAccountNo').text(": " + response.ResponseData.AccountNumber)

                        bankUrl = response.ResponseData.BankUrl;
                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage);

                        break;
                }
            });

            $('#form1').submit(function (e) {
                e.preventDefault();
                window.open(bankUrl);
            });

            (function (a) { (jQuery.browser = jQuery.browser || {}).ios = /ip(hone|od|ad)/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);

            if ($.browser.ios) {
                $(".col-20").hide();
            }
            else {
                $(".col-20").show();
            }

            $('#copyAmount').on('click', function () {
                var amount = $("#txtAmount").text().slice(2); //this will removed the ": "
                copyToClipboard(amount)
            });

            $('#copyAccountName').on('click', function () {
                var accountName = $("#txtBankHolderName").text().slice(2); //this will removed the ": "
                copyToClipboard(accountName)
            });

            $('#copyAccountNo').on('click', function () {
                var accountNo = $("#txtBankAccountNo").text().slice(2); //this will removed the ": "
                copyToClipboard(accountNo)
            });

            function copyToClipboard(text) {
                var input = document.createElement('textarea', { "permissions": ["clipboardWrite"] });
                document.body.appendChild(input);
                input.value = text;
                input.focus();
                input.select();

                var s = document.execCommand('copy', false, null);
                window.w88Mobile.Growl.shout(s == true ? "<%=commonCulture.ElementValues.getResourceString("copied", commonVariables.LeftMenuXML)%>" : "Unable to Copy");

                input.remove();
            }

            var intervalId = setInterval(function () {
                var headers = {
                    'Token': window.User.token,
                    'LanguageCode': window.User.lang
                };

                $.ajax({
                    headers: headers,
                    url: w88Mobile.APIUrl + "/payments/deposit/status/" + transactionId,
                    type: "GET",
                    success: function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                var result = response.ResponseData.Status;
                                $('#txtStatus').text(": " + result);
                                if (result.indexOf("Successful") == 0) {
                                    clearInterval(intervalId);
                                    window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');

                                    setTimeout(function () {
                                        window.location.replace('/FundTransfer/Default.aspx');
                                    }, 2000);

                                } else if (result.indexOf("Failed") == 0) {
                                    clearInterval(intervalId);
                                    window.w88Mobile.FormValidator.disableSubmitButton('#ContentPlaceHolder1_btnSubmit');
                                }
                                break;
                            default:
                        }
                    },
                    error: function (err) {
                        clearInterval(intervalId);
                    }
                });
            }, 5000);
        });
    </script>
</asp:Content>
