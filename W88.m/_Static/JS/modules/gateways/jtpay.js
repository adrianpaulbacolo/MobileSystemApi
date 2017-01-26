function JTPay() {

    var jtpay = {
        Initialize: init,
        Deposit : deposit
    };

    return jtpay;

    function deposit(data, successCallback, errorCallback, completeCallback) {
        window.w88Mobile.Gateways.DefaultPayments.Send("/payments/120262", "POST", successCallback, data, completeCallback);
    }

    function init(version) {
        var translations = amplify.store("translations");
        setTranslations(translations);
        function setTranslations(data) {
            if (!_.isUndefined(data)) {
                $("#paymentNote").text(data.LABEL_PAYMENT_NOTE);

                if (version == "0") {
                    $("#paymentNoteContent").text(data.LABEL_PAYMENT_NOTE0);
                }
                else {
                    $("#paymentNoteContent").text(data.LABEL_PAYMENT_NOTE1);
                }
            }
        }
    }

}

window.w88Mobile.Gateways.JTPay = JTPay();