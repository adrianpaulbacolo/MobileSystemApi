window.w88Mobile.Gateways.Pending = Pending();

function Pending() {

    var pending;
    var withdrawPage = "/v2/Withdrawal/";
    var withdrawStorageKey = w88Mobile.Keys.withdrawalSettings + "-pending";

    try {
        pending = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        pending = {};
    }

    pending.init = function () {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {

                $('label[id$="lblTransactionId"]').text(_w88_contents.translate("LABEL_TRANSACTION_ID"));
                $('label[id$="lblRequestTime"]').text(_w88_contents.translate("LABEL_DATE_TIME"));
                $('label[id$="lblPaymentMethod"]').text(_w88_contents.translate("LABEL_PAYMENT_METHOD"));
                $('label[id$="lblAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
                $('label[id$="lblStatus"]').text(_w88_contents.translate("LABEL_FIELDS_STATUS"));
                $('#btnCancel').text(_w88_contents.translate("BUTTON_CANCEL"));

                $('.gateway-select').hide();
                $('.gateway-restrictions').hide();
                $('#btnSubmitPlacement').hide();
                $('header .header-title').text(_w88_contents.translate("LABEL_FUNDS_WIDRAW"));

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }

        var pendingWithdraw = amplify.store(withdrawStorageKey);

        if (!_.isUndefined(pendingWithdraw)) {
            $('span[id$="txtTransactionId"]').text(pendingWithdraw.TransactionId);
            $('span[id$="txtRequestTime"]').text(pendingWithdraw.RequestDateTime);
            $('span[id$="txtPaymentMethod"]').text(pendingWithdraw.Name);
            $('span[id$="txtAmount"]').text(pendingWithdraw.Amount);
        } else {
            window.location = withdrawPage;
        }

    };

    pending.cancel = function () {

        var pendingWithdraw = amplify.store(withdrawStorageKey);

        _w88_paymentSvcV2.Send("/payments/withdrawal/pending", "POST", pendingWithdraw, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    w88Mobile.Growl.shout("<p>" + response.ResponseMessage + "</p>", function () {
                        window.location = withdrawPage;
                    });
                    
                    break;

                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);
                    break;
            }
        });

    };
    return pending;
}