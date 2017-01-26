function JTPay() {

    var gatewayId = "120262";

    var jtpay = {
        gatewayId: gatewayId,
        Initialize: init,
        Deposit: deposit
    };

    return jtpay;

    function deposit(data, successCallback, errorCallback, completeCallback) {
        window.w88Mobile.Gateways.DefaultPayments.Send("/payments/" + window.w88Mobile.Gateways.JTPay.gatewayId, "POST", successCallback, data, completeCallback);
    }

    function init(version) {

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                if (version == "0") {
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE0"));
                }
                else {
                    $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE1"));
                }
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    }

}

window.w88Mobile.Gateways.JTPay = JTPay();