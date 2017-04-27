window.w88Mobile.Gateways.AliPayTransfer = AliPayTransfer();
var _w88_alipaytransfer = window.w88Mobile.Gateways.AliPayTransfer;

function AliPayTransfer() {

    var alipaytransfer;

    try {
        alipaytransfer = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        alipaytransfer = {};
    }

    alipaytransfer.bankUrl = "";
    alipaytransfer.step = 1;

    alipaytransfer.init = function (methodId) {

        setTranslations();

        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {

                $(".pay-note").show();
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_" + methodId));

                $("#copyAmount").html(_w88_contents.translate("LABEL_COPY"));
                $("#copyAccountName").html(_w88_contents.translate("LABEL_COPY"));
                $("#copyAccountNo").html(_w88_contents.translate("LABEL_COPY"));

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    alipaytransfer.step2 = function (methodId, data) {

        _w88_alipaytransfer.send("/payments/" + methodId, "POST", data, function (response) {
            var transactionId = response.ResponseData.TransactionId;

            setTranslations();

            $('#PaymentInfo').show();
            $('#PaymentAmount').hide();
            $('.arrow-container').hide();

            _w88_alipaytransfer.send("/payments/" + methodId + "/" + transactionId, "GET", "", function (respData) {
                switch (respData.ResponseCode) {
                    case 1:
                        $('#txtStatus').text(": " + respData.ResponseData.Status);
                        $('#txtTransactionId').text(": " + respData.ResponseData.TransactionId);
                        $('#txtStep2Amount').text(": " + respData.ResponseData.Amount);
                        $('#txtBankName').text(": " + respData.ResponseData.Bank);
                        $('#txtBankHolderName').text(": " + respData.ResponseData.AccountName);
                        $('#txtBankAccountNo').text(": " + respData.ResponseData.AccountNumber);

                        _w88_alipaytransfer.step = 2;
                        _w88_alipaytransfer.bankUrl = respData.ResponseData.BankUrl;

                        _w88_alipaytransfer.checkstatus(transactionId);

                        break;
                    default:
                        if (_.isArray(response.ResponseMessage))
                            w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(respData.ResponseMessage));
                        else
                            w88Mobile.Growl.shout(respData.ResponseMessage);

                        break;
                }
            });

            function setTranslations() {
                if (_w88_contents.translate("LABEL_AMOUNT") != "LABEL_AMOUNT") {

                    $('#lblStatus').text(_w88_contents.translate("LABEL_FIELDS_STATUS"));
                    $('#lblTransactionId').text(_w88_contents.translate("LABEL_TRANSACTION_ID"));
                    $('#lblAmount').text(_w88_contents.translate("LABEL_AMOUNT"));
                    $('#lblAmount2').text(_w88_contents.translate("LABEL_AMOUNT"));
                    $('#lblBankName').text(_w88_contents.translate("LABEL_BANK_NAME"));
                    $('#lblBankHolderName').text(_w88_contents.translate("LABEL_ACCOUNT_NAME"));
                    $('#lblBankAccountNo').text(_w88_contents.translate("LABEL_ACCOUNT_NUMBER"));

                    $(".pay-note").show();
                    $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_MSG_AMOUNT_" + methodId));
                } else {
                    window.setInterval(function () {
                        setTranslations();
                    }, 500);
                }
            }
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

        var intervalId = setInterval(function() {

            var headers = {
                'Token': window.User.token,
                'LanguageCode': window.User.lang
            };

            $.ajax({
                type: "GET",
                url: w88Mobile.APIUrl + "/payments/deposit/status/" + transactionId,
                headers: headers,
                success: function(response) {
                    switch (response.ResponseCode) {
                        case 1:
                            var result = response.ResponseData.Status;

                            $('#txtStatus').text(": " + result);

                            if (result.indexOf("Successful") == 0) {

                                clearInterval(intervalId);

                                $('#PaymentInfo').show();
                                $('#PaymentAmount').hide();

                                $('#form1')[0].reset();

                            } else if (result.indexOf("Failed") == 0) {
                                clearInterval(intervalId);
                            }
                        default:
                    }
                },
                error: function () {
                    console.log("Error connecting to api");
                }
            });

        }, 5000);

    };

    return alipaytransfer;
}
