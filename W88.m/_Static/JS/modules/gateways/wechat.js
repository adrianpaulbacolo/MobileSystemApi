window.w88Mobile.Gateways.WeChatV2 = WeChatV2();
var _w88_wechat = window.w88Mobile.Gateways.WeChatV2;

function WeChatV2() {

    var wechat = {};

    wechat.init = function () {

        wechat = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));

        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));
                $("#paymentNoteContent").text(_w88_contents.translate("LABEL_PAYMENT_NOTE_WECHAT"));
                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_AMOUNT"));
            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    wechat.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            ThankYouPage: params.ThankYouPage,
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function (response) {
            switch (response.ResponseCode) {
                case 1:
                    if (response.ResponseData.VendorRedirectionUrl) {
                        window.open(response.ResponseData.VendorRedirectionUrl, '_blank');
                    } else {
                        if (response.ResponseData.PostUrl) {
                            w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                            w88Mobile.PostPaymentForm.submit();
                        } else if (response.ResponseData.DummyURL) {
                            window.open(response.ResponseData.DummyURL, '_blank');
                        }
                    }

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
            }
        },
            function () {
                pubsub.publish('stopLoadItem', { selector: "" });
            });
    }

    return wechat;
}

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