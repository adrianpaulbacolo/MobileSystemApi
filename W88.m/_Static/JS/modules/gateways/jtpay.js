function JTPay() {

    var jtpay = {
        Initialize: init
    };

    return jtpay;

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