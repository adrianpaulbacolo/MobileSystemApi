window.w88Mobile.Gateways.AliPayTransfer = AliPayTransfer();
var _w88_alipaytransfer = window.w88Mobile.Gateways.AliPayTransfer;

function AliPayTransfer() {

    var alipaytransfer;
    var methodId;
    try {
        alipaytransfer = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        alipaytransfer = {};
    }

    alipaytransfer.bankUrl = "";
    alipaytransfer.step = 1;

    alipaytransfer.init = function (id) {
        methodId = id;

        $(".pay-note").show();
        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_" + methodId));

        $("#copyAmount").html(_w88_contents.translate("LABEL_COPY"));
        $("#copyAccountName").html(_w88_contents.translate("LABEL_COPY"));
        $("#copyAccountNo").html(_w88_contents.translate("LABEL_COPY"));

        $('#copyAmount').on('click', function () {
            var amount = $("#txtStep2Amount").text().slice(2); //this will removed the ": "
            alipaytransfer.copytoclipboard(amount);
        });

        $('#copyAccountName').on('click', function () {
            var accountName = $("#txtBankHolderName").text().slice(2); //this will removed the ": "
            alipaytransfer.copytoclipboard(accountName);
        });

        $('#copyAccountNo').on('click', function () {
            var accountNo = $("#txtBankAccountNo").text().slice(2); //this will removed the ": "
            alipaytransfer.copytoclipboard(accountNo);
        });
    };

    alipaytransfer.initTransactionInfo = function () {
        $('#lblStatus').text(_w88_contents.translate("LABEL_FIELDS_STATUS"));
        $('#lblTransactionId').text(_w88_contents.translate("LABEL_TRANSACTION_ID"));
        $('#lblAmount').text(_w88_contents.translate("LABEL_AMOUNT"));
        $('#lblAmount2').text(_w88_contents.translate("LABEL_AMOUNT"));
        $('#lblBankName').text(_w88_contents.translate("LABEL_BANK_NAME"));
        $('#lblBankHolderName').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"));
        $('#lblBankAccountNo').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"));

        $(".pay-note").show();
        $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_AMOUNT_" + methodId));

        $('#PaymentInfo').show();
        $('#PaymentAmount').hide();
        $('.arrow-container').hide();
    }

    alipaytransfer.createDeposit = function (form, data) {
        var _self = this;

        _self.methodId = methodId;
        _self.offlineDeposit(form, data, function (response) {
            var transactionId = response.ResponseData.TransactionId;

            _self.initTransactionInfo();

            _self.send("/payments/" + methodId + "/" + transactionId, "GET", "", function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        $('#txtStatus').text(": " + response.ResponseData.Status);
                        $('#txtTransactionId').text(": " + response.ResponseData.TransactionId);
                        $('#txtStep2Amount').text(": " + response.ResponseData.Amount);
                        $('#txtBankName').text(": " + response.ResponseData.Bank);
                        $('#txtBankHolderName').text(": " + response.ResponseData.AccountName);
                        $('#txtBankAccountNo').text(": " + response.ResponseData.AccountNumber);

                        _self.step = 2;
                        _self.bankUrl = response.ResponseData.BankUrl;

                        _self.checkstatus(transactionId);

                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(response.ResponseMessage);

                        break;
                }
            });
        });
    };

    alipaytransfer.copytoclipboard = function (text) {
        var input = document.createElement('textarea', { "permissions": ["clipboardWrite"] });
        document.body.appendChild(input);
        input.value = text;
        input.focus();
        input.select();

        var s = document.execCommand('copy', false, null);
        window.w88Mobile.Growl.shout(s == true ? _w88_contents.translate("LABEL_COPIED") : "Unable to Copy");

        input.remove();
    };

    alipaytransfer.checkstatus = function (transactionId) {
        var _self = this;

        var intervalId = setInterval(function () {
            _self.send("/payments/deposit/status/" + transactionId, "GET", { selector: "status" }, function (response) {
                switch (response.ResponseCode) {
                    case 1:
                        var result = response.ResponseData.Status;

                        $('#txtStatus').text(": " + result);

                        if (result.indexOf("Successful") == 0) {

                            clearInterval(intervalId);

                            $('#PaymentInfo').show();
                            $('#PaymentAmount').hide();

                            _self.formReset();

                        } else if (result.indexOf("Failed") == 0) {
                            clearInterval(intervalId);
                        }
                    default:
                }
            });
        }, 15000);

    };

    return alipaytransfer;
}
