function Alipay() {

    var alipay = {
        Initialize: init
    };

    return alipay;

    function init() {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE1"));
                $('label[id$="lblSwitchLine"]').text(_w88_contents.translate("LABEL_SWITCH_LINE"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    }
}

window.w88Mobile.Gateways.Alipay = Alipay();