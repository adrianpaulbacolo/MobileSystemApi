window.w88Mobile.Gateways.Pending = Pending();

var _w88_pending = window.w88Mobile.Gateways.Pending;

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
        $('label[id$="lblTransactionId"]').text($.i18n("LABEL_TRANSACTION_ID"));
        $('label[id$="lblRequestTime"]').text($.i18n("LABEL_DATE_TIME"));
        $('label[id$="lblPaymentMethod"]').text($.i18n("LABEL_PAYMENT_METHOD"));
        $('label[id$="lblAmount"]').text($.i18n("LABEL_AMOUNT"));
        $('label[id$="lblStatus"]').text($.i18n("LABEL_FIELDS_STATUS"));

        var widrawText = $.i18n("LABEL_HISTORY_DEPOSITWIDRAW").split("/");
        $('#btnCancel').text($.i18n("BUTTON_CANCEL") + " " + $.trim(widrawText[1]));

        $('.gateway-select').hide();
        $('.gateway-restrictions').hide();
        $('#btnSubmitPlacement').hide();
        $('header .header-title').text($.i18n("LABEL_FUNDS_WIDRAW"));

        var pendingWithdraw = amplify.store(withdrawStorageKey);

        if (!_.isUndefined(pendingWithdraw)) {
            $('span[id$="txtTransactionId"]').text(pendingWithdraw.TransactionId);
            $('span[id$="txtRequestTime"]').text(pendingWithdraw.RequestDateTime);
            $('span[id$="txtPaymentMethod"]').text(pendingWithdraw.Name);
            $('span[id$="txtAmount"]').text(pendingWithdraw.Amount);
        } else {
            window.location = withdrawPage;
        }

        $("#btnCancel").click(function (e) {
            e.preventDefault();
            window.w88Mobile.Gateways.Pending.cancel();
        });
    };

    pending.cancel = function () {
        var _self = this;

        var pendingWithdraw = amplify.store(withdrawStorageKey);

        _self.send("/payments/withdrawal/pending", "POST", pendingWithdraw, function (response) {
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