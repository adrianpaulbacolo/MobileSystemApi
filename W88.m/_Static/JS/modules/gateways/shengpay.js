function ShengPay() {

    var gatewayId = "1202111";

    var shengpay = {
        deposit: deposit
        , withdraw: withdraw
        , gatewayId: gatewayId
        ,Initialize: init
    };

    return shengpay;

    // deposit
    function deposit(data, successCallback, errorCallback, completeCallback) {
        validate(data, "deposit");
        window.w88Mobile.Gateways.DefaultPayments.Send("/payments/1202111", "POST", successCallback, data, completeCallback);
    }

    // withdraw
    function withdraw(data, successCallback, errorCallback, completeCallback) {
    }

    function validate(data, method) {
        // @todo add validation here
        return;
    }

    function init() {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $("#paymentNote").text(data.LABEL_PAYMENT_NOTE);
                $("#paymentNoteContent").text(data.LABEL_PAYMENT_NOTE1);
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    }

}

window.w88Mobile.Gateways.ShengPay = ShengPay();