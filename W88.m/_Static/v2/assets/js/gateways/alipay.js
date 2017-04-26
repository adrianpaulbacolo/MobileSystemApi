window.w88Mobile.Gateways.AlipayV2 = AlipayV2();
var _w88_alipay = window.w88Mobile.Gateways.AlipayV2;

function AlipayV2() {

    var alipay;

    try {
        alipay = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));
    } catch (err) {
        alipay = {};
    }

    alipay.init = function (gatewayId) {

        setTranslations();

        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $('label[id$="lblSwitchLine"]').text(_w88_contents.translate("LABEL_SWITCH_LINE"));
                $("#paymentNote").text(_w88_contents.translate("LABEL_PAYMENT_NOTE"));

                if (!_.isUndefined(gatewayId)) {
                    if (gatewayId == "120254") {
                        $("#paymentNoteContent").html(_w88_contents.translate("LABEL_MSG_120254")); //SDA alipay note
                    }
                } else {
                    $("#paymentNoteContent").html(_w88_contents.translate("LABEL_PAYMENT_NOTE_ALIPAY"));
                }

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    alipay.createDeposit = function () {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            ThankYouPage: params.ThankYouPage,
            SwitchLine: params.SwitchLine,
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
                            w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.DummyURL, "body");
                            w88Mobile.PostPaymentForm.submit();
                        }
                    }

                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage), _self.shoutCallback);
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage, _self.shoutCallback);

                    break;
            }
        },
        function () {
            pubsub.publish('stopLoadItem', { selector: "" });
        });
    }

    return alipay;
}
