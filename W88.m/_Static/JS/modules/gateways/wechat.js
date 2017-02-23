function wechat() {

    var wechat = {
        Initialize: init
    };

    return wechat;

    function init() {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE0"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    }
}

window.w88Mobile.Gateways.Wechat = wechat();